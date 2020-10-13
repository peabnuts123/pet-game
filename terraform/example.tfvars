# Copy or rename this file to `env_[ENVIRONMENT_NAME].tfvars` and fill in the values below
# e.g. `cp example.tfvars env_dev.tfvars`
# Don't forget to pass -var-file="env_dev.tfvars" when running terraform

# Access config
aws_access_key       = ""
aws_secret_key       = ""
heroku_account_email = ""
heroku_api_key       = ""

# General
# AWS region to create resources in (unless not available)
aws_region = "ap-southeast-2"
# URL used for this environment.
# @NOTE will need to be MANUALLY set up in ACM for HTTPS
domain_name = ""
# Unique identifier for this project. All resources will be tagged with this id. Also used for naming resources.
# Must be a simple A-Z0-9 string with optional dashes (-) or underscores (_)
# e.g. "pet-game"
project_id = "pet-game"
# Unique environment identifier. All resources will be tagged with this id. Also used for naming resources.
# Must be a simple A-Z0-9 string with optional dashes (-) or underscores (_)
# e.g. dev
environment_id = ""
# Natural language name for environment
# e.g. Development
environment_name = ""
# Auth0 Client Secret
auth0_client_secret = ""
