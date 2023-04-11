using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace RaidBot.Data.Migrations
{
    /// <inheritdoc />
    public partial class RefactoredEntityClasses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DiscordRole_DiscordMember_DiscordMemberId",
                table: "DiscordRole");

            migrationBuilder.DropForeignKey(
                name: "FK_GuildMember_DiscordMember_DiscordMemberId",
                table: "GuildMember");

            migrationBuilder.DropForeignKey(
                name: "FK_RaidSettings_ActiveRaids_ActiveRaidId",
                table: "RaidSettings");

            migrationBuilder.DropForeignKey(
                name: "FK_SignUpEmojis_RaidSettings_RaidSettingsId",
                table: "SignUpEmojis");

            migrationBuilder.DropTable(
                name: "ActiveRaids");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropIndex(
                name: "IX_SignUpEmojis_RaidSettingsId",
                table: "SignUpEmojis");

            migrationBuilder.DropIndex(
                name: "IX_RaidSettings_ActiveRaidId",
                table: "RaidSettings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DiscordMember",
                table: "DiscordMember");

            migrationBuilder.DropColumn(
                name: "RaidSettingsId",
                table: "SignUpEmojis");

            migrationBuilder.DropColumn(
                name: "ActiveRaidId",
                table: "RaidSettings");

            migrationBuilder.RenameTable(
                name: "DiscordMember",
                newName: "DiscordUser");

            migrationBuilder.AddColumn<decimal>(
                name: "GuildSettingsGuildId",
                table: "SignUpEmojis",
                type: "numeric(20,0)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "GuildSettingsId",
                table: "SignUpEmojis",
                type: "numeric(20,0)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AlterColumn<string>(
                name: "Nickname",
                table: "DiscordUser",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "JoinedAt",
                table: "DiscordUser",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<bool>(
                name: "IsMuted",
                table: "DiscordUser",
                type: "boolean",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeafened",
                table: "DiscordUser",
                type: "boolean",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DiscordUser",
                table: "DiscordUser",
                column: "Id");

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
                    table.ForeignKey(
                        name: "FK_BackUp_RaidSettings_BackUpSettingsId",
                        column: x => x.BackUpSettingsId,
                        principalTable: "RaidSettings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BackUp_RaidSettings_RaidSettingsId",
                        column: x => x.RaidSettingsId,
                        principalTable: "RaidSettings",
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
                    table.ForeignKey(
                        name: "FK_OverFlowRoster_RaidSettings_OverflowSettingsId",
                        column: x => x.OverflowSettingsId,
                        principalTable: "RaidSettings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OverFlowRoster_RaidSettings_RaidSettingsId",
                        column: x => x.RaidSettingsId,
                        principalTable: "RaidSettings",
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
                name: "IX_SignUpEmojis_GuildSettingsGuildId",
                table: "SignUpEmojis",
                column: "GuildSettingsGuildId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SignUpEmojis_GuildSettingsId",
                table: "SignUpEmojis",
                column: "GuildSettingsId");

            migrationBuilder.CreateIndex(
                name: "IX_RaidSettings_GuildId",
                table: "RaidSettings",
                column: "GuildId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_DiscordRole_DiscordUser_DiscordMemberId",
                table: "DiscordRole",
                column: "DiscordMemberId",
                principalTable: "DiscordUser",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GuildMember_DiscordUser_DiscordMemberId",
                table: "GuildMember",
                column: "DiscordMemberId",
                principalTable: "DiscordUser",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RaidSettings_GuildSettings_GuildId",
                table: "RaidSettings",
                column: "GuildId",
                principalTable: "GuildSettings",
                principalColumn: "GuildId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SignUpEmojis_GuildSettings_GuildSettingsGuildId",
                table: "SignUpEmojis",
                column: "GuildSettingsGuildId",
                principalTable: "GuildSettings",
                principalColumn: "GuildId");

            migrationBuilder.AddForeignKey(
                name: "FK_SignUpEmojis_GuildSettings_GuildSettingsId",
                table: "SignUpEmojis",
                column: "GuildSettingsId",
                principalTable: "GuildSettings",
                principalColumn: "GuildId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DiscordRole_DiscordUser_DiscordMemberId",
                table: "DiscordRole");

            migrationBuilder.DropForeignKey(
                name: "FK_GuildMember_DiscordUser_DiscordMemberId",
                table: "GuildMember");

            migrationBuilder.DropForeignKey(
                name: "FK_RaidSettings_GuildSettings_GuildId",
                table: "RaidSettings");

            migrationBuilder.DropForeignKey(
                name: "FK_SignUpEmojis_GuildSettings_GuildSettingsGuildId",
                table: "SignUpEmojis");

            migrationBuilder.DropForeignKey(
                name: "FK_SignUpEmojis_GuildSettings_GuildSettingsId",
                table: "SignUpEmojis");

            migrationBuilder.DropTable(
                name: "BackUp");

            migrationBuilder.DropTable(
                name: "OverFlowRoster");

            migrationBuilder.DropTable(
                name: "Rosters");

            migrationBuilder.DropIndex(
                name: "IX_SignUpEmojis_GuildSettingsGuildId",
                table: "SignUpEmojis");

            migrationBuilder.DropIndex(
                name: "IX_SignUpEmojis_GuildSettingsId",
                table: "SignUpEmojis");

            migrationBuilder.DropIndex(
                name: "IX_RaidSettings_GuildId",
                table: "RaidSettings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DiscordUser",
                table: "DiscordUser");

            migrationBuilder.DropColumn(
                name: "GuildSettingsGuildId",
                table: "SignUpEmojis");

            migrationBuilder.DropColumn(
                name: "GuildSettingsId",
                table: "SignUpEmojis");

            migrationBuilder.RenameTable(
                name: "DiscordUser",
                newName: "DiscordMember");

            migrationBuilder.AddColumn<int>(
                name: "RaidSettingsId",
                table: "SignUpEmojis",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ActiveRaidId",
                table: "RaidSettings",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Nickname",
                table: "DiscordMember",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "JoinedAt",
                table: "DiscordMember",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsMuted",
                table: "DiscordMember",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeafened",
                table: "DiscordMember",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_DiscordMember",
                table: "DiscordMember",
                column: "Id");

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
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RaidSettingsId = table.Column<int>(type: "integer", nullable: false),
                    Dps = table.Column<string>(type: "text", nullable: true),
                    Healer = table.Column<string>(type: "text", nullable: true),
                    Tank = table.Column<string>(type: "text", nullable: true)
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

            migrationBuilder.CreateIndex(
                name: "IX_SignUpEmojis_RaidSettingsId",
                table: "SignUpEmojis",
                column: "RaidSettingsId");

            migrationBuilder.CreateIndex(
                name: "IX_RaidSettings_ActiveRaidId",
                table: "RaidSettings",
                column: "ActiveRaidId");

            migrationBuilder.CreateIndex(
                name: "IX_ActiveRaids_GuildId",
                table: "ActiveRaids",
                column: "GuildId");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_RaidSettingsId",
                table: "Roles",
                column: "RaidSettingsId");

            migrationBuilder.AddForeignKey(
                name: "FK_DiscordRole_DiscordMember_DiscordMemberId",
                table: "DiscordRole",
                column: "DiscordMemberId",
                principalTable: "DiscordMember",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GuildMember_DiscordMember_DiscordMemberId",
                table: "GuildMember",
                column: "DiscordMemberId",
                principalTable: "DiscordMember",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RaidSettings_ActiveRaids_ActiveRaidId",
                table: "RaidSettings",
                column: "ActiveRaidId",
                principalTable: "ActiveRaids",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SignUpEmojis_RaidSettings_RaidSettingsId",
                table: "SignUpEmojis",
                column: "RaidSettingsId",
                principalTable: "RaidSettings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
