using System;
using System.Text.Json.Serialization;

namespace PetGame.Data
{
    public class PlayerInventoryItem
    {
        public Guid Id { get; set; }
        public Item Item { get; set; }
        public int Count { get; set; }

        // Relationships
        [JsonIgnore]
        public string UserId { get; set; }
        [JsonIgnore]
        public User User { get; set; }
    }
}