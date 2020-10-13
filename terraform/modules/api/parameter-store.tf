resource "aws_ssm_parameter" "auth0_client_secret" {
  name        = "${local.parameter_store_prefix}/Configuration/Auth0/ClientSecret"
  description = "Auth0 Client Secret, needed for the API to communicate with Auth0"
  type        = "SecureString"
  value       = var.auth0_client_secret

  tags = {
    project = var.project_id
    environment = var.environment_id
  }
}

resource "aws_ssm_parameter" "database_url" {
  name        = "${local.parameter_store_prefix}/Configuration/DATABASE_URL"
  description = "URI connection string for connecting to database. Must be format of: postgres://[user]:[password]@[host]/[database]"
  type        = "SecureString"
  value       = var.database_url

  tags = {
    project = var.project_id
    environment = var.environment_id
  }
}

resource "aws_ssm_parameter" "www_url" {
  name        = "${local.parameter_store_prefix}/Configuration/WebClient/AbsoluteUrl"
  description = "URL address that the web client is available on"
  type        = "String"
  value       = "https://${var.domain_name}"

  tags = {
    project = var.project_id
    environment = var.environment_id
  }
}
