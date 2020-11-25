using System;
using System.ComponentModel.DataAnnotations;
using PetGame.Data;

namespace PetGame.Web
{
    public class LeaderboardDateDto
    {
        // Required
        [Required]
        public DateTime? date { get; set; }
        [Required]
        public Guid? gameId { get; set; }
        [Required]
        public int? timeZoneOffsetMinutes { get; set; }

        // Optional
        //  - None at present
    }
}