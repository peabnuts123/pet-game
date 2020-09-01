using System;
using Microsoft.EntityFrameworkCore;

namespace PetGame.Data
{
    public class PetGameContext : DbContext
    {
        public PetGameContext(DbContextOptions<PetGameContext> options) : base(options) { }

        public PetGameContext() { }

        /// <summary>
        /// Design-time connection string for DB (e.g. running migrations). From environment variable 'ConnectionStrings__PetGameContext',
        /// which can also be used for project run-time connection (i.e. `dotnet run`)
        /// </summary>
        /// <param name="options"></param>
        protected override void OnConfiguring(DbContextOptionsBuilder options) =>
            options.UseNpgsql(Environment.GetEnvironmentVariable("ConnectionStrings__PetGameContext"));


        // Tables
        public DbSet<Item> Items { get; set; }
        public DbSet<PlayerInventoryItem> PlayerInventoryItems { get; set; }
        public DbSet<TakingTreeInventoryItem> TakingTreeInventoryItems { get; set; }
        public DbSet<User> Users { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // @TODO will one-day have to tidy this up, manage this in a sane way,
            //  rather than just putting ALL the models' logic in this one place
            modelBuilder.Entity<Item>().HasData(Item.ALL_ITEMS);
            modelBuilder.Entity<User>().HasIndex(u => u.AuthId).IsUnique();
        }
    }
}
