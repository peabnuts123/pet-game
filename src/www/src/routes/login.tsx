import { h, FunctionalComponent } from "preact";
import { useRef } from "preact/hooks";
import { observer } from "mobx-react-lite";

import { useServices } from '@app/services';

const LoginRoute: FunctionalComponent = observer(() => {
  const usernameEl = useRef<HTMLInputElement>(null);
  const passwordEl = useRef<HTMLInputElement>(null);

  const { UserService } = useServices();


  const login = (e: h.JSX.TargetedEvent<HTMLFormElement>): void => {
    e.preventDefault();
    e.stopPropagation();

    const username = usernameEl.current.value;
    const password = passwordEl.current.value;

    UserService.logIn(username, password);
  };

  return (
    <div>
      <h1>Log in</h1>

      <form class="form" action="#" onSubmit={login}>
        <label class="form__input-label" htmlFor="username">Username
          <input class="form__input input input--text" type="text" id="username" ref={usernameEl} />
        </label>
        <label class="form__input-label" htmlFor="password">Password
          <input class="form__input input input--text" type="password" id="password" ref={passwordEl} />
        </label>

        <button class="form__button button" type="submit">Log in</button>

      </form>
    </div>
  );
});

export default LoginRoute;
