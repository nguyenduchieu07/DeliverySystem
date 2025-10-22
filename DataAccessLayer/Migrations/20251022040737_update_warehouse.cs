using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class update_warehouse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "HeightM",
                table: "Warehouses",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "LengthM",
                table: "Warehouses",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "WidthM",
                table: "Warehouses",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.UpdateData(
                table: "Addresses",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa20"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 22, 11, 7, 35, 348, DateTimeKind.Local).AddTicks(8313));

            migrationBuilder.UpdateData(
                table: "Addresses",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa21"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 22, 11, 7, 35, 348, DateTimeKind.Local).AddTicks(8321));

            migrationBuilder.UpdateData(
                table: "Addresses",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa22"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 22, 11, 7, 35, 348, DateTimeKind.Local).AddTicks(8324));

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222221"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAELgVs9gHSgUtNBPx17JWh0mVXBLKd1xMdLvvuzW0xGRN9Aoe5i082zCrCwB9qaH1JQ==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAELgVs9gHSgUtNBPx17JWh0mVXBLKd1xMdLvvuzW0xGRN9Aoe5i082zCrCwB9qaH1JQ==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222223"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAELgVs9gHSgUtNBPx17JWh0mVXBLKd1xMdLvvuzW0xGRN9Aoe5i082zCrCwB9qaH1JQ==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000001"),
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "UpdatedAt" },
                values: new object[] { "1ce12557-24e5-4d7f-ac18-554962ee6770", new DateTime(2025, 10, 22, 4, 7, 35, 191, DateTimeKind.Utc).AddTicks(4356), "AQAAAAIAAYagAAAAEF9FnLhKeB7kwmgm6GWttptSABjTcY2wv2lDdBGe8MPPPDP3PphHjGS2fWpD93Nu3Q==", new DateTime(2025, 10, 22, 4, 7, 35, 191, DateTimeKind.Utc).AddTicks(4362) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000002"),
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "UpdatedAt" },
                values: new object[] { "1ead1d32-2520-42ac-925c-011d6459feb2", new DateTime(2025, 10, 22, 4, 7, 35, 268, DateTimeKind.Utc).AddTicks(7407), "AQAAAAIAAYagAAAAEGC+prIck85bTwzcm2ojcgCj17S9wJC1A1tTxO57PoehMsonzDXky9JLd3i2/GhQnQ==", new DateTime(2025, 10, 22, 4, 7, 35, 268, DateTimeKind.Utc).AddTicks(7415) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa01"),
                column: "ConcurrencyStamp",
                value: "222ed2f0-88de-429b-a410-01ed310c4355");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa02"),
                column: "ConcurrencyStamp",
                value: "25b7e0e7-465a-4644-9591-110c8839029d");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 22, 11, 7, 35, 191, DateTimeKind.Local).AddTicks(5200));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 22, 11, 7, 35, 191, DateTimeKind.Local).AddTicks(5211));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 22, 11, 7, 35, 191, DateTimeKind.Local).AddTicks(5215));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000004"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 22, 11, 7, 35, 191, DateTimeKind.Local).AddTicks(5217));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa2-0000-0000-0000-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 22, 11, 7, 35, 191, DateTimeKind.Local).AddTicks(5207));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa2-0000-0000-0000-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 22, 11, 7, 35, 191, DateTimeKind.Local).AddTicks(5219));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa2-0000-0000-0000-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 22, 11, 7, 35, 191, DateTimeKind.Local).AddTicks(5221));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa3-0000-0000-0000-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 22, 11, 7, 35, 191, DateTimeKind.Local).AddTicks(5209));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa3-0000-0000-0000-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 22, 11, 7, 35, 191, DateTimeKind.Local).AddTicks(5222));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa3-0000-0000-0000-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 22, 11, 7, 35, 191, DateTimeKind.Local).AddTicks(5224));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa10"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 22, 11, 7, 35, 348, DateTimeKind.Local).AddTicks(8406));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa11"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 22, 11, 7, 35, 348, DateTimeKind.Local).AddTicks(8622));

            migrationBuilder.UpdateData(
                table: "KycDocuments",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc1"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 16, 4, 7, 35, 348, DateTimeKind.Utc).AddTicks(8095));

            migrationBuilder.UpdateData(
                table: "KycDocuments",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc2"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 16, 4, 7, 35, 348, DateTimeKind.Utc).AddTicks(8099));

            migrationBuilder.UpdateData(
                table: "KycDocuments",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc3"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 16, 4, 7, 35, 348, DateTimeKind.Utc).AddTicks(8101));

            migrationBuilder.UpdateData(
                table: "KycDocuments",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc4"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 17, 4, 7, 35, 348, DateTimeKind.Utc).AddTicks(8103));

            migrationBuilder.UpdateData(
                table: "KycDocuments",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc5"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 17, 4, 7, 35, 348, DateTimeKind.Utc).AddTicks(8106));

            migrationBuilder.UpdateData(
                table: "KycDocuments",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc6"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 7, 4, 7, 35, 348, DateTimeKind.Utc).AddTicks(8108));

            migrationBuilder.UpdateData(
                table: "KycDocuments",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc7"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 7, 4, 7, 35, 348, DateTimeKind.Utc).AddTicks(8110));

            migrationBuilder.UpdateData(
                table: "KycDocuments",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc8"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 7, 4, 7, 35, 348, DateTimeKind.Utc).AddTicks(8112));

            migrationBuilder.UpdateData(
                table: "KycSubmissions",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb1"),
                columns: new[] { "CreatedAt", "SubmittedAt" },
                values: new object[] { new DateTime(2025, 10, 16, 4, 7, 35, 348, DateTimeKind.Utc).AddTicks(8033), new DateTime(2025, 10, 16, 4, 7, 35, 348, DateTimeKind.Utc).AddTicks(8029) });

            migrationBuilder.UpdateData(
                table: "KycSubmissions",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb2"),
                columns: new[] { "CreatedAt", "ReviewedAt", "SubmittedAt" },
                values: new object[] { new DateTime(2025, 10, 17, 4, 7, 35, 348, DateTimeKind.Utc).AddTicks(8042), new DateTime(2025, 10, 18, 4, 7, 35, 348, DateTimeKind.Utc).AddTicks(8037), new DateTime(2025, 10, 17, 4, 7, 35, 348, DateTimeKind.Utc).AddTicks(8036) });

            migrationBuilder.UpdateData(
                table: "KycSubmissions",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb3"),
                columns: new[] { "CreatedAt", "ReviewedAt", "SubmittedAt" },
                values: new object[] { new DateTime(2025, 10, 7, 4, 7, 35, 348, DateTimeKind.Utc).AddTicks(8052), new DateTime(2025, 10, 8, 4, 7, 35, 348, DateTimeKind.Utc).AddTicks(8051), new DateTime(2025, 10, 7, 4, 7, 35, 348, DateTimeKind.Utc).AddTicks(8050) });

            migrationBuilder.UpdateData(
                table: "OrderItems",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa40"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 22, 11, 7, 35, 348, DateTimeKind.Local).AddTicks(8925));

            migrationBuilder.UpdateData(
                table: "OrderItems",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa41"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 22, 11, 7, 35, 348, DateTimeKind.Local).AddTicks(8930));

            migrationBuilder.UpdateData(
                table: "OrderItems",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa42"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 22, 11, 7, 35, 348, DateTimeKind.Local).AddTicks(8932));

            migrationBuilder.UpdateData(
                table: "OrderItems",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa43"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 22, 11, 7, 35, 348, DateTimeKind.Local).AddTicks(8933));

            migrationBuilder.UpdateData(
                table: "PriceRules",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa14"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 22, 11, 7, 35, 348, DateTimeKind.Local).AddTicks(8786));

            migrationBuilder.UpdateData(
                table: "Stores",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa1"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 22, 4, 7, 35, 348, DateTimeKind.Utc).AddTicks(7881), new DateTime(2025, 10, 22, 4, 7, 35, 348, DateTimeKind.Utc).AddTicks(7882) });

            migrationBuilder.UpdateData(
                table: "Stores",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa2"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 22, 4, 7, 35, 348, DateTimeKind.Utc).AddTicks(7891), new DateTime(2025, 10, 22, 4, 7, 35, 348, DateTimeKind.Utc).AddTicks(7891) });

            migrationBuilder.UpdateData(
                table: "Stores",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa3"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 22, 4, 7, 35, 348, DateTimeKind.Utc).AddTicks(7896), new DateTime(2025, 10, 22, 4, 7, 35, 348, DateTimeKind.Utc).AddTicks(7909) });

            migrationBuilder.UpdateData(
                table: "WarehouseSlots",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa61"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 22, 11, 7, 35, 348, DateTimeKind.Local).AddTicks(9082));

            migrationBuilder.UpdateData(
                table: "WarehouseSlots",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa62"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 22, 11, 7, 35, 348, DateTimeKind.Local).AddTicks(9088));

            migrationBuilder.UpdateData(
                table: "WarehouseSlots",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa63"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 22, 11, 7, 35, 348, DateTimeKind.Local).AddTicks(9089));

            migrationBuilder.UpdateData(
                table: "Warehouses",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa60"),
                columns: new[] { "CreatedAt", "HeightM", "LengthM", "WidthM" },
                values: new object[] { new DateTime(2025, 10, 22, 11, 7, 35, 348, DateTimeKind.Local).AddTicks(9033), 0m, 0m, 0m });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HeightM",
                table: "Warehouses");

            migrationBuilder.DropColumn(
                name: "LengthM",
                table: "Warehouses");

            migrationBuilder.DropColumn(
                name: "WidthM",
                table: "Warehouses");

            migrationBuilder.UpdateData(
                table: "Addresses",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa20"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 22, 9, 39, 41, 910, DateTimeKind.Local).AddTicks(6013));

            migrationBuilder.UpdateData(
                table: "Addresses",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa21"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 22, 9, 39, 41, 910, DateTimeKind.Local).AddTicks(6035));

            migrationBuilder.UpdateData(
                table: "Addresses",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa22"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 22, 9, 39, 41, 910, DateTimeKind.Local).AddTicks(6038));

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222221"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEPWBSItnWDUMpqfCt0YQdjyuQE6ERJ6pSR/7cY8KZ8luAQuJLTY63YbZWGZH22Pw8A==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEPWBSItnWDUMpqfCt0YQdjyuQE6ERJ6pSR/7cY8KZ8luAQuJLTY63YbZWGZH22Pw8A==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222223"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEPWBSItnWDUMpqfCt0YQdjyuQE6ERJ6pSR/7cY8KZ8luAQuJLTY63YbZWGZH22Pw8A==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000001"),
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "UpdatedAt" },
                values: new object[] { "97115bef-4ed6-4363-90ba-ca5aef3baf80", new DateTime(2025, 10, 22, 2, 39, 41, 753, DateTimeKind.Utc).AddTicks(9446), "AQAAAAIAAYagAAAAEDVc6tpKYpV2phCVj8Bataa24lSYkVLaj4qrOcIuVUlk8M35lpprB2dNL11JlV2Q7Q==", new DateTime(2025, 10, 22, 2, 39, 41, 753, DateTimeKind.Utc).AddTicks(9450) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000002"),
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "UpdatedAt" },
                values: new object[] { "da80a6a1-ec1a-4f64-afc2-78e8680583ed", new DateTime(2025, 10, 22, 2, 39, 41, 832, DateTimeKind.Utc).AddTicks(6498), "AQAAAAIAAYagAAAAEFMxE1P0hqlJDNuCmOTV/J3ue6ULFbkT4EHnTqjHS9LteHA3EzZjB53UibHWd1YeUA==", new DateTime(2025, 10, 22, 2, 39, 41, 832, DateTimeKind.Utc).AddTicks(6502) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa01"),
                column: "ConcurrencyStamp",
                value: "dab9f18f-546b-4684-a5eb-ab38e9e1f1ee");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa02"),
                column: "ConcurrencyStamp",
                value: "83fdb1bd-8300-40ef-8b31-fb5397b83529");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 22, 9, 39, 41, 754, DateTimeKind.Local).AddTicks(138));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 22, 9, 39, 41, 754, DateTimeKind.Local).AddTicks(153));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 22, 9, 39, 41, 754, DateTimeKind.Local).AddTicks(156));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000004"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 22, 9, 39, 41, 754, DateTimeKind.Local).AddTicks(158));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa2-0000-0000-0000-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 22, 9, 39, 41, 754, DateTimeKind.Local).AddTicks(148));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa2-0000-0000-0000-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 22, 9, 39, 41, 754, DateTimeKind.Local).AddTicks(160));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa2-0000-0000-0000-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 22, 9, 39, 41, 754, DateTimeKind.Local).AddTicks(161));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa3-0000-0000-0000-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 22, 9, 39, 41, 754, DateTimeKind.Local).AddTicks(151));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa3-0000-0000-0000-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 22, 9, 39, 41, 754, DateTimeKind.Local).AddTicks(163));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa3-0000-0000-0000-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 22, 9, 39, 41, 754, DateTimeKind.Local).AddTicks(165));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa10"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 22, 9, 39, 41, 910, DateTimeKind.Local).AddTicks(6082));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa11"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 22, 9, 39, 41, 910, DateTimeKind.Local).AddTicks(6088));

            migrationBuilder.UpdateData(
                table: "KycDocuments",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc1"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 16, 2, 39, 41, 910, DateTimeKind.Utc).AddTicks(5807));

            migrationBuilder.UpdateData(
                table: "KycDocuments",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc2"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 16, 2, 39, 41, 910, DateTimeKind.Utc).AddTicks(5811));

            migrationBuilder.UpdateData(
                table: "KycDocuments",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc3"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 16, 2, 39, 41, 910, DateTimeKind.Utc).AddTicks(5821));

            migrationBuilder.UpdateData(
                table: "KycDocuments",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc4"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 17, 2, 39, 41, 910, DateTimeKind.Utc).AddTicks(5823));

            migrationBuilder.UpdateData(
                table: "KycDocuments",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc5"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 17, 2, 39, 41, 910, DateTimeKind.Utc).AddTicks(5825));

            migrationBuilder.UpdateData(
                table: "KycDocuments",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc6"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 7, 2, 39, 41, 910, DateTimeKind.Utc).AddTicks(5827));

            migrationBuilder.UpdateData(
                table: "KycDocuments",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc7"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 7, 2, 39, 41, 910, DateTimeKind.Utc).AddTicks(5830));

            migrationBuilder.UpdateData(
                table: "KycDocuments",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc8"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 7, 2, 39, 41, 910, DateTimeKind.Utc).AddTicks(5833));

            migrationBuilder.UpdateData(
                table: "KycSubmissions",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb1"),
                columns: new[] { "CreatedAt", "SubmittedAt" },
                values: new object[] { new DateTime(2025, 10, 16, 2, 39, 41, 910, DateTimeKind.Utc).AddTicks(5707), new DateTime(2025, 10, 16, 2, 39, 41, 910, DateTimeKind.Utc).AddTicks(5705) });

            migrationBuilder.UpdateData(
                table: "KycSubmissions",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb2"),
                columns: new[] { "CreatedAt", "ReviewedAt", "SubmittedAt" },
                values: new object[] { new DateTime(2025, 10, 17, 2, 39, 41, 910, DateTimeKind.Utc).AddTicks(5715), new DateTime(2025, 10, 18, 2, 39, 41, 910, DateTimeKind.Utc).AddTicks(5711), new DateTime(2025, 10, 17, 2, 39, 41, 910, DateTimeKind.Utc).AddTicks(5710) });

            migrationBuilder.UpdateData(
                table: "KycSubmissions",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb3"),
                columns: new[] { "CreatedAt", "ReviewedAt", "SubmittedAt" },
                values: new object[] { new DateTime(2025, 10, 7, 2, 39, 41, 910, DateTimeKind.Utc).AddTicks(5724), new DateTime(2025, 10, 8, 2, 39, 41, 910, DateTimeKind.Utc).AddTicks(5723), new DateTime(2025, 10, 7, 2, 39, 41, 910, DateTimeKind.Utc).AddTicks(5717) });

            migrationBuilder.UpdateData(
                table: "OrderItems",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa40"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 22, 9, 39, 41, 910, DateTimeKind.Local).AddTicks(6383));

            migrationBuilder.UpdateData(
                table: "OrderItems",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa41"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 22, 9, 39, 41, 910, DateTimeKind.Local).AddTicks(6387));

            migrationBuilder.UpdateData(
                table: "OrderItems",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa42"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 22, 9, 39, 41, 910, DateTimeKind.Local).AddTicks(6389));

            migrationBuilder.UpdateData(
                table: "OrderItems",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa43"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 22, 9, 39, 41, 910, DateTimeKind.Local).AddTicks(6390));

            migrationBuilder.UpdateData(
                table: "PriceRules",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa14"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 22, 9, 39, 41, 910, DateTimeKind.Local).AddTicks(6270));

            migrationBuilder.UpdateData(
                table: "Stores",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa1"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 22, 2, 39, 41, 910, DateTimeKind.Utc).AddTicks(5618), new DateTime(2025, 10, 22, 2, 39, 41, 910, DateTimeKind.Utc).AddTicks(5619) });

            migrationBuilder.UpdateData(
                table: "Stores",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa2"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 22, 2, 39, 41, 910, DateTimeKind.Utc).AddTicks(5626), new DateTime(2025, 10, 22, 2, 39, 41, 910, DateTimeKind.Utc).AddTicks(5627) });

            migrationBuilder.UpdateData(
                table: "Stores",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa3"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 22, 2, 39, 41, 910, DateTimeKind.Utc).AddTicks(5631), new DateTime(2025, 10, 22, 2, 39, 41, 910, DateTimeKind.Utc).AddTicks(5636) });

            migrationBuilder.UpdateData(
                table: "WarehouseSlots",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa61"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 22, 9, 39, 41, 910, DateTimeKind.Local).AddTicks(6520));

            migrationBuilder.UpdateData(
                table: "WarehouseSlots",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa62"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 22, 9, 39, 41, 910, DateTimeKind.Local).AddTicks(6524));

            migrationBuilder.UpdateData(
                table: "WarehouseSlots",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa63"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 22, 9, 39, 41, 910, DateTimeKind.Local).AddTicks(6526));

            migrationBuilder.UpdateData(
                table: "Warehouses",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa60"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 22, 9, 39, 41, 910, DateTimeKind.Local).AddTicks(6476));
        }
    }
}
