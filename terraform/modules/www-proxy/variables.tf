# Variables (unique to this module)

variable "www_bucket_name" {
  description = "S3 bucket name for WWW assets"
  type        = string
}

variable "code_package_file_path" {
  description = "Path to a zip file containing the compiled code for the API"
  type        = string
}
