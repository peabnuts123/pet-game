import { h } from "preact";
import { render } from '@testing-library/preact';

import Header from "@app/components/header";

describe("Initial Test of the Header", () => {
  test("Header renders 3 nav items", () => {
    const { container } = render(<Header />);

    expect(container.querySelector('h1')?.textContent).toBe("Preact App");
    expect(container.querySelectorAll("a").length).toBe(3);
  });

  test('Matches snapshot', () => {
    const { baseElement } = render(<Header />);

    expect(baseElement).toMatchSnapshot();
  });
});
