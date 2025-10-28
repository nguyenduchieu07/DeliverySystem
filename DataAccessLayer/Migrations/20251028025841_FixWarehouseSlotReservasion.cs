using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class FixWarehouseSlotReservasion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "WarehouseSlotId1",
                table: "SlotReservations",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Addresses",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa20"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 28, 9, 58, 40, 504, DateTimeKind.Local).AddTicks(985));

            migrationBuilder.UpdateData(
                table: "Addresses",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa21"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 28, 9, 58, 40, 504, DateTimeKind.Local).AddTicks(992));

            migrationBuilder.UpdateData(
                table: "Addresses",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa22"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 28, 9, 58, 40, 504, DateTimeKind.Local).AddTicks(995));

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222221"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEKJdRGdFnHBJz//LRyvUYFps4JMxpI8fjKl7IPk9c90jUT2G8ECLk1rWiBelj5+1sA==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEKJdRGdFnHBJz//LRyvUYFps4JMxpI8fjKl7IPk9c90jUT2G8ECLk1rWiBelj5+1sA==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222223"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEKJdRGdFnHBJz//LRyvUYFps4JMxpI8fjKl7IPk9c90jUT2G8ECLk1rWiBelj5+1sA==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000001"),
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "UpdatedAt" },
                values: new object[] { "c6d62ebc-68e9-4d30-968c-b4d7241038a5", new DateTime(2025, 10, 28, 2, 58, 40, 332, DateTimeKind.Utc).AddTicks(3226), "AQAAAAIAAYagAAAAED2mJEOyDMXsOztMK3vGrI1YEO5eggShX39gv33tfCBii+iidFmt0bk1NIWXov/e8A==", new DateTime(2025, 10, 28, 2, 58, 40, 332, DateTimeKind.Utc).AddTicks(3232) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000002"),
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "UpdatedAt" },
                values: new object[] { "d0fe47b9-e386-4f8f-8fb2-da86e6cb7783", new DateTime(2025, 10, 28, 2, 58, 40, 418, DateTimeKind.Utc).AddTicks(5177), "AQAAAAIAAYagAAAAEOPv4dWbIBq/WU+9V6XwHIrJJSrFJ3Au/FoX9OwzsRpE/gcaAZvg/V6FsB3FSlpDqw==", new DateTime(2025, 10, 28, 2, 58, 40, 418, DateTimeKind.Utc).AddTicks(5181) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa01"),
                column: "ConcurrencyStamp",
                value: "b17d4b70-0e2b-4756-9224-ced9faf04b4d");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa02"),
                column: "ConcurrencyStamp",
                value: "a69ab4d0-532f-47ad-94ba-47c8e319e7af");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 28, 9, 58, 40, 332, DateTimeKind.Local).AddTicks(3649));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 28, 9, 58, 40, 332, DateTimeKind.Local).AddTicks(3675));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 28, 9, 58, 40, 332, DateTimeKind.Local).AddTicks(3684));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000004"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 28, 9, 58, 40, 332, DateTimeKind.Local).AddTicks(3688));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa2-0000-0000-0000-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 28, 9, 58, 40, 332, DateTimeKind.Local).AddTicks(3669));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa2-0000-0000-0000-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 28, 9, 58, 40, 332, DateTimeKind.Local).AddTicks(3691));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa2-0000-0000-0000-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 28, 9, 58, 40, 332, DateTimeKind.Local).AddTicks(3694));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa3-0000-0000-0000-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 28, 9, 58, 40, 332, DateTimeKind.Local).AddTicks(3673));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa3-0000-0000-0000-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 28, 9, 58, 40, 332, DateTimeKind.Local).AddTicks(3697));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa3-0000-0000-0000-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 28, 9, 58, 40, 332, DateTimeKind.Local).AddTicks(3700));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa10"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 28, 9, 58, 40, 504, DateTimeKind.Local).AddTicks(1067));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa11"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 28, 9, 58, 40, 504, DateTimeKind.Local).AddTicks(1128));

            migrationBuilder.UpdateData(
                table: "KycDocuments",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc1"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 22, 2, 58, 40, 504, DateTimeKind.Utc).AddTicks(247));

            migrationBuilder.UpdateData(
                table: "KycDocuments",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc2"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 22, 2, 58, 40, 504, DateTimeKind.Utc).AddTicks(249));

            migrationBuilder.UpdateData(
                table: "KycDocuments",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc3"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 22, 2, 58, 40, 504, DateTimeKind.Utc).AddTicks(253));

            migrationBuilder.UpdateData(
                table: "KycDocuments",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc4"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 23, 2, 58, 40, 504, DateTimeKind.Utc).AddTicks(255));

            migrationBuilder.UpdateData(
                table: "KycDocuments",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc5"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 23, 2, 58, 40, 504, DateTimeKind.Utc).AddTicks(257));

            migrationBuilder.UpdateData(
                table: "KycDocuments",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc6"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 13, 2, 58, 40, 504, DateTimeKind.Utc).AddTicks(263));

            migrationBuilder.UpdateData(
                table: "KycDocuments",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc7"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 13, 2, 58, 40, 504, DateTimeKind.Utc).AddTicks(265));

            migrationBuilder.UpdateData(
                table: "KycDocuments",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc8"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 13, 2, 58, 40, 504, DateTimeKind.Utc).AddTicks(267));

            migrationBuilder.UpdateData(
                table: "KycSubmissions",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb1"),
                columns: new[] { "CreatedAt", "SubmittedAt" },
                values: new object[] { new DateTime(2025, 10, 22, 2, 58, 40, 504, DateTimeKind.Utc).AddTicks(208), new DateTime(2025, 10, 22, 2, 58, 40, 504, DateTimeKind.Utc).AddTicks(207) });

            migrationBuilder.UpdateData(
                table: "KycSubmissions",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb2"),
                columns: new[] { "CreatedAt", "ReviewedAt", "SubmittedAt" },
                values: new object[] { new DateTime(2025, 10, 23, 2, 58, 40, 504, DateTimeKind.Utc).AddTicks(215), new DateTime(2025, 10, 24, 2, 58, 40, 504, DateTimeKind.Utc).AddTicks(212), new DateTime(2025, 10, 23, 2, 58, 40, 504, DateTimeKind.Utc).AddTicks(211) });

            migrationBuilder.UpdateData(
                table: "KycSubmissions",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb3"),
                columns: new[] { "CreatedAt", "ReviewedAt", "SubmittedAt" },
                values: new object[] { new DateTime(2025, 10, 13, 2, 58, 40, 504, DateTimeKind.Utc).AddTicks(223), new DateTime(2025, 10, 14, 2, 58, 40, 504, DateTimeKind.Utc).AddTicks(222), new DateTime(2025, 10, 13, 2, 58, 40, 504, DateTimeKind.Utc).AddTicks(221) });

            migrationBuilder.UpdateData(
                table: "OrderItems",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa40"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 28, 9, 58, 40, 504, DateTimeKind.Local).AddTicks(1333));

            migrationBuilder.UpdateData(
                table: "OrderItems",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa41"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 28, 9, 58, 40, 504, DateTimeKind.Local).AddTicks(1337));

            migrationBuilder.UpdateData(
                table: "OrderItems",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa42"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 28, 9, 58, 40, 504, DateTimeKind.Local).AddTicks(1343));

            migrationBuilder.UpdateData(
                table: "OrderItems",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa43"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 28, 9, 58, 40, 504, DateTimeKind.Local).AddTicks(1350));

            migrationBuilder.UpdateData(
                table: "PriceRules",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa14"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 28, 9, 58, 40, 504, DateTimeKind.Local).AddTicks(1213));

            migrationBuilder.UpdateData(
                table: "Stores",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa1"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 28, 2, 58, 40, 504, DateTimeKind.Utc).AddTicks(142), new DateTime(2025, 10, 28, 2, 58, 40, 504, DateTimeKind.Utc).AddTicks(143) });

            migrationBuilder.UpdateData(
                table: "Stores",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa2"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 28, 2, 58, 40, 504, DateTimeKind.Utc).AddTicks(150), new DateTime(2025, 10, 28, 2, 58, 40, 504, DateTimeKind.Utc).AddTicks(151) });

            migrationBuilder.UpdateData(
                table: "Stores",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa3"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 28, 2, 58, 40, 504, DateTimeKind.Utc).AddTicks(155), new DateTime(2025, 10, 28, 2, 58, 40, 504, DateTimeKind.Utc).AddTicks(159) });

            migrationBuilder.UpdateData(
                table: "WarehouseSlots",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa61"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 28, 9, 58, 40, 504, DateTimeKind.Local).AddTicks(1427));

            migrationBuilder.UpdateData(
                table: "WarehouseSlots",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa62"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 28, 9, 58, 40, 504, DateTimeKind.Local).AddTicks(1431));

            migrationBuilder.UpdateData(
                table: "WarehouseSlots",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa63"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 28, 9, 58, 40, 504, DateTimeKind.Local).AddTicks(1432));

            migrationBuilder.UpdateData(
                table: "Warehouses",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa60"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 28, 9, 58, 40, 504, DateTimeKind.Local).AddTicks(1408));

            migrationBuilder.CreateIndex(
                name: "IX_SlotReservations_WarehouseSlotId1",
                table: "SlotReservations",
                column: "WarehouseSlotId1");

            migrationBuilder.AddForeignKey(
                name: "FK_SlotReservations_WarehouseSlots_WarehouseSlotId1",
                table: "SlotReservations",
                column: "WarehouseSlotId1",
                principalTable: "WarehouseSlots",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
                value: new DateTime(2025, 10, 26, 22, 18, 31, 887, DateTimeKind.Local).AddTicks(9770));

            migrationBuilder.UpdateData(
                table: "Addresses",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa21"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 26, 22, 18, 31, 887, DateTimeKind.Local).AddTicks(9779));

            migrationBuilder.UpdateData(
                table: "Addresses",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa22"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 26, 22, 18, 31, 887, DateTimeKind.Local).AddTicks(9782));

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222221"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEAr7uCKsxqq935wf8PdPVAntZKMFQCNtE3IeYgblodpHArCNh7H751/0VxZZPo8eBw==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEAr7uCKsxqq935wf8PdPVAntZKMFQCNtE3IeYgblodpHArCNh7H751/0VxZZPo8eBw==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222223"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEAr7uCKsxqq935wf8PdPVAntZKMFQCNtE3IeYgblodpHArCNh7H751/0VxZZPo8eBw==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000001"),
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "UpdatedAt" },
                values: new object[] { "eb2b2165-4efb-4930-8755-aa058176e0c2", new DateTime(2025, 10, 26, 15, 18, 31, 717, DateTimeKind.Utc).AddTicks(7304), "AQAAAAIAAYagAAAAELiA6LchK/2P0bGnL61zupux89lsGOxB8eUZ7JHYCogQYvnUBotYq9HqNLYQt+qTug==", new DateTime(2025, 10, 26, 15, 18, 31, 717, DateTimeKind.Utc).AddTicks(7314) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000002"),
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "UpdatedAt" },
                values: new object[] { "5bb169ac-3b83-4e10-9c56-d2bce5f0736a", new DateTime(2025, 10, 26, 15, 18, 31, 803, DateTimeKind.Utc).AddTicks(7818), "AQAAAAIAAYagAAAAEOn7sYaHvLKxmKD7xC8XPzQA/LppR9W0iDXbRjruNwwfBhdyevVxRrqZhGtubsafqQ==", new DateTime(2025, 10, 26, 15, 18, 31, 803, DateTimeKind.Utc).AddTicks(7826) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa01"),
                column: "ConcurrencyStamp",
                value: "62e9d316-be0d-4cc7-8750-11cc9ffd3e1a");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa02"),
                column: "ConcurrencyStamp",
                value: "b561968d-162b-4486-91a0-e388afc13cc7");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 26, 22, 18, 31, 717, DateTimeKind.Local).AddTicks(8082));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 26, 22, 18, 31, 717, DateTimeKind.Local).AddTicks(8106));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 26, 22, 18, 31, 717, DateTimeKind.Local).AddTicks(8111));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000004"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 26, 22, 18, 31, 717, DateTimeKind.Local).AddTicks(8113));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa2-0000-0000-0000-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 26, 22, 18, 31, 717, DateTimeKind.Local).AddTicks(8103));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa2-0000-0000-0000-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 26, 22, 18, 31, 717, DateTimeKind.Local).AddTicks(8114));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa2-0000-0000-0000-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 26, 22, 18, 31, 717, DateTimeKind.Local).AddTicks(8116));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa3-0000-0000-0000-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 26, 22, 18, 31, 717, DateTimeKind.Local).AddTicks(8105));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa3-0000-0000-0000-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 26, 22, 18, 31, 717, DateTimeKind.Local).AddTicks(8118));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa3-0000-0000-0000-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 26, 22, 18, 31, 717, DateTimeKind.Local).AddTicks(8119));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa10"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 26, 22, 18, 31, 887, DateTimeKind.Local).AddTicks(9822));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa11"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 26, 22, 18, 31, 887, DateTimeKind.Local).AddTicks(9828));

            migrationBuilder.UpdateData(
                table: "KycDocuments",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc1"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 20, 15, 18, 31, 887, DateTimeKind.Utc).AddTicks(9497));

            migrationBuilder.UpdateData(
                table: "KycDocuments",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc2"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 20, 15, 18, 31, 887, DateTimeKind.Utc).AddTicks(9501));

            migrationBuilder.UpdateData(
                table: "KycDocuments",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc3"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 20, 15, 18, 31, 887, DateTimeKind.Utc).AddTicks(9514));

            migrationBuilder.UpdateData(
                table: "KycDocuments",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc4"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 21, 15, 18, 31, 887, DateTimeKind.Utc).AddTicks(9516));

            migrationBuilder.UpdateData(
                table: "KycDocuments",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc5"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 21, 15, 18, 31, 887, DateTimeKind.Utc).AddTicks(9518));

            migrationBuilder.UpdateData(
                table: "KycDocuments",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc6"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 11, 15, 18, 31, 887, DateTimeKind.Utc).AddTicks(9520));

            migrationBuilder.UpdateData(
                table: "KycDocuments",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc7"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 11, 15, 18, 31, 887, DateTimeKind.Utc).AddTicks(9522));

            migrationBuilder.UpdateData(
                table: "KycDocuments",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc8"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 11, 15, 18, 31, 887, DateTimeKind.Utc).AddTicks(9594));

            migrationBuilder.UpdateData(
                table: "KycSubmissions",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb1"),
                columns: new[] { "CreatedAt", "SubmittedAt" },
                values: new object[] { new DateTime(2025, 10, 20, 15, 18, 31, 887, DateTimeKind.Utc).AddTicks(9396), new DateTime(2025, 10, 20, 15, 18, 31, 887, DateTimeKind.Utc).AddTicks(9394) });

            migrationBuilder.UpdateData(
                table: "KycSubmissions",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb2"),
                columns: new[] { "CreatedAt", "ReviewedAt", "SubmittedAt" },
                values: new object[] { new DateTime(2025, 10, 21, 15, 18, 31, 887, DateTimeKind.Utc).AddTicks(9403), new DateTime(2025, 10, 22, 15, 18, 31, 887, DateTimeKind.Utc).AddTicks(9399), new DateTime(2025, 10, 21, 15, 18, 31, 887, DateTimeKind.Utc).AddTicks(9398) });

            migrationBuilder.UpdateData(
                table: "KycSubmissions",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb3"),
                columns: new[] { "CreatedAt", "ReviewedAt", "SubmittedAt" },
                values: new object[] { new DateTime(2025, 10, 11, 15, 18, 31, 887, DateTimeKind.Utc).AddTicks(9409), new DateTime(2025, 10, 12, 15, 18, 31, 887, DateTimeKind.Utc).AddTicks(9407), new DateTime(2025, 10, 11, 15, 18, 31, 887, DateTimeKind.Utc).AddTicks(9406) });

            migrationBuilder.UpdateData(
                table: "OrderItems",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa40"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 26, 22, 18, 31, 888, DateTimeKind.Local).AddTicks(153));

            migrationBuilder.UpdateData(
                table: "OrderItems",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa41"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 26, 22, 18, 31, 888, DateTimeKind.Local).AddTicks(161));

            migrationBuilder.UpdateData(
                table: "OrderItems",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa42"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 26, 22, 18, 31, 888, DateTimeKind.Local).AddTicks(163));

            migrationBuilder.UpdateData(
                table: "OrderItems",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa43"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 26, 22, 18, 31, 888, DateTimeKind.Local).AddTicks(165));

            migrationBuilder.UpdateData(
                table: "PriceRules",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa14"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 26, 22, 18, 31, 888, DateTimeKind.Local).AddTicks(29));

            migrationBuilder.UpdateData(
                table: "Stores",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa1"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 26, 15, 18, 31, 887, DateTimeKind.Utc).AddTicks(9300), new DateTime(2025, 10, 26, 15, 18, 31, 887, DateTimeKind.Utc).AddTicks(9301) });

            migrationBuilder.UpdateData(
                table: "Stores",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa2"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 26, 15, 18, 31, 887, DateTimeKind.Utc).AddTicks(9308), new DateTime(2025, 10, 26, 15, 18, 31, 887, DateTimeKind.Utc).AddTicks(9317) });

            migrationBuilder.UpdateData(
                table: "Stores",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa3"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 26, 15, 18, 31, 887, DateTimeKind.Utc).AddTicks(9323), new DateTime(2025, 10, 26, 15, 18, 31, 887, DateTimeKind.Utc).AddTicks(9329) });

            migrationBuilder.UpdateData(
                table: "WarehouseSlots",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa61"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 26, 22, 18, 31, 888, DateTimeKind.Local).AddTicks(296));

            migrationBuilder.UpdateData(
                table: "WarehouseSlots",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa62"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 26, 22, 18, 31, 888, DateTimeKind.Local).AddTicks(300));

            migrationBuilder.UpdateData(
                table: "WarehouseSlots",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa63"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 26, 22, 18, 31, 888, DateTimeKind.Local).AddTicks(302));

            migrationBuilder.UpdateData(
                table: "Warehouses",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa60"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 26, 22, 18, 31, 888, DateTimeKind.Local).AddTicks(254));
        }
    }
}
