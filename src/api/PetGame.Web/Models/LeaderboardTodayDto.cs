using System;

namespace PetGame.Web
{
    public class LeaderboardTodayDto
    {
        public Guid gameId { get; set; }
        public int timeZoneOffsetMinutes { get; set; }
    }
}