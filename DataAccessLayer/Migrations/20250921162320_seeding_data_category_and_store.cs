using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class seeding_data_category_and_store : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("1b5a13b8-009c-447c-9fc4-952d773b6bb5"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("5eefa682-a519-4e43-9df5-5c4665cce5e8"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("87b88083-15a0-4853-81ee-76e213364076"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("ff999c60-b428-4e92-aa42-8eab1bfd6569"));

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("aaaaaaa1-0000-0000-0000-000000000001"), null, "Admin", "ADMIN" },
                    { new Guid("aaaaaaa1-0000-0000-0000-000000000002"), null, "Store", "STORE" },
                    { new Guid("aaaaaaa1-0000-0000-0000-000000000003"), null, "StoreStaff", "StoreStaff" },
                    { new Guid("aaaaaaa1-0000-0000-0000-000000000004"), null, "Customer", "CUSTOMER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "CreatedAt", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "Status", "TwoFactorEnabled", "UpdatedAt", "UserName" },
                values: new object[] { new Guid("aaaaaaa1-0000-0000-0000-000000000001"), 0, "1c297548-3e0f-4417-a380-fe4da6cd691f", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "store1@gmail.com", true, false, null, "store1@gmail.com", "store1", "AQAAAAIAAYagAAAAEHjiqHVtDzf62rjKykFI1Wncq+hyA/w7jY6h0UlujMGCz3SPgsXVZVfD+xocg2BwSw==", null, false, null, "Active", false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "store1" });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CreatedAt", "DeletedAt", "Name", "ParentId", "Slug", "SortOrder", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { new Guid("aaaaaaa1-0000-0000-0000-000000000001"), new DateTime(2025, 9, 21, 23, 23, 19, 512, DateTimeKind.Local).AddTicks(3987), null, "Dịch vụ vận chuyển", null, "van-chuyen", 1, null, null },
                    { new Guid("aaaaaaa2-0000-0000-0000-000000000001"), new DateTime(2025, 9, 21, 23, 23, 19, 512, DateTimeKind.Local).AddTicks(4012), null, "Lưu kho", null, "luu-kho", 2, null, null },
                    { new Guid("aaaaaaa3-0000-0000-0000-000000000001"), new DateTime(2025, 9, 21, 23, 23, 19, 512, DateTimeKind.Local).AddTicks(4015), null, "Dọn dẹp", null, "don-dep", 3, null, null }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { new Guid("aaaaaaa1-0000-0000-0000-000000000002"), new Guid("aaaaaaa1-0000-0000-0000-000000000001") });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CreatedAt", "DeletedAt", "Name", "ParentId", "Slug", "SortOrder", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { new Guid("aaaaaaa1-0000-0000-0000-000000000002"), new DateTime(2025, 9, 21, 23, 23, 19, 512, DateTimeKind.Local).AddTicks(4017), null, "Chuyển nhà", new Guid("aaaaaaa1-0000-0000-0000-000000000001"), "chuyen-nha", 1, null, null },
                    { new Guid("aaaaaaa1-0000-0000-0000-000000000003"), new DateTime(2025, 9, 21, 23, 23, 19, 512, DateTimeKind.Local).AddTicks(4020), null, "Chuyển văn phòng", new Guid("aaaaaaa1-0000-0000-0000-000000000001"), "chuyen-van-phong", 2, null, null },
                    { new Guid("aaaaaaa1-0000-0000-0000-000000000004"), new DateTime(2025, 9, 21, 23, 23, 19, 512, DateTimeKind.Local).AddTicks(4022), null, "Xe tải theo km", new Guid("aaaaaaa1-0000-0000-0000-000000000001"), "xe-tai-theo-km", 3, null, null },
                    { new Guid("aaaaaaa2-0000-0000-0000-000000000002"), new DateTime(2025, 9, 21, 23, 23, 19, 512, DateTimeKind.Local).AddTicks(4024), null, "Theo giờ", new Guid("aaaaaaa2-0000-0000-0000-000000000001"), "theo-gio", 1, null, null },
                    { new Guid("aaaaaaa2-0000-0000-0000-000000000003"), new DateTime(2025, 9, 21, 23, 23, 19, 512, DateTimeKind.Local).AddTicks(4028), null, "Theo ngày", new Guid("aaaaaaa2-0000-0000-0000-000000000001"), "theo-ngay", 2, null, null },
                    { new Guid("aaaaaaa3-0000-0000-0000-000000000002"), new DateTime(2025, 9, 21, 23, 23, 19, 512, DateTimeKind.Local).AddTicks(4031), null, "Vệ sinh nhà", new Guid("aaaaaaa3-0000-0000-0000-000000000001"), "ve-sinh-nha", 1, null, null },
                    { new Guid("aaaaaaa3-0000-0000-0000-000000000003"), new DateTime(2025, 9, 21, 23, 23, 19, 512, DateTimeKind.Local).AddTicks(4033), null, "Vệ sinh văn phòng", new Guid("aaaaaaa3-0000-0000-0000-000000000001"), "ve-sinh-van-phong", 2, null, null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000003"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000004"));

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("aaaaaaa1-0000-0000-0000-000000000002"), new Guid("aaaaaaa1-0000-0000-0000-000000000001") });

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000003"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000004"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa2-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa2-0000-0000-0000-000000000003"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa3-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa3-0000-0000-0000-000000000003"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa2-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa3-0000-0000-0000-000000000001"));

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("1b5a13b8-009c-447c-9fc4-952d773b6bb5"), null, "Admin", "ADMIN" },
                    { new Guid("5eefa682-a519-4e43-9df5-5c4665cce5e8"), null, "Customer", "CUSTOMER" },
                    { new Guid("87b88083-15a0-4853-81ee-76e213364076"), null, "StoreStaff", "StoreStaff" },
                    { new Guid("ff999c60-b428-4e92-aa42-8eab1bfd6569"), null, "Store", "STORE" }
                });
        }
    }
}
