import { h, FunctionalComponent } from "preact";
import { observer } from "mobx-react-lite";
import { route } from "preact-router";

import Config from '@app/config';
import Endpoints from "@app/constants/endpoints";
import { useStores } from "@app/stores";
import LoadingSpinner from "@app/components/loading-spinner";

const LoginRoute: FunctionalComponent = observer(() => {
  const { UserStore } = useStores();

  if (UserStore.isUserLoggedIn) {
    route("/", true);
  } else {
    const loginUrl = `${Config.ApiHost}${Endpoints.Auth.login(location.origin)}`;
    window.location.href = loginUrl;
  }

  return (
    <div class="login">
      <LoadingSpinner />
      <p class="login__message">Redirecting you to Auth0 for login&hellip;</p>
    </div>
  );
}) as FunctionalComponent;

export default LoginRoute;
