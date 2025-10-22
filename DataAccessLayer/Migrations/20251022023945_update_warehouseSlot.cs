using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class update_warehouseSlot : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "BasePricePerHour",
                table: "WarehouseSlots",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "Col",
                table: "WarehouseSlots",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "HeightM",
                table: "WarehouseSlots",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<bool>(
                name: "IsBlocked",
                table: "WarehouseSlots",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LeaseEnd",
                table: "WarehouseSlots",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LeaseStart",
                table: "WarehouseSlots",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "LengthM",
                table: "WarehouseSlots",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "Row",
                table: "WarehouseSlots",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "WidthM",
                table: "WarehouseSlots",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

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
                columns: new[] { "BasePricePerHour", "Col", "CreatedAt", "HeightM", "IsBlocked", "LeaseEnd", "LeaseStart", "LengthM", "Row", "WidthM" },
                values: new object[] { 0m, 0, new DateTime(2025, 10, 22, 9, 39, 41, 910, DateTimeKind.Local).AddTicks(6520), 0m, false, null, null, 0m, 0, 0m });

            migrationBuilder.UpdateData(
                table: "WarehouseSlots",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa62"),
                columns: new[] { "BasePricePerHour", "Col", "CreatedAt", "HeightM", "IsBlocked", "LeaseEnd", "LeaseStart", "LengthM", "Row", "WidthM" },
                values: new object[] { 0m, 0, new DateTime(2025, 10, 22, 9, 39, 41, 910, DateTimeKind.Local).AddTicks(6524), 0m, false, null, null, 0m, 0, 0m });

            migrationBuilder.UpdateData(
                table: "WarehouseSlots",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa63"),
                columns: new[] { "BasePricePerHour", "Col", "CreatedAt", "HeightM", "IsBlocked", "LeaseEnd", "LeaseStart", "LengthM", "Row", "WidthM" },
                values: new object[] { 0m, 0, new DateTime(2025, 10, 22, 9, 39, 41, 910, DateTimeKind.Local).AddTicks(6526), 0m, false, null, null, 0m, 0, 0m });

            migrationBuilder.UpdateData(
                table: "Warehouses",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa60"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 22, 9, 39, 41, 910, DateTimeKind.Local).AddTicks(6476));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BasePricePerHour",
                table: "WarehouseSlots");

            migrationBuilder.DropColumn(
                name: "Col",
                table: "WarehouseSlots");

            migrationBuilder.DropColumn(
                name: "HeightM",
                table: "WarehouseSlots");

            migrationBuilder.DropColumn(
                name: "IsBlocked",
                table: "WarehouseSlots");

            migrationBuilder.DropColumn(
                name: "LeaseEnd",
                table: "WarehouseSlots");

            migrationBuilder.DropColumn(
                name: "LeaseStart",
                table: "WarehouseSlots");

            migrationBuilder.DropColumn(
                name: "LengthM",
                table: "WarehouseSlots");

            migrationBuilder.DropColumn(
                name: "Row",
                table: "WarehouseSlots");

            migrationBuilder.DropColumn(
                name: "WidthM",
                table: "WarehouseSlots");

            migrationBuilder.UpdateData(
                table: "Addresses",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa20"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 14, 16, 21, 11, 348, DateTimeKind.Local).AddTicks(8892));

            migrationBuilder.UpdateData(
                table: "Addresses",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa21"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 14, 16, 21, 11, 348, DateTimeKind.Local).AddTicks(8907));

            migrationBuilder.UpdateData(
                table: "Addresses",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa22"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 14, 16, 21, 11, 348, DateTimeKind.Local).AddTicks(8911));

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222221"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEDnOontmGjpZRHUh2sxxYKWWFDfmsKjNHqis6bYcdOjfJlLTeZvJCdC7I0qc0I9HeA==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEDnOontmGjpZRHUh2sxxYKWWFDfmsKjNHqis6bYcdOjfJlLTeZvJCdC7I0qc0I9HeA==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222223"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEDnOontmGjpZRHUh2sxxYKWWFDfmsKjNHqis6bYcdOjfJlLTeZvJCdC7I0qc0I9HeA==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000001"),
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "UpdatedAt" },
                values: new object[] { "8797c4bb-4ac7-4718-a539-9834ba7886c9", new DateTime(2025, 10, 14, 9, 21, 11, 37, DateTimeKind.Utc).AddTicks(117), "AQAAAAIAAYagAAAAEBOUqJDYcdLaBJt3QLvm3/njR92ufOPq2Tkc+IIBB6HZ0grty1vaJtosVVgPPR3V6w==", new DateTime(2025, 10, 14, 9, 21, 11, 37, DateTimeKind.Utc).AddTicks(122) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000002"),
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "UpdatedAt" },
                values: new object[] { "1e500f72-3404-4afd-bed1-dcc6041be2b8", new DateTime(2025, 10, 14, 9, 21, 11, 180, DateTimeKind.Utc).AddTicks(3688), "AQAAAAIAAYagAAAAEIF/4MGj/lW1S6sieqvFUs8aRvcgxY52uWkrUABTIscF+oJWExnzC4/2kcuzjYp1ug==", new DateTime(2025, 10, 14, 9, 21, 11, 180, DateTimeKind.Utc).AddTicks(3694) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa01"),
                column: "ConcurrencyStamp",
                value: "6af9370e-eea5-4c85-9215-5d6c48d49ff3");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa02"),
                column: "ConcurrencyStamp",
                value: "77f21814-ca98-4b1f-88dd-367494ae4a41");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 14, 16, 21, 11, 37, DateTimeKind.Local).AddTicks(1577));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 14, 16, 21, 11, 37, DateTimeKind.Local).AddTicks(1640));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 14, 16, 21, 11, 37, DateTimeKind.Local).AddTicks(1645));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000004"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 14, 16, 21, 11, 37, DateTimeKind.Local).AddTicks(1648));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa2-0000-0000-0000-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 14, 16, 21, 11, 37, DateTimeKind.Local).AddTicks(1592));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa2-0000-0000-0000-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 14, 16, 21, 11, 37, DateTimeKind.Local).AddTicks(1652));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa2-0000-0000-0000-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 14, 16, 21, 11, 37, DateTimeKind.Local).AddTicks(1654));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa3-0000-0000-0000-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 14, 16, 21, 11, 37, DateTimeKind.Local).AddTicks(1596));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa3-0000-0000-0000-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 14, 16, 21, 11, 37, DateTimeKind.Local).AddTicks(1656));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa3-0000-0000-0000-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 14, 16, 21, 11, 37, DateTimeKind.Local).AddTicks(1659));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa10"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 14, 16, 21, 11, 348, DateTimeKind.Local).AddTicks(8985));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa11"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 14, 16, 21, 11, 348, DateTimeKind.Local).AddTicks(8996));

            migrationBuilder.UpdateData(
                table: "KycDocuments",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc1"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 8, 9, 21, 11, 348, DateTimeKind.Utc).AddTicks(8483));

            migrationBuilder.UpdateData(
                table: "KycDocuments",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc2"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 8, 9, 21, 11, 348, DateTimeKind.Utc).AddTicks(8490));

            migrationBuilder.UpdateData(
                table: "KycDocuments",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc3"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 8, 9, 21, 11, 348, DateTimeKind.Utc).AddTicks(8510));

            migrationBuilder.UpdateData(
                table: "KycDocuments",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc4"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 9, 9, 21, 11, 348, DateTimeKind.Utc).AddTicks(8515));

            migrationBuilder.UpdateData(
                table: "KycDocuments",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc5"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 9, 9, 21, 11, 348, DateTimeKind.Utc).AddTicks(8519));

            migrationBuilder.UpdateData(
                table: "KycDocuments",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc6"),
                column: "CreatedAt",
                value: new DateTime(2025, 9, 29, 9, 21, 11, 348, DateTimeKind.Utc).AddTicks(8523));

            migrationBuilder.UpdateData(
                table: "KycDocuments",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc7"),
                column: "CreatedAt",
                value: new DateTime(2025, 9, 29, 9, 21, 11, 348, DateTimeKind.Utc).AddTicks(8527));

            migrationBuilder.UpdateData(
                table: "KycDocuments",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc8"),
                column: "CreatedAt",
                value: new DateTime(2025, 9, 29, 9, 21, 11, 348, DateTimeKind.Utc).AddTicks(8531));

            migrationBuilder.UpdateData(
                table: "KycSubmissions",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb1"),
                columns: new[] { "CreatedAt", "SubmittedAt" },
                values: new object[] { new DateTime(2025, 10, 8, 9, 21, 11, 348, DateTimeKind.Utc).AddTicks(8318), new DateTime(2025, 10, 8, 9, 21, 11, 348, DateTimeKind.Utc).AddTicks(8314) });

            migrationBuilder.UpdateData(
                table: "KycSubmissions",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb2"),
                columns: new[] { "CreatedAt", "ReviewedAt", "SubmittedAt" },
                values: new object[] { new DateTime(2025, 10, 9, 9, 21, 11, 348, DateTimeKind.Utc).AddTicks(8330), new DateTime(2025, 10, 10, 9, 21, 11, 348, DateTimeKind.Utc).AddTicks(8324), new DateTime(2025, 10, 9, 9, 21, 11, 348, DateTimeKind.Utc).AddTicks(8323) });

            migrationBuilder.UpdateData(
                table: "KycSubmissions",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb3"),
                columns: new[] { "CreatedAt", "ReviewedAt", "SubmittedAt" },
                values: new object[] { new DateTime(2025, 9, 29, 9, 21, 11, 348, DateTimeKind.Utc).AddTicks(8345), new DateTime(2025, 9, 30, 9, 21, 11, 348, DateTimeKind.Utc).AddTicks(8342), new DateTime(2025, 9, 29, 9, 21, 11, 348, DateTimeKind.Utc).AddTicks(8334) });

            migrationBuilder.UpdateData(
                table: "OrderItems",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa40"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 14, 16, 21, 11, 348, DateTimeKind.Local).AddTicks(9569));

            migrationBuilder.UpdateData(
                table: "OrderItems",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa41"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 14, 16, 21, 11, 348, DateTimeKind.Local).AddTicks(9574));

            migrationBuilder.UpdateData(
                table: "OrderItems",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa42"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 14, 16, 21, 11, 348, DateTimeKind.Local).AddTicks(9577));

            migrationBuilder.UpdateData(
                table: "OrderItems",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa43"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 14, 16, 21, 11, 348, DateTimeKind.Local).AddTicks(9579));

            migrationBuilder.UpdateData(
                table: "PriceRules",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa14"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 14, 16, 21, 11, 348, DateTimeKind.Local).AddTicks(9362));

            migrationBuilder.UpdateData(
                table: "Stores",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa1"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 14, 9, 21, 11, 348, DateTimeKind.Utc).AddTicks(7967), new DateTime(2025, 10, 14, 9, 21, 11, 348, DateTimeKind.Utc).AddTicks(7968) });

            migrationBuilder.UpdateData(
                table: "Stores",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa2"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 14, 9, 21, 11, 348, DateTimeKind.Utc).AddTicks(8153), new DateTime(2025, 10, 14, 9, 21, 11, 348, DateTimeKind.Utc).AddTicks(8153) });

            migrationBuilder.UpdateData(
                table: "Stores",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa3"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 14, 9, 21, 11, 348, DateTimeKind.Utc).AddTicks(8162), new DateTime(2025, 10, 14, 9, 21, 11, 348, DateTimeKind.Utc).AddTicks(8172) });

            migrationBuilder.UpdateData(
                table: "WarehouseSlots",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa61"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 14, 16, 21, 11, 348, DateTimeKind.Local).AddTicks(9804));

            migrationBuilder.UpdateData(
                table: "WarehouseSlots",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa62"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 14, 16, 21, 11, 348, DateTimeKind.Local).AddTicks(9809));

            migrationBuilder.UpdateData(
                table: "WarehouseSlots",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa63"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 14, 16, 21, 11, 348, DateTimeKind.Local).AddTicks(9812));

            migrationBuilder.UpdateData(
                table: "Warehouses",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa60"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 14, 16, 21, 11, 348, DateTimeKind.Local).AddTicks(9731));
        }
    }
}
