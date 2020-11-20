using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using PetGame.Common;
using PetGame.Common.Test;
using PetGame.Data;

namespace PetGame.Business.Test
{
    [TestFixture]
    public class LeaderboardServiceTests
    {
        // @TODO yeah sorry about these hard-coded values for expectedNumberOfEntries :/
        // Not sure how to do a better system without making a sophisticated mock
        [TestCase(-1, 3)]   // Yesterday
        [TestCase(0, 3)]    // Today
        [TestCase(1, 0)]    // Tomorrow
        [Description("Calling GetUserEntriesForDate() returns the right entries, even if they have the same or different dates in the DB")]
        public async Task GetUserEntriesForDate_WithCertainOffset_ReturnsCorrectEntries(int offsetDays, int expectedNumberOfEntries)
        {
            // SETUP
            var db = MockDbUtility.GetMockDb();

            // Seed mock users
            var mockUsers = MockDbUtility.GetMockUsers();
            await MockDbUtility.SetupMockUsers(db, mockUsers);
            Guid mockUserId = mockUsers[0].Id;

            // Seed mock games
            var mockGames = MockDbUtility.GetMockGames();
            await MockDbUtility.SetupMockGames(db, mockGames);
            Guid mockGameId = mockGames[0].Id;

            // Seed mock leaderboard entries
            int mockTimeZoneOffsetMinutes = 780;
            var mockLeaderboardEntries = MockDbUtility.GetMockLeaderboardEntries(mockUserId, mockGameId, mockTimeZoneOffsetMinutes);
            await MockDbUtility.SetupMockLeaderboardEntries(db, mockLeaderboardEntries);

            // Create mock leaderboardService
            var loggerMock = new Mock<ILogger<LeaderboardService>>();
            LeaderboardService leaderboardService = new LeaderboardService(db, loggerMock.Object);

            // TEST
            // Get user entries for specific 
            IList<LeaderboardEntry> result = await leaderboardService.GetUserEntriesForDate(mockUserId, mockGameId, DateTime.Now + TimeSpan.FromDays(offsetDays), mockTimeZoneOffsetMinutes);

            // ASSERT
            result.Count.Should().Be(expectedNumberOfEntries);
        }

        // @TODO derive these GUIDs from the mock data rather than copypaste
        [TestCase("cbb8e1ee-e053-4e7e-9c82-7167a0d5eb8c", 6)]
        [TestCase("33ff643b-3b5d-4f51-b34e-218155a11462", 0)]
        [Description("Calling GetAllEntriesForGame() with returns all entries in the DB with that gameId")]
        public async Task GetAllEntriesForGame_WithSpecifiedGame_ProducesCorrectResult(Guid mockGameId, int expectedNumberOfEntries)
        {
            // SETUP
            var db = MockDbUtility.GetMockDb();

            // Seed mock users
            var mockUsers = MockDbUtility.GetMockUsers();
            await MockDbUtility.SetupMockUsers(db, mockUsers);
            Guid mockUserId = mockUsers[0].Id;

            // Seed mock games
            var mockGames = MockDbUtility.GetMockGames();
            await MockDbUtility.SetupMockGames(db, mockGames);
            Guid seedGameId = mockGames[0].Id;

            // Seed mock leaderboard entries
            int mockTimeZoneOffsetMinutes = 780;
            var mockLeaderboardEntries = MockDbUtility.GetMockLeaderboardEntries(mockUserId, seedGameId, mockTimeZoneOffsetMinutes);
            await MockDbUtility.SetupMockLeaderboardEntries(db, mockLeaderboardEntries);

            // Create mock leaderboardService
            var loggerMock = new Mock<ILogger<LeaderboardService>>();
            LeaderboardService leaderboardService = new LeaderboardService(db, loggerMock.Object);

            // TEST
            IList<LeaderboardEntry> result = await leaderboardService.GetAllEntriesForGame(mockGameId);

            // ASSERT
            result.Count.Should().Be(expectedNumberOfEntries);
        }

        // @TODO derive these GUIDs from the mock data rather than copypaste
        [TestCase("cbb8e1ee-e053-4e7e-9c82-7167a0d5eb8c", 3)]
        [Description("Calling SaveEntry() when maximum number of submissions for a day has been reached throws an exception")]
        public async Task SaveEntry_WhenMaximumSubmissionsForTodayReached_ThrowsInvalidOperationException(Guid mockGameId, int expectedNumberOfEntriesToday)
        {
            // SETUP
            var db = MockDbUtility.GetMockDb();

            // Seed mock users
            var mockUsers = MockDbUtility.GetMockUsers();
            await MockDbUtility.SetupMockUsers(db, mockUsers);
            Guid mockUserId = mockUsers[0].Id;

            // Seed mock games
            var mockGames = MockDbUtility.GetMockGames();
            await MockDbUtility.SetupMockGames(db, mockGames);
            Guid seedGameId = mockGames[0].Id;

            // Seed mock leaderboard entries
            int mockTimeZoneOffsetMinutes = 780;
            var mockLeaderboardEntries = MockDbUtility.GetMockLeaderboardEntries(mockUserId, seedGameId, mockTimeZoneOffsetMinutes);
            await MockDbUtility.SetupMockLeaderboardEntries(db, mockLeaderboardEntries);

            // Create mock leaderboardService
            var loggerMock = new Mock<ILogger<LeaderboardService>>();
            LeaderboardService leaderboardService = new LeaderboardService(db, loggerMock.Object);

            // TEST
            Func<Task> testFunc = async () =>
            {
                await leaderboardService.SaveEntry(mockUserId, mockGameId, 100, mockTimeZoneOffsetMinutes);
            };

            // ASSERT
            await testFunc.Should().ThrowAsync<InvalidOperationException>().WithMessage($"Maximum submissions for today reached: {expectedNumberOfEntriesToday} of {LeaderboardService.MAX_SUBMISSIONS_PER_DAY}");
        }

        [Test]
        [Description("Calling SaveEntry() under ideal circumstances successfully creates a new entry with the right data")]
        public async Task SaveEntry_WithValidData_CreatesSuccessfully()
        {
            // SETUP
            var db = MockDbUtility.GetMockDb();

            // Seed mock users
            var mockUsers = MockDbUtility.GetMockUsers();
            await MockDbUtility.SetupMockUsers(db, mockUsers);
            Guid mockUserId = mockUsers[0].Id;

            // Seed mock games
            var mockGames = MockDbUtility.GetMockGames();
            await MockDbUtility.SetupMockGames(db, mockGames);
            Guid mockGameId = mockGames[0].Id;

            // Seed mock leaderboard entries
            int mockTimeZoneOffsetMinutes = 780;
            var mockLeaderboardEntries = new List<LeaderboardEntry>();
            await MockDbUtility.SetupMockLeaderboardEntries(db, mockLeaderboardEntries);

            // Create mock leaderboardService
            var loggerMock = new Mock<ILogger<LeaderboardService>>();
            LeaderboardService leaderboardService = new LeaderboardService(db, loggerMock.Object);

            // Mock inputs
            int mockScore = 200;

            // TEST
            var result = await leaderboardService.SaveEntry(mockUserId, mockGameId, mockScore, mockTimeZoneOffsetMinutes);
            var allEntries = await db.LeaderboardEntries.ToListAsync();

            // ASSERT
            allEntries.Count.Should().Be(1, "there should be 1 leaderboard entry created");
            result.Should().NotBeNull("result record should be defined");
            result.Game.Should().NotBeNull("game record should be included in result");
            result.UserId.Should().Be(mockUserId);
            result.GameId.Should().Be(mockGameId);
            result.Score.Should().Be(mockScore);
            result.EntryTimeUTC.Should().BeCloseTo(DateTime.UtcNow, 500);
        }
    }
}
