using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class AddData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("aaaaaaa1-0000-0000-0000-000000000001"), "2f967847-15a6-4ca7-bcfc-cfb573caac22", "Admin", "ADMIN" },
                    { new Guid("aaaaaaa1-0000-0000-0000-000000000002"), "3de7ba5b-3e77-43ef-92b0-951277af1df8", "Store", "STORE" },
                    { new Guid("aaaaaaa1-0000-0000-0000-000000000003"), "bc726670-d382-4881-8f0b-5d6843f2c828", "StoreStaff", "StoreStaff" },
                    { new Guid("aaaaaaa1-0000-0000-0000-000000000004"), "c78f74c1-6505-4265-9b50-1a126ac34323", "Customer", "CUSTOMER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "CreatedAt", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "Status", "TwoFactorEnabled", "UpdatedAt", "UserName" },
                values: new object[,]
                {
                    { new Guid("22222222-2222-2222-2222-222222222221"), 0, "con-blue", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "owner.blue@demo.local", true, false, null, "OWNER.BLUE@DEMO.LOCAL", "BLUEOWNER", "AQAAAAIAAYagAAAAEJNmwJu/1BYqJx3paJqrMcB8ul0UqYzqBJ8Ei4oNFb5VYQmmWmO8gMzjMlJ9i8aVUA==", null, false, "sec-blue", 8, false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "blueowner" },
                    { new Guid("22222222-2222-2222-2222-222222222222"), 0, "con-fresh", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "owner.fresh@demo.local", true, false, null, "OWNER.FRESH@DEMO.LOCAL", "FRESHOWNER", "AQAAAAIAAYagAAAAEJNmwJu/1BYqJx3paJqrMcB8ul0UqYzqBJ8Ei4oNFb5VYQmmWmO8gMzjMlJ9i8aVUA==", null, false, "sec-fresh", 8, false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "freshowner" },
                    { new Guid("22222222-2222-2222-2222-222222222223"), 0, "con-prime", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "owner.prime@demo.local", true, false, null, "OWNER.PRIME@DEMO.LOCAL", "PRIMEOWNER", "AQAAAAIAAYagAAAAEJNmwJu/1BYqJx3paJqrMcB8ul0UqYzqBJ8Ei4oNFb5VYQmmWmO8gMzjMlJ9i8aVUA==", null, false, "sec-prime", 8, false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "primeowner" },
                    { new Guid("aaaaaaa1-0000-0000-0000-000000000001"), 0, "ba54c70c-51e1-4f20-9491-88ec7baf2b93", new DateTime(2025, 10, 28, 11, 11, 15, 549, DateTimeKind.Utc).AddTicks(7635), "SystemAdmin@gmail.com", true, false, null, "SystemAdmin@gmail.com", "SystemAdmin", "AQAAAAIAAYagAAAAEAMvdRjUwV7VVr8eqJuitIdulvsle7C1XvLmyuWURegCpyHvfyBfN5tSrBw5JBk/NQ==", null, false, null, 8, false, new DateTime(2025, 10, 28, 11, 11, 15, 549, DateTimeKind.Utc).AddTicks(7638), "SystemAdmin" },
                    { new Guid("aaaaaaa1-0000-0000-0000-000000000002"), 0, "d6e03fc4-0552-41f9-83bd-aba37b12311d", new DateTime(2025, 10, 28, 11, 11, 15, 503, DateTimeKind.Utc).AddTicks(295), "store1@gmail.com", true, false, null, "store1@gmail.com", "store1", "AQAAAAIAAYagAAAAEBmqNtSZLXy1Kc4GKjbYMRqBrtoMHEcA3+HX/ZZxyc6FaYCPIT1+DZI3gMpgnWpP+w==", null, false, null, 8, false, new DateTime(2025, 10, 28, 11, 11, 15, 503, DateTimeKind.Utc).AddTicks(304), "store1" },
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa01"), 0, "26f1140f-d714-4cdc-bea5-d9c101246ab9", new DateTime(2025, 9, 30, 10, 0, 0, 0, DateTimeKind.Utc), "owner@test.local", true, false, null, "OWNER@TEST.LOCAL", "STOREOWNER", "AQAAAA...", null, false, null, 8, false, new DateTime(2025, 9, 30, 10, 0, 0, 0, DateTimeKind.Utc), "storeowner" },
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa02"), 0, "5c66bde6-cd90-4ee4-bdbd-e0ef3b830bd3", new DateTime(2025, 9, 30, 10, 0, 0, 0, DateTimeKind.Utc), "cust1@test.local", true, false, null, "CUST1@TEST.LOCAL", "CUSTOMER1", "AQAAAA...", null, false, null, 8, false, new DateTime(2025, 9, 30, 10, 0, 0, 0, DateTimeKind.Utc), "customer1" }
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CreatedAt", "DeletedAt", "Description", "Icon", "IsActive", "IsLeaf", "Level", "Name", "ParentId", "Path", "Slug", "SortOrder", "Status", "StoreId", "ThumbnailUrl", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { new Guid("aaaaaaa1-0000-0000-0000-000000000001"), new DateTime(2025, 10, 28, 18, 11, 15, 503, DateTimeKind.Local).AddTicks(635), null, null, null, true, false, 0, "Dịch vụ vận chuyển", null, null, "van-chuyen", 1, 8, null, null, null, null },
                    { new Guid("aaaaaaa2-0000-0000-0000-000000000001"), new DateTime(2025, 10, 28, 18, 11, 15, 503, DateTimeKind.Local).AddTicks(654), null, null, null, true, false, 0, "Lưu kho", null, null, "luu-kho", 2, 8, null, null, null, null },
                    { new Guid("aaaaaaa3-0000-0000-0000-000000000001"), new DateTime(2025, 10, 28, 18, 11, 15, 503, DateTimeKind.Local).AddTicks(655), null, null, null, true, false, 0, "Dọn dẹp", null, null, "don-dep", 3, 8, null, null, null, null },
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa10"), new DateTime(2025, 10, 28, 18, 11, 15, 597, DateTimeKind.Local).AddTicks(3738), null, null, null, true, false, 0, "Moving", null, null, null, null, 8, null, null, null, null },
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa11"), new DateTime(2025, 10, 28, 18, 11, 15, 597, DateTimeKind.Local).AddTicks(3740), null, null, null, true, false, 0, "Storage", null, null, null, null, 8, null, null, null, null }
                });

            migrationBuilder.InsertData(
                table: "Addresses",
                columns: new[] { "Id", "Active", "AddressLine", "City", "CreatedAt", "DeletedAt", "District", "IsDefault", "Label", "Latitude", "Longitude", "StoreId", "UpdatedAt", "UpdatedBy", "UserId", "Ward" },
                values: new object[,]
                {
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa21"), true, "89 Trần Hưng Đạo", "Hà Nội", new DateTime(2025, 10, 28, 18, 11, 15, 597, DateTimeKind.Local).AddTicks(3647), null, "Hoàn Kiếm", true, "Home Pickup", 21.026, 105.84099999999999, null, null, null, new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa02"), "Cửa Nam" },
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa22"), true, "25 Lê Duẩn", "Hồ Chí Minh", new DateTime(2025, 10, 28, 18, 11, 15, 597, DateTimeKind.Local).AddTicks(3695), null, "Q.1", false, "New Apartment", 10.782, 106.7, null, null, null, new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa02"), "Bến Nghé" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { new Guid("aaaaaaa1-0000-0000-0000-000000000002"), new Guid("22222222-2222-2222-2222-222222222221") },
                    { new Guid("aaaaaaa1-0000-0000-0000-000000000002"), new Guid("22222222-2222-2222-2222-222222222222") },
                    { new Guid("aaaaaaa1-0000-0000-0000-000000000002"), new Guid("22222222-2222-2222-2222-222222222223") },
                    { new Guid("aaaaaaa1-0000-0000-0000-000000000001"), new Guid("aaaaaaa1-0000-0000-0000-000000000001") },
                    { new Guid("aaaaaaa1-0000-0000-0000-000000000002"), new Guid("aaaaaaa1-0000-0000-0000-000000000002") }
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CreatedAt", "DeletedAt", "Description", "Icon", "IsActive", "IsLeaf", "Level", "Name", "ParentId", "Path", "Slug", "SortOrder", "Status", "StoreId", "ThumbnailUrl", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { new Guid("aaaaaaa1-0000-0000-0000-000000000002"), new DateTime(2025, 10, 28, 18, 11, 15, 503, DateTimeKind.Local).AddTicks(657), null, null, null, true, false, 0, "Chuyển nhà", new Guid("aaaaaaa1-0000-0000-0000-000000000001"), null, "chuyen-nha", 1, 8, null, null, null, null },
                    { new Guid("aaaaaaa1-0000-0000-0000-000000000003"), new DateTime(2025, 10, 28, 18, 11, 15, 503, DateTimeKind.Local).AddTicks(658), null, null, null, true, false, 0, "Chuyển văn phòng", new Guid("aaaaaaa1-0000-0000-0000-000000000001"), null, "chuyen-van-phong", 2, 8, null, null, null, null },
                    { new Guid("aaaaaaa1-0000-0000-0000-000000000004"), new DateTime(2025, 10, 28, 18, 11, 15, 503, DateTimeKind.Local).AddTicks(660), null, null, null, true, false, 0, "Xe tải theo km", new Guid("aaaaaaa1-0000-0000-0000-000000000001"), null, "xe-tai-theo-km", 3, 8, null, null, null, null },
                    { new Guid("aaaaaaa2-0000-0000-0000-000000000002"), new DateTime(2025, 10, 28, 18, 11, 15, 503, DateTimeKind.Local).AddTicks(661), null, null, null, true, false, 0, "Theo giờ", new Guid("aaaaaaa2-0000-0000-0000-000000000001"), null, "theo-gio", 1, 8, null, null, null, null },
                    { new Guid("aaaaaaa2-0000-0000-0000-000000000003"), new DateTime(2025, 10, 28, 18, 11, 15, 503, DateTimeKind.Local).AddTicks(663), null, null, null, true, false, 0, "Theo ngày", new Guid("aaaaaaa2-0000-0000-0000-000000000001"), null, "theo-ngay", 2, 8, null, null, null, null },
                    { new Guid("aaaaaaa3-0000-0000-0000-000000000002"), new DateTime(2025, 10, 28, 18, 11, 15, 503, DateTimeKind.Local).AddTicks(664), null, null, null, true, false, 0, "Vệ sinh nhà", new Guid("aaaaaaa3-0000-0000-0000-000000000001"), null, "ve-sinh-nha", 1, 8, null, null, null, null },
                    { new Guid("aaaaaaa3-0000-0000-0000-000000000003"), new DateTime(2025, 10, 28, 18, 11, 15, 503, DateTimeKind.Local).AddTicks(666), null, null, null, true, false, 0, "Vệ sinh văn phòng", new Guid("aaaaaaa3-0000-0000-0000-000000000001"), null, "ve-sinh-van-phong", 2, 8, null, null, null, null }
                });

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "Id", "CreatedAt", "DeletedAt", "Email", "FullName", "KycLevel", "PhoneNumber", "PreferredLang", "Tier", "UpdatedAt", "UpdatedBy" },
                values: new object[] { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa02"), new DateTime(2025, 9, 30, 10, 0, 0, 0, DateTimeKind.Utc), null, "a@gmail.com", "Nguyễn Văn A", "None", "0123456789", "vi", "Basic", null, null });

            migrationBuilder.InsertData(
                table: "Stores",
                columns: new[] { "Id", "ActiveRegions", "BankAccountNumber", "BankName", "ContactEmail", "ContactPhone", "CreatedAt", "DeletedAt", "IsVerified", "Latitude", "LegalName", "LicenseExpiryDate", "LicenseNumber", "Longitude", "MaxOrdersPerDay", "OwnerUserId", "RatingAvg", "RatingCount", "ServiceTypes", "Status", "StoreName", "TaxNumber", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa1"), null, null, null, null, null, new DateTime(2025, 10, 28, 11, 11, 15, 597, DateTimeKind.Utc).AddTicks(3230), null, false, null, null, null, null, null, null, new Guid("22222222-2222-2222-2222-222222222221"), 0m, 0, null, 9, "Blue Wash", null, new DateTime(2025, 10, 28, 11, 11, 15, 597, DateTimeKind.Utc).AddTicks(3231), null },
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa2"), null, null, null, null, null, new DateTime(2025, 10, 28, 11, 11, 15, 597, DateTimeKind.Utc).AddTicks(3237), null, false, null, null, null, null, null, null, new Guid("22222222-2222-2222-2222-222222222222"), 0m, 0, null, 9, "Fresh Laundry", null, new DateTime(2025, 10, 28, 11, 11, 15, 597, DateTimeKind.Utc).AddTicks(3238), null }
                });

            migrationBuilder.InsertData(
                table: "Stores",
                columns: new[] { "Id", "ActiveRegions", "BankAccountNumber", "BankName", "ContactEmail", "ContactPhone", "CreatedAt", "DeletedAt", "IsVerified", "KycLevel", "Latitude", "LegalName", "LicenseExpiryDate", "LicenseNumber", "Longitude", "MaxOrdersPerDay", "OwnerUserId", "RatingAvg", "RatingCount", "ServiceTypes", "Status", "StoreName", "TaxNumber", "UpdatedAt", "UpdatedBy" },
                values: new object[] { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa3"), "HN,HCM", null, null, null, null, new DateTime(2025, 9, 28, 11, 11, 15, 597, DateTimeKind.Utc).AddTicks(3307), null, false, "Verified", null, null, null, null, null, 80, new Guid("22222222-2222-2222-2222-222222222223"), 0m, 0, null, 8, "Prime Cleaners", null, new DateTime(2025, 10, 28, 11, 11, 15, 597, DateTimeKind.Utc).AddTicks(3313), null });

            migrationBuilder.InsertData(
                table: "Addresses",
                columns: new[] { "Id", "Active", "AddressLine", "City", "CreatedAt", "DeletedAt", "District", "IsDefault", "Label", "Latitude", "Longitude", "StoreId", "UpdatedAt", "UpdatedBy", "UserId", "Ward" },
                values: new object[] { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa20"), true, "12 Nguyễn Huệ", "Hồ Chí Minh", new DateTime(2025, 10, 28, 18, 11, 15, 597, DateTimeKind.Local).AddTicks(3638), null, "Q.1", true, "Store HQ", 10.772, 106.70399999999999, new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa3"), null, null, null, "Bến Nghé" });

            migrationBuilder.InsertData(
                table: "KycSubmissions",
                columns: new[] { "Id", "AdminNote", "CreatedAt", "DeletedAt", "ReviewedAt", "ReviewedBy", "Status", "StoreId", "SubmittedAt", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb1"), null, new DateTime(2025, 10, 22, 11, 11, 15, 597, DateTimeKind.Utc).AddTicks(3379), null, null, null, 0, new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa1"), new DateTime(2025, 10, 22, 11, 11, 15, 597, DateTimeKind.Utc).AddTicks(3377), null, null },
                    { new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb2"), "Thiếu giấy tờ thuế / ảnh mờ, vui lòng bổ sung.", new DateTime(2025, 10, 23, 11, 11, 15, 597, DateTimeKind.Utc).AddTicks(3384), null, new DateTime(2025, 10, 24, 11, 11, 15, 597, DateTimeKind.Utc).AddTicks(3382), new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd"), 1, new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa2"), new DateTime(2025, 10, 23, 11, 11, 15, 597, DateTimeKind.Utc).AddTicks(3382), null, null },
                    { new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb3"), "Ok", new DateTime(2025, 10, 13, 11, 11, 15, 597, DateTimeKind.Utc).AddTicks(3388), null, new DateTime(2025, 10, 14, 11, 11, 15, 597, DateTimeKind.Utc).AddTicks(3387), new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd"), 2, new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa3"), new DateTime(2025, 10, 13, 11, 11, 15, 597, DateTimeKind.Utc).AddTicks(3387), null, null }
                });

            migrationBuilder.InsertData(
                table: "Orders",
                columns: new[] { "Id", "CreatedAt", "CustomerId", "DeletedAt", "DeliveryDate", "DistanceKm", "DropoffAddressId", "EtaMinutes", "Note", "PickupAddressId", "PickupDate", "ProductCategoryIds", "QuotationId", "Status", "StoreId", "TotalAmount", "UpdatedAt", "UpdatedBy" },
                values: new object[] { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa30"), new DateTime(2025, 8, 15, 9, 0, 0, 0, DateTimeKind.Utc), new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa02"), null, null, 7.2m, new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa22"), 55, "August order", new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa21"), null, null, null, 14, new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa3"), 2200000m, new DateTime(2025, 8, 15, 9, 0, 0, 0, DateTimeKind.Utc), null });

            migrationBuilder.InsertData(
                table: "Orders",
                columns: new[] { "Id", "CreatedAt", "CustomerId", "DeletedAt", "DeliveryDate", "DistanceKm", "DropoffAddressId", "EtaMinutes", "Note", "PickupAddressId", "PickupDate", "ProductCategoryIds", "QuotationId", "StoreId", "TotalAmount", "UpdatedAt", "UpdatedBy" },
                values: new object[] { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa31"), new DateTime(2025, 9, 29, 14, 30, 0, 0, DateTimeKind.Utc), new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa02"), null, null, 5.1m, new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa22"), 45, "Yesterday pending", new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa21"), null, null, null, new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa3"), 1500000m, new DateTime(2025, 9, 29, 14, 30, 0, 0, DateTimeKind.Utc), null });

            migrationBuilder.InsertData(
                table: "Orders",
                columns: new[] { "Id", "CreatedAt", "CustomerId", "DeletedAt", "DeliveryDate", "DistanceKm", "DropoffAddressId", "EtaMinutes", "Note", "PickupAddressId", "PickupDate", "ProductCategoryIds", "QuotationId", "Status", "StoreId", "TotalAmount", "UpdatedAt", "UpdatedBy" },
                values: new object[] { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa32"), new DateTime(2025, 9, 30, 8, 0, 0, 0, DateTimeKind.Utc), new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa02"), null, null, 3.4m, new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa22"), 35, "Today completed", new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa21"), null, null, null, 14, new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa3"), 2000000m, new DateTime(2025, 9, 30, 9, 0, 0, 0, DateTimeKind.Utc), null });

            migrationBuilder.InsertData(
                table: "Orders",
                columns: new[] { "Id", "CreatedAt", "CustomerId", "DeletedAt", "DeliveryDate", "DistanceKm", "DropoffAddressId", "EtaMinutes", "Note", "PickupAddressId", "PickupDate", "ProductCategoryIds", "QuotationId", "StoreId", "TotalAmount", "UpdatedAt", "UpdatedBy" },
                values: new object[] { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa33"), new DateTime(2025, 9, 30, 9, 0, 0, 0, DateTimeKind.Utc), new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa02"), null, null, 9.0m, new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa22"), 70, "Today pending", new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa21"), null, null, null, new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa3"), 3100000m, new DateTime(2025, 9, 30, 9, 40, 0, 0, DateTimeKind.Utc), null });

            migrationBuilder.InsertData(
                table: "Services",
                columns: new[] { "Id", "BasePrice", "CategoryId", "CreatedAt", "DeletedAt", "Description", "IsActive", "Name", "PricingModel", "Status", "StoreId", "Unit", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa12"), 1500000m, new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa10"), new DateTime(2025, 8, 15, 9, 0, 0, 0, DateTimeKind.Utc), null, "Local moving inside city", true, "House Moving (City)", 3, 0, new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa3"), "Job", null, null },
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa13"), 300000m, new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa10"), new DateTime(2025, 8, 15, 9, 0, 0, 0, DateTimeKind.Utc), null, "Boxes & packing", true, "Packing Service", 3, 0, new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa3"), "Package", null, null }
                });

            migrationBuilder.InsertData(
                table: "Feedbacks",
                columns: new[] { "Id", "Comment", "CreatedAt", "DeletedAt", "FromUserId", "OrderId", "Rating", "ToStoreId", "UpdatedAt", "UpdatedBy" },
                values: new object[] { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa50"), "Very smooth job!", new DateTime(2025, 9, 30, 10, 0, 0, 0, DateTimeKind.Utc), null, new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa02"), new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa32"), 5, new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa3"), null, null });

            migrationBuilder.InsertData(
                table: "KycDocuments",
                columns: new[] { "Id", "CreatedAt", "DeletedAt", "DocType", "FilePath", "Hash", "KycSubmissionId", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc1"), new DateTime(2025, 10, 22, 11, 11, 15, 597, DateTimeKind.Utc).AddTicks(3450), null, "License", "/uploads/kyc/blue/license.pdf", null, new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb1"), null, null },
                    { new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc2"), new DateTime(2025, 10, 22, 11, 11, 15, 597, DateTimeKind.Utc).AddTicks(3452), null, "ID", "/uploads/kyc/blue/id.jpg", null, new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb1"), null, null },
                    { new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc3"), new DateTime(2025, 10, 22, 11, 11, 15, 597, DateTimeKind.Utc).AddTicks(3455), null, "Tax", "/uploads/kyc/blue/tax.pdf", null, new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb1"), null, null },
                    { new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc4"), new DateTime(2025, 10, 23, 11, 11, 15, 597, DateTimeKind.Utc).AddTicks(3457), null, "License", "/uploads/kyc/fresh/license.pdf", null, new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb2"), null, null },
                    { new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc5"), new DateTime(2025, 10, 23, 11, 11, 15, 597, DateTimeKind.Utc).AddTicks(3458), null, "ID", "/uploads/kyc/fresh/id.jpg", null, new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb2"), null, null },
                    { new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc6"), new DateTime(2025, 10, 13, 11, 11, 15, 597, DateTimeKind.Utc).AddTicks(3460), null, "License", "/uploads/kyc/prime/license.pdf", null, new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb3"), null, null },
                    { new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc7"), new DateTime(2025, 10, 13, 11, 11, 15, 597, DateTimeKind.Utc).AddTicks(3462), null, "ID", "/uploads/kyc/prime/id.jpg", null, new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb3"), null, null },
                    { new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc8"), new DateTime(2025, 10, 13, 11, 11, 15, 597, DateTimeKind.Utc).AddTicks(3464), null, "Tax", "/uploads/kyc/prime/tax.pdf", null, new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb3"), null, null }
                });

            migrationBuilder.InsertData(
                table: "OrderItems",
                columns: new[] { "Id", "CreatedAt", "DeletedAt", "Description", "HeightM", "ItemName", "LengthM", "OrderId", "Quantity", "ServiceId", "SizeCode", "Subtotal", "UnitPrice", "UpdatedAt", "UpdatedBy", "WeightKg", "WidthM" },
                values: new object[,]
                {
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa40"), new DateTime(2025, 10, 28, 18, 11, 15, 597, DateTimeKind.Local).AddTicks(4000), null, null, null, "Moving Service", null, new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa30"), 1, new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa12"), null, 2200000m, 2200000m, null, null, null, null },
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa41"), new DateTime(2025, 10, 28, 18, 11, 15, 597, DateTimeKind.Local).AddTicks(4006), null, null, null, "Moving Service", null, new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa31"), 1, new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa12"), null, 1200000m, 1200000m, null, null, null, null },
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa42"), new DateTime(2025, 10, 28, 18, 11, 15, 597, DateTimeKind.Local).AddTicks(4010), null, null, null, "Moving Service", null, new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa31"), 1, new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa13"), null, 300000m, 300000m, null, null, null, null },
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa43"), new DateTime(2025, 10, 28, 18, 11, 15, 597, DateTimeKind.Local).AddTicks(4021), null, null, null, "Moving Service", null, new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa32"), 1, new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa12"), null, 2000000m, 2000000m, null, null, null, null }
                });

            migrationBuilder.InsertData(
                table: "PriceRules",
                columns: new[] { "Id", "ApplyModel", "CreatedAt", "DeletedAt", "MaxAreaM2", "MaxDays", "MaxQty", "MaxVolumeM3", "MinAreaM2", "MinDays", "MinQty", "MinVolumeM3", "Price", "ServiceId", "TimeUnit", "UpdatedAt", "UpdatedBy", "ValidFrom", "ValidTo" },
                values: new object[] { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa14"), 3, new DateTime(2025, 10, 28, 18, 11, 15, 597, DateTimeKind.Local).AddTicks(3881), null, null, null, 10m, null, null, null, 3m, null, 280000m, new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa13"), 0, null, null, new DateTime(2025, 8, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 12, 31, 0, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.InsertData(
                table: "Warehouses",
                columns: new[] { "Id", "AddressRefId", "CoverImageUrl", "CreatedAt", "DeletedAt", "HeightM", "LengthM", "MapImageUrl", "Name", "Status", "StoreId", "UpdatedAt", "UpdatedBy", "WidthM" },
                values: new object[] { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa60"), new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa20"), null, new DateTime(2025, 10, 28, 18, 11, 15, 597, DateTimeKind.Local).AddTicks(4148), null, 0m, 0m, null, "Main Warehouse", 0, new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa3"), null, null, 0m });

            migrationBuilder.InsertData(
                table: "WarehouseSlots",
                columns: new[] { "Id", "BasePricePerHour", "Code", "Col", "CreatedAt", "CurrentOrderId", "DeletedAt", "HeightM", "ImageUrl", "IsBlocked", "LeaseEnd", "LeaseStart", "LengthM", "Row", "Size", "Status", "UpdatedAt", "UpdatedBy", "WarehouseId", "WidthM" },
                values: new object[,]
                {
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa61"), 0m, "A1", 0, new DateTime(2025, 10, 28, 18, 11, 15, 597, DateTimeKind.Local).AddTicks(4210), null, null, 0m, null, false, null, null, 0m, 0, null, 4, null, null, new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa60"), 0m },
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa62"), 0m, "A2", 0, new DateTime(2025, 10, 28, 18, 11, 15, 597, DateTimeKind.Local).AddTicks(4239), new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa31"), null, 0m, null, false, null, null, 0m, 0, null, 5, null, null, new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa60"), 0m },
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa63"), 0m, "B1", 0, new DateTime(2025, 10, 28, 18, 11, 15, 597, DateTimeKind.Local).AddTicks(4241), null, null, 0m, null, false, null, null, 0m, 0, null, 4, null, null, new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa60"), 0m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
                keyValues: new object[] { new Guid("aaaaaaa1-0000-0000-0000-000000000002"), new Guid("22222222-2222-2222-2222-222222222221") });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("aaaaaaa1-0000-0000-0000-000000000002"), new Guid("22222222-2222-2222-2222-222222222222") });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("aaaaaaa1-0000-0000-0000-000000000002"), new Guid("22222222-2222-2222-2222-222222222223") });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("aaaaaaa1-0000-0000-0000-000000000001"), new Guid("aaaaaaa1-0000-0000-0000-000000000001") });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("aaaaaaa1-0000-0000-0000-000000000002"), new Guid("aaaaaaa1-0000-0000-0000-000000000002") });

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa01"));

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
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa11"));

            migrationBuilder.DeleteData(
                table: "Feedbacks",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa50"));

            migrationBuilder.DeleteData(
                table: "KycDocuments",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc1"));

            migrationBuilder.DeleteData(
                table: "KycDocuments",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc2"));

            migrationBuilder.DeleteData(
                table: "KycDocuments",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc3"));

            migrationBuilder.DeleteData(
                table: "KycDocuments",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc4"));

            migrationBuilder.DeleteData(
                table: "KycDocuments",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc5"));

            migrationBuilder.DeleteData(
                table: "KycDocuments",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc6"));

            migrationBuilder.DeleteData(
                table: "KycDocuments",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc7"));

            migrationBuilder.DeleteData(
                table: "KycDocuments",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc8"));

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
                table: "PriceRules",
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
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000002"));

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

            migrationBuilder.DeleteData(
                table: "KycSubmissions",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb1"));

            migrationBuilder.DeleteData(
                table: "KycSubmissions",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb2"));

            migrationBuilder.DeleteData(
                table: "KycSubmissions",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb3"));

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
                table: "Stores",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa1"));

            migrationBuilder.DeleteData(
                table: "Stores",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa2"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222221"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa02"));

            migrationBuilder.DeleteData(
                table: "Stores",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa3"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222223"));
        }
    }
}
