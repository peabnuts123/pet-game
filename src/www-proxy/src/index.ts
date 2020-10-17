import { APIGatewayProxyHandlerV2, APIGatewayProxyResultV2 } from 'aws-lambda';
import AWS, { AWSError } from 'aws-sdk';
import { PromiseResult } from 'aws-sdk/lib/request';

const s3 = new AWS.S3();
const ssm = new AWS.SSM();

// Config
const ENVIRONMENT_ID = process.env['ENVIRONMENT_ID'];
const BUCKET_NAME_PARAMETER_NAME = `/pet-game/${ENVIRONMENT_ID}/www-proxy/Configuration/WWWBucketName`;
const ROUTE_MAP_KEY_PARAMETER_NAME = `/pet-game/${ENVIRONMENT_ID}/www-proxy/Configuration/RouteMapKey`;
const PARAMETER_STORE_PATH = `/pet-game/${ENVIRONMENT_ID}/www-proxy/`;

// Fetch data from parameter store
const PARAMETER_STORE_PROMISE = ssm.getParametersByPath({
  Path: PARAMETER_STORE_PATH,
  Recursive: true,
}).promise();

// Read config from parameter store
const BUCKET_NAME_PROMISE = PARAMETER_STORE_PROMISE.then((parameterStoreResult) => tryGetParameter(parameterStoreResult, BUCKET_NAME_PARAMETER_NAME));
const ROUTE_MAP_KEY_PROMISE = PARAMETER_STORE_PROMISE.then((parameterStoreResult) => tryGetParameter(parameterStoreResult, ROUTE_MAP_KEY_PARAMETER_NAME));

// Read common data from S3
/**
 * The route map is a JSON file that defines all the valid routes in our frontend app
 */
const ROUTE_MAP_PROMISE = Promise.all([BUCKET_NAME_PROMISE, ROUTE_MAP_KEY_PROMISE])
  .then(([BUCKET_NAME, ROUTE_MAP_KEY]) => s3.getObject({
    Bucket: BUCKET_NAME,
    Key: ROUTE_MAP_KEY,
  }).promise());

/**
 * index.html is used when serving any kind of 404 - it is the default page
 */
const INDEX_HTML_PROMISE = BUCKET_NAME_PROMISE
  .then((BUCKET_NAME) => s3.getObject({
    Bucket: BUCKET_NAME,
    Key: 'index.html',
  }).promise());

/**
 * Main entry point function for lambda.
 *
 * @param event Request data for the Lambda (from API Gateway)
 * @param _context Data from the lambda environment context
 */
export const handler: APIGatewayProxyHandlerV2 = async (event, _context) => {
  // Ensure parameter store data is loaded
  let BUCKET_NAME, _ROUTE_MAP_KEY;
  try {
    BUCKET_NAME = await BUCKET_NAME_PROMISE;
    _ROUTE_MAP_KEY = await ROUTE_MAP_KEY_PROMISE;
  } catch (e) {
    return errorResponse("Could not read value from parameter store", e);
  }

  try {
    // Forward request to S3
    // It will throw if the request fails e.g. for a file that doesn't exist, or if we don't have auth
    const s3Response = await s3.getObject({
      Bucket: BUCKET_NAME, // @TODO read from config
      Key: event.rawPath.replace(/^\//g, ''), // Remove leading slash,
    }).promise();

    // If we made it here then the request is for a file that exists in S3
    // Forward response from S3
    return forwardS3Response(s3Response);
  } catch (_e) {
    const error: AWSError = _e as AWSError;

    // Check if error is instanceof AWSError
    // @NOTE `AWSError` is not a real class, so instanceof cannot be used
    // We must resort to duck-typing to infer it has the properties we need
    if (error.code !== undefined && (
      (error.code === 'NoSuchKey') || // 404 from S3
      (error.code === 'UriParameterError' && event.rawPath === '/') // Request root which produces S3 key as empty string
    )) {
      // If we 404 from S3, check if it's a valid route in the route map
      try {
        // Ensure route map has finished loading, first (as well as the index.html, which we will send in the response)
        const [routeMapResponse, indexHtmlResponse] = await Promise.all([ROUTE_MAP_PROMISE, INDEX_HTML_PROMISE]);

        // Parse routeMap as JSON (will throw if any of this is not as we expect)
        const routeMap = JSON.parse(routeMapResponse.$response.httpResponse.body.toString('utf-8')) as string[];

        // @NOTE use of `rawPath`, as the leading slash is needed
        if (routeMapContainsPath(routeMap, event.rawPath)) {
          // Request is for a path in the route map
          // Return index.html with code 200
          return forwardS3Response(indexHtmlResponse);
        } else {
          // Request is for a path not in S3 or the application's route map
          // Return index.html with code 404
          return forwardS3Response(indexHtmlResponse, 404);
        }
      } catch (e) {
        // Failed to load route map (or index.html), this is bad
        return errorResponse("Failed while reading route map / index.html from S3 bucket", e);
      }
    }

    // Error is not 'NoSuchKey' - return a valuable error for debugging
    return errorResponse("An error occurred", error);
  }
};

/**
 * Create a generic error response that returns HTTP 500 and displays some kind of error information
 *
 * @param status Error message to display
 * @param error Any kind of error data you want to return
 */
function errorResponse(status: string, error: unknown): APIGatewayProxyResultV2 {
  return {
    statusCode: 500,
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify({
      status,
      error,
    }),
  };
}

/**
 * Test whether the given route map contains the given path
 * e.g. `/user/profile/*` matches `/user/profile/58`
 * @param routeMap Route map instance
 * @param path Path to check
 */
function routeMapContainsPath(routeMap: string[], path: string): boolean {
  return routeMap.some((route: string) => {
    const regex = new RegExp(`^${route.replace(/\*/g, '[^/]+')}/?$`);
    return regex.test(path);
  });
}

/**
 * Create a response based on a response from S3
 *
 * @param s3Response Response object from S3 GetObject
 * @param statusCode Status code to send in the response e.g. `200`
 */
function forwardS3Response(s3Response: PromiseResult<AWS.S3.GetObjectOutput, AWS.AWSError>, statusCode?: number): APIGatewayProxyResultV2 {
  // Re-encode response body as a string
  const responseBody = s3Response.$response.httpResponse.body;
  let isBase64Encoded: boolean = false;
  let body: string = '';

  if (typeof responseBody === 'string') {
    // Already a string - just use it
    body = responseBody;
  } else {
    // Not a string - convert to base64 regardless - it will be converted
    //  to binary by API Gateway
    body = responseBody.toString('base64');
    isBase64Encoded = true;
  }

  // Return the response from S3
  return {
    statusCode: statusCode || s3Response.$response.httpResponse.statusCode,
    headers: s3Response.$response.httpResponse.headers,
    body,
    isBase64Encoded,
  } as APIGatewayProxyResultV2;
}

/**
 * Attempt to get a parameter from the parameter store result. Does generic validation and error handling/throwing.
 *
 * @param parameterStoreResult Result object from SSM.GetParametersByPath
 * @param parameterName Parameter name to try and get
 */
function tryGetParameter(parameterStoreResult: PromiseResult<AWS.SSM.GetParametersByPathResult, AWS.AWSError>, parameterName: string): string {
  if (parameterStoreResult.Parameters === undefined) {
    throw new Error("Failed to fetch from parameter store");
  } else {
    const parameterResponse = parameterStoreResult.Parameters.find((parameter) => parameter.Name === parameterName);

    if (parameterResponse === undefined) {
      throw new Error("No parameter found in parameter store with name: " + parameterName);
    } else if (parameterResponse.Value === undefined) {
      throw new Error(`Parameter is empty: ${parameterName}`);
    } else {
      return parameterResponse.Value;
    }
  }
}
