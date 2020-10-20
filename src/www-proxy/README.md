# PetGame WWW Proxy

This is a reverse proxy that fetches the frontend's content from S3. It looks at requests and will compare 404's from S3 to the exported `route-map.json` to check if the request is a valid route in the SPA. If-so, `index.html` will be served with an HTTP 200. If the request is _still_ not a valid route, then a true 404 will be returned (`index.html` will still be served). This makes the SPA behave much like a regular web-app.

## Architecture

This component is a simple Node.js script intended to be deployed to AWS Lambda. It exports a single handle function that reads incoming requests and forwards them to S3. Based on the response, it will respond with on of the following:

  - If the request is for an object that exists in S3 (e.g. `/manifest.json`), then it will serve that with an HTTP 200 status code
  - If the request does not exist in S3 then `index.html` will be served, to show the client application, but the HTTP status code will vary depending on the request:
    - The WWW Proxy component will first fetch the list of all valid routes for the WWW component, from a file called `route-map.json`. This is expected to be served from the same S3 bucket as the WWW component itself. This route map is then used to determine whether the request is for a valid route in the app
    - If the request is for a valid route in the client application (e.g. `/taking-tree`), then an HTTP 200 status code will be served
    - Otherwise, an HTTP 404 status code will be served
    - In the scenario where some kind of error occurs while processing, an HTTP 500 will be returned, along with some error information

The WWW Proxy component eagerly fetches the things it needs from AWS before handling any requests, so that subsequent requests are faster.

The WWW Proxy component reads the following data from AWS:

  - Config data from SSM parameter store
    - The name of the S3 bucket containing the WWW component
    - The name of the file containing the route map (e.g. `route-map.json`)
  - Common data needed from S3
    - The contents of `index.html`, for serving the frontend app
    - The contents of `route-map.json`, for determining valid routes in the frontend client

Note that, since these are fetched on "boot", these values will be effectively cached until a new Lambda cold boot. Deploying the WWW Proxy component again will force a cold boot of all Lambda instances.

## Local development

Since the WWW Proxy component is designed so strongly around cloud concepts, it currently **cannot be run locally**, it must be deployed to an environment to test it.

You could make any of the following changes to make it run locally:
  - Abstract dependencies on AWS e.g. S3, Parameter Store into separate modules, and stub them out for local dev
  - Inject development AWS credentials to allow the app to successfully reach out to AWS

Likely one day these changes will be done, but for now, it is just left as a simple component.

### Prerequisites (local)

Nonetheless, you will still need the following to develop the project:
  - [Node.js and npm](https://nodejs.org/en/)

### Running the project (local)

_N/A_

## Docker

As above, the project currently does not run locally in Docker either.

### Prerequisites (docker)

_N/A_

### Running the project (docker)

_N/A_

## Deploying the WWW component to a cloud environment

The WWW Proxy component is designed to run in an AWS cloud environment, on Lambda functions.

In order to deploy the WWW Proxy component to a cloud environment:

1. Ensure the environment exists. Run Terraform to make sure the infrastructure exists / is up to date. See the [terraform documentation](../../terraform/README.md) for more details on this.
1. (From project root) Run the script `./builds/scripts/build-deploy-www-proxy.sh [lambda_function_name]` with the appropriate lambda function name to build and deploy the WWW Proxy component to an existing environment. You can get the www-proxy lambda function name out of Terraform's outputs, which you can view with:
    ```sh
    ./builds/scripts/terraform-show.sh
    ```


## Work backlog

  - Tests
    - Mock S3 requests somehow and simulate requests / responses
    - Add test step back to `simulate-build`
  - Make component able to run locally
    - Abstract dependencies into modules? e.g. ConfigModule, FrontendModule etc.
    - Inject develop AWS credentials that allow the app to reach out to AWS