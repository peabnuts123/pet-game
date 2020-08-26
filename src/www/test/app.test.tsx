import { h } from "preact";
import { render } from "@testing-library/preact";

import App from "@app/components/app";

describe('App', () => {
  test('Matches snapshot', () => {
    // Setup
    const { baseElement } = render(<App />);

    // Test / Assert
    expect(baseElement).toMatchSnapshot();
  });
});

