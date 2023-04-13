using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CurrencyManagement.Data.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Currencies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    CurrentPriceInUSD = table.Column<decimal>(type: "numeric", nullable: false),
                    CurrencyType = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currencies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CurrencyDimensions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PriceInUSD = table.Column<decimal>(type: "numeric", nullable: false),
                    FromDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, defaultValue: new DateTime(2023, 4, 13, 13, 58, 27, 725, DateTimeKind.Utc).AddTicks(9462)),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsCurrent = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CurrencyId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurrencyDimensions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CurrencyDimensions_Currencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "Currencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "CurrencyType", "CurrentPriceInUSD", "Name" },
                values: new object[,]
                {
                    { 1, 1, 1.0m, "USD" },
                    { 2, 1, 1.11m, "EUR" },
                    { 3, 1, 0.012m, "RUB" },
                    { 4, 2, 30288.8m, "BTC" },
                    { 5, 2, 2005.87m, "ETH" },
                    { 6, 2, 94.13m, "LTC" }
                });

            migrationBuilder.InsertData(
                table: "CurrencyDimensions",
                columns: new[] { "Id", "CurrencyId", "EndDate", "FromDate", "PriceInUSD" },
                values: new object[,]
                {
                    { 1, 1, null, new DateTime(2023, 4, 13, 13, 58, 27, 726, DateTimeKind.Utc).AddTicks(239), 1.0m },
                    { 2, 2, null, new DateTime(2023, 4, 13, 13, 58, 27, 726, DateTimeKind.Utc).AddTicks(249), 1.11m },
                    { 3, 3, null, new DateTime(2023, 4, 13, 13, 58, 27, 726, DateTimeKind.Utc).AddTicks(252), 0.012m },
                    { 4, 4, null, new DateTime(2023, 4, 13, 13, 58, 27, 726, DateTimeKind.Utc).AddTicks(254), 30288.8m },
                    { 5, 5, null, new DateTime(2023, 4, 13, 13, 58, 27, 726, DateTimeKind.Utc).AddTicks(256), 2005.87m },
                    { 6, 6, null, new DateTime(2023, 4, 13, 13, 58, 27, 726, DateTimeKind.Utc).AddTicks(259), 94.13m }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Currencies_Name",
                table: "Currencies",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CurrencyDimensions_CurrencyId",
                table: "CurrencyDimensions",
                column: "CurrencyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CurrencyDimensions");

            migrationBuilder.DropTable(
                name: "Currencies");
        }
    }
}
