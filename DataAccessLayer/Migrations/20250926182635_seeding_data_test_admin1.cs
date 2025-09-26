using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class seeding_data_test_admin1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000001"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "5cace229-6f72-4d98-9e20-f74c8a0922a1", "AQAAAAIAAYagAAAAENf2lqnPdTjEOio6hBDe4JCHmZJNJHiJv4+W4lOAjzs6eQV1ZDCT4UAh68kLfETrpw==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000002"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "58ebc025-d4cc-47fd-b55e-ad4c656cc3fa", "AQAAAAIAAYagAAAAEIskgfElhtJc9h7C3SkxIuq9L9c7kwbAqiB51GiI6XHLez7mTeIh/itrLIJK8TQ/xA==" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "CreatedAt", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "Status", "TwoFactorEnabled", "UpdatedAt", "UserName" },
                values: new object[,]
                {
                    { new Guid("22222222-2222-2222-2222-222222222221"), 0, "con-blue", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "owner.blue@demo.local", true, false, null, "OWNER.BLUE@DEMO.LOCAL", "BLUEOWNER", "AQAAAAIAAYagAAAAEHSPdv9eW93Cum7crJjcCjPJ2AD7YBcil3lp+z5VNpzGbSO/v8XT6cbJ2WnkqPudNw==", null, false, "sec-blue", "Active", false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "blueowner" },
                    { new Guid("22222222-2222-2222-2222-222222222222"), 0, "con-fresh", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "owner.fresh@demo.local", true, false, null, "OWNER.FRESH@DEMO.LOCAL", "FRESHOWNER", "AQAAAAIAAYagAAAAEHSPdv9eW93Cum7crJjcCjPJ2AD7YBcil3lp+z5VNpzGbSO/v8XT6cbJ2WnkqPudNw==", null, false, "sec-fresh", "Active", false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "freshowner" },
                    { new Guid("22222222-2222-2222-2222-222222222223"), 0, "con-prime", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "owner.prime@demo.local", true, false, null, "OWNER.PRIME@DEMO.LOCAL", "PRIMEOWNER", "AQAAAAIAAYagAAAAEHSPdv9eW93Cum7crJjcCjPJ2AD7YBcil3lp+z5VNpzGbSO/v8XT6cbJ2WnkqPudNw==", null, false, "sec-prime", "Active", false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "primeowner" }
                });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 1, 26, 34, 202, DateTimeKind.Local).AddTicks(8904));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 1, 26, 34, 202, DateTimeKind.Local).AddTicks(8961));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 1, 26, 34, 202, DateTimeKind.Local).AddTicks(8963));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000004"),
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 1, 26, 34, 202, DateTimeKind.Local).AddTicks(8966));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa2-0000-0000-0000-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 1, 26, 34, 202, DateTimeKind.Local).AddTicks(8956));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa2-0000-0000-0000-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 1, 26, 34, 202, DateTimeKind.Local).AddTicks(8968));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa2-0000-0000-0000-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 1, 26, 34, 202, DateTimeKind.Local).AddTicks(8970));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa3-0000-0000-0000-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 1, 26, 34, 202, DateTimeKind.Local).AddTicks(8959));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa3-0000-0000-0000-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 1, 26, 34, 202, DateTimeKind.Local).AddTicks(8972));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa3-0000-0000-0000-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 1, 26, 34, 202, DateTimeKind.Local).AddTicks(8974));

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { new Guid("aaaaaaa1-0000-0000-0000-000000000002"), new Guid("22222222-2222-2222-2222-222222222221") },
                    { new Guid("aaaaaaa1-0000-0000-0000-000000000002"), new Guid("22222222-2222-2222-2222-222222222222") },
                    { new Guid("aaaaaaa1-0000-0000-0000-000000000002"), new Guid("22222222-2222-2222-2222-222222222223") }
                });

            migrationBuilder.InsertData(
                table: "Stores",
                columns: new[] { "Id", "ActiveRegions", "CreatedAt", "DeletedAt", "LegalName", "LicenseNumber", "MaxOrdersPerDay", "OwnerUserId", "RatingAvg", "RatingCount", "Status", "StoreName", "TaxNumber", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa1"), null, new DateTime(2025, 9, 26, 18, 26, 34, 396, DateTimeKind.Utc).AddTicks(4605), null, null, null, null, new Guid("22222222-2222-2222-2222-222222222221"), 0m, 0, "Inactive", "Blue Wash", null, new DateTime(2025, 9, 26, 18, 26, 34, 396, DateTimeKind.Utc).AddTicks(4605), null },
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa2"), null, new DateTime(2025, 9, 26, 18, 26, 34, 396, DateTimeKind.Utc).AddTicks(4632), null, null, null, null, new Guid("22222222-2222-2222-2222-222222222222"), 0m, 0, "Inactive", "Fresh Laundry", null, new DateTime(2025, 9, 26, 18, 26, 34, 396, DateTimeKind.Utc).AddTicks(4632), null }
                });

            migrationBuilder.InsertData(
                table: "Stores",
                columns: new[] { "Id", "ActiveRegions", "CreatedAt", "DeletedAt", "KycLevel", "LegalName", "LicenseNumber", "MaxOrdersPerDay", "OwnerUserId", "RatingAvg", "RatingCount", "Status", "StoreName", "TaxNumber", "UpdatedAt", "UpdatedBy" },
                values: new object[] { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa3"), "HN,HCM", new DateTime(2025, 8, 27, 18, 26, 34, 396, DateTimeKind.Utc).AddTicks(4739), null, "Verified", null, null, 80, new Guid("22222222-2222-2222-2222-222222222223"), 0m, 0, "Active", "Prime Cleaners", null, new DateTime(2025, 9, 26, 18, 26, 34, 396, DateTimeKind.Utc).AddTicks(4749), null });

            migrationBuilder.InsertData(
                table: "KycSubmission",
                columns: new[] { "Id", "AdminNote", "CreatedAt", "DeletedAt", "ReviewedAt", "ReviewedBy", "Status", "StoreId", "SubmittedAt", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb1"), null, new DateTime(2025, 9, 20, 18, 26, 34, 396, DateTimeKind.Utc).AddTicks(4912), null, null, null, 0, new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa1"), new DateTime(2025, 9, 20, 18, 26, 34, 396, DateTimeKind.Utc).AddTicks(4910), null, null },
                    { new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb2"), "Thiếu giấy tờ thuế / ảnh mờ, vui lòng bổ sung.", new DateTime(2025, 9, 21, 18, 26, 34, 396, DateTimeKind.Utc).AddTicks(4925), null, new DateTime(2025, 9, 22, 18, 26, 34, 396, DateTimeKind.Utc).AddTicks(4919), new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd"), 1, new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa2"), new DateTime(2025, 9, 21, 18, 26, 34, 396, DateTimeKind.Utc).AddTicks(4918), null, null },
                    { new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb3"), "Ok", new DateTime(2025, 9, 11, 18, 26, 34, 396, DateTimeKind.Utc).AddTicks(4931), null, new DateTime(2025, 9, 12, 18, 26, 34, 396, DateTimeKind.Utc).AddTicks(4929), new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd"), 2, new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa3"), new DateTime(2025, 9, 11, 18, 26, 34, 396, DateTimeKind.Utc).AddTicks(4928), null, null }
                });

            migrationBuilder.InsertData(
                table: "KycDocument",
                columns: new[] { "Id", "CreatedAt", "DeletedAt", "DocType", "FilePath", "Hash", "KycSubmissionId", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc1"), new DateTime(2025, 9, 20, 18, 26, 34, 396, DateTimeKind.Utc).AddTicks(4988), null, "License", "/uploads/kyc/blue/license.pdf", null, new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb1"), null, null },
                    { new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc2"), new DateTime(2025, 9, 20, 18, 26, 34, 396, DateTimeKind.Utc).AddTicks(4992), null, "ID", "/uploads/kyc/blue/id.jpg", null, new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb1"), null, null },
                    { new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc3"), new DateTime(2025, 9, 20, 18, 26, 34, 396, DateTimeKind.Utc).AddTicks(4995), null, "Tax", "/uploads/kyc/blue/tax.pdf", null, new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb1"), null, null },
                    { new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc4"), new DateTime(2025, 9, 21, 18, 26, 34, 396, DateTimeKind.Utc).AddTicks(5002), null, "License", "/uploads/kyc/fresh/license.pdf", null, new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb2"), null, null },
                    { new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc5"), new DateTime(2025, 9, 21, 18, 26, 34, 396, DateTimeKind.Utc).AddTicks(5005), null, "ID", "/uploads/kyc/fresh/id.jpg", null, new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb2"), null, null },
                    { new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc6"), new DateTime(2025, 9, 11, 18, 26, 34, 396, DateTimeKind.Utc).AddTicks(5008), null, "License", "/uploads/kyc/prime/license.pdf", null, new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb3"), null, null },
                    { new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc7"), new DateTime(2025, 9, 11, 18, 26, 34, 396, DateTimeKind.Utc).AddTicks(5010), null, "ID", "/uploads/kyc/prime/id.jpg", null, new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb3"), null, null },
                    { new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc8"), new DateTime(2025, 9, 11, 18, 26, 34, 396, DateTimeKind.Utc).AddTicks(5014), null, "Tax", "/uploads/kyc/prime/tax.pdf", null, new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb3"), null, null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
                table: "KycDocument",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc1"));

            migrationBuilder.DeleteData(
                table: "KycDocument",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc2"));

            migrationBuilder.DeleteData(
                table: "KycDocument",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc3"));

            migrationBuilder.DeleteData(
                table: "KycDocument",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc4"));

            migrationBuilder.DeleteData(
                table: "KycDocument",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc5"));

            migrationBuilder.DeleteData(
                table: "KycDocument",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc6"));

            migrationBuilder.DeleteData(
                table: "KycDocument",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc7"));

            migrationBuilder.DeleteData(
                table: "KycDocument",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc8"));

            migrationBuilder.DeleteData(
                table: "KycSubmission",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb1"));

            migrationBuilder.DeleteData(
                table: "KycSubmission",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb2"));

            migrationBuilder.DeleteData(
                table: "KycSubmission",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb3"));

            migrationBuilder.DeleteData(
                table: "Stores",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa1"));

            migrationBuilder.DeleteData(
                table: "Stores",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa2"));

            migrationBuilder.DeleteData(
                table: "Stores",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa3"));

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
                keyValue: new Guid("22222222-2222-2222-2222-222222222223"));

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000001"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "ed2df903-0004-4ed8-ba6b-dd4beb79c911", "AQAAAAIAAYagAAAAEFUs91Dc6h3oIWkJrGiqKBUJKDhrP+fvl+rYEBqLXMMaCSp0ARVrSIGLrCf/OSTK1Q==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000002"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "002f81b7-7d33-4e67-be26-e33ba2ea870e", "AQAAAAIAAYagAAAAEJxQhx3cwkik3cMIk9Yde7yzEblY3ne3uBHue4Oa9fG5DdzbYqATWLfumOE2Pe71Jg==" });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2025, 9, 26, 23, 49, 59, 280, DateTimeKind.Local).AddTicks(202));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2025, 9, 26, 23, 49, 59, 280, DateTimeKind.Local).AddTicks(255));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2025, 9, 26, 23, 49, 59, 280, DateTimeKind.Local).AddTicks(258));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000004"),
                column: "CreatedAt",
                value: new DateTime(2025, 9, 26, 23, 49, 59, 280, DateTimeKind.Local).AddTicks(262));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa2-0000-0000-0000-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2025, 9, 26, 23, 49, 59, 280, DateTimeKind.Local).AddTicks(250));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa2-0000-0000-0000-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2025, 9, 26, 23, 49, 59, 280, DateTimeKind.Local).AddTicks(264));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa2-0000-0000-0000-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2025, 9, 26, 23, 49, 59, 280, DateTimeKind.Local).AddTicks(268));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa3-0000-0000-0000-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2025, 9, 26, 23, 49, 59, 280, DateTimeKind.Local).AddTicks(253));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa3-0000-0000-0000-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2025, 9, 26, 23, 49, 59, 280, DateTimeKind.Local).AddTicks(271));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa3-0000-0000-0000-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2025, 9, 26, 23, 49, 59, 280, DateTimeKind.Local).AddTicks(273));
        }
    }
}
