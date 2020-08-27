import { createContext, Context } from "preact";
import { useContext } from "preact/hooks";
import TakingTreeService from "./taking-tree";

import UserService from "./user";

const config = {
  UserService: new UserService(),
  TakingTreeService: new TakingTreeService(),
};

const ServicesContext: Context<typeof config> = createContext(config);

export const useServices = (): typeof config => useContext(ServicesContext);
