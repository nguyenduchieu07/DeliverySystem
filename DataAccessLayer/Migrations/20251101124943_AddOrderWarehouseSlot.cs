using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class AddOrderWarehouseSlot : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OrderWarehouseSlots",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WarehouseSlotId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AssignedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReleasedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderWarehouseSlots", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderWarehouseSlots_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderWarehouseSlots_WarehouseSlots_WarehouseSlotId",
                        column: x => x.WarehouseSlotId,
                        principalTable: "WarehouseSlots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Addresses",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa20"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 1, 19, 49, 42, 614, DateTimeKind.Local).AddTicks(2216));

            migrationBuilder.UpdateData(
                table: "Addresses",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa21"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 1, 19, 49, 42, 614, DateTimeKind.Local).AddTicks(2224));

            migrationBuilder.UpdateData(
                table: "Addresses",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa22"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 1, 19, 49, 42, 614, DateTimeKind.Local).AddTicks(2227));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000001"),
                column: "ConcurrencyStamp",
                value: "57bec7f9-60af-4d6c-973c-645ed872160f");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000002"),
                column: "ConcurrencyStamp",
                value: "09221e7c-6c0e-4564-8e79-e0e2a2c54d3b");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000003"),
                column: "ConcurrencyStamp",
                value: "648f2e77-795f-4eea-ad21-2f85807a362d");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000004"),
                column: "ConcurrencyStamp",
                value: "355b7749-ef44-44ee-8e5f-c117bdeb2ac6");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222221"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEPujXI5+hob2TKtWhXm2njbpbienZR5T7JsBqxkguOERnruSZKUPiJOH3lpsqspYxQ==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEPujXI5+hob2TKtWhXm2njbpbienZR5T7JsBqxkguOERnruSZKUPiJOH3lpsqspYxQ==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222223"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEPujXI5+hob2TKtWhXm2njbpbienZR5T7JsBqxkguOERnruSZKUPiJOH3lpsqspYxQ==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000001"),
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "UpdatedAt" },
                values: new object[] { "05ed82ab-9316-44fc-8ac2-9ec2096cf0bb", new DateTime(2025, 11, 1, 12, 49, 42, 566, DateTimeKind.Utc).AddTicks(4976), "AQAAAAIAAYagAAAAEAljFEK7CdEE7DprTLo7UHvFI9mnxBy+obiqN11bapSvUZK8UqHoE94vX1p57QJkLA==", new DateTime(2025, 11, 1, 12, 49, 42, 566, DateTimeKind.Utc).AddTicks(4982) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000002"),
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "UpdatedAt" },
                values: new object[] { "6f054c48-33c9-4290-a842-44747f3d625e", new DateTime(2025, 11, 1, 12, 49, 42, 518, DateTimeKind.Utc).AddTicks(2613), "AQAAAAIAAYagAAAAEJ4Q/I3EM0Ik86MPysDHQPZXNyUYr+/FsX5mqHgolHezHk1Jo5exGwOtZmnaffPalg==", new DateTime(2025, 11, 1, 12, 49, 42, 518, DateTimeKind.Utc).AddTicks(2619) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa01"),
                column: "ConcurrencyStamp",
                value: "b7685db0-985f-4f42-9943-f85cdeac6c84");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa02"),
                column: "ConcurrencyStamp",
                value: "9d7f9b14-c9e6-4873-8e2a-574dd2f4e51c");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 1, 19, 49, 42, 518, DateTimeKind.Local).AddTicks(3132));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 1, 19, 49, 42, 518, DateTimeKind.Local).AddTicks(3152));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 1, 19, 49, 42, 518, DateTimeKind.Local).AddTicks(3156));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000004"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 1, 19, 49, 42, 518, DateTimeKind.Local).AddTicks(3158));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa2-0000-0000-0000-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 1, 19, 49, 42, 518, DateTimeKind.Local).AddTicks(3147));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa2-0000-0000-0000-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 1, 19, 49, 42, 518, DateTimeKind.Local).AddTicks(3159));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa2-0000-0000-0000-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 1, 19, 49, 42, 518, DateTimeKind.Local).AddTicks(3161));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa3-0000-0000-0000-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 1, 19, 49, 42, 518, DateTimeKind.Local).AddTicks(3149));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa3-0000-0000-0000-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 1, 19, 49, 42, 518, DateTimeKind.Local).AddTicks(3162));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa3-0000-0000-0000-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 1, 19, 49, 42, 518, DateTimeKind.Local).AddTicks(3164));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa10"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 1, 19, 49, 42, 614, DateTimeKind.Local).AddTicks(2283));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa11"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 1, 19, 49, 42, 614, DateTimeKind.Local).AddTicks(2286));

            migrationBuilder.UpdateData(
                table: "KycDocuments",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc1"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 26, 12, 49, 42, 614, DateTimeKind.Utc).AddTicks(1811));

            migrationBuilder.UpdateData(
                table: "KycDocuments",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc2"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 26, 12, 49, 42, 614, DateTimeKind.Utc).AddTicks(1814));

            migrationBuilder.UpdateData(
                table: "KycDocuments",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc3"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 26, 12, 49, 42, 614, DateTimeKind.Utc).AddTicks(1818));

            migrationBuilder.UpdateData(
                table: "KycDocuments",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc4"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 27, 12, 49, 42, 614, DateTimeKind.Utc).AddTicks(1819));

            migrationBuilder.UpdateData(
                table: "KycDocuments",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc5"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 27, 12, 49, 42, 614, DateTimeKind.Utc).AddTicks(1821));

            migrationBuilder.UpdateData(
                table: "KycDocuments",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc6"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 17, 12, 49, 42, 614, DateTimeKind.Utc).AddTicks(1823));

            migrationBuilder.UpdateData(
                table: "KycDocuments",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc7"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 17, 12, 49, 42, 614, DateTimeKind.Utc).AddTicks(1825));

            migrationBuilder.UpdateData(
                table: "KycDocuments",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc8"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 17, 12, 49, 42, 614, DateTimeKind.Utc).AddTicks(1827));

            migrationBuilder.UpdateData(
                table: "KycSubmissions",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb1"),
                columns: new[] { "CreatedAt", "SubmittedAt" },
                values: new object[] { new DateTime(2025, 10, 26, 12, 49, 42, 614, DateTimeKind.Utc).AddTicks(1735), new DateTime(2025, 10, 26, 12, 49, 42, 614, DateTimeKind.Utc).AddTicks(1733) });

            migrationBuilder.UpdateData(
                table: "KycSubmissions",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb2"),
                columns: new[] { "CreatedAt", "ReviewedAt", "SubmittedAt" },
                values: new object[] { new DateTime(2025, 10, 27, 12, 49, 42, 614, DateTimeKind.Utc).AddTicks(1742), new DateTime(2025, 10, 28, 12, 49, 42, 614, DateTimeKind.Utc).AddTicks(1739), new DateTime(2025, 10, 27, 12, 49, 42, 614, DateTimeKind.Utc).AddTicks(1738) });

            migrationBuilder.UpdateData(
                table: "KycSubmissions",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb3"),
                columns: new[] { "CreatedAt", "ReviewedAt", "SubmittedAt" },
                values: new object[] { new DateTime(2025, 10, 17, 12, 49, 42, 614, DateTimeKind.Utc).AddTicks(1755), new DateTime(2025, 10, 18, 12, 49, 42, 614, DateTimeKind.Utc).AddTicks(1753), new DateTime(2025, 10, 17, 12, 49, 42, 614, DateTimeKind.Utc).AddTicks(1752) });

            migrationBuilder.UpdateData(
                table: "OrderItems",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa40"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 1, 19, 49, 42, 614, DateTimeKind.Local).AddTicks(6358));

            migrationBuilder.UpdateData(
                table: "OrderItems",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa41"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 1, 19, 49, 42, 614, DateTimeKind.Local).AddTicks(6366));

            migrationBuilder.UpdateData(
                table: "OrderItems",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa42"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 1, 19, 49, 42, 614, DateTimeKind.Local).AddTicks(6377));

            migrationBuilder.UpdateData(
                table: "OrderItems",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa43"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 1, 19, 49, 42, 614, DateTimeKind.Local).AddTicks(6395));

            migrationBuilder.UpdateData(
                table: "PriceRules",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa14"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 1, 19, 49, 42, 614, DateTimeKind.Local).AddTicks(6115));

            migrationBuilder.UpdateData(
                table: "Stores",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa1"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 1, 12, 49, 42, 614, DateTimeKind.Utc).AddTicks(1645), new DateTime(2025, 11, 1, 12, 49, 42, 614, DateTimeKind.Utc).AddTicks(1646) });

            migrationBuilder.UpdateData(
                table: "Stores",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa2"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 1, 12, 49, 42, 614, DateTimeKind.Utc).AddTicks(1658), new DateTime(2025, 11, 1, 12, 49, 42, 614, DateTimeKind.Utc).AddTicks(1658) });

            migrationBuilder.UpdateData(
                table: "Stores",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa3"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 2, 12, 49, 42, 614, DateTimeKind.Utc).AddTicks(1662), new DateTime(2025, 11, 1, 12, 49, 42, 614, DateTimeKind.Utc).AddTicks(1668) });

            migrationBuilder.UpdateData(
                table: "WarehouseSlots",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa61"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 1, 19, 49, 42, 614, DateTimeKind.Local).AddTicks(6668));

            migrationBuilder.UpdateData(
                table: "WarehouseSlots",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa62"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 1, 19, 49, 42, 614, DateTimeKind.Local).AddTicks(6671));

            migrationBuilder.UpdateData(
                table: "WarehouseSlots",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa63"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 1, 19, 49, 42, 614, DateTimeKind.Local).AddTicks(6673));

            migrationBuilder.UpdateData(
                table: "Warehouses",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa60"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 1, 19, 49, 42, 614, DateTimeKind.Local).AddTicks(6629));

            migrationBuilder.CreateIndex(
                name: "IX_OrderWarehouseSlots_OrderId",
                table: "OrderWarehouseSlots",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderWarehouseSlots_WarehouseSlotId",
                table: "OrderWarehouseSlots",
                column: "WarehouseSlotId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderWarehouseSlots");

            migrationBuilder.UpdateData(
                table: "Addresses",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa20"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 1, 2, 5, 15, 614, DateTimeKind.Local).AddTicks(9254));

            migrationBuilder.UpdateData(
                table: "Addresses",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa21"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 1, 2, 5, 15, 614, DateTimeKind.Local).AddTicks(9261));

            migrationBuilder.UpdateData(
                table: "Addresses",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa22"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 1, 2, 5, 15, 614, DateTimeKind.Local).AddTicks(9264));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000001"),
                column: "ConcurrencyStamp",
                value: "4a5b80b8-2b76-451c-9592-e46e4f3cd723");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000002"),
                column: "ConcurrencyStamp",
                value: "4888ad3a-94d2-413d-9755-b93944940866");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000003"),
                column: "ConcurrencyStamp",
                value: "3f357fc8-0f2e-42fa-bded-c3e5fa3d8272");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000004"),
                column: "ConcurrencyStamp",
                value: "598eaabb-7622-49eb-97bd-07a55146a19e");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222221"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEMeH4CGD6ifrex+B/KjJcNw9T3Yga296sHFIxHYduGJTAhhDr2/WTBLV6OXYElamHA==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEMeH4CGD6ifrex+B/KjJcNw9T3Yga296sHFIxHYduGJTAhhDr2/WTBLV6OXYElamHA==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222223"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEMeH4CGD6ifrex+B/KjJcNw9T3Yga296sHFIxHYduGJTAhhDr2/WTBLV6OXYElamHA==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000001"),
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "UpdatedAt" },
                values: new object[] { "755cb70b-4630-471c-9571-3ad4124e2a78", new DateTime(2025, 10, 31, 19, 5, 15, 572, DateTimeKind.Utc).AddTicks(6461), "AQAAAAIAAYagAAAAEKTrA/6utWFM+PdMfPymFKjRaxLWvC5xriX5mLG3FAiKivQP6vbx0TkDsUVwfZXx1Q==", new DateTime(2025, 10, 31, 19, 5, 15, 572, DateTimeKind.Utc).AddTicks(6469) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000002"),
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "UpdatedAt" },
                values: new object[] { "e5f1d4f0-a9cd-448a-a835-76ed36b48ff8", new DateTime(2025, 10, 31, 19, 5, 15, 527, DateTimeKind.Utc).AddTicks(5310), "AQAAAAIAAYagAAAAEKg//POc2BOCxSbwYu2b2iq4dR2H7xEkxV9L4JhSdRc1cMzwRJ5cGdhUNqLQez7KKA==", new DateTime(2025, 10, 31, 19, 5, 15, 527, DateTimeKind.Utc).AddTicks(5324) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa01"),
                column: "ConcurrencyStamp",
                value: "ee5ffeec-f74e-46f5-a474-6e292c2fdbda");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa02"),
                column: "ConcurrencyStamp",
                value: "036d3413-d538-4386-a57a-34ac5a93e42f");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 1, 2, 5, 15, 527, DateTimeKind.Local).AddTicks(6283));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 1, 2, 5, 15, 527, DateTimeKind.Local).AddTicks(6313));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 1, 2, 5, 15, 527, DateTimeKind.Local).AddTicks(6319));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000004"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 1, 2, 5, 15, 527, DateTimeKind.Local).AddTicks(6320));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa2-0000-0000-0000-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 1, 2, 5, 15, 527, DateTimeKind.Local).AddTicks(6310));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa2-0000-0000-0000-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 1, 2, 5, 15, 527, DateTimeKind.Local).AddTicks(6322));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa2-0000-0000-0000-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 1, 2, 5, 15, 527, DateTimeKind.Local).AddTicks(6324));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa3-0000-0000-0000-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 1, 2, 5, 15, 527, DateTimeKind.Local).AddTicks(6312));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa3-0000-0000-0000-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 1, 2, 5, 15, 527, DateTimeKind.Local).AddTicks(6326));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa3-0000-0000-0000-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 1, 2, 5, 15, 527, DateTimeKind.Local).AddTicks(6327));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa10"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 1, 2, 5, 15, 614, DateTimeKind.Local).AddTicks(9297));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa11"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 1, 2, 5, 15, 614, DateTimeKind.Local).AddTicks(9300));

            migrationBuilder.UpdateData(
                table: "KycDocuments",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc1"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 25, 19, 5, 15, 614, DateTimeKind.Utc).AddTicks(8995));

            migrationBuilder.UpdateData(
                table: "KycDocuments",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc2"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 25, 19, 5, 15, 614, DateTimeKind.Utc).AddTicks(8997));

            migrationBuilder.UpdateData(
                table: "KycDocuments",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc3"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 25, 19, 5, 15, 614, DateTimeKind.Utc).AddTicks(8999));

            migrationBuilder.UpdateData(
                table: "KycDocuments",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc4"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 26, 19, 5, 15, 614, DateTimeKind.Utc).AddTicks(9001));

            migrationBuilder.UpdateData(
                table: "KycDocuments",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc5"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 26, 19, 5, 15, 614, DateTimeKind.Utc).AddTicks(9073));

            migrationBuilder.UpdateData(
                table: "KycDocuments",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc6"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 16, 19, 5, 15, 614, DateTimeKind.Utc).AddTicks(9075));

            migrationBuilder.UpdateData(
                table: "KycDocuments",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc7"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 16, 19, 5, 15, 614, DateTimeKind.Utc).AddTicks(9077));

            migrationBuilder.UpdateData(
                table: "KycDocuments",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc8"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 16, 19, 5, 15, 614, DateTimeKind.Utc).AddTicks(9078));

            migrationBuilder.UpdateData(
                table: "KycSubmissions",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb1"),
                columns: new[] { "CreatedAt", "SubmittedAt" },
                values: new object[] { new DateTime(2025, 10, 25, 19, 5, 15, 614, DateTimeKind.Utc).AddTicks(8948), new DateTime(2025, 10, 25, 19, 5, 15, 614, DateTimeKind.Utc).AddTicks(8947) });

            migrationBuilder.UpdateData(
                table: "KycSubmissions",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb2"),
                columns: new[] { "CreatedAt", "ReviewedAt", "SubmittedAt" },
                values: new object[] { new DateTime(2025, 10, 26, 19, 5, 15, 614, DateTimeKind.Utc).AddTicks(8954), new DateTime(2025, 10, 27, 19, 5, 15, 614, DateTimeKind.Utc).AddTicks(8951), new DateTime(2025, 10, 26, 19, 5, 15, 614, DateTimeKind.Utc).AddTicks(8951) });

            migrationBuilder.UpdateData(
                table: "KycSubmissions",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb3"),
                columns: new[] { "CreatedAt", "ReviewedAt", "SubmittedAt" },
                values: new object[] { new DateTime(2025, 10, 16, 19, 5, 15, 614, DateTimeKind.Utc).AddTicks(8958), new DateTime(2025, 10, 17, 19, 5, 15, 614, DateTimeKind.Utc).AddTicks(8957), new DateTime(2025, 10, 16, 19, 5, 15, 614, DateTimeKind.Utc).AddTicks(8956) });

            migrationBuilder.UpdateData(
                table: "OrderItems",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa40"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 1, 2, 5, 15, 614, DateTimeKind.Local).AddTicks(9531));

            migrationBuilder.UpdateData(
                table: "OrderItems",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa41"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 1, 2, 5, 15, 614, DateTimeKind.Local).AddTicks(9536));

            migrationBuilder.UpdateData(
                table: "OrderItems",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa42"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 1, 2, 5, 15, 614, DateTimeKind.Local).AddTicks(9547));

            migrationBuilder.UpdateData(
                table: "OrderItems",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa43"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 1, 2, 5, 15, 614, DateTimeKind.Local).AddTicks(9562));

            migrationBuilder.UpdateData(
                table: "PriceRules",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa14"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 1, 2, 5, 15, 614, DateTimeKind.Local).AddTicks(9406));

            migrationBuilder.UpdateData(
                table: "Stores",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa1"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 31, 19, 5, 15, 614, DateTimeKind.Utc).AddTicks(8875), new DateTime(2025, 10, 31, 19, 5, 15, 614, DateTimeKind.Utc).AddTicks(8876) });

            migrationBuilder.UpdateData(
                table: "Stores",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa2"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 31, 19, 5, 15, 614, DateTimeKind.Utc).AddTicks(8883), new DateTime(2025, 10, 31, 19, 5, 15, 614, DateTimeKind.Utc).AddTicks(8883) });

            migrationBuilder.UpdateData(
                table: "Stores",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa3"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 1, 19, 5, 15, 614, DateTimeKind.Utc).AddTicks(8888), new DateTime(2025, 10, 31, 19, 5, 15, 614, DateTimeKind.Utc).AddTicks(8892) });

            migrationBuilder.UpdateData(
                table: "WarehouseSlots",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa61"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 1, 2, 5, 15, 614, DateTimeKind.Local).AddTicks(9707));

            migrationBuilder.UpdateData(
                table: "WarehouseSlots",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa62"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 1, 2, 5, 15, 614, DateTimeKind.Local).AddTicks(9711));

            migrationBuilder.UpdateData(
                table: "WarehouseSlots",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa63"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 1, 2, 5, 15, 614, DateTimeKind.Local).AddTicks(9712));

            migrationBuilder.UpdateData(
                table: "Warehouses",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa60"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 1, 2, 5, 15, 614, DateTimeKind.Local).AddTicks(9665));
        }
    }
}
