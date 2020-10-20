# Pet Game

So you can like adopt pets and stuff. You can play games with your pets and look after them. You can interact with other pet owners on the site too. It's totally an original idea and _absolutely not_ a clone of another popular website.

## What is this?

This is a pet project (if you will) of mine combining two of my interests: game development (my hobby), and web development (my job). It's a place to learn more about both subjects by applying new things I learn to a real project.

## Project design

The project currently has a simple experience:
  - A home page telling you about the game
  - The ability to log in/register through Auth0
  - The ability to update your profile
  - An inventory of items on your profile
  - A "Taking Tree" to which players can donate their items, or claim items that have been donated by other players

This experience is the first minimal version of the game, enough to justify an architecture comprised of a few components, but simple enough to implement on a hobbyist basis. It's also generic-enough an experience that it can easily grow, without having to throw away much work.

## Project architecture

The architecture is currently as-follows:
  - PostgreSQL database (also known as `devdb` locally)
  - `api` - .NET Core 3.1 Web API
    - A straight-forward REST API
    - Talks to the database using EFcore
    - Uses cookies for authentication so that login/register must first go through the API (so that the user can be stored in the database)
  - `www` - Preact single-page-application frontend
    - A straight-forward SPA
    - Uses mobx for state management
  - `www-proxy` - JavaScript-based reverse-proxy for serving the frontend with correct HTTP response codes
    - Designed to be hosted in Lambda, this component manages requests to the frontend and works around the problem of "single-page apps always return HTTP 200 even for not-found responses"
    - If a request is for a static file (e.g. `favicon.ico`) then it is served with an HTTP 200 status code
    - If a request is a for a route that exists in the app (e.g. `/taking-tree`) then `index.html` is served with an HTTP 200 status code
    - Otherwise a request is served `index.html` with an HTTP 404 status code
    - Basically, requests to the single-page frontend application respond with correct HTTP status codes
    - The frontend app is also configured to reload the page when crossing between a 404/200 boundry, so the user will always be served the correct response code

For now, due to the size of the application, the architecture is technically delineated into "monolithic" components (i.e. "the _whole_ API is a single project"). As the project grows, the architecture will likely be split up into smaller, more discrete services and frontends. The exact nature of this will be designed as-needed.

## Roadmap

Here is a rough roadmap of the project, as well as some "what about this?" items:

  - Make a proof-of-concept for games / leaderboards / currency
    - Add games to the website that users can play. When they finish playing, they can submit their score to the leaderboards / redeem their score for "points" i.e. currency
    - This raises a lot of anti-cheat concerns which need to be thought about and researched
    - Games will likely be built in Unity3D and compiled to WASM for production
  - Add pets
  - Some ability to spend currency? Buy items? Sell items?
  - ?Every day you receive 5 inventory items?

## Running the project

Each project is its own self-contained "component". A guide for running / developing each component can be found in each respective README:

  - [API](./src/api/README.md)
  - [www](./src/www/README.md)
  - [www-proxy](./src/www-proxy/README.md)
  - [DevDB](./src/devdb/README.me)
  - [Terraform (infrastructure)](./terraform/README.md)

The project as-a-whole is designed to be able to run in a few ways:

  - Manually within each component, for debugging / developing (e.g. `npm start`, `dotnet run`)
  - With docker-compose, for developing services that depend on other components
    - e.g. if you're working on the frontend and just need a working API to be running, you can run the API and the DB in docker, and run the frontend manually
  - On cloud infrastructure e.g. AWS in Lambda
  - In your IDE's debugger e.g. vscode

## Deploying the project

The project is currently designed to run in AWS, across a series of services. This infrastructure can be provisioned through terraform, to stand up an entire environment. Deployments of the code for each individual component is separate from provisioning the infrastructure, and terraform is intentionally not configured to deploy any code.

The process to create a new environment is as-follows:

  1. Configure a `tfvars` file for the environment (e.g. `env_test.tfvars`)
  1. Run terraform
  1. Deploy the code for each component

See [the documentation in the terraform subdirectory](./terraform/README.md) for more details.

## Work backlog / TODO
  - Ensure dev environment is up-to-date
  - Deploy test environment
  - Delete old clicky-click environment
  - Take down heroku/netlify environment
