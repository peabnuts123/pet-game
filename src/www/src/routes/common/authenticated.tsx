import { h, ComponentType } from "preact";
import { route, getCurrentUrl } from "preact-router";

import { useServices } from "@app/services";
import HigherOrderComponent from "@app/types/higher-order-component";
import Logger, { LogLevel } from "@app/util/Logger";


const AuthenticatedRoute: HigherOrderComponent = <TProps extends {}>(RouteComponent: ComponentType<TProps>) => {
  const authenticatedRoute = (props: TProps): h.JSX.Element => {
    const { UserService } = useServices();

    if (UserService.isLoggedOut) {
      Logger.log(LogLevel.debug, `Unauthenticated user trying to access authenticated route ${getCurrentUrl()}. Redirecting to /login...`);
      route('/login');
    }

    return (
      <RouteComponent {...props} />
    );
  };

  return authenticatedRoute;
};

export default AuthenticatedRoute;
