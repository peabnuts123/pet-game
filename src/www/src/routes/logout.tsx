import { h, FunctionalComponent } from "preact";
import { observer } from "mobx-react-lite";
import { route } from "preact-router";

import Config from '@app/config';
import Endpoints from "@app/constants/endpoints";
import { useStores } from "@app/stores";
import LoadingSpinner from "@app/components/loading-spinner";

const LogoutRoute: FunctionalComponent = observer(() => {
  const { UserStore } = useStores();

  if (UserStore.isUserLoggedOut) {
    route("/", true);
  } else {
    const logoutUrl = `${Config.ApiHost}${Endpoints.Auth.logout()}`;
    window.location.href = logoutUrl;
  }

  return (
    <div class="logout">
      <LoadingSpinner />
      <p class="logout__message">Logging you out&hellip;</p>
    </div>
  );
}) as FunctionalComponent;

export default LogoutRoute;
