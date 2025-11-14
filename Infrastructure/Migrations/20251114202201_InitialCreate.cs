using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RestaurantsTable",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    OpeningTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    ClosingTime = table.Column<TimeSpan>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RestaurantsTable", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UsersTable",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(20)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersTable", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AddressesTable",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Street = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ZipCode = table.Column<int>(type: "int", nullable: false),
                    City = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Country = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    RestaurantId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AddressesTable", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AddressesTable_RestaurantsTable_RestaurantId",
                        column: x => x.RestaurantId,
                        principalTable: "RestaurantsTable",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TablesTable",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TableNumber = table.Column<int>(type: "int", nullable: false),
                    Capacity = table.Column<int>(type: "int", nullable: false),
                    RestaurantId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TablesTable", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TablesTable_RestaurantsTable_RestaurantId",
                        column: x => x.RestaurantId,
                        principalTable: "RestaurantsTable",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReservationsTable",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReservationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NumberOfGuests = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    SpecialRequests = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReservationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TableId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReservationsTable", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReservationsTable_TablesTable_TableId",
                        column: x => x.TableId,
                        principalTable: "TablesTable",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReservationsTable_UsersTable_UserId",
                        column: x => x.UserId,
                        principalTable: "UsersTable",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "RestaurantsTable",
                columns: new[] { "Id", "ClosingTime", "Name", "OpeningTime", "PhoneNumber" },
                values: new object[] { 1, new TimeSpan(0, 22, 0, 0, 0), "Bistro Central", new TimeSpan(0, 11, 0, 0, 0), "+1-555-0101" });

            migrationBuilder.InsertData(
                table: "UsersTable",
                columns: new[] { "Id", "CreatedAt", "Email", "FirstName", "LastName", "PhoneNumber", "Role" },
                values: new object[] { "user-1", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "email@gmal.com", "Eugène", "ETOUNDI", "+1-555-0101", "Customer" });

            migrationBuilder.InsertData(
                table: "AddressesTable",
                columns: new[] { "Id", "City", "Country", "RestaurantId", "Street", "ZipCode" },
                values: new object[] { 1, "Angers", "France", 1, "76 Luneau", 49000 });

            migrationBuilder.InsertData(
                table: "TablesTable",
                columns: new[] { "Id", "Capacity", "RestaurantId", "TableNumber" },
                values: new object[,]
                {
                    { 1, 4, 1, 1 },
                    { 2, 2, 1, 2 },
                    { 3, 6, 1, 3 }
                });

            migrationBuilder.InsertData(
                table: "ReservationsTable",
                columns: new[] { "Id", "NumberOfGuests", "ReservationDate", "ReservationTime", "SpecialRequests", "Status", "TableId", "UserId" },
                values: new object[] { 1, 2, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 11, 15, 19, 0, 0, 0, DateTimeKind.Utc), null, "Confirmed", 1, "user-1" });

            migrationBuilder.CreateIndex(
                name: "IX_AddressesTable_City",
                table: "AddressesTable",
                column: "City");

            migrationBuilder.CreateIndex(
                name: "IX_AddressesTable_RestaurantId",
                table: "AddressesTable",
                column: "RestaurantId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ReservationsTable_ReservationTime",
                table: "ReservationsTable",
                column: "ReservationTime");

            migrationBuilder.CreateIndex(
                name: "IX_ReservationsTable_TableId",
                table: "ReservationsTable",
                column: "TableId");

            migrationBuilder.CreateIndex(
                name: "IX_ReservationsTable_UserId",
                table: "ReservationsTable",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_RestaurantsTable_Name",
                table: "RestaurantsTable",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_TablesTable_RestaurantId_TableNumber",
                table: "TablesTable",
                columns: new[] { "RestaurantId", "TableNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UsersTable_FirstName_LastName",
                table: "UsersTable",
                columns: new[] { "FirstName", "LastName" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AddressesTable");

            migrationBuilder.DropTable(
                name: "ReservationsTable");

            migrationBuilder.DropTable(
                name: "TablesTable");

            migrationBuilder.DropTable(
                name: "UsersTable");

            migrationBuilder.DropTable(
                name: "RestaurantsTable");
        }
    }
}
