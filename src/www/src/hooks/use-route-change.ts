import { useEffect } from "preact/hooks";

import { useStores } from "@app/stores";
import { OnRouteChangeFunction } from "@app/stores/route";
import Logger, { LogLevel } from "@app/util/Logger";


const useRouteChange = (callback: OnRouteChangeFunction): void => {
  const { RouteStore } = useStores();

  useEffect(() => {
    Logger.log(LogLevel.debug, "[UseRouteChange] Subscribing to route changes");
    RouteStore.addRouteChangeListener(callback);
    return () => {
      Logger.log(LogLevel.debug, "[UseRouteChange] Unsubscribing to route changes");
      RouteStore.removeRouteChangeListener(callback);
    };
    // @NOTE If you add `callback` to useEffect's dependencies, it still works
    //  but constantly subscribes/unsubscribes every render (as the callback function is redefined)
  }, []); // eslint-disable-line react-hooks/exhaustive-deps
};

export default useRouteChange;
