# WWW proxy

This is a reverse proxy that fetches the frontend's content from S3. It looks at requests and will compare 404's from S3 to the exported `route-map.json` to check if the request is a valid route in the SPA. If-so, `index.html` will be served with an HTTP 200. If the request is _still_ not a valid route, then a true 404 will be returned (`index.html` will still be served). This makes the SPA behave much like a regular web-app.

## Work backlog

  - Infrastructure
    - IAM role for parameter store, logs, s3 bucket
    - Node.js Lambda w/ execution role
    - API gateway proxying all requests to the Lambda
    - Remove static hosting from S3
    - Remove origin-access-identity
    - Change Cloudfront to point at API gateway instead of S3, cache aggressively. Can we make an exception for index.html, though?
  - Tests
    - Mock S3 requests somehow and simulate requests / responses
    - Add test step back to `simulate-build`
