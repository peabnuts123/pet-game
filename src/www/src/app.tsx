import { FunctionalComponent, h } from "preact";
import { Route, Router } from "preact-router";
import { useEffect } from "preact/hooks";

import Logger, { LogLevel } from "@app/util/Logger";

// Components
import Header from "@app/components/header";

// Routes
import HomeRoute from "@app/routes/home";
import NotFoundRoute from "@app/routes/not-found";
import LoginRoute from '@app/routes/login';
import TakingTreeRoute from '@app/routes/taking-tree';
import UserProfileRoute from '@app/routes/user-profile';



// eslint-disable-next-line @typescript-eslint/no-explicit-any
if ((module as any).hot) {
  // tslint:disable-next-line:no-var-requires
  require("preact/debug");
}

const App: FunctionalComponent = () => {
  useEffect(() => {
    Logger.setLogLevel(LogLevel.debug);
    Logger.log(LogLevel.debug, "App loading");
  }, []);

  return (
    <div>
      <Header />

      <div class="container u-padding-top-lg">
        <Router>
          <Route path="/" component={HomeRoute} />
          <Route path="/login" component={LoginRoute} />
          <Route path="/taking-tree" component={TakingTreeRoute} />
          <Route path="/user-profile" component={UserProfileRoute} />
          <NotFoundRoute default />
        </Router>
      </div>
    </div>
  );
};

export default App;
