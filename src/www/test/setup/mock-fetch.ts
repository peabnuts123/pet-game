import fetchMock, { enableFetchMocks, disableFetchMocks } from 'jest-fetch-mock';

export const NotMockedError = new Error('Fetch has not been mocked');

/**
 * Mock 'fetch' calls.
 * This allows tests to control responses from external dependencies such as the API.
 * By default, fetch requests are mocked to fail so that (hopefully) any test accidentally
 * making a real network call will fail.
 */

// Turn mocks off / on between tests so that they don't bleed into other tests
beforeEach(() => {
  enableFetchMocks();
  fetchMock.mockResponse(() => fail(NotMockedError));
});

afterEach(() => {
  disableFetchMocks();
});
