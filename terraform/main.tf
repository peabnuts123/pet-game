# DATA
data "aws_caller_identity" "current" {}

# PROVIDERS
# Configure the Heroku provider
provider "heroku" {
  email   = var.heroku_account_email
  api_key = var.heroku_api_key
}

provider "aws" {
  region     = var.aws_region
  access_key = var.aws_access_key
  secret_key = var.aws_secret_key
}

provider "aws" {
  alias      = "us_east_1"
  region     = "us-east-1"
  access_key = var.aws_access_key
  secret_key = var.aws_secret_key
}


# MODULES
module "db" {
  source = "./modules/db"

  # Common
  aws_region       = var.aws_region
  domain_name      = var.domain_name
  project_id       = var.project_id
  environment_id   = var.environment_id
  environment_name = var.environment_name
  aws_account_id   = data.aws_caller_identity.current.account_id
}
module "www" {
  source = "./modules/www"

  # Common
  aws_region       = var.aws_region
  domain_name      = var.domain_name
  project_id       = var.project_id
  environment_id   = var.environment_id
  environment_name = var.environment_name
  aws_account_id   = data.aws_caller_identity.current.account_id
}
module "api" {
  source = "./modules/api"

  # Common
  aws_region       = var.aws_region
  domain_name      = var.domain_name
  project_id       = var.project_id
  environment_id   = var.environment_id
  environment_name = var.environment_name
  aws_account_id   = data.aws_caller_identity.current.account_id

  # Unique
  database_url           = module.db.database_url
  auth0_client_secret    = var.auth0_client_secret
  code_package_file_path = data.archive_file.empty.output_path
}
module "www_proxy" {
  source = "./modules/www-proxy"

  # Common
  aws_region       = var.aws_region
  domain_name      = var.domain_name
  project_id       = var.project_id
  environment_id   = var.environment_id
  environment_name = var.environment_name
  aws_account_id   = data.aws_caller_identity.current.account_id

  www_bucket_name = module.www.s3_bucket_name
  code_package_file_path = data.archive_file.empty.output_path
}

