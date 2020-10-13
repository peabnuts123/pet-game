import { h } from "preact";
import { render } from '@testing-library/preact';

import Header from "@app/components/header";

describe("Initial Test of the Header", () => {
  test("Header renders 3 nav items", () => {
    const { queryByTestId } = render(<Header />);

    // @TODO write better tests
    expect(queryByTestId("header__brand")?.textContent).toBe("Pet Game");
  });

  test('Matches snapshot', () => {
    const { baseElement } = render(<Header />);

    expect(baseElement).toMatchSnapshot();
  });
});
