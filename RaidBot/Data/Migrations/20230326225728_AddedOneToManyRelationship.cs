using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RaidBot.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedOneToManyRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DiscordRoles_TierRoles_TierRoleId",
                table: "DiscordRoles");

            migrationBuilder.AddForeignKey(
                name: "FK_DiscordRoles_TierRoles_TierRoleId",
                table: "DiscordRoles",
                column: "TierRoleId",
                principalTable: "TierRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DiscordRoles_TierRoles_TierRoleId",
                table: "DiscordRoles");

            migrationBuilder.AddForeignKey(
                name: "FK_DiscordRoles_TierRoles_TierRoleId",
                table: "DiscordRoles",
                column: "TierRoleId",
                principalTable: "TierRoles",
                principalColumn: "Id");
        }
    }
}
