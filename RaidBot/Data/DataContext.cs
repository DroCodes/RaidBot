using Microsoft.EntityFrameworkCore;
using RaidBot.entities;

namespace RaidBot.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<GuildSettings> GuildSettings { get; set; }
        public DbSet<RaidSettings> RaidSettings { get; set; }
        public DbSet<TierRole> TierRoles { get; set; }
        public DbSet<DiscordRoles> DiscordRoles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TierRole>()
                .HasMany(t => t.Roles)
                .WithOne()
                .HasForeignKey("TierRoleId")
                .OnDelete(DeleteBehavior.Cascade);
        }

    }
}