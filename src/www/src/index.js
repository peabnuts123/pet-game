import 'mobx-react-lite/batchingForReactDom';

import "@app/style/index.scss";
import App from "@app/app.tsx";
import Logger, { LogLevel } from '@app/util/Logger';

Logger.setLogLevel(LogLevel.debug);


export default App;
