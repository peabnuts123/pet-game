using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PetGame.Data
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string AuthId { get; set; }
        [Required]
        [MaxLength(40)]
        public string Username { get; set; }

        // Relationships
        public List<PlayerInventoryItem> Inventory { get; set; }
    }
}