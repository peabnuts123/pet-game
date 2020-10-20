#!/usr/bin/env sh

set -eu

# Clear out / install node_modules
npm i;

# Compile app
npm run build;

# Run compiled app
./node_modules/.bin/serve -l 8080 -s --no-clipboard build;
