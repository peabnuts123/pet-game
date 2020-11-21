import Api from '@app/services/api';
import Endpoints from '@app/constants/endpoints';
import LeaderboardEntry, { LeaderboardDateRequestDto, LeaderboardTodayRequestDto, LeaderboardEntryDto } from '@app/models/LeaderboardEntry';
import { getLocalTimeZoneOffsetMinutes } from '@app/util/DateTimeUtilities';

import Store from './base/store';

class LeaderboardStore extends Store {

  /**
   * Get the current user's scores they have submitted today for a specific game
   * @param gameId The ID of the game
   */
  public async getEntriesForToday(gameId: string): Promise<LeaderboardEntry[]> {
    const dto: LeaderboardTodayRequestDto = {
      gameId,
      timeZoneOffsetMinutes: getLocalTimeZoneOffsetMinutes(),
    };

    const leaderboardEntryDtos = await Api.postJson<LeaderboardEntryDto[]>(Endpoints.Leaderboard.getEntriesForToday(), {
      body: dto,
    });

    return leaderboardEntryDtos.map((dto) => new LeaderboardEntry(dto));
  }

  /**
   * Get the current user's scores they have submitted for a specific game, for a specific date
   * @param gameId The ID of the game
   * @param date The date to query for (NOTE: the time component does not matter)
   */
  public async GetEntriesForDate(gameId: string, date: Date): Promise<LeaderboardEntry[]> {
    const dto: LeaderboardDateRequestDto = {
      gameId,
      date: date.toISOString(),
      timeZoneOffsetMinutes: getLocalTimeZoneOffsetMinutes(),
    };

    const leaderboardEntryDtos = await Api.postJson<LeaderboardEntryDto[]>(Endpoints.Leaderboard.getEntriesForDate(), {
      body: dto,
    });

    return leaderboardEntryDtos.map((dto) => new LeaderboardEntry(dto));
  }

  /**
   * Get the top N scores for a specific game. If `topN` is not provided, the API's default will be used instead.
   * 
   * @param gameId The ID of the game
   * @param topN Number of scores to get
   */
  public async GetTopEntriesForGame(gameId: string, topN?: number): Promise<LeaderboardEntry[]> {
    const leaderboardEntryDtos = await Api.get<LeaderboardEntryDto[]>(Endpoints.Leaderboard.getTopEntriesForGame(gameId, topN));

    return leaderboardEntryDtos.map((dto) => new LeaderboardEntry(dto));
  }
}

export default LeaderboardStore;
