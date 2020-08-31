import { FunctionalComponent, h } from "preact";
import { Route, Router } from "preact-router";
import { useEffect } from "preact/hooks";

import Logger, { LogLevel } from "@app/util/Logger";
import { useStores } from "@app/stores";

// Components
import Header from "@app/components/header";

// Routes
import HomeRoute from "@app/routes/home";
import NotFoundRoute from "@app/routes/not-found";
import TakingTreeRoute from '@app/routes/taking-tree';
import UserProfileRoute from '@app/routes/user-profile';
import LoginRoute from "@app/routes/login";
import LogoutRoute from "@app/routes/logout";


// eslint-disable-next-line @typescript-eslint/no-unsafe-member-access
if ((module as any).hot) {
  require("preact/debug");
}

const App: FunctionalComponent = () => {
  const { UserStore } = useStores();

  useEffect(() => {
    Logger.log(LogLevel.debug, "App loading");
    void UserStore.refreshUserProfile();
  }, [UserStore]);

  return (
    <div>
      <Header />

      <div class="container u-padding-top-lg">
        <Router>
          <Route path="/" component={HomeRoute} />
          <Route path="/login" component={LoginRoute} />
          <Route path="/logout" component={LogoutRoute} />
          <Route path="/taking-tree" component={TakingTreeRoute} />
          <Route path="/user-profile" component={UserProfileRoute} />
          <NotFoundRoute default />
        </Router>
      </div>
    </div>
  );
};

export default App;
