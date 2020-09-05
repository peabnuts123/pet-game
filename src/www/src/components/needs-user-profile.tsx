import { useStores } from '@app/stores';
import { FunctionalComponent, h, RenderableProps } from 'preact';
import { Fragment } from 'preact';
import LoadingSpinner from './loading-spinner';

interface Props {
  inverted?: boolean;
}

const NeedsUserProfile: FunctionalComponent<RenderableProps<Props>> = ({ children, inverted }) => {
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
};

export default NeedsUserProfile;
