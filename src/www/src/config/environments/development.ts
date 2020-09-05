import ApplicationConfig from "../Config";
import { LogLevel } from "@app/util/Logger";

const DevelopmentConfig: ApplicationConfig = {
  EnvironmentId: "Development",
  ApiHost: `https://localhost:5000`,
  LogLevel: LogLevel.debug,
  AppVersion: process.env.PACKAGE_VERSION!,
};

export default DevelopmentConfig;
