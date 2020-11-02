#!/usr/bin/env bash

set -e;

# Prerequisites
#   - Terraform CLI

# Validate arguments
if [ -z "$1" ]
then
  echo "No environment id specified."
  echo "Usage: $0 (environment_id)"
  exit 1
fi

# Arguments
environment_id="${1}";

function exit_with_message_and_code() {
  code="$1";
  message="$2";
  echo "Error: ${message}";
  exit "$code";
}

cd terraform || exit_with_message_and_code 1 "Must run $0 from project root.";

terraform apply -var-file "env_${environment_id}.tfvars";
