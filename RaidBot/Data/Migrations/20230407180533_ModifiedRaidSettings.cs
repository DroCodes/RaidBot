using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RaidBot.Data.Migrations
{
    /// <inheritdoc />
    public partial class ModifiedRaidSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RaidSettings_RaidRoles_RolesId",
                table: "RaidSettings");

            migrationBuilder.AlterColumn<int>(
                name: "RolesId",
                table: "RaidSettings",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_RaidSettings_RaidRoles_RolesId",
                table: "RaidSettings",
                column: "RolesId",
                principalTable: "RaidRoles",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RaidSettings_RaidRoles_RolesId",
                table: "RaidSettings");

            migrationBuilder.AlterColumn<int>(
                name: "RolesId",
                table: "RaidSettings",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_RaidSettings_RaidRoles_RolesId",
                table: "RaidSettings",
                column: "RolesId",
                principalTable: "RaidRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
