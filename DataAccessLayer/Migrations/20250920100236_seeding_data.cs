using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class seeding_data : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("00f37bc7-31fd-436b-9493-e0e139550bae"), null, "Admin", "ADMIN" },
                    { new Guid("5aceb254-7559-4403-a227-c0d4293c6240"), null, "Store", "STORE" },
                    { new Guid("6e6cb5a4-f192-4cb0-b379-1606cc8965dc"), null, "Customer", "CUSTOMER" },
                    { new Guid("a9649082-fc6f-49dd-b92a-105ba62782b5"), null, "StoreStaff", "StoreStaff" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("00f37bc7-31fd-436b-9493-e0e139550bae"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("5aceb254-7559-4403-a227-c0d4293c6240"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("6e6cb5a4-f192-4cb0-b379-1606cc8965dc"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("a9649082-fc6f-49dd-b92a-105ba62782b5"));
        }
    }
}
