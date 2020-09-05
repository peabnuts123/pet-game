import { LogLevel } from "@app/util/Logger";

interface ApplicationConfig {
  readonly EnvironmentId: string;
  readonly ApiHost: string;
  readonly LogLevel: LogLevel;
  readonly AppVersion: string;
}

export default ApplicationConfig;
