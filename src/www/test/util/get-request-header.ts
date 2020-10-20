/**
 * Attempt to get the value of a header from a given request.
 *
 * @param request Request to read header from
 * @param headerName Header to read the value of
 */
export default function getRequestHeader(request: RequestInit | undefined, headerName: string): string | null {
  if (!request) {
    // Request is undefined, can't find header in undefined...
    return null;
  } else {
    if (request.headers && (request.headers as Headers).get !== undefined) {
      // Headers object
      const headers: Headers = request.headers as Headers;

      // Use native Headers API
      return headers.get(headerName);
    } else if (Array.isArray(request.headers)) {
      // Tuple array
      // const headers: string[][] = request.headers as string[][];

      // Find header name in array
      for (let i = 0; i < request.headers.length; i++) {
        const [header, value] = request.headers[i];

        // Return value if the header name matches (case insensitive)
        if (header.toLocaleLowerCase() === headerName.toLocaleLowerCase()) {
          return value;
        }
      }

      // Header not found
      return null;
    } else if (toString.call(request.headers) === '[object Object]') {
      // Plain object
      const headers: Record<string, string> = request.headers as Record<string, string>;

      // Case-insensitive search through object keys
      for (const key of Object.keys(headers)) {
        if (key.toLocaleLowerCase() === headerName.toLocaleLowerCase()) {
          return headers[key];
        }
      }

      // Header not found
      return null;
    } else if (request.headers === undefined) {
      // Request has no headers
      return null;
    } else {
      // Unknown type or invalid `request` object
      throw new Error(`Request 'headers' object is not valid type. Got: ${request.headers} - (${typeof request.headers}). Expected: Headers | string[][] | Record<string, string> | undefined.`);
    }
  }
}
