#!/usr/bin/env bash

set -e;

# Prerequisites
#   - Terraform CLI

function exit_with_message_and_code() {
  code="$1";
  message="$2";
  echo "Error: ${message}";
  exit "$code";
}

cd terraform || exit_with_message_and_code 1 "Must run $0 from project root.";

terraform show | tail -n 6;
