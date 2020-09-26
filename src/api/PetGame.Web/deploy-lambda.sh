#!/usr/bin/env bash

set -e;

rm -rf bin/Release;
dotnet publish --configuration Release;
cd bin/Release/netcoreapp3.1/publish || exit 1;
zip -r api.zip .;
aws lambda update-function-code --function-name pet-game-api --zip-file fileb://api.zip --profile 'petgame';
