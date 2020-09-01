using System;
using System.ComponentModel.DataAnnotations;

namespace PetGame.Data
{
    public class Item
    {
        public static readonly Item[] ALL_ITEMS = new Item[] {
            new Item { Id = new Guid("0E21E615-B04E-4213-9D4D-E35E981B9194"), Name = "Crud" },
            new Item { Id = new Guid("0B5AAB7C-3420-4672-9A51-D8F9F8A7B97E"), Name = "Coiled Rope" },
            new Item { Id = new Guid("FEA41875-C84C-4F60-B490-8B4CD2F648E0"), Name = "Ripe Orange" },
            new Item { Id = new Guid("DAB554EE-50E6-417D-B760-2ECA12BCAE79"), Name = "Tacking Nails" },
            new Item { Id = new Guid("BECCB327-2703-4B19-8B67-BB77B63A7C0C"), Name = "Elastic Band" },
            new Item { Id = new Guid("55959D76-A087-4C60-AEA7-619F7C077C6F"), Name = "Elegant Flask" },
            new Item { Id = new Guid("F0147EE7-6FD2-409B-97BB-4796CA3EA099"), Name = "Gold Ring" },
            new Item { Id = new Guid("2EF5A467-84F1-4DBB-B353-7C14C67F0C80"), Name = "WORLDS BEST DAD Mug" },
            new Item { Id = new Guid("0504BCEA-AD14-4BD4-9263-309FADDA52A6"), Name = "Left-hand Glove" },
            new Item { Id = new Guid("9A270DA3-BCDD-46F5-9C29-AF29B34CFAC2"), Name = "Right-hand Glove" },
        };

        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
    }
}