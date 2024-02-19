using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QFX.Migrations
{
    /// <inheritdoc />
    public partial class fixfk : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Locations_Audis_AudiID",
                table: "Locations");

            migrationBuilder.DropIndex(
                name: "IX_Locations_AudiID",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "AudiID",
                table: "Locations");

            migrationBuilder.AddColumn<long>(
                name: "LocationID",
                table: "Audis",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Audis_LocationID",
                table: "Audis",
                column: "LocationID");

            migrationBuilder.AddForeignKey(
                name: "FK_Audis_Locations_LocationID",
                table: "Audis",
                column: "LocationID",
                principalTable: "Locations",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Audis_Locations_LocationID",
                table: "Audis");

            migrationBuilder.DropIndex(
                name: "IX_Audis_LocationID",
                table: "Audis");

            migrationBuilder.DropColumn(
                name: "LocationID",
                table: "Audis");

            migrationBuilder.AddColumn<long>(
                name: "AudiID",
                table: "Locations",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Locations_AudiID",
                table: "Locations",
                column: "AudiID");

            migrationBuilder.AddForeignKey(
                name: "FK_Locations_Audis_AudiID",
                table: "Locations",
                column: "AudiID",
                principalTable: "Audis",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
