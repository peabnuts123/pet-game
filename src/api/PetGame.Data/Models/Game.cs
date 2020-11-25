using System;
using System.ComponentModel.DataAnnotations;

namespace PetGame.Data
{
    public class Game
    {
        public static readonly Game[] ALL_GAMES = new Game[] {
            new Game { Id = new Guid("bf06df8d-276f-40d2-975a-f57f2042d8c2"), Name = "Bappy Flirb" },
        };

        [Key]
        public Guid Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
    }
}