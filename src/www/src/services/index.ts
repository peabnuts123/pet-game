import { createContext, Context } from "preact";
import { useContext } from "preact/hooks";

import UserService from "./user";

const config = {
  UserService: new UserService(),
};

const ServicesContext: Context<typeof config> = createContext(config);

export const useServices = (): typeof config => useContext(ServicesContext);
