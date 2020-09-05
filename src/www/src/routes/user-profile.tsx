import { h, FunctionalComponent } from "preact";
import { observer } from "mobx-react-lite";
import { useState } from "preact/hooks";
import classNames from "classnames";

import { useStores } from "@app/stores";
import LoadingSpinner from "@app/components/loading-spinner";

import AuthenticatedRoute from "./common/authenticated";

const UserProfileRoute = AuthenticatedRoute(observer(() => {
  const { UserStore, TakingTreeStore } = useStores();
  const [itemsBeingDonated, setItemsBeingDonated] = useState<string[]>([]);

  const user = UserStore.currentUserProfile;

  const isItemBeingDonated = (playerInventoryItemId: string): boolean => itemsBeingDonated.includes(playerInventoryItemId);

  const donateItem = async (playerInventoryItemId: string): Promise<void> => {
    setItemsBeingDonated(itemsBeingDonated.concat(playerInventoryItemId));
    await TakingTreeStore.donateItem(playerInventoryItemId);
    await UserStore.refreshUserProfile();
    setItemsBeingDonated(itemsBeingDonated.filter((item) => item !== playerInventoryItemId));
  };

  return (
    <div class="user-profile">
      <h1 class="user-profile__title">{user!.username}</h1>

      <p>Hi! My name is {user!.username} and I love my pets sooo much! I feed them and play games with them every day!! Check out my store for great prices on paint brushes!! This theme was made by neolover_88.</p>

      <h2 class="user-profile__inventory-title">Inventory</h2>
      <div class="user-profile__inventory-list">
        {user?.inventory.map((playerInventoryItem) => (
          <div
            onClick={() => donateItem(playerInventoryItem.id)}
            class={classNames("user-profile__inventory-item", {
              "is-donating": isItemBeingDonated(playerInventoryItem.id),
            })}
            key={playerInventoryItem.item.id}
          >
            {/* Label */}
            <div class="user-profile__inventory-item__label">
              {playerInventoryItem.count} x {playerInventoryItem.item.name}
              {(isItemBeingDonated(playerInventoryItem.id)) && (
                <LoadingSpinner />
              )}
            </div>

            {/* Claim button */}
            {UserStore.isUserLoggedIn && !isItemBeingDonated(playerInventoryItem.id) && (
              <div class="user-profile__inventory-item__button">
                <button class="button">Donate</button>
              </div>
            )}
          </div>
        ))}
      </div>
    </div>
  );
})) as FunctionalComponent;

export default UserProfileRoute;
