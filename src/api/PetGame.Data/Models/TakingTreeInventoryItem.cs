using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PetGame.Data
{
    public class TakingTreeInventoryItem
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [JsonIgnore]
        public Guid ItemId { get; set; }
        [Required]
        [JsonIgnore]
        public Guid DonatedById { get; set; }

        // Relationships
        public Item Item { get; set; }
        public User DonatedBy { get; set; }
    }
}