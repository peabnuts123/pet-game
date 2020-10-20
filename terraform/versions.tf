terraform {
  required_providers {
    aws = {
      source  = "hashicorp/aws"
      version = "~> 3.0"
    }
    heroku = {
      source  = "heroku/heroku"
      version = "~> 2.6"
    }
  }
  required_version = ">= 0.13"
}