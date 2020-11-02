import { observer } from 'mobx-react-lite';
import { Fragment, FunctionalComponent, h } from 'preact';

const GamesRoute = observer(() => {
  return (
    <Fragment>
      <h1>Games!</h1>
      <ul>
        <li>
          <em><small>tooooons!</small></em>
        </li>
        <li>
          <em><small>chawacters</small></em>
        </li>
        <li>
          <em><small>h-emaaail</small></em>
        </li>
      </ul>

    </Fragment>
  );
}) as FunctionalComponent;

export default GamesRoute;
