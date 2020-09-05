import { useStores } from '@app/stores';
import { observer } from 'mobx-react-lite';
import { h, RenderableProps } from 'preact';
import { Fragment } from 'preact';
import LoadingSpinner from './loading-spinner';

interface Props {
  inverted?: boolean;
}

const NeedsUserProfile = observer(({ children, inverted }: RenderableProps<Props>) => {
  const { UserStore } = useStores();

  return (
    <Fragment>
      {UserStore.isFetchingUserProfile && (
        <LoadingSpinner inverted={inverted} />
      )}

      {UserStore.hasFetchedUserProfile && (
        Array.isArray(children) ? (
          children.map((child) => child)
        ) : children
      )}
    </Fragment>
  );
});

export default NeedsUserProfile;
