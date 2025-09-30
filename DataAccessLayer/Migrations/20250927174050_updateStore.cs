using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class updateStore : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BankAccountNumber",
                table: "Stores",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BankName",
                table: "Stores",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContactEmail",
                table: "Stores",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContactPhone",
                table: "Stores",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsVerified",
                table: "Stores",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LicenseExpiryDate",
                table: "Stores",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ServiceTypes",
                table: "Stores",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222221"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEABwv6osOZfgwiF90+/yxPgZj47yMNsk5HVNmJ5qK4KWBnkbwYE8dtA/0MEBVK9+Gw==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEABwv6osOZfgwiF90+/yxPgZj47yMNsk5HVNmJ5qK4KWBnkbwYE8dtA/0MEBVK9+Gw==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222223"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEABwv6osOZfgwiF90+/yxPgZj47yMNsk5HVNmJ5qK4KWBnkbwYE8dtA/0MEBVK9+Gw==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000001"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "13634868-7d93-4c66-a231-a84c3720c739", "AQAAAAIAAYagAAAAECDD6EH9rKpvKW1dOCKIBRVRNzjoy29v34Vk+Lfmp8/UfPaVShLuv44k232biS4rPw==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000002"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "5e940262-78ac-49b0-afa4-54a0f3791a8d", "AQAAAAIAAYagAAAAEP4o2JzWX6TKIPMM4NB9C48o0b3OP7C4KPE8gGuUJM2hMv/7kKjOLAotkwtaWDjtXw==" });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2025, 9, 28, 0, 40, 46, 64, DateTimeKind.Local).AddTicks(9069));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2025, 9, 28, 0, 40, 46, 64, DateTimeKind.Local).AddTicks(9099));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2025, 9, 28, 0, 40, 46, 64, DateTimeKind.Local).AddTicks(9100));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000004"),
                column: "CreatedAt",
                value: new DateTime(2025, 9, 28, 0, 40, 46, 64, DateTimeKind.Local).AddTicks(9102));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa2-0000-0000-0000-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2025, 9, 28, 0, 40, 46, 64, DateTimeKind.Local).AddTicks(9095));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa2-0000-0000-0000-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2025, 9, 28, 0, 40, 46, 64, DateTimeKind.Local).AddTicks(9104));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa2-0000-0000-0000-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2025, 9, 28, 0, 40, 46, 64, DateTimeKind.Local).AddTicks(9106));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa3-0000-0000-0000-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2025, 9, 28, 0, 40, 46, 64, DateTimeKind.Local).AddTicks(9097));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa3-0000-0000-0000-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2025, 9, 28, 0, 40, 46, 64, DateTimeKind.Local).AddTicks(9107));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa3-0000-0000-0000-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2025, 9, 28, 0, 40, 46, 64, DateTimeKind.Local).AddTicks(9109));

            migrationBuilder.UpdateData(
                table: "KycDocument",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc1"),
                column: "CreatedAt",
                value: new DateTime(2025, 9, 21, 17, 40, 46, 156, DateTimeKind.Utc).AddTicks(9013));

            migrationBuilder.UpdateData(
                table: "KycDocument",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc2"),
                column: "CreatedAt",
                value: new DateTime(2025, 9, 21, 17, 40, 46, 156, DateTimeKind.Utc).AddTicks(9015));

            migrationBuilder.UpdateData(
                table: "KycDocument",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc3"),
                column: "CreatedAt",
                value: new DateTime(2025, 9, 21, 17, 40, 46, 156, DateTimeKind.Utc).AddTicks(9019));

            migrationBuilder.UpdateData(
                table: "KycDocument",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc4"),
                column: "CreatedAt",
                value: new DateTime(2025, 9, 22, 17, 40, 46, 156, DateTimeKind.Utc).AddTicks(9026));

            migrationBuilder.UpdateData(
                table: "KycDocument",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc5"),
                column: "CreatedAt",
                value: new DateTime(2025, 9, 22, 17, 40, 46, 156, DateTimeKind.Utc).AddTicks(9028));

            migrationBuilder.UpdateData(
                table: "KycDocument",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc6"),
                column: "CreatedAt",
                value: new DateTime(2025, 9, 12, 17, 40, 46, 156, DateTimeKind.Utc).AddTicks(9030));

            migrationBuilder.UpdateData(
                table: "KycDocument",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc7"),
                column: "CreatedAt",
                value: new DateTime(2025, 9, 12, 17, 40, 46, 156, DateTimeKind.Utc).AddTicks(9032));

            migrationBuilder.UpdateData(
                table: "KycDocument",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc8"),
                column: "CreatedAt",
                value: new DateTime(2025, 9, 12, 17, 40, 46, 156, DateTimeKind.Utc).AddTicks(9403));

            migrationBuilder.UpdateData(
                table: "KycSubmission",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb1"),
                columns: new[] { "CreatedAt", "SubmittedAt" },
                values: new object[] { new DateTime(2025, 9, 21, 17, 40, 46, 156, DateTimeKind.Utc).AddTicks(8932), new DateTime(2025, 9, 21, 17, 40, 46, 156, DateTimeKind.Utc).AddTicks(8930) });

            migrationBuilder.UpdateData(
                table: "KycSubmission",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb2"),
                columns: new[] { "CreatedAt", "ReviewedAt", "SubmittedAt" },
                values: new object[] { new DateTime(2025, 9, 22, 17, 40, 46, 156, DateTimeKind.Utc).AddTicks(8943), new DateTime(2025, 9, 23, 17, 40, 46, 156, DateTimeKind.Utc).AddTicks(8935), new DateTime(2025, 9, 22, 17, 40, 46, 156, DateTimeKind.Utc).AddTicks(8935) });

            migrationBuilder.UpdateData(
                table: "KycSubmission",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb3"),
                columns: new[] { "CreatedAt", "ReviewedAt", "SubmittedAt" },
                values: new object[] { new DateTime(2025, 9, 12, 17, 40, 46, 156, DateTimeKind.Utc).AddTicks(8947), new DateTime(2025, 9, 13, 17, 40, 46, 156, DateTimeKind.Utc).AddTicks(8945), new DateTime(2025, 9, 12, 17, 40, 46, 156, DateTimeKind.Utc).AddTicks(8945) });

            migrationBuilder.UpdateData(
                table: "Stores",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa1"),
                columns: new[] { "BankAccountNumber", "BankName", "ContactEmail", "ContactPhone", "CreatedAt", "IsVerified", "LicenseExpiryDate", "ServiceTypes", "UpdatedAt" },
                values: new object[] { null, null, null, null, new DateTime(2025, 9, 27, 17, 40, 46, 156, DateTimeKind.Utc).AddTicks(8824), false, null, null, new DateTime(2025, 9, 27, 17, 40, 46, 156, DateTimeKind.Utc).AddTicks(8826) });

            migrationBuilder.UpdateData(
                table: "Stores",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa2"),
                columns: new[] { "BankAccountNumber", "BankName", "ContactEmail", "ContactPhone", "CreatedAt", "IsVerified", "LicenseExpiryDate", "ServiceTypes", "UpdatedAt" },
                values: new object[] { null, null, null, null, new DateTime(2025, 9, 27, 17, 40, 46, 156, DateTimeKind.Utc).AddTicks(8835), false, null, null, new DateTime(2025, 9, 27, 17, 40, 46, 156, DateTimeKind.Utc).AddTicks(8835) });

            migrationBuilder.UpdateData(
                table: "Stores",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa3"),
                columns: new[] { "BankAccountNumber", "BankName", "ContactEmail", "ContactPhone", "CreatedAt", "IsVerified", "LicenseExpiryDate", "ServiceTypes", "UpdatedAt" },
                values: new object[] { null, null, null, null, new DateTime(2025, 8, 28, 17, 40, 46, 156, DateTimeKind.Utc).AddTicks(8839), false, null, null, new DateTime(2025, 9, 27, 17, 40, 46, 156, DateTimeKind.Utc).AddTicks(8849) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BankAccountNumber",
                table: "Stores");

            migrationBuilder.DropColumn(
                name: "BankName",
                table: "Stores");

            migrationBuilder.DropColumn(
                name: "ContactEmail",
                table: "Stores");

            migrationBuilder.DropColumn(
                name: "ContactPhone",
                table: "Stores");

            migrationBuilder.DropColumn(
                name: "IsVerified",
                table: "Stores");

            migrationBuilder.DropColumn(
                name: "LicenseExpiryDate",
                table: "Stores");

            migrationBuilder.DropColumn(
                name: "ServiceTypes",
                table: "Stores");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222221"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEHSPdv9eW93Cum7crJjcCjPJ2AD7YBcil3lp+z5VNpzGbSO/v8XT6cbJ2WnkqPudNw==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEHSPdv9eW93Cum7crJjcCjPJ2AD7YBcil3lp+z5VNpzGbSO/v8XT6cbJ2WnkqPudNw==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222223"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEHSPdv9eW93Cum7crJjcCjPJ2AD7YBcil3lp+z5VNpzGbSO/v8XT6cbJ2WnkqPudNw==");

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

            migrationBuilder.UpdateData(
                table: "KycDocument",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc1"),
                column: "CreatedAt",
                value: new DateTime(2025, 9, 20, 18, 26, 34, 396, DateTimeKind.Utc).AddTicks(4988));

            migrationBuilder.UpdateData(
                table: "KycDocument",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc2"),
                column: "CreatedAt",
                value: new DateTime(2025, 9, 20, 18, 26, 34, 396, DateTimeKind.Utc).AddTicks(4992));

            migrationBuilder.UpdateData(
                table: "KycDocument",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc3"),
                column: "CreatedAt",
                value: new DateTime(2025, 9, 20, 18, 26, 34, 396, DateTimeKind.Utc).AddTicks(4995));

            migrationBuilder.UpdateData(
                table: "KycDocument",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc4"),
                column: "CreatedAt",
                value: new DateTime(2025, 9, 21, 18, 26, 34, 396, DateTimeKind.Utc).AddTicks(5002));

            migrationBuilder.UpdateData(
                table: "KycDocument",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc5"),
                column: "CreatedAt",
                value: new DateTime(2025, 9, 21, 18, 26, 34, 396, DateTimeKind.Utc).AddTicks(5005));

            migrationBuilder.UpdateData(
                table: "KycDocument",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc6"),
                column: "CreatedAt",
                value: new DateTime(2025, 9, 11, 18, 26, 34, 396, DateTimeKind.Utc).AddTicks(5008));

            migrationBuilder.UpdateData(
                table: "KycDocument",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc7"),
                column: "CreatedAt",
                value: new DateTime(2025, 9, 11, 18, 26, 34, 396, DateTimeKind.Utc).AddTicks(5010));

            migrationBuilder.UpdateData(
                table: "KycDocument",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc8"),
                column: "CreatedAt",
                value: new DateTime(2025, 9, 11, 18, 26, 34, 396, DateTimeKind.Utc).AddTicks(5014));

            migrationBuilder.UpdateData(
                table: "KycSubmission",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb1"),
                columns: new[] { "CreatedAt", "SubmittedAt" },
                values: new object[] { new DateTime(2025, 9, 20, 18, 26, 34, 396, DateTimeKind.Utc).AddTicks(4912), new DateTime(2025, 9, 20, 18, 26, 34, 396, DateTimeKind.Utc).AddTicks(4910) });

            migrationBuilder.UpdateData(
                table: "KycSubmission",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb2"),
                columns: new[] { "CreatedAt", "ReviewedAt", "SubmittedAt" },
                values: new object[] { new DateTime(2025, 9, 21, 18, 26, 34, 396, DateTimeKind.Utc).AddTicks(4925), new DateTime(2025, 9, 22, 18, 26, 34, 396, DateTimeKind.Utc).AddTicks(4919), new DateTime(2025, 9, 21, 18, 26, 34, 396, DateTimeKind.Utc).AddTicks(4918) });

            migrationBuilder.UpdateData(
                table: "KycSubmission",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb3"),
                columns: new[] { "CreatedAt", "ReviewedAt", "SubmittedAt" },
                values: new object[] { new DateTime(2025, 9, 11, 18, 26, 34, 396, DateTimeKind.Utc).AddTicks(4931), new DateTime(2025, 9, 12, 18, 26, 34, 396, DateTimeKind.Utc).AddTicks(4929), new DateTime(2025, 9, 11, 18, 26, 34, 396, DateTimeKind.Utc).AddTicks(4928) });

            migrationBuilder.UpdateData(
                table: "Stores",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa1"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 26, 18, 26, 34, 396, DateTimeKind.Utc).AddTicks(4605), new DateTime(2025, 9, 26, 18, 26, 34, 396, DateTimeKind.Utc).AddTicks(4605) });

            migrationBuilder.UpdateData(
                table: "Stores",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa2"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 26, 18, 26, 34, 396, DateTimeKind.Utc).AddTicks(4632), new DateTime(2025, 9, 26, 18, 26, 34, 396, DateTimeKind.Utc).AddTicks(4632) });

            migrationBuilder.UpdateData(
                table: "Stores",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa3"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 8, 27, 18, 26, 34, 396, DateTimeKind.Utc).AddTicks(4739), new DateTime(2025, 9, 26, 18, 26, 34, 396, DateTimeKind.Utc).AddTicks(4749) });
        }
    }
}
