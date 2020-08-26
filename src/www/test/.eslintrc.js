// Simple constants for configuring rules
const DISABLED = 'off';
const WARNING = ['warn'];
const ERROR = ['error'];

// Lint config for test project
module.exports = {
  env: {
    node: false,
    browser: true,
    jest: true,
  },
  plugins: [
    'react',
  ],
  extends: [
    'plugin:react/recommended',
  ],
  parserOptions: {
    ecmaFeatures: {
      jsx: true
    },
  },
  rules: {
    // Rules that are explicitly disabled
    '@typescript-eslint/explicit-function-return-type': DISABLED,

    // Rules that are explicitly a warning
    // - none at present -

    // Rules that are explicitly an error
    // - none at present -
  },
  settings: {
    react: {
      pragma: 'h',
      version: 'detect'
    },
  },
};
