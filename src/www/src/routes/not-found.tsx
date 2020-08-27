import { Fragment, FunctionalComponent, h } from "preact";
import { Link } from "preact-router/match";

const NotFoundRoute = (() => {
  return (
    <Fragment>
      <h1>Error 404</h1>

      <p>That page doesn&apos;t exist.</p>

      <Link href="/">
        <h4>Back to Home</h4>
      </Link>
    </Fragment>
  );
}) as FunctionalComponent;

export default NotFoundRoute;
