using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CurrencyManagement.Data.Migrations
{
    /// <inheritdoc />
    public partial class Addcascadedalete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CurrencyDimensions_Currencies_CurrencyId",
                table: "CurrencyDimensions");

            migrationBuilder.AlterColumn<DateTime>(
                name: "FromDate",
                table: "CurrencyDimensions",
                type: "timestamp with time zone",
                nullable: true,
                defaultValue: new DateTime(2023, 3, 28, 9, 53, 19, 265, DateTimeKind.Utc).AddTicks(6772),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 3, 27, 14, 40, 20, 431, DateTimeKind.Utc).AddTicks(1925));

            migrationBuilder.AddForeignKey(
                name: "FK_CurrencyDimensions_Currencies_CurrencyId",
                table: "CurrencyDimensions",
                column: "CurrencyId",
                principalTable: "Currencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CurrencyDimensions_Currencies_CurrencyId",
                table: "CurrencyDimensions");

            migrationBuilder.AlterColumn<DateTime>(
                name: "FromDate",
                table: "CurrencyDimensions",
                type: "timestamp with time zone",
                nullable: true,
                defaultValue: new DateTime(2023, 3, 27, 14, 40, 20, 431, DateTimeKind.Utc).AddTicks(1925),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 3, 28, 9, 53, 19, 265, DateTimeKind.Utc).AddTicks(6772));

            migrationBuilder.AddForeignKey(
                name: "FK_CurrencyDimensions_Currencies_CurrencyId",
                table: "CurrencyDimensions",
                column: "CurrencyId",
                principalTable: "Currencies",
                principalColumn: "Id");
        }
    }
}
