using System;
using System.Text.Json.Serialization;

namespace PetGame.Data
{
    public class TakingTreeInventoryItem
    {
        public Guid Id { get; set; }

        // Relationships
        [JsonIgnore]
        public Guid ItemId { get; set; }
        public Item Item { get; set; }
    }
}