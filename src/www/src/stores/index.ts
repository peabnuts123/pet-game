import { createContext, Context } from "preact";
import { useContext } from "preact/hooks";

import TakingTreeStore from './taking-tree';
import UserStore from "./user";
import RouteStore from "./route";

const config = {
  TakingTreeStore: new TakingTreeStore(),
  UserStore: new UserStore(),
  RouteStore: new RouteStore(),
};

const StoresContext: Context<typeof config> = createContext(config);

export const useStores = (): typeof config => useContext(StoresContext);
