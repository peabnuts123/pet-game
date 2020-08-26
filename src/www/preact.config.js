import path from "path";

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
    });
  },
};
