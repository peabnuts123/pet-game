# Variables (unique to this module)

variable "database_url" {
  description = "URL connection string for the database e.g. postgres://[user]:[password]@[host]/[database]"
  type        = string
}

variable "auth0_client_secret" {
  description = "Client Secret for Auth0"
  type        = string
}

variable "code_package_file_path" {
  description = "Path to a zip file containing the compiled code for the API"
  type        = string
}
