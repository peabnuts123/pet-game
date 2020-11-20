using System;
using System.Collections.Generic;
using PetGame.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace PetGame.Common.Test
{
    public static class MockDbUtility
    {
        public static PetGameContext GetMockDb()
        {
            var options = new DbContextOptionsBuilder<PetGameContext>()
                .UseInMemoryDatabase($"PetGame-{Guid.NewGuid()}")
                .Options;

            return new PetGameContext(options);
        }


        public async static Task SetupMockUsers(PetGameContext db, List<User> mockUsers)
        {
            await db.Users.AddRangeAsync(mockUsers);
            await db.SaveChangesAsync();
        }

        public async static Task SetupMockGames(PetGameContext db, List<Game> mockGames)
        {
            await db.Games.AddRangeAsync(mockGames);
            await db.SaveChangesAsync();
        }

        public async static Task SetupMockLeaderboardEntries(PetGameContext db, List<LeaderboardEntry> mockLeaderboardEntries)
        {
            await db.AddRangeAsync(mockLeaderboardEntries);
            await db.SaveChangesAsync();
        }


        // @NOTE when writing mocks, do not make them depend on each other directly.
        // Instead. Pass foreign keys in as parameters.
        // If you depend on them directly, the references / properties may not match
        //  i.e. the tests will be depending on a different instance than referenced here
        //  (since every invocation of GetMock___() returns a new instance)

        /// <summary>
        /// Create a set of mock games.
        /// </summary>
        public static List<Game> GetMockGames() => new List<Game>
        {
            new Game { Id = new Guid("cbb8e1ee-e053-4e7e-9c82-7167a0d5eb8c"), Name = "Mock game 1" },
            new Game { Id = new Guid("33ff643b-3b5d-4f51-b34e-218155a11462"), Name = "Mock game 2" },
            new Game { Id = new Guid("ed9b74d9-0ff1-47c6-94ef-ace6ac48bb27"), Name = "Mock game 3" },
        };

        /// <summary>
        /// Create a set of mock users.
        /// </summary>
        public static List<User> GetMockUsers() => new List<User>
        {
            new User {
                Id = new Guid("baa3c9b4-a313-4525-8fb2-93b05f7ba1c0"),
                AuthId = "google-oauth2|109182735263122614398",
                Username = "mock_user_1",
            },
            new User {
                Id = new Guid("8aa5a6da-0cb8-4a86-b0e3-e3ff5ea2aa02"),
                AuthId = "google-oauth2|893564287423451213432",
                Username = "mock_user_2",
            },
        };

        /// <summary>
        /// Create a set of mock leaderboard entries.
        /// Contains a smattering of entries for yesterday and today.
        /// @TODO add some other games in here and maybe other users?
        /// </summary>
        /// <param name="userId">The user ID to include in the mock entries</param>
        /// <param name="gameId">The game ID to include in the mock entries</param>
        /// <param name="timeZoneOffsetMinutes">Used to compute mock entries for yesterday / today</param>
        public static List<LeaderboardEntry> GetMockLeaderboardEntries(Guid userId, Guid gameId, int timeZoneOffsetMinutes) => new List<LeaderboardEntry> {
                new LeaderboardEntry {
                    Id = new Guid("e8ea7a5d-4fb4-42a6-9f4a-5bae11c2e75d"),
                    GameId = gameId,
                    Score = 100,
                    UserId = userId,
                    EntryTimeUTC = DateTimeUtility.ConvertLocalTimeToUTC(
                        // Yesterday 9pm
                        DateTime.Now.With(second: 0, minute: 0, hour: 21) - TimeSpan.FromDays(1),
                        timeZoneOffsetMinutes
                    ),
                },
                new LeaderboardEntry {
                    Id = new Guid("b4037cca-5ce5-42b0-b928-e25bdca47cbf"),
                    GameId = gameId,
                    Score = 200,
                    UserId = userId,
                    EntryTimeUTC = DateTimeUtility.ConvertLocalTimeToUTC(
                        // Yesterday 10pm
                        DateTime.Now.With(second: 0, minute: 0, hour: 22) - TimeSpan.FromDays(1),
                        timeZoneOffsetMinutes
                    ),
                },
                new LeaderboardEntry {
                    Id = new Guid("ac448397-5b45-49ef-97ad-e638499fb2fe"),
                    GameId = gameId,
                    Score = 300,
                    UserId = userId,
                    EntryTimeUTC = DateTimeUtility.ConvertLocalTimeToUTC(
                        // Yesterday 11pm
                        DateTime.Now.With(second: 0, minute: 0, hour: 23) - TimeSpan.FromDays(1),
                        timeZoneOffsetMinutes
                    ),
                },
                new LeaderboardEntry {
                    Id = new Guid("cc7ac70f-1e68-4e78-907f-02abaa7ee1e4"),
                    GameId = gameId,
                    Score = 100,
                    UserId = userId,
                    EntryTimeUTC = DateTimeUtility.ConvertLocalTimeToUTC(
                        // Today 1am
                        DateTime.Now.With(second: 0, minute: 0, hour: 1),
                        timeZoneOffsetMinutes
                    ),
                },
                new LeaderboardEntry {
                    Id = new Guid("25fb0dde-423f-4c2e-b793-3bc16fcb5adf"),
                    GameId = gameId,
                    Score = 200,
                    UserId = userId,
                    EntryTimeUTC = DateTimeUtility.ConvertLocalTimeToUTC(
                        // Today 2am
                        DateTime.Now.With(second: 0, minute: 0, hour: 2),
                        timeZoneOffsetMinutes
                    ),
                },
                new LeaderboardEntry {
                    Id = new Guid("7a563cea-6b11-4597-82f9-6e93ff04768a"),
                    GameId = gameId,
                    Score = 300,
                    UserId = userId,
                    EntryTimeUTC = DateTimeUtility.ConvertLocalTimeToUTC(
                        // Today 3am
                        DateTime.Now.With(second: 0, minute: 0, hour: 3),
                        timeZoneOffsetMinutes
                    ),
                },
            };
    }
}