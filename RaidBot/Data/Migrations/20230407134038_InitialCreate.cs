using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace RaidBot.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DiscordMember",
                columns: table => new
                {
                    Id = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    Nickname = table.Column<string>(type: "text", nullable: false),
                    CommunicationDisabledUntil = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    JoinedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    PremiumSince = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    IsDeafened = table.Column<bool>(type: "boolean", nullable: false),
                    IsMuted = table.Column<bool>(type: "boolean", nullable: false),
                    IsPending = table.Column<bool>(type: "boolean", nullable: true),
                    Username = table.Column<string>(type: "text", nullable: false),
                    Discriminator = table.Column<string>(type: "text", nullable: false),
                    BannerHash = table.Column<string>(type: "text", nullable: false),
                    AvatarHash = table.Column<string>(type: "text", nullable: false),
                    IsBot = table.Column<bool>(type: "boolean", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    MfaEnabled = table.Column<bool>(type: "boolean", nullable: true),
                    Verified = table.Column<bool>(type: "boolean", nullable: true),
                    Locale = table.Column<string>(type: "text", nullable: false),
                    OAuthFlags = table.Column<int>(type: "integer", nullable: true),
                    Flags = table.Column<int>(type: "integer", nullable: true),
                    IsSystem = table.Column<bool>(type: "boolean", nullable: true),
                    PremiumType = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscordMember", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GuildSettings",
                columns: table => new
                {
                    GuildId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    RaidChannelId = table.Column<decimal>(type: "numeric(20,0)", nullable: true),
                    RaidChannelGroup = table.Column<decimal>(type: "numeric(20,0)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GuildSettings", x => x.GuildId);
                });

            migrationBuilder.CreateTable(
                name: "DiscordRole",
                columns: table => new
                {
                    Id = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    IsHoisted = table.Column<bool>(type: "boolean", nullable: false),
                    IconHash = table.Column<string>(type: "text", nullable: false),
                    Position = table.Column<int>(type: "integer", nullable: false),
                    Permissions = table.Column<long>(type: "bigint", nullable: false),
                    IsManaged = table.Column<bool>(type: "boolean", nullable: false),
                    IsMentionable = table.Column<bool>(type: "boolean", nullable: false),
                    DiscordMemberId = table.Column<decimal>(type: "numeric(20,0)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscordRole", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DiscordRole_DiscordMember_DiscordMemberId",
                        column: x => x.DiscordMemberId,
                        principalTable: "DiscordMember",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ActiveRaids",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GuildId = table.Column<decimal>(type: "numeric(20,0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActiveRaids", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActiveRaids_GuildSettings_GuildId",
                        column: x => x.GuildId,
                        principalTable: "GuildSettings",
                        principalColumn: "GuildId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GuildMember",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GuildId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    DiscordMemberId = table.Column<decimal>(type: "numeric(20,0)", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GuildMember", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GuildMember_DiscordMember_DiscordMemberId",
                        column: x => x.DiscordMemberId,
                        principalTable: "DiscordMember",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_GuildMember_GuildSettings_GuildId",
                        column: x => x.GuildId,
                        principalTable: "GuildSettings",
                        principalColumn: "GuildId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TierRoles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GuildId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    TierName = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TierRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TierRoles_GuildSettings_GuildId",
                        column: x => x.GuildId,
                        principalTable: "GuildSettings",
                        principalColumn: "GuildId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RaidSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RaidName = table.Column<string>(type: "text", nullable: false),
                    GuildId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    TierRole = table.Column<string>(type: "text", nullable: true),
                    Info = table.Column<string>(type: "text", nullable: true),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Time = table.Column<TimeSpan>(type: "interval", nullable: true),
                    ActiveRaidId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RaidSettings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RaidSettings_ActiveRaids_ActiveRaidId",
                        column: x => x.ActiveRaidId,
                        principalTable: "ActiveRaids",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRaidHistories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GuildMemberId = table.Column<int>(type: "integer", nullable: false),
                    TotalRaidCount = table.Column<int>(type: "integer", nullable: false),
                    TotalRaidCountLastMonth = table.Column<int>(type: "integer", nullable: false),
                    LastRaidDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRaidHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserRaidHistories_GuildMember_GuildMemberId",
                        column: x => x.GuildMemberId,
                        principalTable: "GuildMember",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AssignedTierRoles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TierRoleId = table.Column<int>(type: "integer", nullable: false),
                    RoleName = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssignedTierRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AssignedTierRoles_TierRoles_TierRoleId",
                        column: x => x.TierRoleId,
                        principalTable: "TierRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RaidSettingsId = table.Column<int>(type: "integer", nullable: false),
                    Tank = table.Column<string>(type: "text", nullable: true),
                    Healer = table.Column<string>(type: "text", nullable: true),
                    Dps = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Roles_RaidSettings_RaidSettingsId",
                        column: x => x.RaidSettingsId,
                        principalTable: "RaidSettings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SignUpEmojis",
                columns: table => new
                {
                    Id = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    RaidSettingsId = table.Column<int>(type: "integer", nullable: false),
                    AssignedRole = table.Column<decimal>(type: "numeric(20,0)", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Url = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SignUpEmojis", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SignUpEmojis_RaidSettings_RaidSettingsId",
                        column: x => x.RaidSettingsId,
                        principalTable: "RaidSettings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RaidStatsByRoles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RaidHistoryId = table.Column<int>(type: "integer", nullable: false),
                    Role = table.Column<string>(type: "text", nullable: true),
                    Count = table.Column<int>(type: "integer", nullable: false),
                    UserRaidHistoryId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RaidStatsByRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RaidStatsByRoles_UserRaidHistories_RaidHistoryId",
                        column: x => x.RaidHistoryId,
                        principalTable: "UserRaidHistories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RaidStatsByRoles_UserRaidHistories_UserRaidHistoryId",
                        column: x => x.UserRaidHistoryId,
                        principalTable: "UserRaidHistories",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RaidStatsByTiers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RaidHistoryId = table.Column<int>(type: "integer", nullable: false),
                    TierName = table.Column<string>(type: "text", nullable: true),
                    TierCount = table.Column<int>(type: "integer", nullable: false),
                    UserRaidHistoryId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RaidStatsByTiers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RaidStatsByTiers_UserRaidHistories_RaidHistoryId",
                        column: x => x.RaidHistoryId,
                        principalTable: "UserRaidHistories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RaidStatsByTiers_UserRaidHistories_UserRaidHistoryId",
                        column: x => x.UserRaidHistoryId,
                        principalTable: "UserRaidHistories",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActiveRaids_GuildId",
                table: "ActiveRaids",
                column: "GuildId");

            migrationBuilder.CreateIndex(
                name: "IX_AssignedTierRoles_TierRoleId",
                table: "AssignedTierRoles",
                column: "TierRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_DiscordRole_DiscordMemberId",
                table: "DiscordRole",
                column: "DiscordMemberId");

            migrationBuilder.CreateIndex(
                name: "IX_GuildMember_DiscordMemberId",
                table: "GuildMember",
                column: "DiscordMemberId");

            migrationBuilder.CreateIndex(
                name: "IX_GuildMember_GuildId",
                table: "GuildMember",
                column: "GuildId");

            migrationBuilder.CreateIndex(
                name: "IX_RaidSettings_ActiveRaidId",
                table: "RaidSettings",
                column: "ActiveRaidId");

            migrationBuilder.CreateIndex(
                name: "IX_RaidStatsByRoles_RaidHistoryId",
                table: "RaidStatsByRoles",
                column: "RaidHistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_RaidStatsByRoles_UserRaidHistoryId",
                table: "RaidStatsByRoles",
                column: "UserRaidHistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_RaidStatsByTiers_RaidHistoryId",
                table: "RaidStatsByTiers",
                column: "RaidHistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_RaidStatsByTiers_UserRaidHistoryId",
                table: "RaidStatsByTiers",
                column: "UserRaidHistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_RaidSettingsId",
                table: "Roles",
                column: "RaidSettingsId");

            migrationBuilder.CreateIndex(
                name: "IX_SignUpEmojis_RaidSettingsId",
                table: "SignUpEmojis",
                column: "RaidSettingsId");

            migrationBuilder.CreateIndex(
                name: "IX_TierRoles_GuildId",
                table: "TierRoles",
                column: "GuildId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRaidHistories_GuildMemberId",
                table: "UserRaidHistories",
                column: "GuildMemberId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AssignedTierRoles");

            migrationBuilder.DropTable(
                name: "DiscordRole");

            migrationBuilder.DropTable(
                name: "RaidStatsByRoles");

            migrationBuilder.DropTable(
                name: "RaidStatsByTiers");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "SignUpEmojis");

            migrationBuilder.DropTable(
                name: "TierRoles");

            migrationBuilder.DropTable(
                name: "UserRaidHistories");

            migrationBuilder.DropTable(
                name: "RaidSettings");

            migrationBuilder.DropTable(
                name: "GuildMember");

            migrationBuilder.DropTable(
                name: "ActiveRaids");

            migrationBuilder.DropTable(
                name: "DiscordMember");

            migrationBuilder.DropTable(
                name: "GuildSettings");
        }
    }
}
