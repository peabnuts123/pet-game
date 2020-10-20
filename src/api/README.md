# PetGame API

This is the API for the pet-game project. Currently this single project encompasses the entire API, but will split up into smaller services as the project grows.

## Architecture

The API component is a simple .NET Core Web API. It's as close to standard as possible, intentionally, to keep it simple, and make development on the project as easy as possible.

The project is designed to run in a few different contexts:

  - Locally on your machine (e.g. `dotnet run`)
  - In AWS Lambda (behind API Gateway) (i.e. "Serverless")
  - In Docker and docker-compose

## Local development

### Prerequisites (local)

In order to run the project locally you will need the following things installed / running on your machine
  - [.NET Core SDK](https://dotnet.microsoft.com/download/dotnet/current) (3.1 or newer)
  - [.NET EF Core CLI](https://docs.microsoft.com/en-us/ef/core/miscellaneous/cli/dotnet)
    - This is for applying database migrations
    - You can install this tool by running:
        ```sh
        dotnet tool install --global dotnet-ef
        ```

  - A PostgreSQL instance running somewhere
    - see [DevDB readme](../devdb/README.md) for details on running one locally in Docker
  - An account on Auth0
    - Any tier is fine, you just need to get the client secret from application settings
    - Make sure to allow your local environment to call Auth0:
      - Allowed Callback URLs: `https://localhost:5000/api/auth/callback`
      - You will need to add your production URL there, too
      - The other config is all for frontend (www). See [WWW readme](../www/README.md) for more details on that.

### Running the project (local)

The main entrypoint of the project (i.e. the "startup project") is PetGame.Web.

1. Make sure you are in the `src/api/PetGame.Web` directory
1. Edit `appsettings.json` to include your Auth0 information (Note: NOT your client secret, that comes next)
1. Make a permanent environment variable on your local machine called `ENVIRONMENT_ID` and set it to `local`
    - This variable controls which environment configs to read from
1. Copy the file `_secrets.sample.json` and name it `_secrets.local.json`
    - Name it something different if you set `ENVIRONMENT_ID` to something other than `local`
1. Fill in all the values in `_secrets.local.json`
    - Auth0 client secret can be fetched from Auth0 dashboard
    - Put a connection string for `DATABASE_URL`. If you are running a local docker instance (e.g. devdb) then fill in the credentials you set when you _first ran_ the container (the credentials are NOT updated when you run the container again). Put `localhost` for the host if you are using docker.
    - Leave `PORT` and `HOST_PROTOCOL` alone unless you have some need to change them (Note that the app won't function very well without HTTPS)
    - You can edit `appsettings.local.json` too if you want, but you shouldn't need to change anything in there
1. Ensure the database is up-to-date by running:
    ```sh
    dotnet ef database update
    ```
    - This will fetch the connection string from `_secrets.local.json` as your `ENVIRONMENT_ID` is set to `local`. You can see how this mechanism works in [src/api/PetGame.Config/Configuration.cs](./PetGame.Config/Configuration.cs)
1. You should be good to go. Now you can run the project by running `dotnet run` or `dotnet watch` 👍

## Docker

### Prerequisites (docker)

You don't need as much stuff to run the project in docker, as the application's dependencies are included in the Docker container. However, you still need a database to connect to (which can also run locally, in Docker) and an Auth0 account.

 - [docker](https://docs.docker.com/get-docker/) and [docker-compose](https://docs.docker.com/compose/install/)
  - A PostgreSQL instance running somewhere
    - see [DevDB readme](../devdb/README.md) for details on running one locally in Docker
  - An account on Auth0
    - Any tier is fine, you just need to get the client secret from application settings
    - Make sure to allow your local environment to call Auth0:
      - Allowed Callback URLs: `https://localhost:5000/api/auth/callback`
      - You will need to add your production URL there, too
      - The other config is all for frontend (www). See [WWW readme](../www/README.md) for more details on that.
  - You _must_ set up docker to use the same HTTPS certificate as your localhost. It's kind of a whole thing but only needs to be done once, and only takes a minute, see below.


### Setting up docker to use the same HTTPS certificate as your localhost

Your local version of `dotnet` generates and trusts its own self-signed certificate for `localhost`. When you run the app in docker (or if you are using something like WSL) then you need to configure it to use your local version of `dotnet`'s certificate instead of its own (which your computer will not trust).

You need to export the dotnet HTTPS certificate and reference it in the docker config.

1. Run `dotnet dev-certs https --trust` on your machine to make sure your machine trusts `dotnet`'s self-signed certificate. If you are running linux you will have to do this step manually. If you are running Windows or MacOS this has likely already been done for you by `dotnet`.

1. Copy the file `.env.sample` at the project root and rename it to `.env`
1. You can read [this page](https://docs.microsoft.com/en-us/aspnet/core/security/enforcing-ssl?view=aspnetcore-3.1&tabs=netcore-cli#trust-https-certificate-on-linux) in Microsoft's documentation for details about what values are needed for `ASPNETCORE_Kestrel__Certificates__Default__Password` and `ASPNETCORE_Kestrel__Certificates__Default__Path` but **TL;DR: Run the following command:**
    ```sh
    dotnet dev-certs https -ep <SOME_PATH_ON_YOUR_SYSTEM>/aspnetapp.pfx -p <SOME_PASSWORD>
    ```
    - It's recommended that you place the .pfx somewhere like `$HOME/.aspnet/https/aspnetapp.pfx`
1. Set `ASPNETCORE_Kestrel__Certificates__Default__Path` to `<SOME_PATH_ON_YOUR_SYSTEM>/aspnetapp.pfx`
1. Set `ASPNETCORE_Kestrel__Certificates__Default__Password` to `<SOME_PASSWORD>`
1. This is a one-time step you need to do to set up your machine, so that the docker container uses the same certificate as your local instance of `dotnet`.

### Running the project (docker)

If you just need a running API (say, for developing the frontend project), then you can run the API component in Docker (through docker-compose).

1. Ensure you are in the project root directory (i.e. not in `src/api/PetGame.Web`)
1. Edit `appsettings.json` to include your Auth0 information (Note: NOT your client secret, that comes next)
1. Copy the file `_secrets.sample.json` and name it `_secrets.docker.json`
1. Fill in all the values in `_secrets.docker.json`
    - Auth0 client secret can be fetched from Auth0 dashboard
    - Put a connection string for `DATABASE_URL`. If you are running a local docker instance (e.g. devdb) then fill in the credentials you set when you _first ran_ the container (the credentials are NOT updated when you run the container again). **Put `devdb` for the host if you are running devdb in docker-compose, NOT `localhost`.**
    - Delete the `PORT` variable - it is set by `docker-compose.yml`
    - Leave `HOST_PROTOCOL` alone unless you have some need to change it (Note that the app won't function very well without HTTPS)
    - You can edit `appsettings.docker.json` too if you want, but you shouldn't need to change anything in there
1. Ensure the database is up-to-date. Easiest way to do this is to run through the `local` setup above up to configure a `local` connection string and run:
    ```sh
    dotnet ef database update
    ```
1. You should be good to go. Now you can run the project with `docker-compose`. However, you need to make a choice:
  - If you have made no changes to the API since you last ran it in docker (or if you've _never_ run it in docker), you can simply run:
    ```sh
    docker-compose up -d api
    ```
  - If you have made some changes, you will need to rebuild the docker image:
    ```sh
    docker-compose up --build -d api
    ```
      - Note: Don't run `--build` every time as it will build and tag and fill up your system with identical images.

## Deploying the API to a cloud environment

The API component can also run in an AWS cloud environment, on Lambda functions i.e. "Serverless". In order to deploy the API to a cloud environment:

1. Ensure the environment exists. Run Terraform to make sure the infrastructure exists / is up to date. See the [terraform documentation](../../terraform/README.md) for more details on this.

1. (From project root) Run the script `./builds/scripts/build-deploy-api.sh [lambda_function_name]` with the appropriate lambda function name to build and deploy the API component to an existing environment. You can get the api lambda function name out of Terraform's outputs, which you can view with:
    ```sh
    ./builds/scripts/terraform-show.sh
    ```

## Migrating a database hosted in a cloud environment

We also need to ensure the database in our cloud environment is running on the correct schema version, or else the api won't work.

When running `dotnet ef database update` we need to make sure we are using the right connection string for the database we are targeting. Because you rarely have a production environment's connection string laying around in a `_secrets.___.json` config file, you'll usually need to specify `DATABASE_URL` manually. You can do this by setting an environment variable called `DATABASE_URL` to a connection string pointing to your database. Note that the value of this connection string is highly sensitive, so you should be very careful with it!

A convenience script has been written to do all this for you. You just need to pass the connection string in as a parameter, like so (make sure you are in the project root):

```sh
./builds/scripts/migrate-database.sh [CONNECTION STRING]
```

This will set the environment variable just for the scope of the migration and unset it when you are done.

## Work backlog / TODO
 - Remove pet-game Auth0 config from `appsettings.json`