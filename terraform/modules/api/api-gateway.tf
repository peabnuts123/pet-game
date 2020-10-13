# API Gateway
resource "aws_apigatewayv2_api" "default" {
  name          = "Pet Game (${var.environment_id})"
  description   = "The Pet Game ${var.environment_name} environment"
  protocol_type = "HTTP"

  tags = {
    project = var.project_id
    environment = var.environment_id
  }
}

# Default stage, auto-deploy
# @NOTE no need for other stages, as separate environments will be
#   entirely separate deployments of this whole infrastructure.
resource "aws_apigatewayv2_stage" "default" {
  api_id      = aws_apigatewayv2_api.default.id
  name        = "$default"
  auto_deploy = true
}

# Integrations
# API / Lambda integration
resource "aws_apigatewayv2_integration" "api" {
  api_id      = aws_apigatewayv2_api.default.id
  description = "Proxy to API Lambda"

  integration_type       = "AWS_PROXY"
  connection_type        = "INTERNET"
  integration_method     = "POST"
  integration_uri        = aws_lambda_function.api.invoke_arn
  payload_format_version = "1.0"
}

# Routes
resource "aws_apigatewayv2_route" "api" {
  api_id    = aws_apigatewayv2_api.default.id
  route_key = "ANY /api/{proxy+}"
  target    = "integrations/${aws_apigatewayv2_integration.api.id}"
}
