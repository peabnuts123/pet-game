locals {
  www_origin_id = "${var.project_id}_${var.environment_id}_www"
  api_origin_id = "${var.project_id}_${var.environment_id}_api"
}