using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QFX.Migrations
{
    /// <inheritdoc />
    public partial class addfk : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShowSeats_Shows_ShowID",
                table: "ShowSeats");

            migrationBuilder.RenameColumn(
                name: "ShowID",
                table: "ShowSeats",
                newName: "ShowTimeID");

            migrationBuilder.RenameIndex(
                name: "IX_ShowSeats_ShowID",
                table: "ShowSeats",
                newName: "IX_ShowSeats_ShowTimeID");

            migrationBuilder.AddForeignKey(
                name: "FK_ShowSeats_ShowTimes_ShowTimeID",
                table: "ShowSeats",
                column: "ShowTimeID",
                principalTable: "ShowTimes",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShowSeats_ShowTimes_ShowTimeID",
                table: "ShowSeats");

            migrationBuilder.RenameColumn(
                name: "ShowTimeID",
                table: "ShowSeats",
                newName: "ShowID");

            migrationBuilder.RenameIndex(
                name: "IX_ShowSeats_ShowTimeID",
                table: "ShowSeats",
                newName: "IX_ShowSeats_ShowID");

            migrationBuilder.AddForeignKey(
                name: "FK_ShowSeats_Shows_ShowID",
                table: "ShowSeats",
                column: "ShowID",
                principalTable: "Shows",
                principalColumn: "ID");
        }
    }
}
