import { h, FunctionalComponent } from "preact";
import { observer } from "mobx-react-lite";

import { useServices } from "@app/services";

import AuthenticatedRoute from "./common/authenticated";

const UserProfileRoute = observer(AuthenticatedRoute(() => {
  const { UserService } = useServices();

  return (
    <div class="user-profile">
      <h1>User Profile: {UserService.currentUser?.username}</h1>

      <p><em>It&apos;s a bit empty here right now.</em></p>
    </div>
  );
})) as FunctionalComponent;

export default UserProfileRoute;
