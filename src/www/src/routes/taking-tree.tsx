import { h, FunctionalComponent, Fragment } from "preact";
import { useEffect } from "preact/hooks";
import { Link } from "preact-router";
import { observer } from "mobx-react-lite";
import classNames from "classnames";

import { useStores } from '@app/stores';
import LoadingSpinner from "@app/components/loading-spinner";

const TakingTreeRoute = observer(() => {
  const { TakingTreeStore, UserStore } = useStores();

  const inventoryItems = TakingTreeStore.inventoryItems;
  const hasLoadedItems = inventoryItems !== null;
  const hasItems = hasLoadedItems && inventoryItems!.length > 0;

  useEffect(() => {
    void TakingTreeStore.refreshInventoryItems();
  }, [TakingTreeStore]);

  const claimItem = async (takingTreeInventoryItemId: string): Promise<void> => {
    if (UserStore.isUserLoggedOut) {
      // No-op
      return;
    }

    await TakingTreeStore.claimItem(takingTreeInventoryItemId);
    await UserStore.refreshUserProfile();
  };

  return (
    <div class="taking-tree">
      <h1 class="taking-tree__title">The Taking Tree</h1>

      <p>It&apos;s The Taking Tree! Everybody&apos;s favourite place to camp out and <span class="taking-tree__text--strikethrough">mercilessly snatch anything you can</span> have a picnic. People leave items here from time to time for others to take, see if you can get your hands on something great!</p>


      {/* Tree contents */}
      <h2 class="taking-tree__whats-under-the-tree-title">What&apos;s under the tree right now?</h2>

      <div class="taking-tree__item-list">

        {!hasLoadedItems ? (
          /* Loading state */
          <div class="taking-tree__loading">
            <LoadingSpinner />
            <span class="u-margin-top-lg">Loading items&hellip;</span>
          </div>
        ) : (
            /* Finished loading */
            (!hasItems ? (
              /* Tree inventory is empty */
              <p class="u-margin-top-0">There&apos;s nothing under the tree right now. Why don&apos;t you head to <Link href="/user-profile">your profile</Link> and donate something?</p>
            ) : (
              <Fragment>
                  {/* Tree inventory contents */}
                  {inventoryItems!.map((inventoryItem) => (
                    /* Item card container */
                    <div
                      class={classNames('taking-tree__item', {
                        'is-enabled': UserStore.isUserLoggedIn,
                        'is-taking': TakingTreeStore.isItemBeingClaimed(inventoryItem.id),
                      })}
                      onClick={() => claimItem(inventoryItem.id)}
                      key={inventoryItem.id}
                    >
                      {/* Label */}
                      <div class="taking-tree__item__label">
                        {inventoryItem.item.name}
                        {(TakingTreeStore.isItemBeingClaimed(inventoryItem.id)) && (
                          <LoadingSpinner />
                        )}
                      </div>

                      {/* Claim button */}
                      {UserStore.isUserLoggedIn && !(TakingTreeStore.isItemBeingClaimed(inventoryItem.id)) && (
                        <div class="taking-tree__item__button">
                          <button class="button">Claim</button>
                        </div>
                      )}
                    </div>
                  ))}

                  {/* Logged out message */}
                  {UserStore.isUserLoggedOut && (
                    <p class="taking-tree__logged-out-text">You must be logged in to claim items from The Taking Tree.</p>
                  )}
                </Fragment>
              ))
          )}
      </div>


    </div>
  );
}) as FunctionalComponent;

export default TakingTreeRoute;
