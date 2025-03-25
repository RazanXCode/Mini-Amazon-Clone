using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Mini_Amazon_Clone.Migrations
{
    /// <inheritdoc />
    public partial class SeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserID", "Email", "Name", "Password", "Role" },
                values: new object[,]
                {
                    { 1, "admin@example.com", "Admin User", "admin123", "Admin" },
                    { 2, "john@example.com", "John Doe", "password123", "Customer" }
                });

            migrationBuilder.InsertData(
                table: "Orders",
                columns: new[] { "OrderID", "OrderDate", "Status", "TotalAmount", "UserID" },
                values: new object[] { 1, new DateTime(2025, 3, 24, 0, 42, 32, 703, DateTimeKind.Utc).AddTicks(8097), "Completed", 2000.48m, 2 });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "ProductID", "CreatedBy", "Description", "Name", "Price", "Stock" },
                values: new object[,]
                {
                    { 1, 1, "High-performance laptop", "Laptop", 1200.99m, 10 },
                    { 2, 1, "Latest model smartphone", "Smartphone", 799.49m, 15 }
                });

            migrationBuilder.InsertData(
                table: "OrderItems",
                columns: new[] { "OrderItemID", "OrderID", "Price", "ProductID", "Quantity" },
                values: new object[,]
                {
                    { 1, 1, 1200.99m, 1, 1 },
                    { 2, 1, 799.49m, 2, 1 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "OrderItems",
                keyColumn: "OrderItemID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "OrderItems",
                keyColumn: "OrderItemID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "OrderID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserID",
                keyValue: 2);
        }
    }
}
