import { h, ComponentType } from "preact";
import { route, getCurrentUrl } from "preact-router";

import { useServices } from "@app/services";
import HigherOrderComponent from "@app/types/higher-order-component";
import Logger, { LogLevel } from "@app/util/Logger";


const AuthenticatedRoute: HigherOrderComponent = <TProps extends Record<string, unknown>>(RouteComponent: ComponentType<TProps>) => {
  const WrappedAuthenticatedRoute = (props: TProps): h.JSX.Element => {
    const { UserService } = useServices();

    if (UserService.isLoggedOut) {
      Logger.log(LogLevel.debug, `Unauthenticated user trying to access authenticated route ${getCurrentUrl()}. Redirecting to /login...`);
      route('/login');
    }

    return (
      <RouteComponent {...props} />
    );
  };

  return WrappedAuthenticatedRoute;
};

export default AuthenticatedRoute;
