using System;

namespace PetGame.Web
{
    public class LeaderboardEntryDto
    {
        public Guid gameId { get; set; }
        public int score { get; set; }
        public int timeZoneOffsetMinutes { get; set; }
    }
}