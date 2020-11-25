using System;
using System.ComponentModel.DataAnnotations;

namespace PetGame.Web
{
    public class LeaderboardTopUserGameEntriesDto
    {
        // Required
        [Required]
        public Guid? gameId { get; set; }

        // Optional
        public Guid? userId { get; set; }
        public int? topN { get; set; }
    }
}