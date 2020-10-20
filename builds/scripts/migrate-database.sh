#!/usr/bin/env bash

set -e;

# Prerequisites
#   - .NET Core CLI


# Install EF tools
if ! (hash dotnet-ef &> /dev/null); then
  echo "Installing dotnet-ef..."
  dotnet tool install --global dotnet-ef;
fi

# Validate arguments
if [ -z "$1" ]
then
  echo "No connection string specified.";
  echo "Usage: $0 (connection_string)";
  exit 1;
fi

# Arguments
# @NOTE shellcheck complains because this isn't used, but it is used by sub-process when migrating
export DATABASE_URL="${1}";

function exit_with_message_and_code() {
  code="$1";
  message="$2";
  echo "Error: ${message}";
  exit "$code";
}

cd src/api/PetGame.Web || exit_with_message_and_code 1 "Must run $0 from project root.";

dotnet ef database update;

echo "Successfully migrated database";
