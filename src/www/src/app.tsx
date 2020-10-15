import { FunctionalComponent, h } from "preact";
import { Route, Router } from "preact-router";
import { useEffect } from "preact/hooks";

import Logger, { LogLevel } from "@app/util/Logger";
import { useStores } from "@app/stores";

// Components
import Header from "@app/components/header";

// Routes
import RouteMap from './route-map';


if ((module as any).hot) {
  require("preact/debug");
}

const App: FunctionalComponent = () => {
  const { UserStore, RouteStore } = useStores();

  useEffect(() => {
    Logger.log(LogLevel.debug, "App loading");
    void UserStore.refreshUserProfile();

    // Listen to route changes, and refresh the page when crossing between a 404/200 response
    //  i.e. when going to/from the not-found "default" route
    RouteStore.addRouteChangeListener((args) => {
      if ((args.currentRoute.props as any).default !== (args.previousRoute?.props as any)?.default) {
        location.reload();
      }
    });
  }, [UserStore, RouteStore]);

  return (
    <div>
      <Header />

      <div class="container u-padding-top-lg">
        <Router onChange={(args) => RouteStore.onRouteChange(args)}>
          {RouteMap.map((route) => {
            if (route.default) {
              // Default route
              return (<Route component={route.component} default />);
            } else {
              // Normal route
              return (<Route path={route.path} component={route.component} />);
            }
          })}
        </Router>
      </div>
    </div>
  );
};

export default App;
