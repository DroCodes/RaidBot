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
                name: "DiscordUser",
                columns: table => new
                {
                    Id = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    Username = table.Column<string>(type: "text", nullable: false),
                    Discriminator = table.Column<string>(type: "text", nullable: false),
                    BannerHash = table.Column<string>(type: "text", nullable: false),
                    AvatarHash = table.Column<string>(type: "text", nullable: false),
                    IsBot = table.Column<bool>(type: "boolean", nullable: false),
                    MfaEnabled = table.Column<bool>(type: "boolean", nullable: true),
                    IsSystem = table.Column<bool>(type: "boolean", nullable: true),
                    Verified = table.Column<bool>(type: "boolean", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: false),
                    PremiumType = table.Column<int>(type: "integer", nullable: true),
                    Locale = table.Column<string>(type: "text", nullable: false),
                    OAuthFlags = table.Column<int>(type: "integer", nullable: true),
                    Flags = table.Column<int>(type: "integer", nullable: true),
                    Nickname = table.Column<string>(type: "text", nullable: true),
                    CommunicationDisabledUntil = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    JoinedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    PremiumSince = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    IsDeafened = table.Column<bool>(type: "boolean", nullable: true),
                    IsMuted = table.Column<bool>(type: "boolean", nullable: true),
                    IsPending = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscordUser", x => x.Id);
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
                        name: "FK_DiscordRole_DiscordUser_DiscordMemberId",
                        column: x => x.DiscordMemberId,
                        principalTable: "DiscordUser",
                        principalColumn: "Id");
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
                        name: "FK_GuildMember_DiscordUser_DiscordMemberId",
                        column: x => x.DiscordMemberId,
                        principalTable: "DiscordUser",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_GuildMember_GuildSettings_GuildId",
                        column: x => x.GuildId,
                        principalTable: "GuildSettings",
                        principalColumn: "GuildId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SignUpEmojis",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GuildId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    RaidRole = table.Column<string>(type: "text", nullable: true),
                    EmojiName = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SignUpEmojis", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SignUpEmojis_GuildSettings_GuildId",
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

            migrationBuilder.CreateTable(
                name: "BackUp",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BackUpSettingsId = table.Column<int>(type: "integer", nullable: false),
                    MemberId = table.Column<decimal>(type: "numeric(20,0)", nullable: true),
                    RoleId = table.Column<decimal>(type: "numeric(20,0)", nullable: true),
                    RaidSettingsId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BackUp", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BackUp_DiscordRole_RoleId",
                        column: x => x.RoleId,
                        principalTable: "DiscordRole",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BackUp_DiscordUser_MemberId",
                        column: x => x.MemberId,
                        principalTable: "DiscordUser",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "OverFlowRoster",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MemberId = table.Column<decimal>(type: "numeric(20,0)", nullable: true),
                    RoleId = table.Column<decimal>(type: "numeric(20,0)", nullable: true),
                    OverflowSettingsId = table.Column<int>(type: "integer", nullable: false),
                    RaidSettingsId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OverFlowRoster", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OverFlowRoster_DiscordRole_RoleId",
                        column: x => x.RoleId,
                        principalTable: "DiscordRole",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OverFlowRoster_DiscordUser_MemberId",
                        column: x => x.MemberId,
                        principalTable: "DiscordUser",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RaidRoles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoleSettingsId = table.Column<int>(type: "integer", nullable: false),
                    TankRole = table.Column<int>(type: "integer", nullable: true),
                    HealerRole = table.Column<int>(type: "integer", nullable: true),
                    DpsRole = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RaidRoles", x => x.Id);
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
                    RolesId = table.Column<int>(type: "integer", nullable: true),
                    Info = table.Column<string>(type: "text", nullable: true),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Time = table.Column<TimeSpan>(type: "interval", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RaidSettings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RaidSettings_GuildSettings_GuildId",
                        column: x => x.GuildId,
                        principalTable: "GuildSettings",
                        principalColumn: "GuildId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RaidSettings_RaidRoles_RolesId",
                        column: x => x.RolesId,
                        principalTable: "RaidRoles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Rosters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GroupMemberId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    MemberId = table.Column<decimal>(type: "numeric(20,0)", nullable: true),
                    RoleId = table.Column<decimal>(type: "numeric(20,0)", nullable: true),
                    RosterSettingsId = table.Column<int>(type: "integer", nullable: false),
                    RaidSettingsId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rosters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rosters_DiscordRole_RoleId",
                        column: x => x.RoleId,
                        principalTable: "DiscordRole",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Rosters_DiscordUser_GroupMemberId",
                        column: x => x.GroupMemberId,
                        principalTable: "DiscordUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Rosters_DiscordUser_MemberId",
                        column: x => x.MemberId,
                        principalTable: "DiscordUser",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Rosters_RaidSettings_RaidSettingsId",
                        column: x => x.RaidSettingsId,
                        principalTable: "RaidSettings",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Rosters_RaidSettings_RosterSettingsId",
                        column: x => x.RosterSettingsId,
                        principalTable: "RaidSettings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AssignedTierRoles_TierRoleId",
                table: "AssignedTierRoles",
                column: "TierRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_BackUp_BackUpSettingsId",
                table: "BackUp",
                column: "BackUpSettingsId");

            migrationBuilder.CreateIndex(
                name: "IX_BackUp_MemberId",
                table: "BackUp",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_BackUp_RaidSettingsId",
                table: "BackUp",
                column: "RaidSettingsId");

            migrationBuilder.CreateIndex(
                name: "IX_BackUp_RoleId",
                table: "BackUp",
                column: "RoleId");

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
                name: "IX_OverFlowRoster_MemberId",
                table: "OverFlowRoster",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_OverFlowRoster_OverflowSettingsId",
                table: "OverFlowRoster",
                column: "OverflowSettingsId");

            migrationBuilder.CreateIndex(
                name: "IX_OverFlowRoster_RaidSettingsId",
                table: "OverFlowRoster",
                column: "RaidSettingsId");

            migrationBuilder.CreateIndex(
                name: "IX_OverFlowRoster_RoleId",
                table: "OverFlowRoster",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_RaidRoles_RoleSettingsId",
                table: "RaidRoles",
                column: "RoleSettingsId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RaidSettings_GuildId",
                table: "RaidSettings",
                column: "GuildId");

            migrationBuilder.CreateIndex(
                name: "IX_RaidSettings_RolesId",
                table: "RaidSettings",
                column: "RolesId");

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
                name: "IX_Rosters_GroupMemberId",
                table: "Rosters",
                column: "GroupMemberId");

            migrationBuilder.CreateIndex(
                name: "IX_Rosters_MemberId",
                table: "Rosters",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_Rosters_RaidSettingsId",
                table: "Rosters",
                column: "RaidSettingsId");

            migrationBuilder.CreateIndex(
                name: "IX_Rosters_RoleId",
                table: "Rosters",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Rosters_RosterSettingsId",
                table: "Rosters",
                column: "RosterSettingsId");

            migrationBuilder.CreateIndex(
                name: "IX_SignUpEmojis_GuildId",
                table: "SignUpEmojis",
                column: "GuildId");

            migrationBuilder.CreateIndex(
                name: "IX_TierRoles_GuildId",
                table: "TierRoles",
                column: "GuildId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRaidHistories_GuildMemberId",
                table: "UserRaidHistories",
                column: "GuildMemberId");

            migrationBuilder.AddForeignKey(
                name: "FK_BackUp_RaidSettings_BackUpSettingsId",
                table: "BackUp",
                column: "BackUpSettingsId",
                principalTable: "RaidSettings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BackUp_RaidSettings_RaidSettingsId",
                table: "BackUp",
                column: "RaidSettingsId",
                principalTable: "RaidSettings",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OverFlowRoster_RaidSettings_OverflowSettingsId",
                table: "OverFlowRoster",
                column: "OverflowSettingsId",
                principalTable: "RaidSettings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OverFlowRoster_RaidSettings_RaidSettingsId",
                table: "OverFlowRoster",
                column: "RaidSettingsId",
                principalTable: "RaidSettings",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RaidRoles_RaidSettings_RoleSettingsId",
                table: "RaidRoles",
                column: "RoleSettingsId",
                principalTable: "RaidSettings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RaidRoles_RaidSettings_RoleSettingsId",
                table: "RaidRoles");

            migrationBuilder.DropTable(
                name: "AssignedTierRoles");

            migrationBuilder.DropTable(
                name: "BackUp");

            migrationBuilder.DropTable(
                name: "OverFlowRoster");

            migrationBuilder.DropTable(
                name: "RaidStatsByRoles");

            migrationBuilder.DropTable(
                name: "RaidStatsByTiers");

            migrationBuilder.DropTable(
                name: "Rosters");

            migrationBuilder.DropTable(
                name: "SignUpEmojis");

            migrationBuilder.DropTable(
                name: "TierRoles");

            migrationBuilder.DropTable(
                name: "UserRaidHistories");

            migrationBuilder.DropTable(
                name: "DiscordRole");

            migrationBuilder.DropTable(
                name: "GuildMember");

            migrationBuilder.DropTable(
                name: "DiscordUser");

            migrationBuilder.DropTable(
                name: "RaidSettings");

            migrationBuilder.DropTable(
                name: "GuildSettings");

            migrationBuilder.DropTable(
                name: "RaidRoles");
        }
    }
}
