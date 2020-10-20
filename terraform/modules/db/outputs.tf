output "database_url" {
  value = heroku_app.db.all_config_vars.DATABASE_URL
}
