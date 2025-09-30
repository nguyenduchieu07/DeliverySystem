using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class seeding_datadash_board : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Addresses",
                columns: new[] { "Id", "Active", "AddressLine", "City", "CreatedAt", "DeletedAt", "District", "IsDefault", "Label", "Latitude", "Longitude", "StoreId", "UpdatedAt", "UpdatedBy", "UserId", "Ward" },
                values: new object[] { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa20"), true, "12 Nguyễn Huệ", "Hồ Chí Minh", new DateTime(2025, 9, 30, 23, 5, 10, 937, DateTimeKind.Local).AddTicks(8874), null, "Q.1", true, "Store HQ", 10.772, 106.70399999999999, new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa3"), null, null, null, "Bến Nghé" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222221"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEHXKFcOxRoxsuy4mW3KUT7MIHM1GwUmTEmc3wYt6h8O0+Q/mC42PUvBf5j4FlWsWSg==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEHXKFcOxRoxsuy4mW3KUT7MIHM1GwUmTEmc3wYt6h8O0+Q/mC42PUvBf5j4FlWsWSg==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222223"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEHXKFcOxRoxsuy4mW3KUT7MIHM1GwUmTEmc3wYt6h8O0+Q/mC42PUvBf5j4FlWsWSg==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000001"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "98d48881-4f2e-4d55-9e28-e10434a58044", "AQAAAAIAAYagAAAAEKmXgJQKHaxghbKEm191Obrkwu47osN0n8/gN5nE4K8tqxKSVJKqtvmyQN1PhsOrbA==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000002"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "d5c4ef69-5202-470e-a836-6b88b21d68b3", "AQAAAAIAAYagAAAAEJSBC4jdbL14in04t7341AHRspzr5/ebFX5DutcnWxZWNP9ivcSa/nHRCgwnkcqJwg==" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "CreatedAt", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "Status", "TwoFactorEnabled", "UpdatedAt", "UserName" },
                values: new object[,]
                {
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa01"), 0, "42c2a633-6ee8-47ce-80d2-b855a38ba047", new DateTime(2025, 9, 30, 10, 0, 0, 0, DateTimeKind.Utc), "owner@test.local", true, false, null, "OWNER@TEST.LOCAL", "STOREOWNER", "AQAAAA...", null, false, null, "Active", false, new DateTime(2025, 9, 30, 10, 0, 0, 0, DateTimeKind.Utc), "storeowner" },
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa02"), 0, "d8f72ca2-1ed7-4d53-b5b8-a1abd7697ff9", new DateTime(2025, 9, 30, 10, 0, 0, 0, DateTimeKind.Utc), "cust1@test.local", true, false, null, "CUST1@TEST.LOCAL", "CUSTOMER1", "AQAAAA...", null, false, null, "Active", false, new DateTime(2025, 9, 30, 10, 0, 0, 0, DateTimeKind.Utc), "customer1" }
                });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2025, 9, 30, 23, 5, 10, 797, DateTimeKind.Local).AddTicks(9927));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2025, 9, 30, 23, 5, 10, 797, DateTimeKind.Local).AddTicks(9947));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2025, 9, 30, 23, 5, 10, 797, DateTimeKind.Local).AddTicks(9950));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000004"),
                column: "CreatedAt",
                value: new DateTime(2025, 9, 30, 23, 5, 10, 797, DateTimeKind.Local).AddTicks(9951));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa2-0000-0000-0000-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2025, 9, 30, 23, 5, 10, 797, DateTimeKind.Local).AddTicks(9944));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa2-0000-0000-0000-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2025, 9, 30, 23, 5, 10, 797, DateTimeKind.Local).AddTicks(9953));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa2-0000-0000-0000-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2025, 9, 30, 23, 5, 10, 797, DateTimeKind.Local).AddTicks(9955));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa3-0000-0000-0000-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2025, 9, 30, 23, 5, 10, 797, DateTimeKind.Local).AddTicks(9946));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa3-0000-0000-0000-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2025, 9, 30, 23, 5, 10, 797, DateTimeKind.Local).AddTicks(9957));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa3-0000-0000-0000-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2025, 9, 30, 23, 5, 10, 797, DateTimeKind.Local).AddTicks(9959));

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CreatedAt", "DeletedAt", "Name", "ParentId", "Slug", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa10"), new DateTime(2025, 9, 30, 23, 5, 10, 937, DateTimeKind.Local).AddTicks(8930), null, "Moving", null, null, null, null },
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa11"), new DateTime(2025, 9, 30, 23, 5, 10, 937, DateTimeKind.Local).AddTicks(8932), null, "Storage", null, null, null, null }
                });

            migrationBuilder.UpdateData(
                table: "KycDocument",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc1"),
                column: "CreatedAt",
                value: new DateTime(2025, 9, 24, 16, 5, 10, 937, DateTimeKind.Utc).AddTicks(8580));

            migrationBuilder.UpdateData(
                table: "KycDocument",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc2"),
                column: "CreatedAt",
                value: new DateTime(2025, 9, 24, 16, 5, 10, 937, DateTimeKind.Utc).AddTicks(8583));

            migrationBuilder.UpdateData(
                table: "KycDocument",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc3"),
                column: "CreatedAt",
                value: new DateTime(2025, 9, 24, 16, 5, 10, 937, DateTimeKind.Utc).AddTicks(8587));

            migrationBuilder.UpdateData(
                table: "KycDocument",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc4"),
                column: "CreatedAt",
                value: new DateTime(2025, 9, 25, 16, 5, 10, 937, DateTimeKind.Utc).AddTicks(8593));

            migrationBuilder.UpdateData(
                table: "KycDocument",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc5"),
                column: "CreatedAt",
                value: new DateTime(2025, 9, 25, 16, 5, 10, 937, DateTimeKind.Utc).AddTicks(8595));

            migrationBuilder.UpdateData(
                table: "KycDocument",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc6"),
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 16, 5, 10, 937, DateTimeKind.Utc).AddTicks(8598));

            migrationBuilder.UpdateData(
                table: "KycDocument",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc7"),
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 16, 5, 10, 937, DateTimeKind.Utc).AddTicks(8599));

            migrationBuilder.UpdateData(
                table: "KycDocument",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc8"),
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 16, 5, 10, 937, DateTimeKind.Utc).AddTicks(8601));

            migrationBuilder.UpdateData(
                table: "KycSubmission",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb1"),
                columns: new[] { "CreatedAt", "SubmittedAt" },
                values: new object[] { new DateTime(2025, 9, 24, 16, 5, 10, 937, DateTimeKind.Utc).AddTicks(8468), new DateTime(2025, 9, 24, 16, 5, 10, 937, DateTimeKind.Utc).AddTicks(8465) });

            migrationBuilder.UpdateData(
                table: "KycSubmission",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb2"),
                columns: new[] { "CreatedAt", "ReviewedAt", "SubmittedAt" },
                values: new object[] { new DateTime(2025, 9, 25, 16, 5, 10, 937, DateTimeKind.Utc).AddTicks(8511), new DateTime(2025, 9, 26, 16, 5, 10, 937, DateTimeKind.Utc).AddTicks(8471), new DateTime(2025, 9, 25, 16, 5, 10, 937, DateTimeKind.Utc).AddTicks(8470) });

            migrationBuilder.UpdateData(
                table: "KycSubmission",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb3"),
                columns: new[] { "CreatedAt", "ReviewedAt", "SubmittedAt" },
                values: new object[] { new DateTime(2025, 9, 15, 16, 5, 10, 937, DateTimeKind.Utc).AddTicks(8515), new DateTime(2025, 9, 16, 16, 5, 10, 937, DateTimeKind.Utc).AddTicks(8514), new DateTime(2025, 9, 15, 16, 5, 10, 937, DateTimeKind.Utc).AddTicks(8513) });

            migrationBuilder.UpdateData(
                table: "Stores",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa1"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 30, 16, 5, 10, 937, DateTimeKind.Utc).AddTicks(7168), new DateTime(2025, 9, 30, 16, 5, 10, 937, DateTimeKind.Utc).AddTicks(7168) });

            migrationBuilder.UpdateData(
                table: "Stores",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa2"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 30, 16, 5, 10, 937, DateTimeKind.Utc).AddTicks(8294), new DateTime(2025, 9, 30, 16, 5, 10, 937, DateTimeKind.Utc).AddTicks(8294) });

            migrationBuilder.UpdateData(
                table: "Stores",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa3"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 8, 31, 16, 5, 10, 937, DateTimeKind.Utc).AddTicks(8300), new DateTime(2025, 9, 30, 16, 5, 10, 937, DateTimeKind.Utc).AddTicks(8306) });

            migrationBuilder.InsertData(
                table: "Addresses",
                columns: new[] { "Id", "Active", "AddressLine", "City", "CreatedAt", "DeletedAt", "District", "IsDefault", "Label", "Latitude", "Longitude", "StoreId", "UpdatedAt", "UpdatedBy", "UserId", "Ward" },
                values: new object[,]
                {
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa21"), true, "89 Trần Hưng Đạo", "Hà Nội", new DateTime(2025, 9, 30, 23, 5, 10, 937, DateTimeKind.Local).AddTicks(8882), null, "Hoàn Kiếm", true, "Home Pickup", 21.026, 105.84099999999999, null, null, null, new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa02"), "Cửa Nam" },
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa22"), true, "25 Lê Duẩn", "Hồ Chí Minh", new DateTime(2025, 9, 30, 23, 5, 10, 937, DateTimeKind.Local).AddTicks(8885), null, "Q.1", false, "New Apartment", 10.782, 106.7, null, null, null, new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa02"), "Bến Nghé" }
                });

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "Id", "CreatedAt", "DeletedAt", "FullName", "KycLevel", "PreferredLang", "Tier", "UpdatedAt", "UpdatedBy" },
                values: new object[] { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa02"), new DateTime(2025, 9, 30, 10, 0, 0, 0, DateTimeKind.Utc), null, "Nguyễn Văn A", "None", "vi", "Basic", null, null });

            migrationBuilder.InsertData(
                table: "Services",
                columns: new[] { "Id", "BasePrice", "CategoryId", "CreatedAt", "DeletedAt", "Description", "IsActive", "Name", "StoreId", "Unit", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa12"), 1500000m, new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa10"), new DateTime(2025, 8, 15, 9, 0, 0, 0, DateTimeKind.Utc), null, "Local moving inside city", true, "House Moving (City)", new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa3"), "Job", null, null },
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa13"), 300000m, new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa10"), new DateTime(2025, 8, 15, 9, 0, 0, 0, DateTimeKind.Utc), null, "Boxes & packing", true, "Packing Service", new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa3"), "Package", null, null }
                });

            migrationBuilder.InsertData(
                table: "Warehouses",
                columns: new[] { "Id", "AddressRefId", "CreatedAt", "DeletedAt", "Name", "StoreId", "UpdatedAt", "UpdatedBy" },
                values: new object[] { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa60"), new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa20"), new DateTime(2025, 9, 30, 23, 5, 10, 937, DateTimeKind.Local).AddTicks(9285), null, "Main Warehouse", new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa3"), null, null });

            migrationBuilder.InsertData(
                table: "Orders",
                columns: new[] { "Id", "CreatedAt", "CustomerId", "DeletedAt", "DistanceKm", "DropoffAddressId", "EtaMinutes", "Note", "PickupAddressId", "QuotationId", "Status", "StoreId", "TotalAmount", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa30"), new DateTime(2025, 8, 15, 9, 0, 0, 0, DateTimeKind.Utc), new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa02"), null, 7.2m, new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa22"), 55, "August order", new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa21"), null, "completed", new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa3"), 2200000m, new DateTime(2025, 8, 15, 9, 0, 0, 0, DateTimeKind.Utc), null },
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa31"), new DateTime(2025, 9, 29, 14, 30, 0, 0, DateTimeKind.Utc), new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa02"), null, 5.1m, new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa22"), 45, "Yesterday pending", new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa21"), null, "pending", new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa3"), 1500000m, new DateTime(2025, 9, 29, 14, 30, 0, 0, DateTimeKind.Utc), null },
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa32"), new DateTime(2025, 9, 30, 8, 0, 0, 0, DateTimeKind.Utc), new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa02"), null, 3.4m, new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa22"), 35, "Today completed", new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa21"), null, "completed", new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa3"), 2000000m, new DateTime(2025, 9, 30, 9, 0, 0, 0, DateTimeKind.Utc), null },
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa33"), new DateTime(2025, 9, 30, 9, 0, 0, 0, DateTimeKind.Utc), new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa02"), null, 9.0m, new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa22"), 70, "Today pending", new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa21"), null, "pending", new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa3"), 3100000m, new DateTime(2025, 9, 30, 9, 40, 0, 0, DateTimeKind.Utc), null }
                });

            migrationBuilder.InsertData(
                table: "ServicePrices",
                columns: new[] { "Id", "CreatedAt", "DeletedAt", "MaxQty", "MinQty", "Price", "ServiceId", "UpdatedAt", "UpdatedBy", "ValidFrom", "ValidTo" },
                values: new object[] { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa14"), new DateTime(2025, 9, 30, 23, 5, 10, 937, DateTimeKind.Local).AddTicks(9033), null, 10, 3, 280000m, new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa13"), null, null, new DateTime(2025, 8, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 12, 31, 0, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.InsertData(
                table: "WarehouseSlots",
                columns: new[] { "Id", "Code", "CreatedAt", "CurrentOrderId", "DeletedAt", "Status", "UpdatedAt", "UpdatedBy", "WarehouseId" },
                values: new object[,]
                {
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa61"), "A1", new DateTime(2025, 9, 30, 23, 5, 10, 937, DateTimeKind.Local).AddTicks(9339), null, null, "Available", null, null, new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa60") },
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa62"), "A2", new DateTime(2025, 9, 30, 23, 5, 10, 937, DateTimeKind.Local).AddTicks(9343), new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa31"), null, "Reserved", null, null, new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa60") },
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa63"), "B1", new DateTime(2025, 9, 30, 23, 5, 10, 937, DateTimeKind.Local).AddTicks(9344), null, null, "Available", null, null, new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa60") }
                });

            migrationBuilder.InsertData(
                table: "Feedbacks",
                columns: new[] { "Id", "Comment", "CreatedAt", "DeletedAt", "FromUserId", "OrderId", "Rating", "ToStoreId", "UpdatedAt", "UpdatedBy" },
                values: new object[] { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa50"), "Very smooth job!", new DateTime(2025, 9, 30, 10, 0, 0, 0, DateTimeKind.Utc), null, new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa02"), new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa32"), 5, new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa3"), null, null });

            migrationBuilder.InsertData(
                table: "OrderItems",
                columns: new[] { "Id", "CreatedAt", "DeletedAt", "OrderId", "Quantity", "ServiceId", "Subtotal", "UnitPrice", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa40"), new DateTime(2025, 9, 30, 23, 5, 10, 937, DateTimeKind.Local).AddTicks(9185), null, new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa30"), 1, new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa12"), 2200000m, 2200000m, null, null },
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa41"), new DateTime(2025, 9, 30, 23, 5, 10, 937, DateTimeKind.Local).AddTicks(9188), null, new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa31"), 1, new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa12"), 1200000m, 1200000m, null, null },
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa42"), new DateTime(2025, 9, 30, 23, 5, 10, 937, DateTimeKind.Local).AddTicks(9195), null, new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa31"), 1, new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa13"), 300000m, 300000m, null, null },
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa43"), new DateTime(2025, 9, 30, 23, 5, 10, 937, DateTimeKind.Local).AddTicks(9207), null, new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa32"), 1, new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa12"), 2000000m, 2000000m, null, null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa01"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa11"));

            migrationBuilder.DeleteData(
                table: "Feedbacks",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa50"));

            migrationBuilder.DeleteData(
                table: "OrderItems",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa40"));

            migrationBuilder.DeleteData(
                table: "OrderItems",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa41"));

            migrationBuilder.DeleteData(
                table: "OrderItems",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa42"));

            migrationBuilder.DeleteData(
                table: "OrderItems",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa43"));

            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa33"));

            migrationBuilder.DeleteData(
                table: "ServicePrices",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa14"));

            migrationBuilder.DeleteData(
                table: "WarehouseSlots",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa61"));

            migrationBuilder.DeleteData(
                table: "WarehouseSlots",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa62"));

            migrationBuilder.DeleteData(
                table: "WarehouseSlots",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa63"));

            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa30"));

            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa31"));

            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa32"));

            migrationBuilder.DeleteData(
                table: "Services",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa12"));

            migrationBuilder.DeleteData(
                table: "Services",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa13"));

            migrationBuilder.DeleteData(
                table: "Warehouses",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa60"));

            migrationBuilder.DeleteData(
                table: "Addresses",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa20"));

            migrationBuilder.DeleteData(
                table: "Addresses",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa21"));

            migrationBuilder.DeleteData(
                table: "Addresses",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa22"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa10"));

            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa02"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa02"));

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222221"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEBLi917LNuqE95EqCJRQv6CkytvNPKs7qLnbUESOJnUKZGXSk582m6a0qtzP0Ar8ew==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEBLi917LNuqE95EqCJRQv6CkytvNPKs7qLnbUESOJnUKZGXSk582m6a0qtzP0Ar8ew==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222223"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEBLi917LNuqE95EqCJRQv6CkytvNPKs7qLnbUESOJnUKZGXSk582m6a0qtzP0Ar8ew==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000001"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "96b4e728-8c77-474a-adb2-c18cab405074", "AQAAAAIAAYagAAAAEL5zUzcK+LeWk22u/ZnFJVkGaPq1vSH+Nkcr2el3nqinhLsgr8OMDMtMVeBq57/stw==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000002"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "982dfefc-0a55-4ecc-a0e9-92fd36f2dfc3", "AQAAAAIAAYagAAAAEOVrZLxPmlzQw9tjnJpQ2Yoc9tbzl0CsHxycWr8cHLQw0aSMVxsoUBYRsLYzQUiRMw==" });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2025, 9, 29, 15, 14, 25, 645, DateTimeKind.Local).AddTicks(986));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2025, 9, 29, 15, 14, 25, 645, DateTimeKind.Local).AddTicks(1006));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2025, 9, 29, 15, 14, 25, 645, DateTimeKind.Local).AddTicks(1008));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000004"),
                column: "CreatedAt",
                value: new DateTime(2025, 9, 29, 15, 14, 25, 645, DateTimeKind.Local).AddTicks(1011));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa2-0000-0000-0000-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2025, 9, 29, 15, 14, 25, 645, DateTimeKind.Local).AddTicks(1002));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa2-0000-0000-0000-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2025, 9, 29, 15, 14, 25, 645, DateTimeKind.Local).AddTicks(1012));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa2-0000-0000-0000-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2025, 9, 29, 15, 14, 25, 645, DateTimeKind.Local).AddTicks(1078));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa3-0000-0000-0000-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2025, 9, 29, 15, 14, 25, 645, DateTimeKind.Local).AddTicks(1004));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa3-0000-0000-0000-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2025, 9, 29, 15, 14, 25, 645, DateTimeKind.Local).AddTicks(1080));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa3-0000-0000-0000-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2025, 9, 29, 15, 14, 25, 645, DateTimeKind.Local).AddTicks(1082));

            migrationBuilder.UpdateData(
                table: "KycDocument",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc1"),
                column: "CreatedAt",
                value: new DateTime(2025, 9, 23, 8, 14, 25, 797, DateTimeKind.Utc).AddTicks(6930));

            migrationBuilder.UpdateData(
                table: "KycDocument",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc2"),
                column: "CreatedAt",
                value: new DateTime(2025, 9, 23, 8, 14, 25, 797, DateTimeKind.Utc).AddTicks(6934));

            migrationBuilder.UpdateData(
                table: "KycDocument",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc3"),
                column: "CreatedAt",
                value: new DateTime(2025, 9, 23, 8, 14, 25, 797, DateTimeKind.Utc).AddTicks(6938));

            migrationBuilder.UpdateData(
                table: "KycDocument",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc4"),
                column: "CreatedAt",
                value: new DateTime(2025, 9, 24, 8, 14, 25, 797, DateTimeKind.Utc).AddTicks(6946));

            migrationBuilder.UpdateData(
                table: "KycDocument",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc5"),
                column: "CreatedAt",
                value: new DateTime(2025, 9, 24, 8, 14, 25, 797, DateTimeKind.Utc).AddTicks(6948));

            migrationBuilder.UpdateData(
                table: "KycDocument",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc6"),
                column: "CreatedAt",
                value: new DateTime(2025, 9, 14, 8, 14, 25, 797, DateTimeKind.Utc).AddTicks(6951));

            migrationBuilder.UpdateData(
                table: "KycDocument",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc7"),
                column: "CreatedAt",
                value: new DateTime(2025, 9, 14, 8, 14, 25, 797, DateTimeKind.Utc).AddTicks(6954));

            migrationBuilder.UpdateData(
                table: "KycDocument",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc8"),
                column: "CreatedAt",
                value: new DateTime(2025, 9, 14, 8, 14, 25, 797, DateTimeKind.Utc).AddTicks(6956));

            migrationBuilder.UpdateData(
                table: "KycSubmission",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb1"),
                columns: new[] { "CreatedAt", "SubmittedAt" },
                values: new object[] { new DateTime(2025, 9, 23, 8, 14, 25, 797, DateTimeKind.Utc).AddTicks(6786), new DateTime(2025, 9, 23, 8, 14, 25, 797, DateTimeKind.Utc).AddTicks(6782) });

            migrationBuilder.UpdateData(
                table: "KycSubmission",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb2"),
                columns: new[] { "CreatedAt", "ReviewedAt", "SubmittedAt" },
                values: new object[] { new DateTime(2025, 9, 24, 8, 14, 25, 797, DateTimeKind.Utc).AddTicks(6796), new DateTime(2025, 9, 25, 8, 14, 25, 797, DateTimeKind.Utc).AddTicks(6790), new DateTime(2025, 9, 24, 8, 14, 25, 797, DateTimeKind.Utc).AddTicks(6789) });

            migrationBuilder.UpdateData(
                table: "KycSubmission",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb3"),
                columns: new[] { "CreatedAt", "ReviewedAt", "SubmittedAt" },
                values: new object[] { new DateTime(2025, 9, 14, 8, 14, 25, 797, DateTimeKind.Utc).AddTicks(6801), new DateTime(2025, 9, 15, 8, 14, 25, 797, DateTimeKind.Utc).AddTicks(6800), new DateTime(2025, 9, 14, 8, 14, 25, 797, DateTimeKind.Utc).AddTicks(6799) });

            migrationBuilder.UpdateData(
                table: "Stores",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa1"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 29, 8, 14, 25, 797, DateTimeKind.Utc).AddTicks(4930), new DateTime(2025, 9, 29, 8, 14, 25, 797, DateTimeKind.Utc).AddTicks(4931) });

            migrationBuilder.UpdateData(
                table: "Stores",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa2"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 29, 8, 14, 25, 797, DateTimeKind.Utc).AddTicks(6553), new DateTime(2025, 9, 29, 8, 14, 25, 797, DateTimeKind.Utc).AddTicks(6554) });

            migrationBuilder.UpdateData(
                table: "Stores",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa3"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 8, 30, 8, 14, 25, 797, DateTimeKind.Utc).AddTicks(6562), new DateTime(2025, 9, 29, 8, 14, 25, 797, DateTimeKind.Utc).AddTicks(6570) });
        }
    }
}
