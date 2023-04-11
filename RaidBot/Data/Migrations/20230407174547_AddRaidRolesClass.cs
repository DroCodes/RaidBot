using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace RaidBot.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddRaidRolesClass : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RolesId",
                table: "RaidSettings",
                type: "integer",
                nullable: false,
                defaultValue: 0);

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
                    table.ForeignKey(
                        name: "FK_RaidRoles_RaidSettings_RoleSettingsId",
                        column: x => x.RoleSettingsId,
                        principalTable: "RaidSettings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RaidSettings_RolesId",
                table: "RaidSettings",
                column: "RolesId");

            migrationBuilder.CreateIndex(
                name: "IX_RaidRoles_RoleSettingsId",
                table: "RaidRoles",
                column: "RoleSettingsId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_RaidSettings_RaidRoles_RolesId",
                table: "RaidSettings",
                column: "RolesId",
                principalTable: "RaidRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RaidSettings_RaidRoles_RolesId",
                table: "RaidSettings");

            migrationBuilder.DropTable(
                name: "RaidRoles");

            migrationBuilder.DropIndex(
                name: "IX_RaidSettings_RolesId",
                table: "RaidSettings");

            migrationBuilder.DropColumn(
                name: "RolesId",
                table: "RaidSettings");
        }
    }
}
