import { FunctionalComponent, h } from "preact";
import { Link } from "preact-router/match";

import { useServices } from '@app/services';
import { observer } from "mobx-react-lite";

const Header = observer(() => {

  const { UserService } = useServices();

  return (
    <header class="header">
      <div class="header__container">
        <Link activeClassName="is-active" href="/">
          <h1>Pet Game</h1>
        </Link>

        <nav>
          <Link activeClassName="is-active" href="/taking-tree">The Taking Tree</Link>

          {UserService.isLoggedIn ? (
            <a aria-role="button" onClick={() => UserService.logOut()}>Log out</a>
          ) : (
              <Link activeClassName="is-active" href="/login">Log in</Link>
            )}
        </nav>
      </div>
    </header>
  );
}) as FunctionalComponent;

export default Header;
