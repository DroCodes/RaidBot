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
        public DbSet<RaidSettings> RaidSettings { get; set; }
        public DbSet<GuildMember> GuildMember { get; set; }
        public DbSet<TierRole> TierRoles { get; set; }
        public DbSet<AssignedTierRoles> AssignedTierRoles { get; set; }
        public DbSet<UserRaidHistory> UserRaidHistories { get; set; }
        public DbSet<RaidStatsByRole> RaidStatsByRoles { get; set; }
        public DbSet<RaidStatsByTier> RaidStatsByTiers { get; set; }
        public DbSet<SignUpEmoji> SignUpEmojis { get; set; }
        public DbSet<RaidRoles> RaidRoles { get; set; }
        public DbSet<Roster> Rosters { get; set; }
        public OverFlowRoster OverFlow { get; set; }
        public DbSet<BackUpRoster> BackUp { get; set; }
        public DbSet<MainRoster> MainRosters { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GuildSettings>()
                .HasKey(g => g.GuildId);
            
            modelBuilder.Entity<GuildSettings>()
                .HasMany(g => g.RaidList)
                .WithOne(a => a.GuildSettings)
                .HasForeignKey(a => a.GuildId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<RaidSettings>()
                .HasKey(r => r.Id);

            modelBuilder.Entity<RaidSettings>()
                .HasOne<GuildSettings>(t => t.GuildSettings)
                .WithMany(t => t.RaidList)
                .HasForeignKey(r => r.GuildId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<RaidRoles>()
                .HasKey(r => r.Id);

            modelBuilder.Entity<RaidRoles>()
                .HasOne<RaidSettings>(r => r.RaidSettings)
                .WithOne()
                .HasForeignKey<RaidRoles>(r => r.RoleSettingsId);

            modelBuilder.Entity<Roster>()
                .HasOne<RaidSettings>(r => r.RaidSettings)
                .WithOne()
                .HasForeignKey<Roster>(r => r.RosterSettingsId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<MainRoster>()
                .HasOne<Roster>(r => r.Roster)
                .WithMany(r => r.MainRoster)
                .HasForeignKey(r => r.MainRosterId)
                .OnDelete(DeleteBehavior.Cascade);
            
            modelBuilder.Entity<OverFlowRoster>()
                .HasOne<Roster>(r => r.Roster)
                .WithMany(r => r.OverFlowRoster)
                .HasForeignKey(r => r.OverFlowRosterId)
                .OnDelete(DeleteBehavior.Cascade);
            
            modelBuilder.Entity<BackUpRoster>()
                .HasOne<Roster>(r => r.Roster)
                .WithMany(r => r.BackUpRoster)
                .HasForeignKey(r => r.BackUpRosterId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SignUpEmoji>()
                .HasOne<GuildSettings>(s => s.GuildSettings)
                .WithMany()
                .HasForeignKey(s => s.GuildSettingsId)
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