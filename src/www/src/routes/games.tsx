import { observer } from 'mobx-react-lite';
import { Fragment, FunctionalComponent, h } from 'preact';
import { useEffect, useRef } from 'preact/hooks';
import { getCurrentUrl } from 'preact-router';

import Logger, { LogLevel } from '@app/util/Logger';

const GamesRoute = observer(() => {
  const aspectRatioMaintainerRef = useRef<HTMLDivElement | null>(null);

  useEffect(() => {
    window.addEventListener('resize', onResize);
    onResize();

    const RESIZE_DEBOUNCE_TIME_MS = 30;
    let onResizeTimer: number | undefined = undefined;

    return () => window.removeEventListener('resize', onResize);

    function onResize(): void {
      clearTimeout(onResizeTimer);
      onResizeTimer = window.setTimeout(() => {
        // Ensure we don't do this after user has navigated
        if (getCurrentUrl().startsWith('/games') && aspectRatioMaintainerRef.current) {
          Logger.log(LogLevel.debug, "Game route is resizing", getCurrentUrl());
          const aspectRatioMaintainerEl = aspectRatioMaintainerRef.current;
          aspectRatioMaintainerEl.style['paddingBottom'] = `${100 * (window.innerHeight / window.innerWidth)}%`;
        }
      }, RESIZE_DEBOUNCE_TIME_MS);
    }
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
