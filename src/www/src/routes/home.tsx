import { Fragment, FunctionalComponent, h } from "preact";
import { Link } from "preact-router";
import { observer } from 'mobx-react-lite';

import { useStores } from "@app/stores";


const HomeRoute = observer(() => {
  const { UserStore } = useStores();

  return (
    <Fragment>
      <h1>Home</h1>

      <p>This is a pet game. You play it.</p>

      <p>Things you can do right now:</p>
      <ul>
        {UserStore.isUserLoggedIn ? (
          <li class="u-margin-bottom-md"><Link class="button" href="/logout">Log out</Link></li>
        ) : (
            <li class="u-margin-bottom-md"><Link class="button" href="/login">Log in</Link></li>
          )}

        <li><Link class="button u-margin-bottom-md" href="/taking-tree">Visit The Taking Tree</Link></li>
        <li><Link class="button" href="/user-profile">View your profile</Link></li>
      </ul>

    </Fragment>
  );
}) as FunctionalComponent;

export default HomeRoute;
