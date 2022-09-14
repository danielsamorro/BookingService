using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BookingService.Infrastructure.Migrations
{
    public partial class AddReservationDateTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "Reservation");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "Reservation");

            migrationBuilder.CreateTable(
                name: "ReservationDate",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ReservedOn = table.Column<DateTime>(nullable: false),
                    ReservationId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReservationDate", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReservationDate_Reservation_ReservationId",
                        column: x => x.ReservationId,
                        principalTable: "Reservation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReservationDate_ReservationId",
                table: "ReservationDate",
                column: "ReservationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReservationDate");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "Reservation",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "Reservation",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
