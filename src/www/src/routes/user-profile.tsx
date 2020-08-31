import { h, FunctionalComponent } from "preact";
import { observer } from "mobx-react-lite";

import { useStores } from "@app/stores";

import AuthenticatedRoute from "./common/authenticated";

const UserProfileRoute = AuthenticatedRoute(observer(() => {
  const { UserStore, TakingTreeStore } = useStores();

  const user = UserStore.currentUserProfile;

  const donateItem = async (playerInventoryItemId: string): Promise<void> => {
    await TakingTreeStore.donateItem(playerInventoryItemId);
    await UserStore.refreshUserProfile();
  };

  return (
    <div class="user-profile">
      <h1>User Profile: {user?.username}</h1>

      <h2>Inventory</h2>
      <ul>
        {user?.inventory.map((playerInventoryItem) => (
          <li key={playerInventoryItem.item.id}>
            {playerInventoryItem.count}x {playerInventoryItem.item.name} &nbsp;
            <button class="button" onClick={() => donateItem(playerInventoryItem.id)}>Donate to Taking Tree</button>
          </li>
        ))}
      </ul>
    </div>
  );
})) as FunctionalComponent;

export default UserProfileRoute;
