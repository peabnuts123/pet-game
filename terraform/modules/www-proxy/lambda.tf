resource "aws_lambda_function" "www_proxy" {
  function_name = local.lambda_name
  filename      = var.code_package_file_path
  description   = "www-proxy component for ${var.project_id}"
  role          = aws_iam_role.lambda.arn
  handler       = "index.handler"
  runtime       = "nodejs12.x"
  memory_size   = 256
  timeout       = 3

  tags = {
    project     = var.project_id
    environment = var.environment_id
  }

  environment {
    variables = {
      NODE_ENV = "production"
      ENVIRONMENT_ID = var.environment_id
    }
  }
}

resource "aws_lambda_permission" "www_proxy" {
  function_name = aws_lambda_function.www_proxy.function_name
  action        = "lambda:InvokeFunction"
  principal     = "apigateway.amazonaws.com"

  # The /*/*/* part allows invocation from any stage, method and resource path
  # within API Gateway REST API.
  source_arn = "${aws_apigatewayv2_api.www_proxy.execution_arn}/*/*/*"
}
