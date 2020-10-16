import fs from 'fs';
import path from 'path';

import RouteMap from '@app/route-map';
import Logger from '@app/util/Logger';

/**
 * PURPOSE
 * Generate a .json file containing all valid routes in the app,
 * for consumption by www-proxy. This is so the proxy can return 404s for
 * paths that are not valid routes in the app.
 * This artifact is copied to the S3 bucket on build/deploy, and subsequently
 * read by www-proxy when serving requests.
 */

// Config
const OUTPUT_FILE_NAME = 'route-map.json';
const OUTPUT_PATH = path.join(__dirname, OUTPUT_FILE_NAME);

const allRoutes = RouteMap
  // Remove not-found i.e. "Default" route
  .filter((route) => !route.default)
  // Remove any routes that have no path configured
  .filter((route) => route.path !== undefined)
  // Map to route path
  .map((route) => route.path as string)
  // Replace path variables with *
  .map((path) => path.replace(/:[^/]+/g, '*'));

Logger.log(`Writing route map...`);

fs.writeFileSync(OUTPUT_PATH, JSON.stringify(allRoutes));

Logger.log(`Successfully wrote route map to ${OUTPUT_PATH}`);
