#!/usr/bin/env bash

set -e;

# Prerequisites
#   - Node.js and npm
#   - AWS CLI

# Validate arguments
if [ -z "$1" ]
then
  echo "No S3 bucket name specified."
  echo "Usage: $0 (s3_bucket_name)"
  exit 1
fi

# Arguments
s3_bucket_name="${1}";

# Environment
export NODE_ENV='production';

function exit_with_message_and_code() {
  code="$1";
  message="$2";
  echo "Error: ${message}";
  exit "$code";
}

cd src/www || exit_with_message_and_code 1 "Must run $0 from project root.";

npm install;
npm run build;
cd build || exit_with_message_and_code 2 "Can't find build directory";
aws s3 sync . "s3://${s3_bucket_name}" --acl 'private' --profile 'petgame'

echo "Successfully deployed www";
