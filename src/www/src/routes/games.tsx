import { observer } from 'mobx-react-lite';
import { Fragment, FunctionalComponent, h } from 'preact';
import { useEffect, useRef, useState } from 'preact/hooks';
import { getCurrentUrl } from 'preact-router';

import Logger, { LogLevel } from '@app/util/Logger';
import LeaderboardEntry from '@app/models/LeaderboardEntry';
import { useStores } from '@app/stores';
import LoadingSpinner from '@app/components/loading-spinner';

/** Time period to debounce resize function calls */
const RESIZE_DEBOUNCE_TIME_MS = 30;

/** Unique GUID for Bappy Flirb game - matches API / DB */
const BAPPY_FLIRB_GAME_ID: string = 'bf06df8d-276f-40d2-975a-f57f2042d8c2';

const NUM_TOP_SCORES: number = 20;

const GamesRoute = observer(() => {
  const { LeaderboardStore } = useStores();

  const aspectRatioMaintainerRef = useRef<HTMLDivElement | null>(null);
  const [isLoadingTopScores, setIsLoadingTopScores] = useState<boolean>(true);
  const [topScores, setTopScores] = useState<LeaderboardEntry[]>([]);

  useEffect(() => {
    // Fetch scores from API
    void LeaderboardStore.GetTopEntriesForGame(BAPPY_FLIRB_GAME_ID, NUM_TOP_SCORES)
      .then((result: LeaderboardEntry[]) => {
        setTopScores(result);
        setIsLoadingTopScores(false);
      })
      .catch((error) => {
        setIsLoadingTopScores(false);
        Logger.log(LogLevel.debug, `Failed to fetch top scores: `, error);
      });


    // Window resize logic for iframe
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

    // Subscribe to 'resize' event (and invoke immediately)
    window.addEventListener('resize', onResize);
    onResize();

    // Remove listener on leaving page
    return () => window.removeEventListener('resize', onResize);
  }, [LeaderboardStore]);

  return (
    <Fragment>
      <h1>Games</h1>
      <p>Here you can play various games with your pets. There aren&apos;t very many games here yet 😢 but there will be more in the future!</p>

      <div class="games__aspect-maintainer" ref={aspectRatioMaintainerRef}>
        <div class="games__container">
          <iframe src="https://pet-game-dev-game-bappy-flirb.s3.ap-southeast-2.amazonaws.com/index.html" frameBorder="0" class="games__iframe"></iframe>
        </div>
      </div>

      <h2>Leaderboard</h2>
      <div class="games__leaderboard">
        {isLoadingTopScores ? (
          <LoadingSpinner />
        ) : (
            (topScores.length === 0 ? (
              <p class="games__leaderboard__no-scores-message">Nobody has submitted a score for this game. Be the first!</p>
            ) : (
                <table class="games__leaderboard__table">
                  <thead>
                    <tr>
                      <th>#</th>
                      <th>Score</th>
                      <th class="games__leaderboard__table__user--th">User</th>
                    </tr>
                  </thead>
                  <tbody>
                    {topScores.map((score, index) => (
                      <tr key={score.id}>
                        <td>{index + 1}</td>
                        <td>{score.score.toLocaleString()}</td>
                        <td class="games__leaderboard__table__user--td">{score.user.username}</td>
                      </tr>
                    ))}
                  </tbody>
                </table>
              )
            )
          )
        }
      </div>
    </Fragment>
  );
}) as FunctionalComponent;

export default GamesRoute;
