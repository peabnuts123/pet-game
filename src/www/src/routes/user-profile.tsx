import { h, FunctionalComponent } from "preact";
import { observer } from "mobx-react-lite";
import { useState } from "preact/hooks";
import classNames from "classnames";

import { useStores } from "@app/stores";
import LoadingSpinner from "@app/components/loading-spinner";

import AuthenticatedRoute from "./common/authenticated";
import { useRef } from "react";
import UserProfile from "@app/models/UserProfile";

const UserProfileRoute = AuthenticatedRoute(observer(() => {
  const { UserStore, TakingTreeStore } = useStores();
  const [itemsBeingDonated, setItemsBeingDonated] = useState<string[]>([]);

  // Editing profile form
  const usernameEl = useRef<HTMLInputElement>();
  const [isEditingProfile, setIsEditingProfile] = useState<boolean>(false);
  const [isSavingProfile, setIsSavingProfile] = useState<boolean>(false);

  const user: UserProfile = UserStore.currentUserProfile!;

  const isItemBeingDonated = (playerInventoryItemId: string): boolean => itemsBeingDonated.includes(playerInventoryItemId);

  const donateItem = async (playerInventoryItemId: string): Promise<void> => {
    setItemsBeingDonated(itemsBeingDonated.concat(playerInventoryItemId));
    await TakingTreeStore.donateItem(playerInventoryItemId);
    await UserStore.refreshUserProfile();
    setItemsBeingDonated(itemsBeingDonated.filter((item) => item !== playerInventoryItemId));
  };

  const beginEditingProfile = (): void => {
    setIsEditingProfile(true);

    setTimeout(() => {
      usernameEl.current!.value = user.username;
    });
  };

  const onSaveProfile = async (e: h.JSX.TargetedEvent<HTMLFormElement>): Promise<void> => {
    e.preventDefault();
    e.stopPropagation();

    if (isSavingProfile) return;
    if (usernameEl.current!.value.trim().length < 1) return;

    setIsSavingProfile(true);

    await UserStore.updateUserProfile({
      ...user,
      username: usernameEl.current!.value,
    });
    setIsSavingProfile(false);
    setIsEditingProfile(false);
  };

  return (
    <div class="user-profile">
      <div class="user-profile__title-container">
        <h1 class="user-profile__title">{user.username}</h1>

        {!isEditingProfile && (
          <button class="button button--small user-profile__title-button" onClick={() => beginEditingProfile()}>Edit</button>
        )}
      </div>

      {isEditingProfile && (
        <form action="#" class="form" onSubmit={onSaveProfile}>
          <label htmlFor="profile__username" class="form__input-label">Username
            <input id="profile__username"
              name="profile__username"
              type="text"
              class="input input--text form__input"
              ref={usernameEl}
              disabled={isSavingProfile}
              minLength={1}
              maxLength={40}
            />
          </label>

          <button type="submit" class="form__button button button--small u-margin-bottom-md" disabled={isSavingProfile}>Save</button>

          {isSavingProfile && (
            <LoadingSpinner />
          )}
        </form>
      )}

      {!isEditingProfile && (
        <p>Hi! My name is {user.username} and I love my pets sooo much! I feed them and play games with them every day!! Check out my store for great prices on paint brushes!! This theme was made by neolover_88.</p>
      )}

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
