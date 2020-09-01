using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PetGame.Data
{
    public class PlayerInventoryItem
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public int Count { get; set; }
        [Required]
        [JsonIgnore]
        public Guid ItemId { get; set; }
        [Required]
        [JsonIgnore]
        public Guid UserId { get; set; }

        // Relationships
        public Item Item { get; set; }

        [JsonIgnore]
        public User User { get; set; }
    }
}