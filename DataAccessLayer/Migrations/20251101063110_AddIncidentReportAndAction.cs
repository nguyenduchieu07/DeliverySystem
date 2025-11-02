using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class AddIncidentReportAndAction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "IncidentReports",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrderItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IncidentType = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    ReportedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsReturned = table.Column<bool>(type: "bit", nullable: false),
                    IsCompensated = table.Column<bool>(type: "bit", nullable: false),
                    CompensationAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IncidentReports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IncidentReports_OrderItems_OrderItemId",
                        column: x => x.OrderItemId,
                        principalTable: "OrderItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IncidentReports_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "IncidentActions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IncidentReportId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ActionType = table.Column<int>(type: "int", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ActionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StaffId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IncidentActions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IncidentActions_IncidentReports_IncidentReportId",
                        column: x => x.IncidentReportId,
                        principalTable: "IncidentReports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IncidentActions_StoreStaff_StaffId",
                        column: x => x.StaffId,
                        principalTable: "StoreStaff",
                        principalColumn: "Id");
                });

            migrationBuilder.UpdateData(
                table: "Addresses",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa20"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 1, 13, 31, 7, 751, DateTimeKind.Local).AddTicks(9381));

            migrationBuilder.UpdateData(
                table: "Addresses",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa21"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 1, 13, 31, 7, 751, DateTimeKind.Local).AddTicks(9392));

            migrationBuilder.UpdateData(
                table: "Addresses",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa22"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 1, 13, 31, 7, 751, DateTimeKind.Local).AddTicks(9395));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000001"),
                column: "ConcurrencyStamp",
                value: "3fac24b3-123d-42b2-b1b9-2d2a0e62b0f2");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000002"),
                column: "ConcurrencyStamp",
                value: "c0801755-e45d-43b4-b273-848f94855873");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000003"),
                column: "ConcurrencyStamp",
                value: "1de136ba-ab0d-4519-a4ca-38aa1c3cfda1");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000004"),
                column: "ConcurrencyStamp",
                value: "33d42138-23b0-4697-9079-6960f4a68fd4");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222221"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEGfdlLHsMeul0qcmh7tgdkif8ylEzh5pKk3Sc0FolKJ9ImnLKnuQ7vBDdYdwLh9SPw==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEGfdlLHsMeul0qcmh7tgdkif8ylEzh5pKk3Sc0FolKJ9ImnLKnuQ7vBDdYdwLh9SPw==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222223"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEGfdlLHsMeul0qcmh7tgdkif8ylEzh5pKk3Sc0FolKJ9ImnLKnuQ7vBDdYdwLh9SPw==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000001"),
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "UpdatedAt" },
                values: new object[] { "562fe4c2-aec1-4e02-88ff-e9029a7d2a42", new DateTime(2025, 11, 1, 6, 31, 7, 703, DateTimeKind.Utc).AddTicks(2334), "AQAAAAIAAYagAAAAEFFXuQzswsnf3YD44yf4qhEmaBkOJrXyWvWCY2uYrq8SWvF/hI25aEczRz8N/NNeeQ==", new DateTime(2025, 11, 1, 6, 31, 7, 703, DateTimeKind.Utc).AddTicks(2341) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000002"),
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "UpdatedAt" },
                values: new object[] { "86ea6603-7a3d-4e29-9684-7f9bac2ccdb1", new DateTime(2025, 11, 1, 6, 31, 7, 655, DateTimeKind.Utc).AddTicks(4719), "AQAAAAIAAYagAAAAEL3hwmi78d1KOnded5XxJBe2UzA/v0ghlOSAS4iBTf96nOBJQvjrUiMQkGI+hrzaVQ==", new DateTime(2025, 11, 1, 6, 31, 7, 655, DateTimeKind.Utc).AddTicks(4729) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa01"),
                column: "ConcurrencyStamp",
                value: "2abc6d56-05e9-4922-a052-ae2f8e8a9e29");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa02"),
                column: "ConcurrencyStamp",
                value: "0f53f258-3aab-480c-baca-14dc1c7e64a2");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 1, 13, 31, 7, 655, DateTimeKind.Local).AddTicks(5744));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 1, 13, 31, 7, 655, DateTimeKind.Local).AddTicks(5766));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 1, 13, 31, 7, 655, DateTimeKind.Local).AddTicks(5770));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000004"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 1, 13, 31, 7, 655, DateTimeKind.Local).AddTicks(5771));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa2-0000-0000-0000-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 1, 13, 31, 7, 655, DateTimeKind.Local).AddTicks(5762));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa2-0000-0000-0000-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 1, 13, 31, 7, 655, DateTimeKind.Local).AddTicks(5773));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa2-0000-0000-0000-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 1, 13, 31, 7, 655, DateTimeKind.Local).AddTicks(5774));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa3-0000-0000-0000-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 1, 13, 31, 7, 655, DateTimeKind.Local).AddTicks(5764));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa3-0000-0000-0000-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 1, 13, 31, 7, 655, DateTimeKind.Local).AddTicks(5776));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa3-0000-0000-0000-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 1, 13, 31, 7, 655, DateTimeKind.Local).AddTicks(5777));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa10"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 1, 13, 31, 7, 751, DateTimeKind.Local).AddTicks(9472));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa11"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 1, 13, 31, 7, 751, DateTimeKind.Local).AddTicks(9475));

            migrationBuilder.UpdateData(
                table: "KycDocuments",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc1"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 26, 6, 31, 7, 751, DateTimeKind.Utc).AddTicks(9220));

            migrationBuilder.UpdateData(
                table: "KycDocuments",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc2"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 26, 6, 31, 7, 751, DateTimeKind.Utc).AddTicks(9223));

            migrationBuilder.UpdateData(
                table: "KycDocuments",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc3"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 26, 6, 31, 7, 751, DateTimeKind.Utc).AddTicks(9226));

            migrationBuilder.UpdateData(
                table: "KycDocuments",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc4"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 27, 6, 31, 7, 751, DateTimeKind.Utc).AddTicks(9228));

            migrationBuilder.UpdateData(
                table: "KycDocuments",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc5"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 27, 6, 31, 7, 751, DateTimeKind.Utc).AddTicks(9230));

            migrationBuilder.UpdateData(
                table: "KycDocuments",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc6"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 17, 6, 31, 7, 751, DateTimeKind.Utc).AddTicks(9232));

            migrationBuilder.UpdateData(
                table: "KycDocuments",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc7"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 17, 6, 31, 7, 751, DateTimeKind.Utc).AddTicks(9233));

            migrationBuilder.UpdateData(
                table: "KycDocuments",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc8"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 17, 6, 31, 7, 751, DateTimeKind.Utc).AddTicks(9235));

            migrationBuilder.UpdateData(
                table: "KycSubmissions",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb1"),
                columns: new[] { "CreatedAt", "SubmittedAt" },
                values: new object[] { new DateTime(2025, 10, 26, 6, 31, 7, 751, DateTimeKind.Utc).AddTicks(9168), new DateTime(2025, 10, 26, 6, 31, 7, 751, DateTimeKind.Utc).AddTicks(9166) });

            migrationBuilder.UpdateData(
                table: "KycSubmissions",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb2"),
                columns: new[] { "CreatedAt", "ReviewedAt", "SubmittedAt" },
                values: new object[] { new DateTime(2025, 10, 27, 6, 31, 7, 751, DateTimeKind.Utc).AddTicks(9179), new DateTime(2025, 10, 28, 6, 31, 7, 751, DateTimeKind.Utc).AddTicks(9171), new DateTime(2025, 10, 27, 6, 31, 7, 751, DateTimeKind.Utc).AddTicks(9170) });

            migrationBuilder.UpdateData(
                table: "KycSubmissions",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb3"),
                columns: new[] { "CreatedAt", "ReviewedAt", "SubmittedAt" },
                values: new object[] { new DateTime(2025, 10, 17, 6, 31, 7, 751, DateTimeKind.Utc).AddTicks(9183), new DateTime(2025, 10, 18, 6, 31, 7, 751, DateTimeKind.Utc).AddTicks(9182), new DateTime(2025, 10, 17, 6, 31, 7, 751, DateTimeKind.Utc).AddTicks(9182) });

            migrationBuilder.UpdateData(
                table: "OrderItems",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa40"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 1, 13, 31, 7, 751, DateTimeKind.Local).AddTicks(9742));

            migrationBuilder.UpdateData(
                table: "OrderItems",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa41"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 1, 13, 31, 7, 751, DateTimeKind.Local).AddTicks(9749));

            migrationBuilder.UpdateData(
                table: "OrderItems",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa42"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 1, 13, 31, 7, 751, DateTimeKind.Local).AddTicks(9751));

            migrationBuilder.UpdateData(
                table: "OrderItems",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa43"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 1, 13, 31, 7, 751, DateTimeKind.Local).AddTicks(9753));

            migrationBuilder.UpdateData(
                table: "PriceRules",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa14"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 1, 13, 31, 7, 751, DateTimeKind.Local).AddTicks(9605));

            migrationBuilder.UpdateData(
                table: "Stores",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa1"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 1, 6, 31, 7, 751, DateTimeKind.Utc).AddTicks(9072), new DateTime(2025, 11, 1, 6, 31, 7, 751, DateTimeKind.Utc).AddTicks(9073) });

            migrationBuilder.UpdateData(
                table: "Stores",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa2"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 1, 6, 31, 7, 751, DateTimeKind.Utc).AddTicks(9079), new DateTime(2025, 11, 1, 6, 31, 7, 751, DateTimeKind.Utc).AddTicks(9080) });

            migrationBuilder.UpdateData(
                table: "Stores",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa3"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 2, 6, 31, 7, 751, DateTimeKind.Utc).AddTicks(9084), new DateTime(2025, 11, 1, 6, 31, 7, 751, DateTimeKind.Utc).AddTicks(9089) });

            migrationBuilder.UpdateData(
                table: "WarehouseSlots",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa61"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 1, 13, 31, 7, 751, DateTimeKind.Local).AddTicks(9896));

            migrationBuilder.UpdateData(
                table: "WarehouseSlots",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa62"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 1, 13, 31, 7, 751, DateTimeKind.Local).AddTicks(9901));

            migrationBuilder.UpdateData(
                table: "WarehouseSlots",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa63"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 1, 13, 31, 7, 751, DateTimeKind.Local).AddTicks(9923));

            migrationBuilder.UpdateData(
                table: "Warehouses",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa60"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 1, 13, 31, 7, 751, DateTimeKind.Local).AddTicks(9856));

            migrationBuilder.CreateIndex(
                name: "IX_IncidentActions_IncidentReportId",
                table: "IncidentActions",
                column: "IncidentReportId");

            migrationBuilder.CreateIndex(
                name: "IX_IncidentActions_StaffId",
                table: "IncidentActions",
                column: "StaffId");

            migrationBuilder.CreateIndex(
                name: "IX_IncidentReports_OrderId",
                table: "IncidentReports",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_IncidentReports_OrderItemId",
                table: "IncidentReports",
                column: "OrderItemId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IncidentActions");

            migrationBuilder.DropTable(
                name: "IncidentReports");

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
