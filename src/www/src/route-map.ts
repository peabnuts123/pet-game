import { RouteProps } from 'preact-router';

// Routes
import HomeRoute from "@app/routes/home";
import NotFoundRoute from "@app/routes/not-found";
import TakingTreeRoute from '@app/routes/taking-tree';
import UserProfileRoute from '@app/routes/user-profile';
import LoginRoute from "@app/routes/login";
import LogoutRoute from "@app/routes/logout";
import GamesRoute from '@app/routes/games';

interface RouteMapEntry extends RouteProps<any> {
  path?: string;
  default?: true;
}

const RouteMap: RouteMapEntry[] = [
  { path: "/", component: HomeRoute },
  { path: "/login", component: LoginRoute },
  { path: "/logout", component: LogoutRoute },
  { path: "/taking-tree", component: TakingTreeRoute },
  { path: "/user-profile", component: UserProfileRoute },
  { path: "/games", component: GamesRoute },
  { default: true, component: NotFoundRoute },
];

export default RouteMap;
