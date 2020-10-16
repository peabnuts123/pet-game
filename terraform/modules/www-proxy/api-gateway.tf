# API Gateway
resource "aws_apigatewayv2_api" "www_proxy" {
  name          = "Pet Game - www-proxy (${var.environment_id})"
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
  api_id      = aws_apigatewayv2_api.www_proxy.id
  name        = "$default"
  auto_deploy = true
}

# Integrations
# API / Lambda integration
resource "aws_apigatewayv2_integration" "www_proxy" {
  api_id      = aws_apigatewayv2_api.www_proxy.id
  description = "Proxy to www-proxy Lambda"

  integration_type       = "AWS_PROXY"
  connection_type        = "INTERNET"
  integration_method     = "POST"
  integration_uri        = aws_lambda_function.www_proxy.invoke_arn
  payload_format_version = "2.0"
}

# Routes
resource "aws_apigatewayv2_route" "www_proxy" {
  api_id    = aws_apigatewayv2_api.www_proxy.id
  route_key = "ANY /{proxy+}"
  target    = "integrations/${aws_apigatewayv2_integration.www_proxy.id}"
}
