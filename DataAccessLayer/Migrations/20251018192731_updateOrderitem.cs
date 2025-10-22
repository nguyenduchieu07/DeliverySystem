using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class updateOrderitem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_Orders",
                table: "OrderItems");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_Services",
                table: "OrderItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK__OrderIte__57ED0681941C3C3C",
                table: "OrderItems");

            migrationBuilder.AlterColumn<decimal>(
                name: "UnitPrice",
                table: "OrderItems",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(12,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Subtotal",
                table: "OrderItems",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(12,2)");

            migrationBuilder.AlterColumn<Guid>(
                name: "ServiceId",
                table: "OrderItems",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "OrderItems",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldDefaultValueSql: "(newid())");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "OrderItems",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "HeightM",
                table: "OrderItems",
                type: "decimal(5,2)",
                precision: 5,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ItemName",
                table: "OrderItems",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "LengthM",
                table: "OrderItems",
                type: "decimal(5,2)",
                precision: 5,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SizeCode",
                table: "OrderItems",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "WeightKg",
                table: "OrderItems",
                type: "decimal(6,2)",
                precision: 6,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "WidthM",
                table: "OrderItems",
                type: "decimal(5,2)",
                precision: 5,
                scale: 2,
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderItems",
                table: "OrderItems",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "Addresses",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa20"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 19, 2, 27, 27, 571, DateTimeKind.Local).AddTicks(3268));

            migrationBuilder.UpdateData(
                table: "Addresses",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa21"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 19, 2, 27, 27, 571, DateTimeKind.Local).AddTicks(3280));

            migrationBuilder.UpdateData(
                table: "Addresses",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa22"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 19, 2, 27, 27, 571, DateTimeKind.Local).AddTicks(3285));

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222221"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEImHFIWpQkSJ2wobU3+C0nSy8GzPjfBFJ0Yy4Pd6yyb5xr22mDomtyP5PXwMXurFSg==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEImHFIWpQkSJ2wobU3+C0nSy8GzPjfBFJ0Yy4Pd6yyb5xr22mDomtyP5PXwMXurFSg==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222223"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEImHFIWpQkSJ2wobU3+C0nSy8GzPjfBFJ0Yy4Pd6yyb5xr22mDomtyP5PXwMXurFSg==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000001"),
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "UpdatedAt" },
                values: new object[] { "be10a959-e635-47fc-ba62-d9dfc3bda7a1", new DateTime(2025, 10, 18, 19, 27, 27, 475, DateTimeKind.Utc).AddTicks(8297), "AQAAAAIAAYagAAAAEAgfvE4M3HEnJJughuFC1i6pcpKvhSysgpAwYJGhJsH0pcs3JNhYqWHxKxQQRim8CQ==", new DateTime(2025, 10, 18, 19, 27, 27, 475, DateTimeKind.Utc).AddTicks(8305) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000002"),
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "UpdatedAt" },
                values: new object[] { "fa0aeb61-2444-47cf-aa47-2107e19ad81d", new DateTime(2025, 10, 18, 19, 27, 27, 522, DateTimeKind.Utc).AddTicks(6669), "AQAAAAIAAYagAAAAEBja5+OOkPTvrWYrPwPktUQUcrjnHICTHSiUT5tPSOu0XLLP1NkPC4QMaqThezfG2g==", new DateTime(2025, 10, 18, 19, 27, 27, 522, DateTimeKind.Utc).AddTicks(6678) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa01"),
                column: "ConcurrencyStamp",
                value: "0084abab-80f8-4291-bb4d-7f130dd41858");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa02"),
                column: "ConcurrencyStamp",
                value: "b1c0d5b3-fa8e-4d4b-b3d6-b0142a0eba62");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 19, 2, 27, 27, 475, DateTimeKind.Local).AddTicks(8819));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 19, 2, 27, 27, 475, DateTimeKind.Local).AddTicks(8842));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 19, 2, 27, 27, 475, DateTimeKind.Local).AddTicks(8844));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000004"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 19, 2, 27, 27, 475, DateTimeKind.Local).AddTicks(8845));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa2-0000-0000-0000-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 19, 2, 27, 27, 475, DateTimeKind.Local).AddTicks(8838));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa2-0000-0000-0000-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 19, 2, 27, 27, 475, DateTimeKind.Local).AddTicks(8847));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa2-0000-0000-0000-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 19, 2, 27, 27, 475, DateTimeKind.Local).AddTicks(8849));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa3-0000-0000-0000-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 19, 2, 27, 27, 475, DateTimeKind.Local).AddTicks(8840));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa3-0000-0000-0000-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 19, 2, 27, 27, 475, DateTimeKind.Local).AddTicks(8851));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa3-0000-0000-0000-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 19, 2, 27, 27, 475, DateTimeKind.Local).AddTicks(8852));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa10"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 19, 2, 27, 27, 571, DateTimeKind.Local).AddTicks(3331));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa11"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 19, 2, 27, 27, 571, DateTimeKind.Local).AddTicks(3334));

            migrationBuilder.UpdateData(
                table: "KycDocuments",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc1"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 12, 19, 27, 27, 571, DateTimeKind.Utc).AddTicks(2792));

            migrationBuilder.UpdateData(
                table: "KycDocuments",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc2"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 12, 19, 27, 27, 571, DateTimeKind.Utc).AddTicks(2797));

            migrationBuilder.UpdateData(
                table: "KycDocuments",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc3"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 12, 19, 27, 27, 571, DateTimeKind.Utc).AddTicks(2800));

            migrationBuilder.UpdateData(
                table: "KycDocuments",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc4"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 13, 19, 27, 27, 571, DateTimeKind.Utc).AddTicks(2802));

            migrationBuilder.UpdateData(
                table: "KycDocuments",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc5"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 13, 19, 27, 27, 571, DateTimeKind.Utc).AddTicks(2804));

            migrationBuilder.UpdateData(
                table: "KycDocuments",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc6"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 3, 19, 27, 27, 571, DateTimeKind.Utc).AddTicks(2806));

            migrationBuilder.UpdateData(
                table: "KycDocuments",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc7"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 3, 19, 27, 27, 571, DateTimeKind.Utc).AddTicks(2809));

            migrationBuilder.UpdateData(
                table: "KycDocuments",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc8"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 3, 19, 27, 27, 571, DateTimeKind.Utc).AddTicks(2811));

            migrationBuilder.UpdateData(
                table: "KycSubmissions",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb1"),
                columns: new[] { "CreatedAt", "SubmittedAt" },
                values: new object[] { new DateTime(2025, 10, 12, 19, 27, 27, 571, DateTimeKind.Utc).AddTicks(2608), new DateTime(2025, 10, 12, 19, 27, 27, 571, DateTimeKind.Utc).AddTicks(2602) });

            migrationBuilder.UpdateData(
                table: "KycSubmissions",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb2"),
                columns: new[] { "CreatedAt", "ReviewedAt", "SubmittedAt" },
                values: new object[] { new DateTime(2025, 10, 13, 19, 27, 27, 571, DateTimeKind.Utc).AddTicks(2623), new DateTime(2025, 10, 14, 19, 27, 27, 571, DateTimeKind.Utc).AddTicks(2613), new DateTime(2025, 10, 13, 19, 27, 27, 571, DateTimeKind.Utc).AddTicks(2613) });

            migrationBuilder.UpdateData(
                table: "KycSubmissions",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb3"),
                columns: new[] { "CreatedAt", "ReviewedAt", "SubmittedAt" },
                values: new object[] { new DateTime(2025, 10, 3, 19, 27, 27, 571, DateTimeKind.Utc).AddTicks(2629), new DateTime(2025, 10, 4, 19, 27, 27, 571, DateTimeKind.Utc).AddTicks(2627), new DateTime(2025, 10, 3, 19, 27, 27, 571, DateTimeKind.Utc).AddTicks(2626) });

            migrationBuilder.UpdateData(
                table: "OrderItems",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa40"),
                columns: new[] { "CreatedAt", "Description", "HeightM", "ItemName", "LengthM", "SizeCode", "WeightKg", "WidthM" },
                values: new object[] { new DateTime(2025, 10, 19, 2, 27, 27, 571, DateTimeKind.Local).AddTicks(3615), null, null, "Moving Service", null, null, null, null });

            migrationBuilder.UpdateData(
                table: "OrderItems",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa41"),
                columns: new[] { "CreatedAt", "Description", "HeightM", "ItemName", "LengthM", "SizeCode", "WeightKg", "WidthM" },
                values: new object[] { new DateTime(2025, 10, 19, 2, 27, 27, 571, DateTimeKind.Local).AddTicks(3660), null, null, "Moving Service", null, null, null, null });

            migrationBuilder.UpdateData(
                table: "OrderItems",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa42"),
                columns: new[] { "CreatedAt", "Description", "HeightM", "ItemName", "LengthM", "SizeCode", "WeightKg", "WidthM" },
                values: new object[] { new DateTime(2025, 10, 19, 2, 27, 27, 571, DateTimeKind.Local).AddTicks(3669), null, null, "Moving Service", null, null, null, null });

            migrationBuilder.UpdateData(
                table: "OrderItems",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa43"),
                columns: new[] { "CreatedAt", "Description", "HeightM", "ItemName", "LengthM", "SizeCode", "WeightKg", "WidthM" },
                values: new object[] { new DateTime(2025, 10, 19, 2, 27, 27, 571, DateTimeKind.Local).AddTicks(3691), null, null, "Moving Service", null, null, null, null });

            migrationBuilder.UpdateData(
                table: "PriceRules",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa14"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 19, 2, 27, 27, 571, DateTimeKind.Local).AddTicks(3488));

            migrationBuilder.UpdateData(
                table: "Stores",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa1"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 18, 19, 27, 27, 571, DateTimeKind.Utc).AddTicks(1521), new DateTime(2025, 10, 18, 19, 27, 27, 571, DateTimeKind.Utc).AddTicks(1522) });

            migrationBuilder.UpdateData(
                table: "Stores",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa2"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 18, 19, 27, 27, 571, DateTimeKind.Utc).AddTicks(1527), new DateTime(2025, 10, 18, 19, 27, 27, 571, DateTimeKind.Utc).AddTicks(1527) });

            migrationBuilder.UpdateData(
                table: "Stores",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa3"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 18, 19, 27, 27, 571, DateTimeKind.Utc).AddTicks(1534), new DateTime(2025, 10, 18, 19, 27, 27, 571, DateTimeKind.Utc).AddTicks(1540) });

            migrationBuilder.UpdateData(
                table: "WarehouseSlots",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa61"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 19, 2, 27, 27, 571, DateTimeKind.Local).AddTicks(3919));

            migrationBuilder.UpdateData(
                table: "WarehouseSlots",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa62"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 19, 2, 27, 27, 571, DateTimeKind.Local).AddTicks(3922));

            migrationBuilder.UpdateData(
                table: "WarehouseSlots",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa63"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 19, 2, 27, 27, 571, DateTimeKind.Local).AddTicks(3925));

            migrationBuilder.UpdateData(
                table: "Warehouses",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa60"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 19, 2, 27, 27, 571, DateTimeKind.Local).AddTicks(3844));

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_Orders_OrderId",
                table: "OrderItems",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_Services_ServiceId",
                table: "OrderItems",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_Orders_OrderId",
                table: "OrderItems");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_Services_ServiceId",
                table: "OrderItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderItems",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "HeightM",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "ItemName",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "LengthM",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "SizeCode",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "WeightKg",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "WidthM",
                table: "OrderItems");

            migrationBuilder.AlterColumn<decimal>(
                name: "UnitPrice",
                table: "OrderItems",
                type: "decimal(12,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldPrecision: 18,
                oldScale: 2,
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Subtotal",
                table: "OrderItems",
                type: "decimal(12,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldPrecision: 18,
                oldScale: 2,
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "ServiceId",
                table: "OrderItems",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "OrderItems",
                type: "uniqueidentifier",
                nullable: false,
                defaultValueSql: "(newid())",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddPrimaryKey(
                name: "PK__OrderIte__57ED0681941C3C3C",
                table: "OrderItems",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "Addresses",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa20"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 16, 23, 54, 13, 367, DateTimeKind.Local).AddTicks(991));

            migrationBuilder.UpdateData(
                table: "Addresses",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa21"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 16, 23, 54, 13, 367, DateTimeKind.Local).AddTicks(1003));

            migrationBuilder.UpdateData(
                table: "Addresses",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa22"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 16, 23, 54, 13, 367, DateTimeKind.Local).AddTicks(1006));

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222221"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEDPjaeMuPBzpsz2ocNESzanug8DZydJa1zTwn1TcZ6NgSTyZ2H24Hm0y+zgEJDWLRw==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEDPjaeMuPBzpsz2ocNESzanug8DZydJa1zTwn1TcZ6NgSTyZ2H24Hm0y+zgEJDWLRw==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222223"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEDPjaeMuPBzpsz2ocNESzanug8DZydJa1zTwn1TcZ6NgSTyZ2H24Hm0y+zgEJDWLRw==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000001"),
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "UpdatedAt" },
                values: new object[] { "4684dfd7-987a-44ee-b694-b1624dc3326c", new DateTime(2025, 10, 16, 16, 54, 13, 276, DateTimeKind.Utc).AddTicks(5553), "AQAAAAIAAYagAAAAELKDcX9I1IZTzkjg0kl1bVDNm2uD48D+FVON1TJijaRH3Bvqk9i6IbDG72CkxCk3jw==", new DateTime(2025, 10, 16, 16, 54, 13, 276, DateTimeKind.Utc).AddTicks(5559) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000002"),
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "UpdatedAt" },
                values: new object[] { "3dd99e70-8fdd-4da8-911a-8b17606c2bfa", new DateTime(2025, 10, 16, 16, 54, 13, 321, DateTimeKind.Utc).AddTicks(8202), "AQAAAAIAAYagAAAAEGYuq8KZDETVtInDItr3LiRgYkmwq/VDmzNmFVSKFORZ56Y4898WcA/zK/l2GDoSmw==", new DateTime(2025, 10, 16, 16, 54, 13, 321, DateTimeKind.Utc).AddTicks(8206) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa01"),
                column: "ConcurrencyStamp",
                value: "bad03977-a522-44bf-acf2-bd2392ab51a3");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa02"),
                column: "ConcurrencyStamp",
                value: "76f753e1-7c0f-43b0-9f0c-c5c081c5e131");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 16, 23, 54, 13, 276, DateTimeKind.Local).AddTicks(5958));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 16, 23, 54, 13, 276, DateTimeKind.Local).AddTicks(5979));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 16, 23, 54, 13, 276, DateTimeKind.Local).AddTicks(5980));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000004"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 16, 23, 54, 13, 276, DateTimeKind.Local).AddTicks(5982));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa2-0000-0000-0000-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 16, 23, 54, 13, 276, DateTimeKind.Local).AddTicks(5975));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa2-0000-0000-0000-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 16, 23, 54, 13, 276, DateTimeKind.Local).AddTicks(5984));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa2-0000-0000-0000-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 16, 23, 54, 13, 276, DateTimeKind.Local).AddTicks(5985));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa3-0000-0000-0000-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 16, 23, 54, 13, 276, DateTimeKind.Local).AddTicks(5977));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa3-0000-0000-0000-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 16, 23, 54, 13, 276, DateTimeKind.Local).AddTicks(5987));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa3-0000-0000-0000-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 16, 23, 54, 13, 276, DateTimeKind.Local).AddTicks(5988));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa10"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 16, 23, 54, 13, 367, DateTimeKind.Local).AddTicks(1084));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa11"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 16, 23, 54, 13, 367, DateTimeKind.Local).AddTicks(1087));

            migrationBuilder.UpdateData(
                table: "KycDocuments",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc1"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 10, 16, 54, 13, 367, DateTimeKind.Utc).AddTicks(28));

            migrationBuilder.UpdateData(
                table: "KycDocuments",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc2"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 10, 16, 54, 13, 367, DateTimeKind.Utc).AddTicks(35));

            migrationBuilder.UpdateData(
                table: "KycDocuments",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc3"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 10, 16, 54, 13, 367, DateTimeKind.Utc).AddTicks(38));

            migrationBuilder.UpdateData(
                table: "KycDocuments",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc4"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 11, 16, 54, 13, 367, DateTimeKind.Utc).AddTicks(40));

            migrationBuilder.UpdateData(
                table: "KycDocuments",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc5"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 11, 16, 54, 13, 367, DateTimeKind.Utc).AddTicks(43));

            migrationBuilder.UpdateData(
                table: "KycDocuments",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc6"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 1, 16, 54, 13, 367, DateTimeKind.Utc).AddTicks(45));

            migrationBuilder.UpdateData(
                table: "KycDocuments",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc7"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 1, 16, 54, 13, 367, DateTimeKind.Utc).AddTicks(47));

            migrationBuilder.UpdateData(
                table: "KycDocuments",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc8"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 1, 16, 54, 13, 367, DateTimeKind.Utc).AddTicks(49));

            migrationBuilder.UpdateData(
                table: "KycSubmissions",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb1"),
                columns: new[] { "CreatedAt", "SubmittedAt" },
                values: new object[] { new DateTime(2025, 10, 10, 16, 54, 13, 366, DateTimeKind.Utc).AddTicks(8392), new DateTime(2025, 10, 10, 16, 54, 13, 366, DateTimeKind.Utc).AddTicks(8391) });

            migrationBuilder.UpdateData(
                table: "KycSubmissions",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb2"),
                columns: new[] { "CreatedAt", "ReviewedAt", "SubmittedAt" },
                values: new object[] { new DateTime(2025, 10, 11, 16, 54, 13, 366, DateTimeKind.Utc).AddTicks(8399), new DateTime(2025, 10, 12, 16, 54, 13, 366, DateTimeKind.Utc).AddTicks(8395), new DateTime(2025, 10, 11, 16, 54, 13, 366, DateTimeKind.Utc).AddTicks(8395) });

            migrationBuilder.UpdateData(
                table: "KycSubmissions",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb3"),
                columns: new[] { "CreatedAt", "ReviewedAt", "SubmittedAt" },
                values: new object[] { new DateTime(2025, 10, 1, 16, 54, 13, 366, DateTimeKind.Utc).AddTicks(8414), new DateTime(2025, 10, 2, 16, 54, 13, 366, DateTimeKind.Utc).AddTicks(8413), new DateTime(2025, 10, 1, 16, 54, 13, 366, DateTimeKind.Utc).AddTicks(8412) });

            migrationBuilder.UpdateData(
                table: "OrderItems",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa40"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 16, 23, 54, 13, 367, DateTimeKind.Local).AddTicks(1508));

            migrationBuilder.UpdateData(
                table: "OrderItems",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa41"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 16, 23, 54, 13, 367, DateTimeKind.Local).AddTicks(1514));

            migrationBuilder.UpdateData(
                table: "OrderItems",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa42"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 16, 23, 54, 13, 367, DateTimeKind.Local).AddTicks(1521));

            migrationBuilder.UpdateData(
                table: "OrderItems",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa43"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 16, 23, 54, 13, 367, DateTimeKind.Local).AddTicks(1547));

            migrationBuilder.UpdateData(
                table: "PriceRules",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa14"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 16, 23, 54, 13, 367, DateTimeKind.Local).AddTicks(1289));

            migrationBuilder.UpdateData(
                table: "Stores",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa1"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 16, 16, 54, 13, 366, DateTimeKind.Utc).AddTicks(8232), new DateTime(2025, 10, 16, 16, 54, 13, 366, DateTimeKind.Utc).AddTicks(8232) });

            migrationBuilder.UpdateData(
                table: "Stores",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa2"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 16, 16, 54, 13, 366, DateTimeKind.Utc).AddTicks(8253), new DateTime(2025, 10, 16, 16, 54, 13, 366, DateTimeKind.Utc).AddTicks(8253) });

            migrationBuilder.UpdateData(
                table: "Stores",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa3"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 16, 16, 54, 13, 366, DateTimeKind.Utc).AddTicks(8260), new DateTime(2025, 10, 16, 16, 54, 13, 366, DateTimeKind.Utc).AddTicks(8273) });

            migrationBuilder.UpdateData(
                table: "WarehouseSlots",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa61"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 16, 23, 54, 13, 367, DateTimeKind.Local).AddTicks(1786));

            migrationBuilder.UpdateData(
                table: "WarehouseSlots",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa62"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 16, 23, 54, 13, 367, DateTimeKind.Local).AddTicks(1793));

            migrationBuilder.UpdateData(
                table: "WarehouseSlots",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa63"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 16, 23, 54, 13, 367, DateTimeKind.Local).AddTicks(1795));

            migrationBuilder.UpdateData(
                table: "Warehouses",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa60"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 16, 23, 54, 13, 367, DateTimeKind.Local).AddTicks(1740));

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_Orders",
                table: "OrderItems",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_Services",
                table: "OrderItems",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "Id");
        }
    }
}
