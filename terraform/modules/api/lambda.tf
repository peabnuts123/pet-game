resource "aws_lambda_function" "api" {
  function_name = local.lambda_name
  filename      = var.code_package_file_path
  description   = "API component for ${var.project_id}"
  role          = aws_iam_role.lambda.arn
  handler       = "PetGame.Web::PetGame.Web.LambdaEntryPoint::FunctionHandlerAsync"
  runtime       = "dotnetcore3.1"
  memory_size   = 512
  timeout       = 29

  tags = {
    project     = var.project_id
    environment = var.environment_id
  }

  environment {
    variables = {
      ASPNETCORE_ENVIRONMENT = "Production"
      ENVIRONMENT_ID = var.environment_id
    }
  }
}

resource "aws_lambda_permission" "api" {
  function_name = aws_lambda_function.api.function_name
  action        = "lambda:InvokeFunction"
  principal     = "apigateway.amazonaws.com"

  # The /*/*/* part allows invocation from any stage, method and resource path
  # within API Gateway REST API.
  source_arn = "${aws_apigatewayv2_api.default.execution_arn}/*/*/*"
}
