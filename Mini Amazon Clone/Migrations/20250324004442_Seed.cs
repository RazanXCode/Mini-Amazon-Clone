using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mini_Amazon_Clone.Migrations
{
    /// <inheritdoc />
    public partial class Seed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "OrderID",
                keyValue: 1,
                column: "OrderDate",
                value: new DateTime(2024, 3, 23, 12, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "OrderID",
                keyValue: 1,
                column: "OrderDate",
                value: new DateTime(2025, 3, 24, 0, 42, 32, 703, DateTimeKind.Utc).AddTicks(8097));
        }
    }
}
