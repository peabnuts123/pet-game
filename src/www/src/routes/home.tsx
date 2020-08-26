import { FunctionalComponent, h } from "preact";
import { Link } from "preact-router";
import { observer } from 'mobx-react-lite';

import { useServices } from '@app/services';

const HomeRoute: FunctionalComponent = observer(() => {
  const { UserService } = useServices();

  return (
    <div class="home">
      <h1>Home</h1>

      <p>This is a pet game. You play it.</p>

      <p>Things you can do right now:</p>
      <ul>
        {UserService.isLoggedIn ? (
          <li class="u-margin-bottom-md"><button class="button" onClick={() => UserService.logOut()}>Log out</button></li>
        ) : (
            <li class="u-margin-bottom-md"><Link class="button" href="/login">Log in</Link></li>
          )}

        <li><Link class="button u-margin-bottom-md" href="/taking-tree">Visit The Taking Tree</Link></li>
        <li><Link class="button" href="/user-profile">View your profile</Link></li>
      </ul>

    </div>
  );
});

export default HomeRoute;
