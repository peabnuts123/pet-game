using System;
using System.ComponentModel.DataAnnotations;

namespace PetGame.Web
{
    public class LeaderboardTodayDto
    {
        // Required
        [Required]
        public Guid? gameId { get; set; }
        [Required]
        public int? timeZoneOffsetMinutes { get; set; }

        // Optional
        // - None at present
    }
}