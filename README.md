# Pet Game

So you can like adopt pets and stuff. You can play games with your pets and look after them. You can interact with other pet owners on the site too. It's totally an original idea and _absolutely not_ a clone of another popular website.

## TODO for real
  - Replace references to Heroku
  - Rewrite this documentation
    - Document how to deploy this thing
  - Remove un-needed infrastructure in Heroku

## Architecture

The first phase of this project will have the following architecture:
  - A single .NET Core Web API
  - A preact single-page-application frontend
  - An Auth0 Application for OAuth
  - A database for storing data

In the future there will be architectural decisions made to split the app up into manageable chunks, but this will all be in a single chunk for now.

## Basic experience
  - Home page describing WTF you're looking at
  - Profile page
    - User information
    - What items you have + ability to donate to The Taking Tree
  - Login page
  - The Taking Tree
    - Logged out you can view what's there
    - Logged in you can take stuff / maybe you can see your inventory / donate stuff
  - ?Every day you receive 5 inventory items?

## Backlog
  - Design a basic experience
  - Make an API that implements this logic (in-memory persistence is fine)
  - Make a frontend that implements this experience and calls the API
  - Add some shit auth to it to complete the experience
  - Deploy this to Heroku, Netlify
  - Replace the shit auth with Auth0
  - Add database connection and some CRUD actions (replacing in-memory stuff) and deploy
  - ...

## Pile of TODO
  - ? Shitty, hand-rolled auth for fun / dev / testing
    - [See implementation here](https://jasonwatmore.com/post/2019/10/21/aspnet-core-3-basic-authentication-tutorial-with-example-api#:~:text=The%20basic%20authentication%20handler%20is,overriding%20the%20HandleAuthenticateAsync)
    - /login endpoint which gives you a User object if you get username/password right
    - Manual authorization?
  - Auth0 integration
    - Frontend: Add Auth0.js package and call authenticate function, handle callback (look into non-redirect based alternatives) [See also](https://auth0.com/docs/libraries/auth0-single-page-app-sdk/migrate-from-auth0-js-to-the-auth0-single-page-app-sdk) [and also](https://auth0.com/docs/quickstart/spa) [and for preact specifically](https://auth0.com/blog/preact-authentication-tutorial/)
    - Backend: Add Auth0 nuget package and add stuff to startup.cs / appsettings.json. [See also](https://auth0.com/docs/quickstart/webapp/aspnet-core-3)
  - Hosting
    - First instance, host the API in Heroku, and the frontend in Netlify
    - Database can also start out in Heroku, but look into AWS RDS first + other AWS database options
    - Figure out what AWS / "Serverless" infrastructure is good for the API - Lambda? API Gateway?
    - Host static compiled frontend in Cloudfront / S3 combo
    - How can we configure this cloud infrastructure through Terraform / some automated fashion?
  - Deploying
    - Azure pipelines build (configured through YAML, I guess) that deploys on merge into master
  - EFCore / connecting to the database
    - I guess just Heroku DB for the first instance though
    - Can theoretically host a PostgreSQL-compliant Aurora DB in AWS RDS for cents a month even for large amounts of data
    - [Aurora Serverless with Entity Framework Core](https://www.chaseaucoin.com/posts/aurora-serverless-lambda-with-entity-framework-core/#time-to-make-the-donuts)
    - [Using EFcore in AWS Lambda](https://blog.tonysneed.com/2018/12/21/use-ef-core-with-aws-lambda-functions/)
    - [Some AWS docs on using EFCore](https://docs.aws.amazon.com/elasticbeanstalk/latest/dg/create_deploy_NET.rds.html)