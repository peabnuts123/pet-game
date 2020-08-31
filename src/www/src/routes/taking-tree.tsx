import { h, FunctionalComponent, Fragment } from "preact";
import { useEffect } from "preact/hooks";
import { observer } from "mobx-react-lite";

import { useStores } from '@app/stores';

const TakingTreeRoute = observer(() => {
  const { TakingTreeStore, UserStore } = useStores();

  const inventoryItems = TakingTreeStore.inventoryItems;
  const hasLoadedItems = inventoryItems !== null;
  const hasItems = hasLoadedItems && inventoryItems!.length > 0;

  useEffect(() => {
    void TakingTreeStore.refreshInventoryItems();
  }, [TakingTreeStore]);

  const claimItem = async (takingTreeInventoryItemId: string): Promise<void> => {
    await TakingTreeStore.claimItem(takingTreeInventoryItemId);
    await UserStore.refreshUserProfile();
  };

  return (
    <Fragment>
      <h1>The Taking Tree</h1>

      {UserStore.isUserLoggedIn ? (
        <p>You are logged in. You can take and also donate items to The Taking Tree.</p>
      ) : (
          <p>You must log in to take items from The Taking Tree.</p>
        )}

      {/* Tree contents */}
      <h2>Items under the tree</h2>

      {!hasLoadedItems ? (
        /* Loading state */
        <em>Loading items...</em>
      ) : (
          /* Finished loading */
          (!hasItems ? (
            /* Tree inventory is empty */
            <em>There&apos;s nothing under the tree right now.</em>
          ) : (
              /* Tree inventory contents */
              <ul>
                {inventoryItems!.map((inventoryItem) => (
                  <li key={inventoryItem.id}>
                    {inventoryItem.item.name} &nbsp;
                    {UserStore.isUserLoggedIn && (
                      <button class="button" onClick={() => claimItem(inventoryItem.id)}>Claim</button>
                    )}
                  </li>
                ))}
              </ul>
            ))
        )}
    </Fragment>
  );
}) as FunctionalComponent;

export default TakingTreeRoute;
