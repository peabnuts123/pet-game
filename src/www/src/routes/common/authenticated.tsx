import { h, ComponentType, Fragment } from "preact";
import { route, getCurrentUrl } from "preact-router";

import { useStores } from "@app/stores";
import HigherOrderComponent from "@app/types/higher-order-component";
import Logger, { LogLevel } from "@app/util/Logger";


const AuthenticatedRoute: HigherOrderComponent = <TProps extends Record<string, unknown>>(RouteComponent: ComponentType<TProps>) => {
  const WrappedAuthenticatedRoute = (props: TProps): h.JSX.Element => {
    const { UserStore } = useStores();

    if (UserStore.isUserLoggedOut) {
      Logger.log(LogLevel.debug, `Unauthenticated user trying to access authenticated route ${getCurrentUrl()}. Redirecting to /login...`);
      route('/login');
      return (
        <Fragment />
      );
    } else {
      return (
        <RouteComponent {...props} />
      );
    }
  };

  return WrappedAuthenticatedRoute;
};

export default AuthenticatedRoute;
