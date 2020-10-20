# RESOURCES
# S3 Bucket
resource "aws_s3_bucket" "www" {
  bucket = local.s3_bucket_name
  acl    = "private"

  # AWS tags
  tags = {
    project = var.project_id
    environment = var.environment_id
  }
}
