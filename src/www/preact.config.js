import path from "path";
import { DefinePlugin } from 'webpack';

export default {
  /**
   * Function that mutates the original webpack config.
   * Supports asynchronous changes when a promise is returned (or it's an async function).
   *
   * @param {object} config - original webpack config.
   * @param {object} env - options passed to the CLI.
   * @param {WebpackConfigHelpers} helpers - object with useful helpers for working with the webpack config.
   * @param {object} options - this is mainly relevant for plugins (will always be empty in the config), default to an empty object
   **/
  webpack(config, _env, _helpers, _options) {
    // Alias import paths
    config.resolve = config.resolve || {};
    config.resolve.alias = config.resolve.alias || {};
    Object.assign(config.resolve.alias, {
      // `@app` resolves the project root
      "@app": path.resolve(__dirname, 'src/'),

      // Set entrypoint to src/index
      "preact-cli-entrypoint": path.resolve(
        process.cwd(),
        "src",
        "index",
      ),

      mobx: path.resolve(__dirname + "/node_modules/mobx/lib/mobx.es6.js"),
    });

    // Define these variables as globals in resulting JavaScript
    config.plugins.push(new DefinePlugin({
      // For some god awful reason the values need to be converted to valid code
      //  i.e. if the value is "world" it will plop the string "world" into your code
      //  where you reference the variable. So this must be converted to `'world'` i.e. a string
      //  within a string, so that the resulting code receives a string

      // @TODO BETTER environment config
      API_BASE: JSON.stringify(process.env.NODE_ENV === 'development' ? `https://localhost:5000` : `https://pet-game-dev.herokuapp.com`),
    }));
  },
};
