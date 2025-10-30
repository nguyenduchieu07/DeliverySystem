using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class FixDoubleFK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SlotReservations_WarehouseSlots_WarehouseSlotId1",
                table: "SlotReservations");

            migrationBuilder.DropIndex(
                name: "IX_SlotReservations_WarehouseSlotId1",
                table: "SlotReservations");

            migrationBuilder.DropColumn(
                name: "WarehouseSlotId1",
                table: "SlotReservations");

            migrationBuilder.UpdateData(
                table: "Addresses",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa20"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 29, 19, 1, 37, 530, DateTimeKind.Local).AddTicks(7159));

            migrationBuilder.UpdateData(
                table: "Addresses",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa21"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 29, 19, 1, 37, 530, DateTimeKind.Local).AddTicks(7166));

            migrationBuilder.UpdateData(
                table: "Addresses",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa22"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 29, 19, 1, 37, 530, DateTimeKind.Local).AddTicks(7169));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000001"),
                column: "ConcurrencyStamp",
                value: "1927d872-4d44-4c9a-a926-62044dff4345");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000002"),
                column: "ConcurrencyStamp",
                value: "81c5de66-a184-4a0c-9fde-13d64bb42599");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000003"),
                column: "ConcurrencyStamp",
                value: "247df641-100a-4e25-9922-8db3e0a98e16");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000004"),
                column: "ConcurrencyStamp",
                value: "f770dba6-0ab1-4001-b36c-f2dbb0de1013");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222221"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEItn2EXrmf0BGZDS19zEUOtmGVLQHsY+Uxy2nPSDWqZnk2fo8ARzgsiioVLXVyn1rg==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEItn2EXrmf0BGZDS19zEUOtmGVLQHsY+Uxy2nPSDWqZnk2fo8ARzgsiioVLXVyn1rg==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222223"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEItn2EXrmf0BGZDS19zEUOtmGVLQHsY+Uxy2nPSDWqZnk2fo8ARzgsiioVLXVyn1rg==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000001"),
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "UpdatedAt" },
                values: new object[] { "2ff489a1-a056-4ffe-bf53-9c88edac8b33", new DateTime(2025, 10, 29, 12, 1, 37, 483, DateTimeKind.Utc).AddTicks(2498), "AQAAAAIAAYagAAAAEOlNuR329lq4hGjmMuAGEPU/U0iJEw6L9DPYKHE1wdmks/WNhwEXUOVyXgFhaAU/Fg==", new DateTime(2025, 10, 29, 12, 1, 37, 483, DateTimeKind.Utc).AddTicks(2501) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000002"),
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "UpdatedAt" },
                values: new object[] { "79b8f812-17e4-4aa8-a516-817082543af1", new DateTime(2025, 10, 29, 12, 1, 37, 435, DateTimeKind.Utc).AddTicks(3988), "AQAAAAIAAYagAAAAEACb90BygQ6cdd7Wms7elZsZcnCyDgECwfe0pcL1eVSkMTgjLpOOoFN+3C7EbGWcCw==", new DateTime(2025, 10, 29, 12, 1, 37, 435, DateTimeKind.Utc).AddTicks(3989) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa01"),
                column: "ConcurrencyStamp",
                value: "9dbe6549-4ee9-4c92-a9f8-6af6d2e52d8b");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa02"),
                column: "ConcurrencyStamp",
                value: "aa43516e-e702-4ad7-9f5c-d270c42e4089");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 29, 19, 1, 37, 435, DateTimeKind.Local).AddTicks(4260));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 29, 19, 1, 37, 435, DateTimeKind.Local).AddTicks(4285));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 29, 19, 1, 37, 435, DateTimeKind.Local).AddTicks(4287));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000004"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 29, 19, 1, 37, 435, DateTimeKind.Local).AddTicks(4289));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa2-0000-0000-0000-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 29, 19, 1, 37, 435, DateTimeKind.Local).AddTicks(4281));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa2-0000-0000-0000-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 29, 19, 1, 37, 435, DateTimeKind.Local).AddTicks(4290));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa2-0000-0000-0000-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 29, 19, 1, 37, 435, DateTimeKind.Local).AddTicks(4292));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa3-0000-0000-0000-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 29, 19, 1, 37, 435, DateTimeKind.Local).AddTicks(4283));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa3-0000-0000-0000-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 29, 19, 1, 37, 435, DateTimeKind.Local).AddTicks(4294));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa3-0000-0000-0000-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 29, 19, 1, 37, 435, DateTimeKind.Local).AddTicks(4296));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa10"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 29, 19, 1, 37, 530, DateTimeKind.Local).AddTicks(7232));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa11"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 29, 19, 1, 37, 530, DateTimeKind.Local).AddTicks(7235));

            migrationBuilder.UpdateData(
                table: "KycDocuments",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc1"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 23, 12, 1, 37, 530, DateTimeKind.Utc).AddTicks(6875));

            migrationBuilder.UpdateData(
                table: "KycDocuments",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc2"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 23, 12, 1, 37, 530, DateTimeKind.Utc).AddTicks(6878));

            migrationBuilder.UpdateData(
                table: "KycDocuments",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc3"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 23, 12, 1, 37, 530, DateTimeKind.Utc).AddTicks(6881));

            migrationBuilder.UpdateData(
                table: "KycDocuments",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc4"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 24, 12, 1, 37, 530, DateTimeKind.Utc).AddTicks(6884));

            migrationBuilder.UpdateData(
                table: "KycDocuments",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc5"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 24, 12, 1, 37, 530, DateTimeKind.Utc).AddTicks(6932));

            migrationBuilder.UpdateData(
                table: "KycDocuments",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc6"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 14, 12, 1, 37, 530, DateTimeKind.Utc).AddTicks(6934));

            migrationBuilder.UpdateData(
                table: "KycDocuments",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc7"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 14, 12, 1, 37, 530, DateTimeKind.Utc).AddTicks(6936));

            migrationBuilder.UpdateData(
                table: "KycDocuments",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc8"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 14, 12, 1, 37, 530, DateTimeKind.Utc).AddTicks(6939));

            migrationBuilder.UpdateData(
                table: "KycSubmissions",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb1"),
                columns: new[] { "CreatedAt", "SubmittedAt" },
                values: new object[] { new DateTime(2025, 10, 23, 12, 1, 37, 530, DateTimeKind.Utc).AddTicks(6816), new DateTime(2025, 10, 23, 12, 1, 37, 530, DateTimeKind.Utc).AddTicks(6815) });

            migrationBuilder.UpdateData(
                table: "KycSubmissions",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb2"),
                columns: new[] { "CreatedAt", "ReviewedAt", "SubmittedAt" },
                values: new object[] { new DateTime(2025, 10, 24, 12, 1, 37, 530, DateTimeKind.Utc).AddTicks(6820), new DateTime(2025, 10, 25, 12, 1, 37, 530, DateTimeKind.Utc).AddTicks(6818), new DateTime(2025, 10, 24, 12, 1, 37, 530, DateTimeKind.Utc).AddTicks(6818) });

            migrationBuilder.UpdateData(
                table: "KycSubmissions",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb3"),
                columns: new[] { "CreatedAt", "ReviewedAt", "SubmittedAt" },
                values: new object[] { new DateTime(2025, 10, 14, 12, 1, 37, 530, DateTimeKind.Utc).AddTicks(6830), new DateTime(2025, 10, 15, 12, 1, 37, 530, DateTimeKind.Utc).AddTicks(6828), new DateTime(2025, 10, 14, 12, 1, 37, 530, DateTimeKind.Utc).AddTicks(6827) });

            migrationBuilder.UpdateData(
                table: "OrderItems",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa40"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 29, 19, 1, 37, 530, DateTimeKind.Local).AddTicks(7579));

            migrationBuilder.UpdateData(
                table: "OrderItems",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa41"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 29, 19, 1, 37, 530, DateTimeKind.Local).AddTicks(7590));

            migrationBuilder.UpdateData(
                table: "OrderItems",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa42"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 29, 19, 1, 37, 530, DateTimeKind.Local).AddTicks(7594));

            migrationBuilder.UpdateData(
                table: "OrderItems",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa43"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 29, 19, 1, 37, 530, DateTimeKind.Local).AddTicks(7604));

            migrationBuilder.UpdateData(
                table: "PriceRules",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa14"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 29, 19, 1, 37, 530, DateTimeKind.Local).AddTicks(7351));

            migrationBuilder.UpdateData(
                table: "Stores",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa1"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 29, 12, 1, 37, 530, DateTimeKind.Utc).AddTicks(6731), new DateTime(2025, 10, 29, 12, 1, 37, 530, DateTimeKind.Utc).AddTicks(6731) });

            migrationBuilder.UpdateData(
                table: "Stores",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa2"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 29, 12, 1, 37, 530, DateTimeKind.Utc).AddTicks(6741), new DateTime(2025, 10, 29, 12, 1, 37, 530, DateTimeKind.Utc).AddTicks(6742) });

            migrationBuilder.UpdateData(
                table: "Stores",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa3"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 29, 12, 1, 37, 530, DateTimeKind.Utc).AddTicks(6749), new DateTime(2025, 10, 29, 12, 1, 37, 530, DateTimeKind.Utc).AddTicks(6752) });

            migrationBuilder.UpdateData(
                table: "WarehouseSlots",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa61"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 29, 19, 1, 37, 530, DateTimeKind.Local).AddTicks(7764));

            migrationBuilder.UpdateData(
                table: "WarehouseSlots",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa62"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 29, 19, 1, 37, 530, DateTimeKind.Local).AddTicks(7767));

            migrationBuilder.UpdateData(
                table: "WarehouseSlots",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa63"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 29, 19, 1, 37, 530, DateTimeKind.Local).AddTicks(7769));

            migrationBuilder.UpdateData(
                table: "Warehouses",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa60"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 29, 19, 1, 37, 530, DateTimeKind.Local).AddTicks(7714));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "WarehouseSlotId1",
                table: "SlotReservations",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.UpdateData(
                table: "Addresses",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa20"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 29, 18, 23, 16, 855, DateTimeKind.Local).AddTicks(5544));

            migrationBuilder.UpdateData(
                table: "Addresses",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa21"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 29, 18, 23, 16, 855, DateTimeKind.Local).AddTicks(5556));

            migrationBuilder.UpdateData(
                table: "Addresses",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa22"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 29, 18, 23, 16, 855, DateTimeKind.Local).AddTicks(5559));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000001"),
                column: "ConcurrencyStamp",
                value: "f328dabc-53e2-44ed-96a3-c3f4469fca59");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000002"),
                column: "ConcurrencyStamp",
                value: "debf4d89-3487-4383-980c-8fde899c14f7");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000003"),
                column: "ConcurrencyStamp",
                value: "6b9d3ad2-1fdc-4630-883f-f0e29e1dca56");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000004"),
                column: "ConcurrencyStamp",
                value: "f6d5e961-d8a8-4d59-a6f6-4375bb1e8f44");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222221"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEBHLjcQqsoXXd84Nj2RbG2BOGkaNsXHfV+vdKhz3i+elv2IdQX23FEiWHWiQPVlzTg==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEBHLjcQqsoXXd84Nj2RbG2BOGkaNsXHfV+vdKhz3i+elv2IdQX23FEiWHWiQPVlzTg==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222223"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEBHLjcQqsoXXd84Nj2RbG2BOGkaNsXHfV+vdKhz3i+elv2IdQX23FEiWHWiQPVlzTg==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000001"),
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "UpdatedAt" },
                values: new object[] { "43d9ec85-5712-4d7f-a45d-b0f047afdfc4", new DateTime(2025, 10, 29, 11, 23, 16, 808, DateTimeKind.Utc).AddTicks(285), "AQAAAAIAAYagAAAAEAL41rOdFBVqAr0ZWBSgDZVtP6rj1PV1KF6Wu0URTi9VwTFgbvw4lmj7ABEZrmWIXg==", new DateTime(2025, 10, 29, 11, 23, 16, 808, DateTimeKind.Utc).AddTicks(290) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000002"),
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "UpdatedAt" },
                values: new object[] { "4751fa85-3cd4-4e69-9f06-7f8626790c54", new DateTime(2025, 10, 29, 11, 23, 16, 759, DateTimeKind.Utc).AddTicks(8815), "AQAAAAIAAYagAAAAEJ9YDJEDDQu9zJLcizgEpNDMno15xpWl06jf5VKPvRObUcvnfbVW+MzGsyJXsMgngQ==", new DateTime(2025, 10, 29, 11, 23, 16, 759, DateTimeKind.Utc).AddTicks(8819) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa01"),
                column: "ConcurrencyStamp",
                value: "d4012ce4-9fda-44f1-b7dd-05b5efd8f5cf");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa02"),
                column: "ConcurrencyStamp",
                value: "7a8e1ab8-2898-4419-837d-94efb3653add");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 29, 18, 23, 16, 760, DateTimeKind.Local).AddTicks(8689));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 29, 18, 23, 16, 760, DateTimeKind.Local).AddTicks(8716));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 29, 18, 23, 16, 760, DateTimeKind.Local).AddTicks(8718));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000004"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 29, 18, 23, 16, 760, DateTimeKind.Local).AddTicks(8721));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa2-0000-0000-0000-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 29, 18, 23, 16, 760, DateTimeKind.Local).AddTicks(8711));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa2-0000-0000-0000-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 29, 18, 23, 16, 760, DateTimeKind.Local).AddTicks(8722));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa2-0000-0000-0000-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 29, 18, 23, 16, 760, DateTimeKind.Local).AddTicks(8725));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa3-0000-0000-0000-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 29, 18, 23, 16, 760, DateTimeKind.Local).AddTicks(8714));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa3-0000-0000-0000-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 29, 18, 23, 16, 760, DateTimeKind.Local).AddTicks(8727));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa3-0000-0000-0000-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 29, 18, 23, 16, 760, DateTimeKind.Local).AddTicks(8728));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa10"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 29, 18, 23, 16, 855, DateTimeKind.Local).AddTicks(5605));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa11"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 29, 18, 23, 16, 855, DateTimeKind.Local).AddTicks(5611));

            migrationBuilder.UpdateData(
                table: "KycDocuments",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc1"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 23, 11, 23, 16, 855, DateTimeKind.Utc).AddTicks(5239));

            migrationBuilder.UpdateData(
                table: "KycDocuments",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc2"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 23, 11, 23, 16, 855, DateTimeKind.Utc).AddTicks(5242));

            migrationBuilder.UpdateData(
                table: "KycDocuments",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc3"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 23, 11, 23, 16, 855, DateTimeKind.Utc).AddTicks(5244));

            migrationBuilder.UpdateData(
                table: "KycDocuments",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc4"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 24, 11, 23, 16, 855, DateTimeKind.Utc).AddTicks(5246));

            migrationBuilder.UpdateData(
                table: "KycDocuments",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc5"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 24, 11, 23, 16, 855, DateTimeKind.Utc).AddTicks(5249));

            migrationBuilder.UpdateData(
                table: "KycDocuments",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc6"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 14, 11, 23, 16, 855, DateTimeKind.Utc).AddTicks(5251));

            migrationBuilder.UpdateData(
                table: "KycDocuments",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc7"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 14, 11, 23, 16, 855, DateTimeKind.Utc).AddTicks(5252));

            migrationBuilder.UpdateData(
                table: "KycDocuments",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc8"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 14, 11, 23, 16, 855, DateTimeKind.Utc).AddTicks(5254));

            migrationBuilder.UpdateData(
                table: "KycSubmissions",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb1"),
                columns: new[] { "CreatedAt", "SubmittedAt" },
                values: new object[] { new DateTime(2025, 10, 23, 11, 23, 16, 855, DateTimeKind.Utc).AddTicks(5127), new DateTime(2025, 10, 23, 11, 23, 16, 855, DateTimeKind.Utc).AddTicks(5125) });

            migrationBuilder.UpdateData(
                table: "KycSubmissions",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb2"),
                columns: new[] { "CreatedAt", "ReviewedAt", "SubmittedAt" },
                values: new object[] { new DateTime(2025, 10, 24, 11, 23, 16, 855, DateTimeKind.Utc).AddTicks(5135), new DateTime(2025, 10, 25, 11, 23, 16, 855, DateTimeKind.Utc).AddTicks(5132), new DateTime(2025, 10, 24, 11, 23, 16, 855, DateTimeKind.Utc).AddTicks(5131) });

            migrationBuilder.UpdateData(
                table: "KycSubmissions",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb3"),
                columns: new[] { "CreatedAt", "ReviewedAt", "SubmittedAt" },
                values: new object[] { new DateTime(2025, 10, 14, 11, 23, 16, 855, DateTimeKind.Utc).AddTicks(5140), new DateTime(2025, 10, 15, 11, 23, 16, 855, DateTimeKind.Utc).AddTicks(5139), new DateTime(2025, 10, 14, 11, 23, 16, 855, DateTimeKind.Utc).AddTicks(5137) });

            migrationBuilder.UpdateData(
                table: "OrderItems",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa40"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 29, 18, 23, 16, 855, DateTimeKind.Local).AddTicks(6057));

            migrationBuilder.UpdateData(
                table: "OrderItems",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa41"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 29, 18, 23, 16, 855, DateTimeKind.Local).AddTicks(6063));

            migrationBuilder.UpdateData(
                table: "OrderItems",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa42"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 29, 18, 23, 16, 855, DateTimeKind.Local).AddTicks(6065));

            migrationBuilder.UpdateData(
                table: "OrderItems",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa43"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 29, 18, 23, 16, 855, DateTimeKind.Local).AddTicks(6067));

            migrationBuilder.UpdateData(
                table: "PriceRules",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa14"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 29, 18, 23, 16, 855, DateTimeKind.Local).AddTicks(5859));

            migrationBuilder.UpdateData(
                table: "Stores",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa1"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 29, 11, 23, 16, 855, DateTimeKind.Utc).AddTicks(3818), new DateTime(2025, 10, 29, 11, 23, 16, 855, DateTimeKind.Utc).AddTicks(3819) });

            migrationBuilder.UpdateData(
                table: "Stores",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa2"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 29, 11, 23, 16, 855, DateTimeKind.Utc).AddTicks(3835), new DateTime(2025, 10, 29, 11, 23, 16, 855, DateTimeKind.Utc).AddTicks(3843) });

            migrationBuilder.UpdateData(
                table: "Stores",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa3"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 29, 11, 23, 16, 855, DateTimeKind.Utc).AddTicks(3850), new DateTime(2025, 10, 29, 11, 23, 16, 855, DateTimeKind.Utc).AddTicks(3856) });

            migrationBuilder.UpdateData(
                table: "WarehouseSlots",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa61"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 29, 18, 23, 16, 855, DateTimeKind.Local).AddTicks(6214));

            migrationBuilder.UpdateData(
                table: "WarehouseSlots",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa62"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 29, 18, 23, 16, 855, DateTimeKind.Local).AddTicks(6218));

            migrationBuilder.UpdateData(
                table: "WarehouseSlots",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa63"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 29, 18, 23, 16, 855, DateTimeKind.Local).AddTicks(6219));

            migrationBuilder.UpdateData(
                table: "Warehouses",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa60"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 29, 18, 23, 16, 855, DateTimeKind.Local).AddTicks(6171));

            migrationBuilder.CreateIndex(
                name: "IX_SlotReservations_WarehouseSlotId1",
                table: "SlotReservations",
                column: "WarehouseSlotId1");

            migrationBuilder.AddForeignKey(
                name: "FK_SlotReservations_WarehouseSlots_WarehouseSlotId1",
                table: "SlotReservations",
                column: "WarehouseSlotId1",
                principalTable: "WarehouseSlots",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
