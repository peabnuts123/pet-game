import { h } from "preact";
import { render } from "@testing-library/preact";

import App from "@app/app";
import mockRequests from "./util/mock-requests";

describe('App', () => {
  beforeEach(() => {
    mockRequests([{
      test: (url: string) => /api\/user\/profile$/g.test(url),
      response: 'Not authorized',
    }]);
  });

  test.skip('Matches snapshot', () => {
    // Setup
    const { baseElement } = render(<App />);

    // Test / Assert
    expect(baseElement).toMatchSnapshot();
  });
});

