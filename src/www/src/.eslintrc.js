/* eslint-env node */

// Simple constants for configuring rules
const DISABLED = 'off';
// const WARNING = ['warn'];
const ERROR = ['error'];

// @NOTE implicitly extends from the config in ../.eslintrc.js
module.exports = {
  env: {
    node: false,
    browser: true,
  },
  plugins: [
    'react',
    'react-hooks',
  ],
  extends: [
    'plugin:react/recommended',
    'plugin:react-hooks/recommended',
  ],
  parserOptions: {
    ecmaFeatures: {
      jsx: true,
    },
  },
  rules: {
    // Rules that are explicitly disabled
    'react/prop-types': DISABLED,

    // Rules that are explicitly a warning
    // - none at present -

    // Rules that are explicitly an error
    'react/no-unknown-property': ['error', { ignore: ['class'] }],
    '@typescript-eslint/no-var-requires': ERROR,
  },
  settings: {
    react: {
      pragma: 'h',
      version: 'detect',
    },
  },
};
