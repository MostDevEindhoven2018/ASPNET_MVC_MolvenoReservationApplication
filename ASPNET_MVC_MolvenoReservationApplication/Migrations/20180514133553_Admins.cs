using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ASPNET_MVC_MolvenoReservationApplication.Migrations
{
    public partial class Admins : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "_resDateOfReservation",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "_resHourOfReservation",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "_resMinuteOfReservation",
                table: "Reservations");

            migrationBuilder.AddColumn<string>(
                name: "Date",
                table: "Reservations",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Time",
                table: "Reservations",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "_resArrivingTime",
                table: "Reservations",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "_resHidePrices",
                table: "Reservations",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "Admins",
                columns: table => new
                {
                    AdminID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ClosingHours = table.Column<int>(nullable: false),
                    OpeningHour = table.Column<int>(nullable: false),
                    PercentageMaxCapacity = table.Column<int>(nullable: false),
                    _resDurationHour = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Admins", x => x.AdminID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Admins");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "Time",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "_resArrivingTime",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "_resHidePrices",
                table: "Reservations");

            migrationBuilder.AddColumn<string>(
                name: "_resDateOfReservation",
                table: "Reservations",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "_resHourOfReservation",
                table: "Reservations",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "_resMinuteOfReservation",
                table: "Reservations",
                nullable: false,
                defaultValue: 0);
        }
    }
}
