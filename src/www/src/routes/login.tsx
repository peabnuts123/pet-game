import { h, FunctionalComponent } from "preact";
import { observer } from "mobx-react-lite";
import { route } from "preact-router";

import Config from '@app/config';
import Endpoints from "@app/constants/endpoints";
import { useStores } from "@app/stores";

const LoginRoute: FunctionalComponent = observer(() => {
  const { UserStore } = useStores();

  if (UserStore.isUserLoggedIn) {
    route("/", true);
  } else {
    const loginUrl = `${Config.ApiHost}${Endpoints.Auth.login(location.origin)}`;
    window.location.href = loginUrl;
  }

  return (
    <p><em>Redirecting you to Auth0 for login...</em></p>
  );
}) as FunctionalComponent;

export default LoginRoute;
