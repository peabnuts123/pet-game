resource "heroku_app" "db" {
  name   = "${var.project_id}-${var.environment_id}-db"
  region = "us"
}

resource "heroku_addon" "db" {
  app  = heroku_app.db.name
  plan = "heroku-postgresql:hobby-dev"
}
