const { defaults: TS_JEST } = require('ts-jest/presets');

// See jest docs for config options: https://jestjs.io/docs/en/configuration
module.exports = {
  preset: "jest-preset-preact",
  transform: {
    ...TS_JEST.transform,
  },
  clearMocks: true,
  restoreMocks: true,
  verbose: true,
  testURL: "http://localhost:8080",
  setupFilesAfterEnv: [
    '<rootDir>/test/setup/configure-logger.ts',
    '<rootDir>/test/setup/fail-on-no-assertion.ts',
    '<rootDir>/test/setup/mock-fetch.ts',
  ],
  testRegex: [
    "test/.*\\.test.[jt]sx?$",
  ],
  moduleNameMapper: {
    // === @app
    "^@app/(.*)$": "<rootDir>/src/$1",
    // === @test
    "^@test/(.*)$": "<rootDir>/test/$1",
  },
  coverageDirectory: 'coverage',
  collectCoverageFrom: [
    '<rootDir>/src/**/*.{js,jsx,ts,tsx}',
  ],
};
