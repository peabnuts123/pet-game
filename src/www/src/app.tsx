import { FunctionalComponent, h } from "preact";
import { Route, Router } from "preact-router";

import Home from "@app/routes/home";
import Profile from "@app/routes/profile";
import NotFoundPage from "@app/routes/notfound";
import Header from "@app/components/header";

// eslint-disable-next-line @typescript-eslint/no-explicit-any
if ((module as any).hot) {
  // tslint:disable-next-line:no-var-requires
  require("preact/debug");
}

const App: FunctionalComponent = () => {
  return (
    <div id="app">
      <Header />
      <Router>
        <Route path="/" component={Home} />
        <Route path="/profile/" component={Profile} user="me" />
        <Route path="/profile/:user" component={Profile} />
        <NotFoundPage default />
      </Router>
    </div>
  );
};

export default App;
