using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ServiceAddon_Services_ServiceId",
                table: "ServiceAddon");

            migrationBuilder.DropForeignKey(
                name: "FK_ServicePrices_Services_ServiceId",
                table: "ServicePrices");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceSizeOption_Services_ServiceId",
                table: "ServiceSizeOption");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ServiceSizeOption",
                table: "ServiceSizeOption");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ServicePrices",
                table: "ServicePrices");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ServiceAddon",
                table: "ServiceAddon");

            migrationBuilder.RenameTable(
                name: "ServiceSizeOption",
                newName: "ServiceSizeOptions");

            migrationBuilder.RenameTable(
                name: "ServicePrices",
                newName: "PriceRules");

            migrationBuilder.RenameTable(
                name: "ServiceAddon",
                newName: "ServiceAddons");

            migrationBuilder.RenameIndex(
                name: "IX_ServiceSizeOption_ServiceId_Code",
                table: "ServiceSizeOptions",
                newName: "IX_ServiceSizeOptions_ServiceId_Code");

            migrationBuilder.RenameIndex(
                name: "IX_ServicePrices_ServiceId_ValidFrom_ValidTo",
                table: "PriceRules",
                newName: "IX_PriceRules_ServiceId_ValidFrom_ValidTo");

            migrationBuilder.RenameIndex(
                name: "IX_ServicePrices_ServiceId_MinVolumeM3_MaxVolumeM3_MinDays_MaxDays",
                table: "PriceRules",
                newName: "IX_PriceRules_ServiceId_MinVolumeM3_MaxVolumeM3_MinDays_MaxDays");

            migrationBuilder.RenameIndex(
                name: "IX_ServiceAddon_ServiceId_Name",
                table: "ServiceAddons",
                newName: "IX_ServiceAddons_ServiceId_Name");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ServiceSizeOptions",
                table: "ServiceSizeOptions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PriceRules",
                table: "PriceRules",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ServiceAddons",
                table: "ServiceAddons",
                column: "Id");

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

            migrationBuilder.AddForeignKey(
                name: "FK_PriceRules_Services_ServiceId",
                table: "PriceRules",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceAddons_Services_ServiceId",
                table: "ServiceAddons",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceSizeOptions_Services_ServiceId",
                table: "ServiceSizeOptions",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PriceRules_Services_ServiceId",
                table: "PriceRules");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceAddons_Services_ServiceId",
                table: "ServiceAddons");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceSizeOptions_Services_ServiceId",
                table: "ServiceSizeOptions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ServiceSizeOptions",
                table: "ServiceSizeOptions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ServiceAddons",
                table: "ServiceAddons");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PriceRules",
                table: "PriceRules");

            migrationBuilder.RenameTable(
                name: "ServiceSizeOptions",
                newName: "ServiceSizeOption");

            migrationBuilder.RenameTable(
                name: "ServiceAddons",
                newName: "ServiceAddon");

            migrationBuilder.RenameTable(
                name: "PriceRules",
                newName: "ServicePrices");

            migrationBuilder.RenameIndex(
                name: "IX_ServiceSizeOptions_ServiceId_Code",
                table: "ServiceSizeOption",
                newName: "IX_ServiceSizeOption_ServiceId_Code");

            migrationBuilder.RenameIndex(
                name: "IX_ServiceAddons_ServiceId_Name",
                table: "ServiceAddon",
                newName: "IX_ServiceAddon_ServiceId_Name");

            migrationBuilder.RenameIndex(
                name: "IX_PriceRules_ServiceId_ValidFrom_ValidTo",
                table: "ServicePrices",
                newName: "IX_ServicePrices_ServiceId_ValidFrom_ValidTo");

            migrationBuilder.RenameIndex(
                name: "IX_PriceRules_ServiceId_MinVolumeM3_MaxVolumeM3_MinDays_MaxDays",
                table: "ServicePrices",
                newName: "IX_ServicePrices_ServiceId_MinVolumeM3_MaxVolumeM3_MinDays_MaxDays");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ServiceSizeOption",
                table: "ServiceSizeOption",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ServiceAddon",
                table: "ServiceAddon",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ServicePrices",
                table: "ServicePrices",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "Addresses",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa20"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 14, 16, 14, 23, 454, DateTimeKind.Local).AddTicks(8489));

            migrationBuilder.UpdateData(
                table: "Addresses",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa21"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 14, 16, 14, 23, 454, DateTimeKind.Local).AddTicks(8499));

            migrationBuilder.UpdateData(
                table: "Addresses",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa22"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 14, 16, 14, 23, 454, DateTimeKind.Local).AddTicks(8503));

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222221"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEHKMQwEucanBL3693YknbnMvee6W39zbtsBE+Q8DxTid7K13PlYPQneuMWRp5U2nKQ==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEHKMQwEucanBL3693YknbnMvee6W39zbtsBE+Q8DxTid7K13PlYPQneuMWRp5U2nKQ==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222223"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEHKMQwEucanBL3693YknbnMvee6W39zbtsBE+Q8DxTid7K13PlYPQneuMWRp5U2nKQ==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000001"),
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "UpdatedAt" },
                values: new object[] { "fc4a8f42-dca6-442e-85a5-10ca41b5da43", new DateTime(2025, 10, 14, 9, 14, 23, 253, DateTimeKind.Utc).AddTicks(73), "AQAAAAIAAYagAAAAEEzD0dCcX7PxABwl6gqPSD3iy/5Kj+v+2hG8Uh1JbypH3YWRJ1lEumx+yh+W3VNX8A==", new DateTime(2025, 10, 14, 9, 14, 23, 253, DateTimeKind.Utc).AddTicks(77) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000002"),
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "UpdatedAt" },
                values: new object[] { "fe115472-067e-4419-b60c-2918d1c3092f", new DateTime(2025, 10, 14, 9, 14, 23, 350, DateTimeKind.Utc).AddTicks(1606), "AQAAAAIAAYagAAAAEG1rrr1j/MtX3+Ofu8qwgHh8TUKtciWU4hwAyaVJEz0WqkIsOTmAKfFSEwImzUB8nw==", new DateTime(2025, 10, 14, 9, 14, 23, 350, DateTimeKind.Utc).AddTicks(1610) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa01"),
                column: "ConcurrencyStamp",
                value: "59fc6ab3-d195-4c65-aafd-002ae379f451");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa02"),
                column: "ConcurrencyStamp",
                value: "61d2b551-28a3-40be-9fab-14bbe42a1108");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 14, 16, 14, 23, 253, DateTimeKind.Local).AddTicks(836));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 14, 16, 14, 23, 253, DateTimeKind.Local).AddTicks(864));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 14, 16, 14, 23, 253, DateTimeKind.Local).AddTicks(867));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000004"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 14, 16, 14, 23, 253, DateTimeKind.Local).AddTicks(869));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa2-0000-0000-0000-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 14, 16, 14, 23, 253, DateTimeKind.Local).AddTicks(859));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa2-0000-0000-0000-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 14, 16, 14, 23, 253, DateTimeKind.Local).AddTicks(872));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa2-0000-0000-0000-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 14, 16, 14, 23, 253, DateTimeKind.Local).AddTicks(874));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa3-0000-0000-0000-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 14, 16, 14, 23, 253, DateTimeKind.Local).AddTicks(861));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa3-0000-0000-0000-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 14, 16, 14, 23, 253, DateTimeKind.Local).AddTicks(877));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa3-0000-0000-0000-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 14, 16, 14, 23, 253, DateTimeKind.Local).AddTicks(880));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa10"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 14, 16, 14, 23, 454, DateTimeKind.Local).AddTicks(8540));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa11"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 14, 16, 14, 23, 454, DateTimeKind.Local).AddTicks(8546));

            migrationBuilder.UpdateData(
                table: "KycDocuments",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc1"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 8, 9, 14, 23, 454, DateTimeKind.Utc).AddTicks(8261));

            migrationBuilder.UpdateData(
                table: "KycDocuments",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc2"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 8, 9, 14, 23, 454, DateTimeKind.Utc).AddTicks(8270));

            migrationBuilder.UpdateData(
                table: "KycDocuments",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc3"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 8, 9, 14, 23, 454, DateTimeKind.Utc).AddTicks(8278));

            migrationBuilder.UpdateData(
                table: "KycDocuments",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc4"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 9, 9, 14, 23, 454, DateTimeKind.Utc).AddTicks(8281));

            migrationBuilder.UpdateData(
                table: "KycDocuments",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc5"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 9, 9, 14, 23, 454, DateTimeKind.Utc).AddTicks(8287));

            migrationBuilder.UpdateData(
                table: "KycDocuments",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc6"),
                column: "CreatedAt",
                value: new DateTime(2025, 9, 29, 9, 14, 23, 454, DateTimeKind.Utc).AddTicks(8290));

            migrationBuilder.UpdateData(
                table: "KycDocuments",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc7"),
                column: "CreatedAt",
                value: new DateTime(2025, 9, 29, 9, 14, 23, 454, DateTimeKind.Utc).AddTicks(8293));

            migrationBuilder.UpdateData(
                table: "KycDocuments",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc8"),
                column: "CreatedAt",
                value: new DateTime(2025, 9, 29, 9, 14, 23, 454, DateTimeKind.Utc).AddTicks(8296));

            migrationBuilder.UpdateData(
                table: "KycSubmissions",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb1"),
                columns: new[] { "CreatedAt", "SubmittedAt" },
                values: new object[] { new DateTime(2025, 10, 8, 9, 14, 23, 454, DateTimeKind.Utc).AddTicks(8138), new DateTime(2025, 10, 8, 9, 14, 23, 454, DateTimeKind.Utc).AddTicks(8136) });

            migrationBuilder.UpdateData(
                table: "KycSubmissions",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb2"),
                columns: new[] { "CreatedAt", "ReviewedAt", "SubmittedAt" },
                values: new object[] { new DateTime(2025, 10, 9, 9, 14, 23, 454, DateTimeKind.Utc).AddTicks(8147), new DateTime(2025, 10, 10, 9, 14, 23, 454, DateTimeKind.Utc).AddTicks(8144), new DateTime(2025, 10, 9, 9, 14, 23, 454, DateTimeKind.Utc).AddTicks(8142) });

            migrationBuilder.UpdateData(
                table: "KycSubmissions",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb3"),
                columns: new[] { "CreatedAt", "ReviewedAt", "SubmittedAt" },
                values: new object[] { new DateTime(2025, 9, 29, 9, 14, 23, 454, DateTimeKind.Utc).AddTicks(8153), new DateTime(2025, 9, 30, 9, 14, 23, 454, DateTimeKind.Utc).AddTicks(8151), new DateTime(2025, 9, 29, 9, 14, 23, 454, DateTimeKind.Utc).AddTicks(8150) });

            migrationBuilder.UpdateData(
                table: "OrderItems",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa40"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 14, 16, 14, 23, 454, DateTimeKind.Local).AddTicks(8837));

            migrationBuilder.UpdateData(
                table: "OrderItems",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa41"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 14, 16, 14, 23, 454, DateTimeKind.Local).AddTicks(8842));

            migrationBuilder.UpdateData(
                table: "OrderItems",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa42"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 14, 16, 14, 23, 454, DateTimeKind.Local).AddTicks(8845));

            migrationBuilder.UpdateData(
                table: "OrderItems",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa43"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 14, 16, 14, 23, 454, DateTimeKind.Local).AddTicks(8847));

            migrationBuilder.UpdateData(
                table: "ServicePrices",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa14"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 14, 16, 14, 23, 454, DateTimeKind.Local).AddTicks(8688));

            migrationBuilder.UpdateData(
                table: "Stores",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa1"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 14, 9, 14, 23, 454, DateTimeKind.Utc).AddTicks(8031), new DateTime(2025, 10, 14, 9, 14, 23, 454, DateTimeKind.Utc).AddTicks(8031) });

            migrationBuilder.UpdateData(
                table: "Stores",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa2"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 14, 9, 14, 23, 454, DateTimeKind.Utc).AddTicks(8043), new DateTime(2025, 10, 14, 9, 14, 23, 454, DateTimeKind.Utc).AddTicks(8044) });

            migrationBuilder.UpdateData(
                table: "Stores",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa3"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 14, 9, 14, 23, 454, DateTimeKind.Utc).AddTicks(8050), new DateTime(2025, 10, 14, 9, 14, 23, 454, DateTimeKind.Utc).AddTicks(8055) });

            migrationBuilder.UpdateData(
                table: "WarehouseSlots",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa61"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 14, 16, 14, 23, 454, DateTimeKind.Local).AddTicks(8971));

            migrationBuilder.UpdateData(
                table: "WarehouseSlots",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa62"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 14, 16, 14, 23, 454, DateTimeKind.Local).AddTicks(8975));

            migrationBuilder.UpdateData(
                table: "WarehouseSlots",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa63"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 14, 16, 14, 23, 454, DateTimeKind.Local).AddTicks(8977));

            migrationBuilder.UpdateData(
                table: "Warehouses",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa60"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 14, 16, 14, 23, 454, DateTimeKind.Local).AddTicks(8934));

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceAddon_Services_ServiceId",
                table: "ServiceAddon",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ServicePrices_Services_ServiceId",
                table: "ServicePrices",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceSizeOption_Services_ServiceId",
                table: "ServiceSizeOption",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
