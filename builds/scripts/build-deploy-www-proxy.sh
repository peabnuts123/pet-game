#!/usr/bin/env bash

set -e;

# Prerequisites
#   - Node.js and npm
#   - zip command-line utility
#   - AWS CLI

# Validate arguments
if [ -z "$1" ]
then
  echo "No lambda function name specified."
  echo "Usage: $0 (lambda_function_name)"
  exit 1
fi

# Arguments
lambda_function_name="${1}";
zip_file_name="www-proxy.zip";
NODE_ENV='production';


function exit_with_message_and_code() {
  code="$1";
  message="$2";
  echo "Error: ${message}";
  exit "$code";
}

cd src/www-proxy || exit_with_message_and_code 1 "Must run $0 from project root.";

npm install;
npm run build;
cp package*.json build
cd build || exit_with_message_and_code 2 "Can't find build directory";
npm ci --only production
zip -r "${zip_file_name}" .;
aws lambda update-function-code --function-name "${lambda_function_name}" --zip-file "fileb://${zip_file_name}" --profile 'petgame';
rm "${zip_file_name}";

echo "Successfully deployed www-proxy";
