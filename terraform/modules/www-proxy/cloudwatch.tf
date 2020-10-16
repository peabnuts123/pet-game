resource "aws_cloudwatch_log_group" "lambda_www_proxy" {
  name              = local.lambda_log_group_name
  retention_in_days = 14

  tags = {
    project = var.project_id
    environment = var.environment_id
  }
}
