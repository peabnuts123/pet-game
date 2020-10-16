# OUTPUTS

output "www_bucket_name" {
  value       = module.www.s3_bucket_name
  description = "S3 bucket name, for uploading www files to. Use `aws s3 sync <DEPLOY_FOLDER> s3://<BUCKET_NAME> --acl 'private'` to deploy"
}

# Output the cloudfront domain, so we know how to access the distribution
#   We will also need this to add as a CNAME DNS record for our domain
output "cloudfront_domain_name" {
  value       = aws_cloudfront_distribution.www.domain_name
  description = "CloudFront distribution domain name. Add DNS record `CNAME <DOMAIN_NAME> <CLOUDFRONT_DOMAIN_NAME>` to finish configuration"
}

output "api_lambda_function_name" {
  value = module.api.lambda_function_name
  description = "Lambda function name, for deploying api component to. Use `aws lambda update-function-code --function-name <LAMBDA_FUNCTION_NAME> --zip-file fileb://<API_CODE_PACKAGE>.zip` to deploy."
}

output "www-proxy_lambda_function_name" {
  value = module.www_proxy.lambda_function_name
  description = "Lambda function name, for deploying api component to. Use `aws lambda update-function-code --function-name <LAMBDA_FUNCTION_NAME> --zip-file fileb://<API_CODE_PACKAGE>.zip` to deploy."
}