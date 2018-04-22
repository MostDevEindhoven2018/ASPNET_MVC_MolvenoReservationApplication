using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ASPNET_MVC_MolvenoReservationApplication.Migrations
{
    public partial class AddedTableAreas : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Guests",
                columns: table => new
                {
                    GuestID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    _guestEmail = table.Column<string>(maxLength: 40, nullable: true),
                    _guestName = table.Column<string>(maxLength: 20, nullable: false),
                    _guestPhone = table.Column<string>(maxLength: 13, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Guests", x => x.GuestID);
                });

            migrationBuilder.CreateTable(
                name: "Tables",
                columns: table => new
                {
                    TableID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    _tableArea = table.Column<int>(nullable: false),
                    _tableCapacity = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tables", x => x.TableID);
                });

            migrationBuilder.CreateTable(
                name: "Reservations",
                columns: table => new
                {
                    ReservationID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    _resArrivingTime = table.Column<DateTime>(nullable: false),
                    _resComments = table.Column<string>(nullable: true),
                    _resGuestGuestID = table.Column<int>(nullable: true),
                    _resHidePrices = table.Column<bool>(nullable: false),
                    _resLeavingTime = table.Column<DateTime>(nullable: false),
                    _resPartySize = table.Column<int>(nullable: false),
                    _resTableTableID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservations", x => x.ReservationID);
                    table.ForeignKey(
                        name: "FK_Reservations_Guests__resGuestGuestID",
                        column: x => x._resGuestGuestID,
                        principalTable: "Guests",
                        principalColumn: "GuestID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Reservations_Tables__resTableTableID",
                        column: x => x._resTableTableID,
                        principalTable: "Tables",
                        principalColumn: "TableID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reservations__resGuestGuestID",
                table: "Reservations",
                column: "_resGuestGuestID");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations__resTableTableID",
                table: "Reservations",
                column: "_resTableTableID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Reservations");

            migrationBuilder.DropTable(
                name: "Guests");

            migrationBuilder.DropTable(
                name: "Tables");
        }
    }
}
