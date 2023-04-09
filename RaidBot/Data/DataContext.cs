using DSharpPlus.Entities;
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
        public DbSet<ActiveRaids> ActiveRaids { get; set; }
        public DbSet<RaidSettings> RaidSettings { get; set; }
        public DbSet<GuildMember> GuildMember { get; set; }
        public DbSet<TierRole> TierRoles { get; set; }
        public DbSet<AssignedTierRoles> AssignedTierRoles { get; set; }
        public DbSet<UserRaidHistory> UserRaidHistories { get; set; }
        public DbSet<RaidStatsByRole> RaidStatsByRoles { get; set; }
        public DbSet<RaidStatsByTier> RaidStatsByTiers { get; set; }
        public DbSet<Roles> Roles { get; set; }
        public DbSet<SignUpEmoji> SignUpEmojis { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GuildSettings>()
                .HasKey(g => g.GuildId);
            
            modelBuilder.Entity<GuildSettings>()
                .HasMany(g => g.ActiveRaids!)
                .WithOne(a => a.GuildSettings!)
                .HasForeignKey(a => a.GuildId)
                .OnDelete(DeleteBehavior.Cascade);
            // start of ActivityRaids table relations
            modelBuilder.Entity<ActiveRaids>()
                .HasKey(a => a.Id);

            modelBuilder.Entity<ActiveRaids>()
                .HasOne(a => a.GuildSettings!)
                .WithMany(g => g.ActiveRaids!)
                .HasForeignKey(a => a.GuildId)
                .OnDelete(DeleteBehavior.Cascade);
            
            modelBuilder.Entity<RaidSettings>()
                .HasKey(r => r.Id);

            modelBuilder.Entity<RaidSettings>()
                .HasOne<ActiveRaids>(t => t.ActiveRaids)
                .WithMany(t => t.Raids)
                .HasForeignKey(r => r.ActiveRaidId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<RaidRoles>()
                .HasKey(r => r.Id);

            modelBuilder.Entity<RaidRoles>()
                .HasOne<RaidSettings>(r => r.RaidSettings)
                .WithOne()
                .HasForeignKey<RaidRoles>(r => r.RoleSettingsId);

            // modelBuilder.Entity<ActiveRaids>()
            //     .HasOne(a => a.GuildSettings)
            //     .WithMany()
            //     .HasForeignKey(x => x.GuildId)
            //     .OnDelete(DeleteBehavior.Cascade);
            //
            // modelBuilder.Entity<RaidSettings>()
            //     .HasOne<ActiveRaids>(t => t.ActiveRaids)
            //     .WithMany(t => t.Raids)
            //     .HasForeignKey(r => r.RaidId)
            //     .OnDelete(DeleteBehavior.Cascade);

            
            modelBuilder.Entity<Roles>()
                .HasOne<RaidSettings>(r => r.RaidSettings)
                .WithMany()
                .HasForeignKey(r => r.RaidSettingsId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SignUpEmoji>()
                .HasOne<RaidSettings>(s => s.RaidSettings)
                .WithMany()
                .HasForeignKey(s => s.RaidSettingsId)
                .OnDelete(DeleteBehavior.Cascade);
            // end of ActivityRaids relationship
            // Beginning of Tier Role relationships
            modelBuilder.Entity<TierRole>()
                .HasOne<GuildSettings>(t => t.GuildSettings)
                .WithMany()
                .HasForeignKey(t => t.GuildId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<AssignedTierRoles>()
                .HasOne<TierRole>(t => t.TierRole)
                .WithMany(t => t.Roles)
                .HasForeignKey(r => r.TierRoleId)
                .OnDelete(DeleteBehavior.Cascade);
            // end of TierRole relationship
            // beggniing of guild member relations
            modelBuilder.Entity<GuildMember>()
                .HasOne(g => g.GuildSettings)
                .WithMany()
                .HasForeignKey(g => g.GuildId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserRaidHistory>()
                .HasOne<GuildMember>(u => u.GuildMember)
                .WithMany()
                .HasForeignKey(u => u.GuildMemberId)
                .OnDelete(DeleteBehavior.Cascade);
            
            modelBuilder.Entity<RaidStatsByRole>()
                .HasOne<UserRaidHistory>(t => t.UserRaidHistory)
                .WithMany()
                .HasForeignKey(r => r.RaidHistoryId)
                .OnDelete(DeleteBehavior.Cascade);
            
            modelBuilder.Entity<RaidStatsByTier>()
                .HasOne<UserRaidHistory>(t => t.UserRaidHistory)
                .WithMany()
                .HasForeignKey(r => r.RaidHistoryId)
                .OnDelete(DeleteBehavior.Cascade);
            // end of GuildMember relations
            
            // Ignores DSharpPlus DiscordRoleTags Entity
            modelBuilder.Ignore<DiscordRoleTags>();

        }
        
    }
}