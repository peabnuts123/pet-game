import { h, FunctionalComponent } from "preact";
import { observer } from "mobx-react-lite";
import { useServices } from "@app/services";

const TakingTreeRoute: FunctionalComponent = observer(() => {

  const { UserService } = useServices();

  return (
    <div>
      <h1>The Taking Tree</h1>

      {UserService.isLoggedIn ? (
        <p>You are logged in. You can take and also donate items to The Taking Tree.</p>
      ) : (
        <p>You must log in to take items from The Taking Tree.</p>
      )}
    </div>
  );
});

export default TakingTreeRoute;
