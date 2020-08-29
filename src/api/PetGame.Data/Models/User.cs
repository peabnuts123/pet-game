using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PetGame.Data
{
    public class User
    {
        [Key]
        public string Id { get; set; }
        public string Username { get; set; }

        // Relationships
        public List<PlayerInventoryItem> Inventory { get; set; }
    }
}