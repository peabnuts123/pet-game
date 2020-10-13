import fetchMock from 'jest-fetch-mock';
import { MockResponseInit } from "jest-fetch-mock/types";

interface CustomMockResponseInit extends MockResponseInit {
  /**
   * JSON response - will be stringified and `Content-Type` header will be set to `application/json`.
   * Can only use one of: `json`, `text` or `body`.
   */
  json?: any;
  /**
   * Plain text response - will set `Content-Type` header to `text/plain`.
   * Can only use one of: `json`, `text` or `body`.
   */
  text?: string;
}

interface MockRequestDefinition {
  /** Test what requests this should mock */
  test?: (url: string) => boolean;
  /** Mock response */
  response: CustomMockResponseInit | string;
}

/**
 * Utility function for configuring `fetchMock` to mock one or more requests.
 * Calling this without any parameters will put a simple mock in-place for all requests.
 * Calling this with a string will return that response for all requests.
 * Pass an array of MockRequestDefinition objects to mock multiple different endpoints at once.
 * If multiple endpoints are mocked then any request that does not hit one of these endpoints will
 * fail.
 * If a MockRequestDefinition has no `test` property then it will always match.
 *
 * @param mockRequestDefinitions Definitions for request mocks
 */
export default function mockRequests(mockRequestDefinitions?: MockRequestDefinition[] | string): void {
  if (mockRequestDefinitions === undefined) {
    // Param is empty
    fetchMock.mockResponse('');
  } else if (typeof mockRequestDefinitions === 'string') {
    // Param is just a string
    fetchMock.mockResponse(mockRequestDefinitions);
  } else {
    // Param is an array of mock request definitions
    fetchMock.mockResponse((request) => {
      const mockRequestDefinition = mockRequestDefinitions.find((mockRequestDefinition) => mockRequestDefinition.test === undefined || mockRequestDefinition.test(request.url));

      if (mockRequestDefinition) {
        const response: CustomMockResponseInit | string = mockRequestDefinition.response;

        // Check if response is an object (i.e. it is not a string)
        if (typeof response !== 'string') {
          // Custom properties logic
          if (response.json) {
            // Convert JSON and set content type
            response.body = JSON.stringify(response.json);
            response.headers = response.headers || {};
            (response.headers as Record<string, string>)['Content-Type'] = 'application/json';
          } else if (response.text) {
            // Set text body and content type
            response.body = response.text;
            response.headers = response.headers || {};
            (response.headers as Record<string, string>)['Content-Type'] = 'text/plain';
          }

          // Ensure custom properties are not sent
          delete response.json;
          delete response.text;
        }

        return Promise.resolve(response);
      } else {
        throw new Error(`Endpoint not mocked: ${request.url}`);
      }
    });
  }
}
