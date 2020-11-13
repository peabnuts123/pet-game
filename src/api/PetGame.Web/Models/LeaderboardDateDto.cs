using System;

namespace PetGame.Web
{
    public class LeaderboardDateDto
    {
        public DateTime date { get; set; }
        public Guid gameId { get; set; }
        public int timeZoneOffsetMinutes { get; set; }
    }
}