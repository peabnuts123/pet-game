import ApplicationConfig from "../Config";
import { LogLevel } from "@app/util/Logger";

const ProductionConfig: ApplicationConfig = {
  EnvironmentId: "Production",
  ApiHost: 'https://pet-game-dev.herokuapp.com',
  LogLevel: LogLevel.none,
  AppVersion: process.env.PACKAGE_VERSION!,
};

export default ProductionConfig;
