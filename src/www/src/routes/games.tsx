import { observer } from 'mobx-react-lite';
import { Fragment, FunctionalComponent, h } from 'preact';
import { useEffect, useRef } from 'preact/hooks';
import { getCurrentUrl } from 'preact-router';

import Logger, { LogLevel } from '@app/util/Logger';

const GamesRoute = observer(() => {
  const aspectRatioMaintainerRef = useRef<HTMLDivElement | null>(null);

  useEffect(() => {
    /** Time period to debounce resize function calls */
    const RESIZE_DEBOUNCE_TIME_MS = 30;
    /** Timer handle to last setTimeout */
    let onResizeTimer: number | undefined = undefined;

    /**
     * OnResize callback, called at most every `RESIZE_DEBOUNCE_TIME_MS`
     */
    const onResize = (): void => {
      clearTimeout(onResizeTimer);
      onResizeTimer = window.setTimeout(() => {
        // Ensure we don't do this after user has navigated to a different page
        if (getCurrentUrl().startsWith('/games') && aspectRatioMaintainerRef.current) {
          // Set the aspect ration on the "aspect ratio maintainer" element
          //  equal to the aspect ratio of the current window
          const aspectRatioMaintainerEl = aspectRatioMaintainerRef.current;
          const windowAspectRatio = window.innerHeight / window.innerWidth;
          aspectRatioMaintainerEl.style['paddingBottom'] = `${100 * windowAspectRatio}%`;
        }
      }, RESIZE_DEBOUNCE_TIME_MS);
    };

    Logger.log(LogLevel.debug, "Subscring to resize listener");

    // Subscribe to 'resize' event (and invoke immediately)
    window.addEventListener('resize', onResize);
    onResize();

    // Remove listener on leaving page
    return () => window.removeEventListener('resize', onResize);
  }, []);

  return (
    <Fragment>
      <h1>Games!</h1>

      <div class="games__aspect-maintainer" ref={aspectRatioMaintainerRef}>
        <div class="games__container">
          <iframe src="https://pet-game-dev-game-bappy-flirb.s3.ap-southeast-2.amazonaws.com/index.html" frameBorder="0" class="games__iframe"></iframe>
        </div>
      </div>
    </Fragment>
  );
}) as FunctionalComponent;

export default GamesRoute;
