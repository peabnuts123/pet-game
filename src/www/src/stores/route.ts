import { action, observable } from "mobx";
import { RouterOnChangeArgs } from "preact-router";

import Store from "./base/store";

export type OnRouteChangeFunction = (args: RouterOnChangeArgs) => void;

class RouteStore extends Store {
  @observable
  private _onRouteChangeCallbacks: OnRouteChangeFunction[];

  public constructor() {
    super();
    this._onRouteChangeCallbacks = [];
  }

  @action
  public addRouteChangeListener(listenerFunction: OnRouteChangeFunction): void {
    this._onRouteChangeCallbacks.push(listenerFunction);
  }

  @action
  public removeRouteChangeListener(listenerFunction: OnRouteChangeFunction): void {
    if (!this._onRouteChangeCallbacks.includes(listenerFunction)) {
      throw new Error("Failed to unsubscribe route change listener - listener function is not currently subscribed. Did you pass the same instance?");
    }

    this._onRouteChangeCallbacks = this._onRouteChangeCallbacks.filter((callback) => callback !== listenerFunction);
  }

  @action
  public onRouteChange(args: RouterOnChangeArgs): void {
    this._onRouteChangeCallbacks.forEach((callback) => callback(args));
  }
}

export default RouteStore;
