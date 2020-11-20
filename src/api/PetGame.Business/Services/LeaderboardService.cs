using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetGame.Common;
using PetGame.Data;

namespace PetGame.Business
{
    public class LeaderboardService : ILeaderboardService
    {
        // Static Config
        /// <summary>
        /// The maximum number of times a user can submit a score for a game 
        /// on any given calendar day within their local time zone (NOT 24-hour period)
        /// </summary>
        public static readonly int MAX_SUBMISSIONS_PER_DAY = 3;


        private PetGameContext db;
        private ILogger<LeaderboardService> logger;

        public LeaderboardService(PetGameContext db, ILogger<LeaderboardService> logger)
        {
            this.db = db;
            this.logger = logger;
        }

        /// <summary>
        /// Get a user's scores for a game for a particular date in their local time zone
        /// </summary>
        /// <param name="userId">ID of the user</param>
        /// <param name="gameId">ID of the game</param>
        /// <param name="localDate">Local date</param>
        /// <param name="timeZoneOffsetMinutes">The timezone, specified by number of minutes ahead of UTC. e.g. UTC+10 is 600, UTC-5 is -300</param>
        /// <returns>List of all LeaderboardEntry objects that match the criteria</returns>
        public async Task<IList<LeaderboardEntry>> GetUserEntriesForDate(Guid userId, Guid gameId, DateTime localDate, int timeZoneOffsetMinutes)
        {
            // Extract midnight-midnight datetimes from the local time (the time is ignored)
            var filterStartLocal = localDate.With(second: 0, minute: 0, hour: 0);
            var filterEndLocal = filterStartLocal + TimeSpan.FromDays(1);

            // Convert the local datetime to UTC
            DateTime filterStartUtc = DateTimeUtility.ConvertLocalTimeToUTC(filterStartLocal, timeZoneOffsetMinutes);
            DateTime filterEndUtc = DateTimeUtility.ConvertLocalTimeToUTC(filterEndLocal, timeZoneOffsetMinutes);

            return await this.db.LeaderboardEntries.Where((entry) =>
                // This user's scores
                entry.UserId == userId &&
                // for this game
                entry.GameId == gameId &&
                // within this timeframe (a certain day)
                entry.EntryTimeUTC >= filterStartUtc &&
                entry.EntryTimeUTC < filterEndUtc
            ).ToListAsync();
        }

        /// <summary>
        /// Get top N scores for a particular game
        /// </summary>
        /// <param name="gameId">ID of the game</param>
        /// <param name="topN">Number of top scores to get</param>
        /// <returns>List of N LeaderboardEntry objects for the given game</returns>
        public async Task<IList<LeaderboardEntry>> GetTopEntriesForGame(Guid gameId, int topN)
        {
            return await this.db.LeaderboardEntries
                .Include((entry) => entry.User)
                .Where((entry) => entry.GameId == gameId)
                .OrderByDescending((entry) => entry.Score)
                .Take(topN)
                .ToListAsync();
        }

        /// <summary>
        /// Submit a user's score for a particular game
        /// </summary>
        /// <param name="userId">ID of the user</param>
        /// <param name="gameId">ID of the game</param>
        /// <param name="score">The score being submitted</param>
        /// <param name="timeZoneOffsetMinutes">The timezone, specified by number of minutes ahead of UTC. e.g. UTC+10 is 600, UTC-5 is -300</param>
        /// <returns>Newly created LeaderboardEntry instance</returns>
        public async Task<LeaderboardEntry> SaveEntry(Guid userId, Guid gameId, int score, int timeZoneOffsetMinutes)
        {
            // Test to see if the user has reached their max number of submissions for today
            IList<LeaderboardEntry> entriesForToday = await GetUserEntriesForDate(userId, gameId, DateTimeUtility.GetLocalDateTimeNow(timeZoneOffsetMinutes), timeZoneOffsetMinutes);
            if (entriesForToday.Count >= MAX_SUBMISSIONS_PER_DAY)
            {
                throw new InvalidOperationException($"Maximum submissions for today reached: {entriesForToday.Count} of {MAX_SUBMISSIONS_PER_DAY}");
            }

            // Construct and save new entry
            LeaderboardEntry newEntry = new LeaderboardEntry
            {
                UserId = userId,
                GameId = gameId,
                Score = score,
                EntryTimeUTC = DateTime.UtcNow,
            };

            await this.db.LeaderboardEntries.AddAsync(newEntry);

            await this.db.SaveChangesAsync();

            // Fetch new record and join with game data
            // @NOTE no need to make a trip to DB if `.Include` is removed
            return this.db.LeaderboardEntries
                .Include((entry) => entry.Game)
                .Single((entry) => entry.Id == newEntry.Id);
        }
    }
}
