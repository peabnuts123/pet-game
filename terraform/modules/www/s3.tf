# RESOURCES
# S3 Bucket
resource "aws_s3_bucket" "www" {
  bucket = local.s3_bucket_name
  acl    = "private"
  policy = <<-POLICY
  {
    "Version": "2008-10-17",
    "Id": "PolicyForCloudFrontPrivateContent",
    "Statement": [
      {
        "Sid": "1",
        "Effect": "Allow",
        "Principal": {
          "AWS": "${var.cloudfront_origin_access_identity_arn}"
        },
        "Action": "s3:GetObject",
        "Resource": "arn:aws:s3:::${local.s3_bucket_name}/*"
      }
    ]
  }
  POLICY

  # AWS tags
  tags = {
    project = var.project_id
    environment = var.environment_id
  }

  # Set up static site hosting in S3
  website {
    index_document = "index.html"
    error_document = "index.html"
  }
}
