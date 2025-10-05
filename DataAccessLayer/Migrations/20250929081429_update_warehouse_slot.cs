using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class update_warehouse_slot : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SlotReservation_WarehouseSlot_WarehouseSlotId",
                table: "SlotReservation");

            migrationBuilder.DropForeignKey(
                name: "FK_Warehouse_Stores_StoreId",
                table: "Warehouse");

            migrationBuilder.DropForeignKey(
                name: "FK_WarehouseSlot_Warehouse_WarehouseId",
                table: "WarehouseSlot");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WarehouseSlot",
                table: "WarehouseSlot");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Warehouse",
                table: "Warehouse");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SlotReservation",
                table: "SlotReservation");

            migrationBuilder.RenameTable(
                name: "WarehouseSlot",
                newName: "WarehouseSlots");

            migrationBuilder.RenameTable(
                name: "Warehouse",
                newName: "Warehouses");

            migrationBuilder.RenameTable(
                name: "SlotReservation",
                newName: "SlotReservations");

            migrationBuilder.RenameIndex(
                name: "IX_WarehouseSlot_WarehouseId_Status",
                table: "WarehouseSlots",
                newName: "IX_WarehouseSlots_WarehouseId_Status");

            migrationBuilder.RenameIndex(
                name: "IX_WarehouseSlot_WarehouseId_Code",
                table: "WarehouseSlots",
                newName: "IX_WarehouseSlots_WarehouseId_Code");

            migrationBuilder.RenameIndex(
                name: "IX_Warehouse_StoreId_Name",
                table: "Warehouses",
                newName: "IX_Warehouses_StoreId_Name");

            migrationBuilder.RenameIndex(
                name: "IX_SlotReservation_WarehouseSlotId_ExpiresAt",
                table: "SlotReservations",
                newName: "IX_SlotReservations_WarehouseSlotId_ExpiresAt");

            migrationBuilder.RenameIndex(
                name: "IX_SlotReservation_OrderId",
                table: "SlotReservations",
                newName: "IX_SlotReservations_OrderId");

            migrationBuilder.AlterColumn<Guid>(
                name: "AddressRefId",
                table: "Warehouses",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_WarehouseSlots",
                table: "WarehouseSlots",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Warehouses",
                table: "Warehouses",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SlotReservations",
                table: "SlotReservations",
                column: "Id");

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

            migrationBuilder.CreateIndex(
                name: "IX_Warehouses_AddressRefId",
                table: "Warehouses",
                column: "AddressRefId");

            migrationBuilder.AddForeignKey(
                name: "FK_SlotReservations_WarehouseSlots_WarehouseSlotId",
                table: "SlotReservations",
                column: "WarehouseSlotId",
                principalTable: "WarehouseSlots",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Warehouses_Addresses_AddressRefId",
                table: "Warehouses",
                column: "AddressRefId",
                principalTable: "Addresses",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Warehouses_Stores_StoreId",
                table: "Warehouses",
                column: "StoreId",
                principalTable: "Stores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WarehouseSlots_Warehouses_WarehouseId",
                table: "WarehouseSlots",
                column: "WarehouseId",
                principalTable: "Warehouses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SlotReservations_WarehouseSlots_WarehouseSlotId",
                table: "SlotReservations");

            migrationBuilder.DropForeignKey(
                name: "FK_Warehouses_Addresses_AddressRefId",
                table: "Warehouses");

            migrationBuilder.DropForeignKey(
                name: "FK_Warehouses_Stores_StoreId",
                table: "Warehouses");

            migrationBuilder.DropForeignKey(
                name: "FK_WarehouseSlots_Warehouses_WarehouseId",
                table: "WarehouseSlots");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WarehouseSlots",
                table: "WarehouseSlots");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Warehouses",
                table: "Warehouses");

            migrationBuilder.DropIndex(
                name: "IX_Warehouses_AddressRefId",
                table: "Warehouses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SlotReservations",
                table: "SlotReservations");

            migrationBuilder.RenameTable(
                name: "WarehouseSlots",
                newName: "WarehouseSlot");

            migrationBuilder.RenameTable(
                name: "Warehouses",
                newName: "Warehouse");

            migrationBuilder.RenameTable(
                name: "SlotReservations",
                newName: "SlotReservation");

            migrationBuilder.RenameIndex(
                name: "IX_WarehouseSlots_WarehouseId_Status",
                table: "WarehouseSlot",
                newName: "IX_WarehouseSlot_WarehouseId_Status");

            migrationBuilder.RenameIndex(
                name: "IX_WarehouseSlots_WarehouseId_Code",
                table: "WarehouseSlot",
                newName: "IX_WarehouseSlot_WarehouseId_Code");

            migrationBuilder.RenameIndex(
                name: "IX_Warehouses_StoreId_Name",
                table: "Warehouse",
                newName: "IX_Warehouse_StoreId_Name");

            migrationBuilder.RenameIndex(
                name: "IX_SlotReservations_WarehouseSlotId_ExpiresAt",
                table: "SlotReservation",
                newName: "IX_SlotReservation_WarehouseSlotId_ExpiresAt");

            migrationBuilder.RenameIndex(
                name: "IX_SlotReservations_OrderId",
                table: "SlotReservation",
                newName: "IX_SlotReservation_OrderId");

            migrationBuilder.AlterColumn<string>(
                name: "AddressRefId",
                table: "Warehouse",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_WarehouseSlot",
                table: "WarehouseSlot",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Warehouse",
                table: "Warehouse",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SlotReservation",
                table: "SlotReservation",
                column: "Id");

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

            migrationBuilder.AddForeignKey(
                name: "FK_SlotReservation_WarehouseSlot_WarehouseSlotId",
                table: "SlotReservation",
                column: "WarehouseSlotId",
                principalTable: "WarehouseSlot",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Warehouse_Stores_StoreId",
                table: "Warehouse",
                column: "StoreId",
                principalTable: "Stores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WarehouseSlot_Warehouse_WarehouseId",
                table: "WarehouseSlot",
                column: "WarehouseId",
                principalTable: "Warehouse",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
