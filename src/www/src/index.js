import 'mobx-react-lite/batchingForReactDom';

import Config from '@app/config';
import "@app/style/index.scss";
import App from "@app/app.tsx";
import Logger from '@app/util/Logger';

Logger.setLogLevel(Config.LogLevel);


export default App;
