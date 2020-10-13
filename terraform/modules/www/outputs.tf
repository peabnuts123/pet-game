# Name of the S3 bucket, so we can upload our files to it after provisioning
output "s3_bucket_name" {
  value = aws_s3_bucket.www.bucket
}

# Domain name of the S3 bucket, to point cloudfront at
output "s3_bucket_regional_domain_name" {
  value = aws_s3_bucket.www.bucket_regional_domain_name
}
