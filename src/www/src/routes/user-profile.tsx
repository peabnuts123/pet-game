import { h, FunctionalComponent } from "preact";
import { observer } from "mobx-react-lite";

import { useServices } from "@app/services";

import AuthenticatedRoute from "./common/authenticated";

const UserProfileRoute = AuthenticatedRoute(observer(() => {
  const { UserService } = useServices();

  const user = UserService.currentUser;

  return (
    <div class="user-profile">
      <h1>User Profile: {user?.username}</h1>

      <h2>Inventory</h2>
      {user?.inventory.items.map((item) => (
        <li key={item.item.name}>{item.amount}x {item.item.name}</li>
      ))}
    </div>
  );
})) as FunctionalComponent;

export default UserProfileRoute;
