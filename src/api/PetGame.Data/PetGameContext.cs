using System;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using PetGame.Config;

namespace PetGame.Data
{
    public class PetGameContext : DbContext
    {
        public PetGameContext(DbContextOptions<PetGameContext> options) : base(options) { }

        public PetGameContext() { }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            // Parse `postgres://[user]:[password]@[host]/[database]`-formatted connection string
            string databaseUrl = Configuration.Base["DATABASE_URL"];
            Uri databaseUri = new Uri(databaseUrl);
            string[] userInfo = Uri.UnescapeDataString(databaseUri.UserInfo).Split(':');

            var builder = new NpgsqlConnectionStringBuilder
            {
                Host = databaseUri.Host,
                Port = databaseUri.Port,
                Username = userInfo[0],
                Password = userInfo[1],
                Database = databaseUri.LocalPath.TrimStart('/'),
                SslMode = SslMode.Prefer,
                TrustServerCertificate = true, // @TODO validate server certificate
            };
            options.UseNpgsql(builder.ToString());
        }


        // Tables
        public DbSet<Item> Items { get; set; }
        public DbSet<PlayerInventoryItem> PlayerInventoryItems { get; set; }
        public DbSet<TakingTreeInventoryItem> TakingTreeInventoryItems { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<LeaderboardEntry> LeaderboardEntries { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // @TODO will one-day have to tidy this up, manage this in a sane way,
            //  rather than just putting ALL the models' logic in this one place
            modelBuilder.Entity<Item>().HasData(Item.ALL_ITEMS);
            modelBuilder.Entity<User>().HasIndex(u => u.AuthId).IsUnique();
            modelBuilder.Entity<Game>().HasData(Game.ALL_GAMES);
        }
    }
}
