using System;
using System.ComponentModel.DataAnnotations;

namespace PetGame.Web
{
    public class LeaderboardTopGameEntriesDto
    {
        // Required
        [Required]
        public Guid? gameId { get; set; }

        // Optional
        public int? topN { get; set; }
    }
}