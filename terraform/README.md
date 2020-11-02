# Terraform / cloud infrastructure

This folder contains all the infrastructure code to run the entire environment. One can easily spin-up an  environment by providing some config in a `.tfvars` file and running `terraform apply`.

## Architecture

Currently, the code for all of the infrastructure is in one place. Each component has its own module and takes in the dependencies it needs as variables, all of which is architected from `main.tf`:

```md
# Environment entrypoint (the thing your domain name points at)
  - cloudfront.tf
  - cert.tf
  - etc.

# Modules / components
  - main.tf
    - modules/api
      - api-gateway.tf
      - cloudwatch.tf
      - etc.
    - modules/db
      - heroku.tf
      - etc.
    - etc.
```

In the future, the module code will likely move into each component, along with their deployment scripts, especially as they are split into separate repositories. However, orchestrating all these separate pieces will become more and more difficult as the code becomes more separated. These problems of scale are left to be solved only when they are needed to be solved, and so all the code will live in a central location for now.

## Deploying an environment

Since Terraform is designed to be idempotent, you can use the same process to create a new environment or update an existing one.

### Prerequisites

You need to have / know a few things before you can create an environment:
  - The Access/Secret Key of an AWS user with the following permissions:
    - AWSLambdaFullAccess
    - IAMFullAccess
    - AmazonS3FullAccess
    - CloudFrontFullAccess
    - CloudWatchLogsFullAccess
    - AmazonAPIGatewayAdministrator
    - AmazonSSMFullAccess
    - AWSKeyManagementServicePowerUser
    - AWSCertificateManagerFullAccess
  - The API key and email for a Heroku account
  - An Auth0 account with a Regular Web Application set up
    - You will need the Client Secret, in particular
  - A domain set up (and active) in ACM
    - TL;DR:
    1. Add your domain to ACM
    1. Add a CNAME record to your domain with the name/value that ACM gives you
    1. Wait for it to validate
    1. Now you are good to go
    - You will use this domain in the config e.g. `pet-game.winsauce.com`

### Deploying

Once you have everything you need, you can do the following to create the infrastructure for an environment:

1. Copy the file `example.tfvars` and name it `env_[ENVIRONMENT NAME].tfvars` - name it after your environment e.g. `env_dev.tfvars`.
    - The name of this file matters if you are going to use the helper script `terraform-apply.sh` (discussed later).
    - If you don't care to use `terraform-apply.sh` then you can name it whatever you like
1. Fill in all the values in the new file. Some are already provided for you. You must provide every value.
1. Now you are good to go, you have two options:
    1. Run `terraform apply -var-file [YOUR VAR FILE].tfvars` (whatever you named your `.tfvars` file)
    1. (From the project root) Run `./builds/scripts/terraform-apply.sh [ENVIRONMENT NAME]`. This will use the var file `env_[ENVIRONMENT NAME].tfvars` (e.g. `env_dev.tfvars` if you put `dev`, etc.)
1. Terraform will do a bunch of processing and then provide you with a plan. You will be asked if you want to continue; type 'yes' and hit enter. It will take a while to spin up all the infrastructure it needs (the first time), but eventually your environment will finish creating, and you will be presented with some outputs
1. You've just created the skeleton infrastructure needed to run your environment - now you need to deploy the code for each component 🙂

## Deploying components' code

Each component has code that needs to be deployed into the environment as a separate step. There are scripts in the `builds/scripts/` directory for deploying each of these, and the documentation for deploying the code for each component lives in each respective README.

Generally speaking, these scripts just need the ID of the infrastructure component they are deploying to e.g. the name S3 bucket, or the name Lambda function. These come from the Terraform outputs, and can be viewed by running `./builds/scripts/terraform-show.sh` (from the project root).

## Backlog / TODO

  - Update project_id/environment_id validation to remove underscores
  - Probably redo the whole "common variable" thing and just use the variables that are needed
  - Document that you need an AWS CLI profile named `petgame`
  - For that matter, document build/deploy scripts dependencies
  - Add `terraform init` to documentation
  - Add "Comment" to each Cloudfront to describe what the heck it is