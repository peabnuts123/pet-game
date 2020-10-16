resource "aws_ssm_parameter" "route_map_key" {
  name        = "${local.parameter_store_prefix}/Configuration/RouteMapKey"
  description = "File name for route map in www s3 bucket"
  type        = "String"
  value       = "route-map.json" # @TODO pass this as a variable to www

  tags = {
    project = var.project_id
    environment = var.environment_id
  }
}

resource "aws_ssm_parameter" "www_bucket_name" {
  name        = "${local.parameter_store_prefix}/Configuration/WWWBucketName"
  description = "S3 bucket name for WWW assets"
  type        = "String"
  value       = var.www_bucket_name

  tags = {
    project = var.project_id
    environment = var.environment_id
  }
}
