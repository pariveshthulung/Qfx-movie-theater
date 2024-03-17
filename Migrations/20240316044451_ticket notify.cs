using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QFX.Migrations
{
    /// <inheritdoc />
    public partial class ticketnotify : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentIntendID",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "PaymentStatus",
                table: "Reservations");

            migrationBuilder.AlterColumn<long>(
                name: "ShowTimeID",
                table: "Reservations",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.CreateTable(
                name: "TicketNotifies",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MovieID = table.Column<long>(type: "bigint", nullable: false),
                    UserID = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketNotifies", x => x.ID);
                    table.ForeignKey(
                        name: "FK_TicketNotifies_Movies_MovieID",
                        column: x => x.MovieID,
                        principalTable: "Movies",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TicketNotifies_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_ShowTimeID",
                table: "Reservations",
                column: "ShowTimeID");

            migrationBuilder.CreateIndex(
                name: "IX_TicketNotifies_MovieID",
                table: "TicketNotifies",
                column: "MovieID");

            migrationBuilder.CreateIndex(
                name: "IX_TicketNotifies_UserID",
                table: "TicketNotifies",
                column: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_ShowTimes_ShowTimeID",
                table: "Reservations",
                column: "ShowTimeID",
                principalTable: "ShowTimes",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_ShowTimes_ShowTimeID",
                table: "Reservations");

            migrationBuilder.DropTable(
                name: "TicketNotifies");

            migrationBuilder.DropIndex(
                name: "IX_Reservations_ShowTimeID",
                table: "Reservations");

            migrationBuilder.AlterColumn<long>(
                name: "ShowTimeID",
                table: "Reservations",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PaymentIntendID",
                table: "Reservations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PaymentStatus",
                table: "Reservations",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
