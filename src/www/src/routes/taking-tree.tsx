import { h, FunctionalComponent, Fragment } from "preact";
import { observer } from "mobx-react-lite";

import { useServices } from "@app/services";

const TakingTreeRoute = observer(() => {

  const { UserService, TakingTreeService } = useServices();

  const items = TakingTreeService.items;

  const claimItem = (index: number): void => {
    const item = items[index];
    TakingTreeService.takeItem(index);
    UserService.currentUser?.inventory.addItem(item.name);
  };

  return (
    <Fragment>
      <h1>The Taking Tree</h1>

      {UserService.isLoggedIn ? (
        <p>You are logged in. You can take and also donate items to The Taking Tree.</p>
      ) : (
          <p>You must log in to take items from The Taking Tree.</p>
        )}

      <h2>Items under the tree</h2>
      {items.length === 0 ? (
        <em>There&apos;s nothing under the tree right now.</em>
      ) : (
          <ul>
            {items.map((item, index) => (
              <li key={index}>
                {item.name} &nbsp;
                {UserService.isLoggedIn && (
                  <button class="button" onClick={() => claimItem(index)}>TAKE</button>
                )}
              </li>
            ))}
          </ul>
        )}
    </Fragment>
  );
}) as FunctionalComponent;

export default TakingTreeRoute;
