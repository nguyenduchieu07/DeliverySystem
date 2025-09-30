using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class AddEmailPhoneToCustomer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customers_Users",
                table: "Customers");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Customers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Customers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222221"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEPK1lCkCSja4kvOylnpx7C+T8XDZW+UIOLSjPbX0GY1h0c17yHSj1bt9lwoUEy8J7w==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEPK1lCkCSja4kvOylnpx7C+T8XDZW+UIOLSjPbX0GY1h0c17yHSj1bt9lwoUEy8J7w==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222223"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEPK1lCkCSja4kvOylnpx7C+T8XDZW+UIOLSjPbX0GY1h0c17yHSj1bt9lwoUEy8J7w==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000001"),
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "UpdatedAt" },
                values: new object[] { "9b5ee87b-4fee-4821-ba16-b76f2c14a756", new DateTime(2025, 9, 29, 18, 33, 17, 747, DateTimeKind.Utc).AddTicks(1356), "AQAAAAIAAYagAAAAECPe6W6ipt6XJSLYcxd1l6YT7mx4woRt01vSj7Ulsp5vGvja1IyZ2dsu/ICrGnMwvw==", new DateTime(2025, 9, 29, 18, 33, 17, 747, DateTimeKind.Utc).AddTicks(1364) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000002"),
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "UpdatedAt" },
                values: new object[] { "4c3c34d5-2dd2-4875-a13e-561686b22cc2", new DateTime(2025, 9, 29, 18, 33, 17, 789, DateTimeKind.Utc).AddTicks(5773), "AQAAAAIAAYagAAAAEL9+2YKsh6pR+7Vq9lZqVdvg5wcygd+5f6IvOzk3BJh6/hwTZ8rj+PYQlNBWqdWFMg==", new DateTime(2025, 9, 29, 18, 33, 17, 789, DateTimeKind.Utc).AddTicks(5773) });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2025, 9, 30, 1, 33, 17, 747, DateTimeKind.Local).AddTicks(1909));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2025, 9, 30, 1, 33, 17, 747, DateTimeKind.Local).AddTicks(1930));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2025, 9, 30, 1, 33, 17, 747, DateTimeKind.Local).AddTicks(1931));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000004"),
                column: "CreatedAt",
                value: new DateTime(2025, 9, 30, 1, 33, 17, 747, DateTimeKind.Local).AddTicks(1933));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa2-0000-0000-0000-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2025, 9, 30, 1, 33, 17, 747, DateTimeKind.Local).AddTicks(1925));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa2-0000-0000-0000-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2025, 9, 30, 1, 33, 17, 747, DateTimeKind.Local).AddTicks(1935));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa2-0000-0000-0000-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2025, 9, 30, 1, 33, 17, 747, DateTimeKind.Local).AddTicks(1939));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa3-0000-0000-0000-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2025, 9, 30, 1, 33, 17, 747, DateTimeKind.Local).AddTicks(1927));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa3-0000-0000-0000-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2025, 9, 30, 1, 33, 17, 747, DateTimeKind.Local).AddTicks(1940));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa3-0000-0000-0000-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2025, 9, 30, 1, 33, 17, 747, DateTimeKind.Local).AddTicks(1942));

            migrationBuilder.UpdateData(
                table: "KycDocument",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc1"),
                column: "CreatedAt",
                value: new DateTime(2025, 9, 23, 18, 33, 17, 835, DateTimeKind.Utc).AddTicks(8493));

            migrationBuilder.UpdateData(
                table: "KycDocument",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc2"),
                column: "CreatedAt",
                value: new DateTime(2025, 9, 23, 18, 33, 17, 835, DateTimeKind.Utc).AddTicks(8495));

            migrationBuilder.UpdateData(
                table: "KycDocument",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc3"),
                column: "CreatedAt",
                value: new DateTime(2025, 9, 23, 18, 33, 17, 835, DateTimeKind.Utc).AddTicks(8498));

            migrationBuilder.UpdateData(
                table: "KycDocument",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc4"),
                column: "CreatedAt",
                value: new DateTime(2025, 9, 24, 18, 33, 17, 835, DateTimeKind.Utc).AddTicks(8500));

            migrationBuilder.UpdateData(
                table: "KycDocument",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc5"),
                column: "CreatedAt",
                value: new DateTime(2025, 9, 24, 18, 33, 17, 835, DateTimeKind.Utc).AddTicks(8502));

            migrationBuilder.UpdateData(
                table: "KycDocument",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc6"),
                column: "CreatedAt",
                value: new DateTime(2025, 9, 14, 18, 33, 17, 835, DateTimeKind.Utc).AddTicks(8504));

            migrationBuilder.UpdateData(
                table: "KycDocument",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc7"),
                column: "CreatedAt",
                value: new DateTime(2025, 9, 14, 18, 33, 17, 835, DateTimeKind.Utc).AddTicks(8507));

            migrationBuilder.UpdateData(
                table: "KycDocument",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc8"),
                column: "CreatedAt",
                value: new DateTime(2025, 9, 14, 18, 33, 17, 835, DateTimeKind.Utc).AddTicks(8509));

            migrationBuilder.UpdateData(
                table: "KycSubmission",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb1"),
                columns: new[] { "CreatedAt", "SubmittedAt" },
                values: new object[] { new DateTime(2025, 9, 23, 18, 33, 17, 835, DateTimeKind.Utc).AddTicks(8435), new DateTime(2025, 9, 23, 18, 33, 17, 835, DateTimeKind.Utc).AddTicks(8434) });

            migrationBuilder.UpdateData(
                table: "KycSubmission",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb2"),
                columns: new[] { "CreatedAt", "ReviewedAt", "SubmittedAt" },
                values: new object[] { new DateTime(2025, 9, 24, 18, 33, 17, 835, DateTimeKind.Utc).AddTicks(8443), new DateTime(2025, 9, 25, 18, 33, 17, 835, DateTimeKind.Utc).AddTicks(8438), new DateTime(2025, 9, 24, 18, 33, 17, 835, DateTimeKind.Utc).AddTicks(8438) });

            migrationBuilder.UpdateData(
                table: "KycSubmission",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb3"),
                columns: new[] { "CreatedAt", "ReviewedAt", "SubmittedAt" },
                values: new object[] { new DateTime(2025, 9, 14, 18, 33, 17, 835, DateTimeKind.Utc).AddTicks(8447), new DateTime(2025, 9, 15, 18, 33, 17, 835, DateTimeKind.Utc).AddTicks(8445), new DateTime(2025, 9, 14, 18, 33, 17, 835, DateTimeKind.Utc).AddTicks(8445) });

            migrationBuilder.UpdateData(
                table: "Stores",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa1"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 29, 18, 33, 17, 835, DateTimeKind.Utc).AddTicks(8219), new DateTime(2025, 9, 29, 18, 33, 17, 835, DateTimeKind.Utc).AddTicks(8220) });

            migrationBuilder.UpdateData(
                table: "Stores",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa2"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 29, 18, 33, 17, 835, DateTimeKind.Utc).AddTicks(8352), new DateTime(2025, 9, 29, 18, 33, 17, 835, DateTimeKind.Utc).AddTicks(8353) });

            migrationBuilder.UpdateData(
                table: "Stores",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa3"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 8, 30, 18, 33, 17, 835, DateTimeKind.Utc).AddTicks(8357), new DateTime(2025, 9, 29, 18, 33, 17, 835, DateTimeKind.Utc).AddTicks(8363) });

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_Users",
                table: "Customers",
                column: "Id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customers_Users",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Customers");

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
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "UpdatedAt" },
                values: new object[] { "13634868-7d93-4c66-a231-a84c3720c739", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "AQAAAAIAAYagAAAAECDD6EH9rKpvKW1dOCKIBRVRNzjoy29v34Vk+Lfmp8/UfPaVShLuv44k232biS4rPw==", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000002"),
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "UpdatedAt" },
                values: new object[] { "5e940262-78ac-49b0-afa4-54a0f3791a8d", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "AQAAAAIAAYagAAAAEP4o2JzWX6TKIPMM4NB9C48o0b3OP7C4KPE8gGuUJM2hMv/7kKjOLAotkwtaWDjtXw==", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

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
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 27, 17, 40, 46, 156, DateTimeKind.Utc).AddTicks(8824), new DateTime(2025, 9, 27, 17, 40, 46, 156, DateTimeKind.Utc).AddTicks(8826) });

            migrationBuilder.UpdateData(
                table: "Stores",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa2"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 27, 17, 40, 46, 156, DateTimeKind.Utc).AddTicks(8835), new DateTime(2025, 9, 27, 17, 40, 46, 156, DateTimeKind.Utc).AddTicks(8835) });

            migrationBuilder.UpdateData(
                table: "Stores",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa3"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 8, 28, 17, 40, 46, 156, DateTimeKind.Utc).AddTicks(8839), new DateTime(2025, 9, 27, 17, 40, 46, 156, DateTimeKind.Utc).AddTicks(8849) });

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_Users",
                table: "Customers",
                column: "Id",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
