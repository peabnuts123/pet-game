// Simple constants for configuring rules
const DISABLED = 'off';
// const WARNING = ['warn'];
const ERROR = ['error'];

module.exports = {
  root: true,
  env: {
    node: true,
    es6: true,
  },
  plugins: ["@typescript-eslint"],
  extends: [
    "eslint:recommended",
    "plugin:@typescript-eslint/eslint-recommended",
    "plugin:@typescript-eslint/recommended",
    "plugin:@typescript-eslint/recommended-requiring-type-checking",
  ],
  parser: "@typescript-eslint/parser",
  parserOptions: {
    ecmaVersion: 2018,
    sourceType: 'module',
    createDefaultProgram: true, // WHY IS THIS SO MUCH WORK JUST TO LINT FILES
    project: 'tsconfig.json',
  },
  rules: {
    // Rules that are explicitly disabled
    '@typescript-eslint/no-explicit-any': DISABLED,
    '@typescript-eslint/no-empty-function': DISABLED,
    '@typescript-eslint/no-inferrable-types': DISABLED,
    '@typescript-eslint/no-use-before-define': DISABLED,
    '@typescript-eslint/no-non-null-assertion': DISABLED,
    '@typescript-eslint/restrict-template-expressions': DISABLED,
    'no-console': DISABLED,

    // Rules that are explicitly a warning
    // - none at present -

    // Rules that are explicitly an error
    'eol-last': ERROR,
    'semi': ERROR,
    'comma-dangle': ['error', 'always-multiline'],
    '@typescript-eslint/explicit-function-return-type': ['error', { allowExpressions: true }],
    '@typescript-eslint/no-unused-vars': ['warn', {
      'argsIgnorePattern': '^_',
      'varsIgnorePattern': '^_',
      'caughtErrorsIgnorePattern': '^_',
     }],
  },
  overrides: [
    // JavaScript-specific rules
    {
      'files': ['*.js', '*.jsx'],
      'rules': {
        '@typescript-eslint/explicit-function-return-type': DISABLED,
        '@typescript-eslint/explicit-module-boundary-types': DISABLED,
        '@typescript-eslint/no-unsafe-member-access': DISABLED,
        '@typescript-eslint/no-unsafe-call': DISABLED,
        '@typescript-eslint/no-var-requires': DISABLED,
        '@typescript-eslint/no-unsafe-assignment': DISABLED,
      },
    },
  ],
};
