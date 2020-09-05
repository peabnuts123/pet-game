import { h, ComponentType } from "preact";
import { route } from "preact-router";
import { observer } from "mobx-react-lite";

import { useStores } from "@app/stores";
import HigherOrderComponent from "@app/types/higher-order-component";
import LoadingSpinner from "@app/components/loading-spinner";


const AuthenticatedRoute: HigherOrderComponent = <TProps extends Record<string, unknown>>(RouteComponent: ComponentType<TProps>) => {
  const WrappedAuthenticatedRoute = observer((props: TProps): h.JSX.Element => {
    // eslint-disable-next-line react-hooks/rules-of-hooks
    const { UserStore } = useStores();

    if (UserStore.isFetchingUserProfile) {
      /* Currently checking user auth */
      return (<div class="authenticated__loading-message">
        <LoadingSpinner />
        <p>Hold on a sec while you&apos;re logging in&hellip;</p>
      </div>);
    } else if (UserStore.isUserLoggedOut) {
      /* User is not logged in, redirect to login */
      route('/login');

      return (<LoadingSpinner />);
    } else {
      /* User is logged in, display route */
      return (
        <RouteComponent {...props} />
      );
    }
  });

  return WrappedAuthenticatedRoute;
};

export default AuthenticatedRoute;
