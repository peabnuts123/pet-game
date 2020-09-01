import { h, FunctionalComponent } from "preact";
import { observer } from "mobx-react-lite";
import { route } from "preact-router";

import Endpoints from "@app/constants/endpoints";
import { useStores } from "@app/stores";

const LogoutRoute: FunctionalComponent = observer(() => {
  const { UserStore } = useStores();

  if (UserStore.isUserLoggedOut) {
    route("/", true);
  } else {
    const logoutUrl = `${API_BASE}${Endpoints.Auth.logout()}`;
    window.location.href = logoutUrl;
  }

  return (
    <p><em>Logging you out...</em></p>
  );
}) as FunctionalComponent;

export default LogoutRoute;