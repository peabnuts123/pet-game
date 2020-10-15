import { APIGatewayProxyHandlerV2, APIGatewayProxyResultV2 } from 'aws-lambda';
// import { APIGatewayProxyEventV2, Context } from 'aws-lambda';
import AWS, { AWSError } from 'aws-sdk';
import { PromiseResult } from 'aws-sdk/lib/request';

const s3 = new AWS.S3();
const ROUTE_MAP_KEY = 'route-map.json'; // @TODO fetch from parameter store
const BUCKET_NAME = 'pet-game-dev-www'; // @TODO fetch from parameter store

/**
 * @TODO remove
 * Simply send back the data that the function was sent, for debugging.
 */
// function debug_echoRequest(event: APIGatewayProxyEventV2, context: Context): APIGatewayProxyResultV2 {
//   return {
//     statusCode: 200,
//     headers: {
//       'Content-Type': 'application/json',
//     },
//     body: JSON.stringify({
//       status: "[DEBUG] Echoing request",
//       event,
//       context,
//     }),
//   } as APIGatewayProxyResultV2;
// }

export const handler: APIGatewayProxyHandlerV2 = async (event, _context) => {
  // return debug_echoRequest(event, _context);

  // Proactively fetch the route-map and index.html if we need them, to prevent 2 serial trips
  //  to S3.
  // @NOTE this is only okay because this lambda is not designed to serve heavy amounts of traffic,
  //  it's expected that the response from this lambda will be strongly cached
  const routeMapPromise = s3.getObject({
    Bucket: BUCKET_NAME, // @TODO read from config
    Key: ROUTE_MAP_KEY,
  }).promise();
  const indexHtmlPromise = s3.getObject({
    Bucket: BUCKET_NAME, // @TODO read from config
    Key: 'index.html',
  }).promise();

  try {
    console.log(`[DEBUG] Fetching '${event.rawPath}' from s3 bucket`);

    // Forward request to S3
    // It will throw if the request fails e.g. for a file that doesn't exist, or if we don't have auth
    const s3Response = await s3.getObject({
      Bucket: BUCKET_NAME, // @TODO read from config
      Key: event.rawPath.replace(/^\//g, ''), // Remove leading slash,
    }).promise();

    console.log(`[DEBUG] Successfully fetched data from s3 bucket.`, s3Response.$response);

    // Forward response from S3
    return forwardS3Response(s3Response);
  } catch (_e) {
    console.error(`[DEBUG] Error occurred: `, _e);
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
        // Ensure route map has finished loading, first (as well as the index.html we will send in the response)
        const [routeMapResponse, indexHtmlResponse] = await Promise.all([routeMapPromise, indexHtmlPromise]);

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
        return {
          statusCode: 500,
          headers: {
            'Content-Type': 'application/json',
          },
          body: JSON.stringify({
            status: 'Failed while reading route map / index.html from S3 bucket',
            error: e as unknown,
          }),
        };
      }
    }

    // Error is not 'NoSuchKey' - return a valuable error for debugging
    return {
      statusCode: 500,
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify({
        status: 'An error occurred',
        error,
      }),
    };
  }
};

function routeMapContainsPath(routeMap: string[], path: string): boolean {
  return routeMap.some((route: string) => {
    const regex = new RegExp(`^${route.replace(/\*/g, '[^/]+')}/?$`);
    return regex.test(path);
  });
}

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
