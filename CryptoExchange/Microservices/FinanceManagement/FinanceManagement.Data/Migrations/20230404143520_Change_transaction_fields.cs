using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinanceManagement.Data.Migrations
{
    /// <inheritdoc />
    public partial class Change_transaction_fields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PriceUSD",
                table: "Transactions",
                newName: "NewCurrencyPricePerUnit");

            migrationBuilder.RenameColumn(
                name: "CurrencyId",
                table: "Transactions",
                newName: "NewCurrencyId");

            migrationBuilder.RenameColumn(
                name: "CurrencyAmount",
                table: "Transactions",
                newName: "NewCurrencyAmount");

            migrationBuilder.AlterColumn<DateTime>(
                name: "TransactionDate",
                table: "Transactions",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(2023, 4, 4, 17, 35, 20, 544, DateTimeKind.Local).AddTicks(545),
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldDefaultValue: new DateTime(2023, 4, 4, 16, 11, 55, 973, DateTimeKind.Local).AddTicks(4592));

            migrationBuilder.AddColumn<decimal>(
                name: "FromCurrencyPricePerUnit",
                table: "Transactions",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "FullTransactionPriceUSD",
                table: "Transactions",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FromCurrencyPricePerUnit",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "FullTransactionPriceUSD",
                table: "Transactions");

            migrationBuilder.RenameColumn(
                name: "NewCurrencyPricePerUnit",
                table: "Transactions",
                newName: "PriceUSD");

            migrationBuilder.RenameColumn(
                name: "NewCurrencyId",
                table: "Transactions",
                newName: "CurrencyId");

            migrationBuilder.RenameColumn(
                name: "NewCurrencyAmount",
                table: "Transactions",
                newName: "CurrencyAmount");

            migrationBuilder.AlterColumn<DateTime>(
                name: "TransactionDate",
                table: "Transactions",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(2023, 4, 4, 16, 11, 55, 973, DateTimeKind.Local).AddTicks(4592),
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldDefaultValue: new DateTime(2023, 4, 4, 17, 35, 20, 544, DateTimeKind.Local).AddTicks(545));
        }
    }
}
