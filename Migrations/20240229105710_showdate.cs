using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QFX.Migrations
{
    /// <inheritdoc />
    public partial class showdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Date",
                table: "Shows");

            migrationBuilder.DropColumn(
                name: "Time",
                table: "Shows");

            migrationBuilder.CreateTable(
                name: "ShowDates",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ShowID = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShowDates", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ShowDates_Shows_ShowID",
                        column: x => x.ShowID,
                        principalTable: "Shows",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ShowTimes",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ShowDateID = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShowTimes", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ShowTimes_ShowDates_ShowDateID",
                        column: x => x.ShowDateID,
                        principalTable: "ShowDates",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ShowDates_ShowID",
                table: "ShowDates",
                column: "ShowID");

            migrationBuilder.CreateIndex(
                name: "IX_ShowTimes_ShowDateID",
                table: "ShowTimes",
                column: "ShowDateID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ShowTimes");

            migrationBuilder.DropTable(
                name: "ShowDates");

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "Shows",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "Time",
                table: "Shows",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
