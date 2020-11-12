using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
            var filterStartLocal = new DateTime(localDate.Year, localDate.Month, localDate.Day, 0, 0, 0);
            var filterEndLocal = filterStartLocal + TimeSpan.FromDays(1);

            // Convert the local datetime to UTC
            DateTime filterStartUtc = ConvertLocalTimeToUTC(filterStartLocal, timeZoneOffsetMinutes);
            DateTime filterEndUtc = ConvertLocalTimeToUTC(filterEndLocal, timeZoneOffsetMinutes);

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
        /// Get all scores for a particular game
        /// </summary>
        /// <param name="gameId">ID of the game</param>
        /// <returns>List of all LeaderboardEntry objects for the given game</returns>
        public async Task<IList<LeaderboardEntry>> GetAllEntriesForGame(Guid gameId)
        {
            // @TODO probably take a param for "top N scores"
            return await this.db.LeaderboardEntries
                .Include((entry) => entry.User)
                .Where((entry) => entry.GameId == gameId).ToListAsync();
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
            IList<LeaderboardEntry> entriesForToday = await GetUserEntriesForDate(userId, gameId, GetLocalDateTimeNow(timeZoneOffsetMinutes), timeZoneOffsetMinutes);
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

            await this.db.AddAsync(newEntry);

            await this.db.SaveChangesAsync();

            // Fetch new record and join with game data
            // @NOTE no need to make a trip to DB if `.Include` is removed
            return this.db.LeaderboardEntries
                .Include((entry) => entry.Game)
                .Single((entry) => entry.Id == newEntry.Id);
        }

        /// <summary>
        /// Convert a local time to UTC
        /// </summary>
        /// <param name="localTime">The actual local time e.g. 8PM</param>
        /// <param name="timeZoneOffsetMinutes">The timezone, specified by number of minutes ahead of UTC. e.g. UTC+10 is 600, UTC-5 is -300</param>
        /// <returns>UTC equivalent of `localTime`</returns>
        public DateTime ConvertLocalTimeToUTC(DateTime localTime, int timeZoneOffsetMinutes)
        {
            // Ensure localTime has correct `Kind` property, which is immutable, 
            //  so we must construct a new DateTime object
            DateTime localTimeUnspecified = new DateTime(localTime.Ticks, DateTimeKind.Unspecified);
            // Create a custom timezone based on `timeZoneOffsetMinutes`
            TimeZoneInfo localTimeZone = TimeZoneInfo.CreateCustomTimeZone("LocalTime", TimeSpan.FromMinutes(timeZoneOffsetMinutes), "Local timezone", "LocalTime");
            // Convert the specified local time to UTC as if it is from the specified time zone
            DateTime utcTime = TimeZoneInfo.ConvertTimeToUtc(localTimeUnspecified, localTimeZone);

            return utcTime;
        }

        /// <summary>
        /// Get the current time in a local time zone, as specified by `timeZoneOffsetMinutes`
        /// </summary>
        /// <param name="timeZoneOffsetMinutes">The timezone, specified by number of minutes ahead of UTC. e.g. UTC+10 is 600, UTC-5 is -300</param>
        /// <returns>DateTime in local time zone</returns>
        public DateTime GetLocalDateTimeNow(int timeZoneOffsetMinutes)
        {
            // Create a custom timezone based on `tzOffsetMinutes`
            TimeZoneInfo localTimeZone = TimeZoneInfo.CreateCustomTimeZone("LocalTime", TimeSpan.FromMinutes(timeZoneOffsetMinutes), "Local timezone", "LocalTime");
            // Convert the specified local time to UTC as if it is from the specified time zone
            DateTime localTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, localTimeZone);

            return localTime;
        }
    }
}
