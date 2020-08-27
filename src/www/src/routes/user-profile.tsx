import { h, FunctionalComponent } from "preact";
import { observer } from "mobx-react-lite";

import { useServices } from "@app/services";
import { InventoryItem } from "@app/services/user";

import AuthenticatedRoute from "./common/authenticated";

const UserProfileRoute = AuthenticatedRoute(observer(() => {
  const { UserService, TakingTreeService } = useServices();

  const user = UserService.currentUser;

  const donateItem = (inventoryItem: InventoryItem): void => {
    user?.inventory.removeItem(inventoryItem.item);
    TakingTreeService.addItem(inventoryItem.item);
  };

  return (
    <div class="user-profile">
      <h1>User Profile: {user?.username}</h1>

      <h2>Inventory</h2>
      <ul>
        {user?.inventory.items.map((item) => (
          <li key={item.item.name}>{item.amount}x {item.item.name} &nbsp;<button class="button" onClick={() => donateItem(item)}>Donate to Taking Tree</button></li>
        ))}
      </ul>
    </div>
  );
})) as FunctionalComponent;

export default UserProfileRoute;
