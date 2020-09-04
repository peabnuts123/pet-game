using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace PetGame.Data
{
    public class PetGameContext : DbContext
    {
        public PetGameContext(DbContextOptions<PetGameContext> options) : base(options) { }

        public PetGameContext() { }

        // @TODO can this just move to PetGame.Web and run migrations from there?
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            // Read general configuration from config files (if any)
            //  as well as environment variables
            IConfiguration Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("_secrets.json", optional: true)
                .AddJsonFile($"_secrets.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

            // Parse `postgres://[user]:[password]@[host]/[database]`-formatted connection string
            string databaseUrl = Configuration["DATABASE_URL"];
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


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // @TODO will one-day have to tidy this up, manage this in a sane way,
            //  rather than just putting ALL the models' logic in this one place
            modelBuilder.Entity<Item>().HasData(Item.ALL_ITEMS);
            modelBuilder.Entity<User>().HasIndex(u => u.AuthId).IsUnique();
        }
    }
}
