output "invoke_url" {
  # @TODO can we get the URL better here? Can we use the ARN or something?
  value       = trim(aws_apigatewayv2_stage.default.invoke_url, "https://")
  description = "URL for invoking / accessing the API through API gateway"
}

output "lambda_function_name" {
  value = aws_lambda_function.api.function_name
  description = "Lambda function name, for deploying code. Use `aws lambda update-function-code --function-name <LAMBDA_FUNCTION_NAME> --zip-file fileb://<API_CODE_PACKAGE>.zip` to deploy."
}