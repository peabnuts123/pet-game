# PetGame DevDB

This is a PostgreSQL docker container for developing locally, it's purely a convenient alternative to having to host a PostgreSQL DB somewhere; though you could develop against a hosted instance (say, in Heroku) if you wanted.

This directory only exists to house the `.env` config for the DB (i.e. define the credentials) and to store its data (so that it is persisted between runs).

## Docker

### Prerequisites (docker)

  - Naturally, you need [docker](https://docs.docker.com/get-docker/) and [docker-compose](https://docs.docker.com/compose/install/) to run this docker container

### Running the db (docker)

You can read about the postgres docker image in  detail [on DockerHub](https://hub.docker.com/_/postgres), but here is the TL;DR:

1. Copy the file `.env.sample` and rename it to `.env`
1. Ensure all the values are filled in e.g.
    ```properties
    POSTGRES_USER=petgameuser
    POSTGRES_PASSWORD=mariomario
    POSTGRES_DB=petgame
    ```
1. Run the following command:
    ```sh
    docker-compose up -d devdb
    ```

Then you can make a connection string for the API component's config like:

```
postgres://petgameuser:mariomario@localhost:5432/petgame
```

Note that if you are running the API in docker too then the hostname will be `devdb` and not `localhost`.