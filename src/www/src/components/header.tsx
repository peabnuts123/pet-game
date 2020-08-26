import { FunctionalComponent, h } from "preact";
import { Link } from "preact-router/match";

const Header: FunctionalComponent = () => {
  return (
    <header class="header">
      <h1>Preact App</h1>
      <nav>
        <Link activeClassName="is-active" href="/">
          Home
        </Link>
        <Link activeClassName="is-active" href="/profile">
          Me
        </Link>
        <Link activeClassName="is-active" href="/profile/john">
          John
        </Link>
      </nav>
    </header>
  );
};

export default Header;
