using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CurrencyManagement.Data.Migrations
{
    /// <inheritdoc />
    public partial class Addcurrencytype : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "FromDate",
                table: "CurrencyDimensions",
                type: "timestamp with time zone",
                nullable: true,
                defaultValue: new DateTime(2023, 4, 13, 13, 1, 11, 569, DateTimeKind.Utc).AddTicks(1290),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 4, 13, 12, 59, 10, 24, DateTimeKind.Utc).AddTicks(7289));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "FromDate",
                table: "CurrencyDimensions",
                type: "timestamp with time zone",
                nullable: true,
                defaultValue: new DateTime(2023, 4, 13, 12, 59, 10, 24, DateTimeKind.Utc).AddTicks(7289),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 4, 13, 13, 1, 11, 569, DateTimeKind.Utc).AddTicks(1290));
        }
    }
}
