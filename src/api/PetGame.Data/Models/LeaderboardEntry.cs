using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace PetGame.Data
{
    public class LeaderboardEntry
    {
        // Source: https://en.wikipedia.org/wiki/List_of_UTC_time_offsets
        public static readonly int MIN_TIME_ZONE_OFFSET_HOURS = -12;
        public static readonly int MAX_TIME_ZONE_OFFSET_HOURS = 14;

        [Key]
        public Guid Id { get; set; }
        [Required]
        public int Score { get; set; }
        [Required]
        [JsonIgnore]
        public Guid UserId { get; set; }
        [Required]
        [JsonIgnore]
        public Guid GameId { get; set; }
        [Required]
        // @TODO readonly
        public DateTime EntryTimeUTC { get; set; }

        // Relationships
        public User User { get; set; }
        public Game Game { get; set; }
    }
}