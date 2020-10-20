# VARIABLES
variable "aws_region" {
  description = "AWS Region to create resources in e.g. us-east-1"
  type        = string
}
variable "aws_account_id" {
  description = "ID of the current AWS account"
  type        = string
}
variable "domain_name" {
  description = "URL for HTTPS certificate domain"
  type        = string
}
variable "project_id" {
  description = "Unique simple identifier for project. Must only use A-Z, 0-9, - or _ characters e.g. \"pet-game\""
  type        = string

  validation {
    condition     = can(regex("^[A-Za-z0-9-_]+$", var.project_id))
    error_message = "Variable `project_id` must only be characters A-Z (or a-z), 0-9, hypen (-) or underscore (_)."
  }
}
variable "environment_id" {
  description = "Unique simple identifier for environment. Must only use A-Z, 0-9, - or _ characters e.g. \"dev\" or \"test-2\""
  type        = string

  validation {
    condition     = can(regex("^[A-Za-z0-9-_]+$", var.environment_id))
    error_message = "Variable `environment_id` must only be characters A-Z (or a-z), 0-9, hypen (-) or underscore (_)."
  }
}
variable "environment_name" {
  description = "Natural langauge name for the environment e.g. \"Development\" or \"Production\""
  type        = string
}
