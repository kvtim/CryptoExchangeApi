using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FinanceManagement.Data.Migrations
{
    /// <inheritdoc />
    public partial class DataSeeding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "TransactionDate",
                table: "Transactions",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(2023, 4, 13, 17, 27, 22, 640, DateTimeKind.Local).AddTicks(3156),
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldDefaultValue: new DateTime(2023, 4, 4, 17, 35, 20, 544, DateTimeKind.Local).AddTicks(545));

            migrationBuilder.InsertData(
                table: "Wallets",
                columns: new[] { "Id", "CurrencyAmount", "CurrencyId", "UserId" },
                values: new object[,]
                {
                    { 1, 100000.0m, 1, 1 },
                    { 2, 0.0m, 1, 2 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Wallets",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Wallets",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.AlterColumn<DateTime>(
                name: "TransactionDate",
                table: "Transactions",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(2023, 4, 4, 17, 35, 20, 544, DateTimeKind.Local).AddTicks(545),
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldDefaultValue: new DateTime(2023, 4, 13, 17, 27, 22, 640, DateTimeKind.Local).AddTicks(3156));
        }
    }
}
