# PetGame WWW

This is the frontend for the pet-game project. Currently this single project encompasses the entire frontend, but will split up into smaller sites as the project grows.

## Architecture

The project is a standard [Preact](https://preactjs.com/) application, which is to say, it's almost identicaly to a React application. It is a single-page web application.

The project uses [MobX](https://mobx.js.org/README.html) for state management. Several "Stores" contain different sets of state for different areas of the app. These may contain client-side state, or a local cache of data fetched from the API.

The frontend project fetches all of the data it needs, including authentication details through the [API](../api/README.md) project.

For production, the API is expected to be hosted on the same domain as the frontend, on the path `/api`. For development purposes, though, the API is expected to be on a separate domain (e.g. https://localhost:5000 vs. http://localhost:8080).

### Exported routes, 404s
This component uses client-side routing which means that it uses JavaScript to "navigate" around the site, rewriting the URL as you do-so. As a result, all single-page apps have the downside of the server not knowing which requests are valid, and which are not-found. To work around this, a `route-map.json` file is exported at build time, containing a list of all valid routes within the frontend client. This file is deployed alongside the client, and consumed by the [www-proxy](../www-proxy/README.md) component to determine whether the request should be served an HTTP 200, or an HTTP 404. This is only the case in production; for local development, all requests are served an HTTP 200.

## Local development

### Prerequisites (local)

In order to run the project locally, you will need the following:

  - [Node.js and npm](https://nodejs.org/en/)
  - You must have an instance of the API running in order for the site to work properly. You can run it locally in another terminal window, or by running it in docker. See [the API's README](../api/README.md) on how to do this.

### Running the project (local)

1. Make sure you are in the `src/www` directory
1. First make sure you have installed the project's dependencies by running:
    ```sh
    npm install
    ```
1. You can start the development server by running:
    ```sh
    npm start
    ```

There are more npm scripts that you can run. Here is what each of them does:
  - `npm run build` - Build the project
    - You must set the `NODE_ENV` environment variable to specify which build configuration is included in the build e.g. `NODE_ENV=production`
  - `npm test` - Run the tests
    - You can also run `npm run test:coverage` to run the tests and collect test coverage statistics, which will then be served on a URL upon completion (to be opened in your browser). Hit CTRL-C to stop the web server after you're finished with it.
  - `npm run lint` - Run code-quality checks across the project
  - `npm run type-check` - Use the TypeScript compiler to check your project has no errors
  - `npm run simulate-build` - "Simulate" the CI server by running all of `test`, `lint`, `type-check` and `build` in-order, to verify that everything is okay

## Docker

### Prerequisites (docker)

You don't need as much stuff to run the project in docker, as the application's dependencies are included in the Docker container. However, you still need an instance of the API running (which can also run locally, in Docker).

  - [docker](https://docs.docker.com/get-docker/) and [docker-compose](https://docs.docker.com/compose/install/)
  - You must have an instance of the API running in order for the site to work properly. You can run it locally in another terminal window, or by running it in docker. See [the API's README](../api/README.md) on how to do this.


### Running the project (docker)

Running the WWW component in docker-compose is very simple.

Unlike running the API in docker-compose, you never need to run `--build` for the WWW component, as it runs without a Dockerfile.

1. Ensure you are in the project root directory (i.e. not in `src/www`)
1. Run the following command:
    ```sh
    docker-compose up -d www
    ```

You should be good to go!

## Deploying the WWW component to a cloud environment

The WWW component can also run in an AWS cloud environment, hosted in S3, and served by CloudFront. The way pet-game is currently architected, it's actually served by a reverse-proxy in-between CloudFront and S3, which is the [www-proxy](../www-proxy/README.md) component.

1. Ensure the environment exists. Run Terraform to make sure the infrastructure exists / is up to date. See the [terraform documentation](../../terraform/README.md) for more details on this.
1. (From project root) Run the script `./builds/scripts/build-deploy-www.sh [s3_bucket_name]` with the appropriate S3 bucket name to build and deploy the WWW component to an existing environment. You can get the WWW S3 bucket name out of Terraform's outputs, which you can view with:
    ```sh
    ./builds/scripts/terraform-show.sh
    ```


## Work backlog / TODO

  - _Nothing at-present_.
