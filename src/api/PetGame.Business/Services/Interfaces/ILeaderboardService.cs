using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PetGame.Data;

namespace PetGame.Business
{
    public interface ILeaderboardService
    {
        /// <summary>
        /// Get a user's scores for a game for a particular date in their local time zone
        /// </summary>
        /// <param name="userId">ID of the user</param>
        /// <param name="gameId">ID of the game</param>
        /// <param name="localDate">Local date</param>
        /// <param name="timeZoneOffsetMinutes">The timezone, specified by number of minutes ahead of UTC. e.g. UTC+10 is 600, UTC-5 is -300</param>
        /// <returns>List of all LeaderboardEntry objects that match the criteria</returns>
        Task<IList<LeaderboardEntry>> GetUserEntriesForDate(Guid userId, Guid gameId, DateTime localDate, int timeZoneOffsetMinutes);

        /// <summary>
        /// Get all scores for a particular game
        /// </summary>
        /// <param name="gameId">ID of the game</param>
        /// <returns>List of all LeaderboardEntry objects for the given game</returns>
        Task<IList<LeaderboardEntry>> GetAllEntriesForGame(Guid gameId);

        /// <summary>
        /// Submit a user's score for a particular game
        /// </summary>
        /// <param name="userId">ID of the user</param>
        /// <param name="gameId">ID of the game</param>
        /// <param name="score">The score being submitted</param>
        /// <param name="timeZoneOffsetMinutes">The timezone, specified by number of minutes ahead of UTC. e.g. UTC+10 is 600, UTC-5 is -300</param>
        /// <returns>Newly created LeaderboardEntry instance</returns>
        Task<LeaderboardEntry> SaveEntry(Guid userId, Guid gameId, int score, int timeZoneOffsetMinutes);
    }
}