import getRequestHeader from '@test/util/get-request-header';

/**
 * Unit tests for getRequestHeader test utility function.
 *
 * Sorry to test the tests, I just need this function for a lot of tests
 * to avoid type laundering, and the function implementation was too
 * complicated for me to feel comfortable with it.
 */

describe("getRequestHeader test utility", () => {
  it("with empty request object returns null", () => {
    // Setup
    const mockRequest = undefined;
    const mockHeaderName = 'x-mock-header';

    // Test
    const actualHeaderValue = getRequestHeader(mockRequest, mockHeaderName);

    // Assert
    expect(actualHeaderValue).toBeNull();
  });

  it("with undefined headers object returns null", () => {
    // Setup
    const mockRequest = {
      headers: undefined,
    };
    const mockHeaderName = 'x-mock-header';

    // Test
    const actualHeaderValue = getRequestHeader(mockRequest, mockHeaderName);

    // Assert
    expect(actualHeaderValue).toBeNull();
  });


  it("with misconfigured headers object throws an error", () => {
    // Setup
    const mockHeaders = 2;
    const mockRequest = {
      headers: mockHeaders,
    };
    const mockHeaderName = 'x-mock-header';

    // Test
    const testFunc = () => {
      // @NOTE intentionally passing the wrong type
      getRequestHeader(mockRequest as any, mockHeaderName);
    };

    // Assert
    expect(testFunc).toThrowError(`Request 'headers' object is not valid type. Got: ${mockHeaders} - (${typeof mockHeaders}). Expected: Headers | string[][] | Record<string, string> | undefined.`);
  });

  describe("with Headers object", () => {
    it("gets correct header", () => {
      // Setup
      const mockHeaderName = 'x-mock-header';
      const mockHeaderValue = 'mock value';
      const request = {
        headers: new Headers(),
      };
      request.headers.set(mockHeaderName, mockHeaderValue);

      // Test / Assert
      runCorrectHeaderTest(request, mockHeaderName, mockHeaderValue);
    });

    it("is case insensitive", () => {
      // Setup
      const mockHeaderName = 'x-mock-header';
      const mockHeaderValue = 'mock value';
      const request = {
        headers: new Headers(),
      };
      request.headers.set(mockHeaderName, mockHeaderValue);

      // Test / Assert
      runCaseSensitivityTest(request, mockHeaderName, mockHeaderValue);
    });

    it("returns null for a header that doesn't exist", () => {
      // Setup
      const mockHeaderName = 'x-non-existent-header';
      const request = {
        headers: new Headers(),
      };

      // Test / Assert
      runMissingHeaderTest(request as Request, mockHeaderName);
    });
  });

  describe("with 2D string array", () => {
    it("gets correct header", () => {
      // Setup
      const mockHeaderName = 'x-mock-header';
      const mockHeaderValue = 'mock value';
      const request: RequestInit = {
        headers: [
          [mockHeaderName, mockHeaderValue],
        ],
      };

      // Test / Assert
      runCorrectHeaderTest(request, mockHeaderName, mockHeaderValue);
    });

    it("is case insensitive", () => {
      // Setup
      const mockHeaderName = 'x-mock-header';
      const mockHeaderValue = 'mock value';
      const request: RequestInit = {
        headers: [
          [mockHeaderName, mockHeaderValue],
        ],
      };

      // Test / Assert
      runCaseSensitivityTest(request, mockHeaderName, mockHeaderValue);
    });

    it("returns null for a header that doesn't exist", () => {
      // Setup
      const mockHeaderName = 'x-non-existent-header';
      const request: RequestInit = {
        headers: [],
      };

      // Test / Assert
      runMissingHeaderTest(request, mockHeaderName);
    });
  });

  describe("with plain object", () => {
    it("gets correct header", () => {
      // Setup
      const mockHeaderName = 'x-mock-header';
      const mockHeaderValue = 'mock value';
      const request: RequestInit = {
        headers: {
          [mockHeaderName]: mockHeaderValue,
        },
      };

      // Test / Assert
      runCorrectHeaderTest(request, mockHeaderName, mockHeaderValue);
    });

    it("is case insensitive", () => {
      // Setup
      const mockHeaderName = 'x-mock-header';
      const mockHeaderValue = 'mock value';
      const request: RequestInit = {
        headers: {
          [mockHeaderName]: mockHeaderValue,
        },
      };

      // Test / Assert
      runCaseSensitivityTest(request, mockHeaderName, mockHeaderValue);
    });

    it("returns null for a header that doesn't exist", () => {
      // Setup
      const mockHeaderName = 'x-non-existent-header';
      const request: RequestInit = {
        headers: {},
      };

      // Test / Assert
      runMissingHeaderTest(request, mockHeaderName);
    });
  });
});

function runCorrectHeaderTest(request: RequestInit, mockHeaderName: string, mockHeaderValue: string): void {
  // Test
  const actualHeaderValue = getRequestHeader(request, mockHeaderName);

  // Assert
  expect(actualHeaderValue).toBe(mockHeaderValue);
}

function runCaseSensitivityTest(request: RequestInit, mockHeaderName: string, mockHeaderValue: string): void {
  // Test
  const actualHeaderValue = getRequestHeader(request, mockHeaderName.toUpperCase());

  // Assert
  expect(actualHeaderValue).toBe(mockHeaderValue);
}

function runMissingHeaderTest(request: RequestInit, mockHeaderName: string): void {
  // Test
  const actualHeaderValue = getRequestHeader(request, mockHeaderName);

  // Assert
  expect(actualHeaderValue).toBeNull();
}
