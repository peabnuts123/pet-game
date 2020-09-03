import { Fragment, FunctionalComponent, h } from "preact";
import { Link } from "preact-router";
import { observer } from 'mobx-react-lite';

import { useStores } from "@app/stores";


const HomeRoute = observer(() => {
  const { UserStore } = useStores();

  return (
    <div class="home">
      <h1 class="home__title">Welcome!</h1>

      <p class="home__description">Welcome to Pet Game, a totally original game that is <em>definitely not</em> a clone of a popular virtual pet website. As you can probably tell, the site is only in the early stages of development right now, but eventually you will be able to have lots of fun here. You will be able to adopt pets, play games with them, and explore!</p>

      <h2 class="home__what-can-i-do-title">What things can I do?</h2>

      <p>Right now there is not a lot you can do, but you can take a poke around, visit The Taking Tree, or view your own profile (including your inventory).</p>

      <ul class="home__list">
        {UserStore.isUserLoggedIn ? (
          <li class="home__list-item"><Link class="" href="/logout">Log out</Link></li>
        ) : (
            <li class="home__list-item"><Link class="" href="/login">Log in / Register</Link></li>
          )}

        <li class="home__list-item"><Link class="" href="/taking-tree">Visit The Taking Tree</Link></li>
        <li class="home__list-item"><Link class="" href="/user-profile">View your profile</Link></li>
      </ul>

    </div>
  );
}) as FunctionalComponent;

export default HomeRoute;
