locals {
  lambda_name            = "${var.project_id}_${var.environment_id}_www-proxy"
  lambda_log_group_name  = "/aws/lambda/${local.lambda_name}"
  parameter_store_prefix = "/${var.project_id}/${var.environment_id}/www-proxy"
}
