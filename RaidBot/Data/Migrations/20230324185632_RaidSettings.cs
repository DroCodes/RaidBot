using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RaidBot.Data.Migrations
{
    /// <inheritdoc />
    public partial class RaidSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DiscordUser",
                columns: table => new
                {
                    Id = table.Column<ulong>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Username = table.Column<string>(type: "TEXT", nullable: true),
                    Discriminator = table.Column<string>(type: "TEXT", nullable: true),
                    BannerHash = table.Column<string>(type: "TEXT", nullable: true),
                    AvatarHash = table.Column<string>(type: "TEXT", nullable: true),
                    IsBot = table.Column<bool>(type: "INTEGER", nullable: false),
                    MfaEnabled = table.Column<bool>(type: "INTEGER", nullable: true),
                    IsSystem = table.Column<bool>(type: "INTEGER", nullable: true),
                    Verified = table.Column<bool>(type: "INTEGER", nullable: true),
                    Email = table.Column<string>(type: "TEXT", nullable: true),
                    PremiumType = table.Column<int>(type: "INTEGER", nullable: true),
                    Locale = table.Column<string>(type: "TEXT", nullable: true),
                    OAuthFlags = table.Column<int>(type: "INTEGER", nullable: true),
                    Flags = table.Column<int>(type: "INTEGER", nullable: true),
                    RaidSettingsId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscordUser", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RaidSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    GuildId = table.Column<ulong>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    RaidLeaderId = table.Column<ulong>(type: "INTEGER", nullable: true),
                    Info = table.Column<string>(type: "TEXT", nullable: true),
                    Tank = table.Column<int>(type: "INTEGER", nullable: false),
                    Healer = table.Column<int>(type: "INTEGER", nullable: false),
                    Dps = table.Column<int>(type: "INTEGER", nullable: false),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Time = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Tier = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RaidSettings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RaidSettings_DiscordUser_RaidLeaderId",
                        column: x => x.RaidLeaderId,
                        principalTable: "DiscordUser",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_DiscordUser_RaidSettingsId",
                table: "DiscordUser",
                column: "RaidSettingsId");

            migrationBuilder.CreateIndex(
                name: "IX_RaidSettings_RaidLeaderId",
                table: "RaidSettings",
                column: "RaidLeaderId");

            migrationBuilder.AddForeignKey(
                name: "FK_DiscordUser_RaidSettings_RaidSettingsId",
                table: "DiscordUser",
                column: "RaidSettingsId",
                principalTable: "RaidSettings",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DiscordUser_RaidSettings_RaidSettingsId",
                table: "DiscordUser");

            migrationBuilder.DropTable(
                name: "RaidSettings");

            migrationBuilder.DropTable(
                name: "DiscordUser");
        }
    }
}
