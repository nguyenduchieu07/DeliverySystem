using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class AddContract : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SlotReservations_WarehouseSlots_WarehouseSlotId",
                table: "SlotReservations");

            migrationBuilder.DropForeignKey(
                name: "FK_SlotReservations_WarehouseSlots_WarehouseSlotId1",
                table: "SlotReservations");

            migrationBuilder.AlterColumn<Guid>(
                name: "WarehouseSlotId1",
                table: "SlotReservations",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "From",
                table: "SlotReservations",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "To",
                table: "SlotReservations",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "Contracts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuotationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StoreId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WarehouseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WarehouseSlotId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PdfUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    TermsAndConditions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contracts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Contracts_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Contracts_Quotations_QuotationId",
                        column: x => x.QuotationId,
                        principalTable: "Quotations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Contracts_Stores_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Stores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Contracts_WarehouseSlots_WarehouseSlotId",
                        column: x => x.WarehouseSlotId,
                        principalTable: "WarehouseSlots",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Contracts_Warehouses_WarehouseId",
                        column: x => x.WarehouseId,
                        principalTable: "Warehouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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
                name: "IX_Contracts_CustomerId",
                table: "Contracts",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_QuotationId",
                table: "Contracts",
                column: "QuotationId");

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_StoreId",
                table: "Contracts",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_WarehouseId",
                table: "Contracts",
                column: "WarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_WarehouseSlotId",
                table: "Contracts",
                column: "WarehouseSlotId");

            migrationBuilder.AddForeignKey(
                name: "FK_SlotReservations_WarehouseSlots_WarehouseSlotId",
                table: "SlotReservations",
                column: "WarehouseSlotId",
                principalTable: "WarehouseSlots",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SlotReservations_WarehouseSlots_WarehouseSlotId1",
                table: "SlotReservations",
                column: "WarehouseSlotId1",
                principalTable: "WarehouseSlots",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SlotReservations_WarehouseSlots_WarehouseSlotId",
                table: "SlotReservations");

            migrationBuilder.DropForeignKey(
                name: "FK_SlotReservations_WarehouseSlots_WarehouseSlotId1",
                table: "SlotReservations");

            migrationBuilder.DropTable(
                name: "Contracts");

            migrationBuilder.DropColumn(
                name: "From",
                table: "SlotReservations");

            migrationBuilder.DropColumn(
                name: "To",
                table: "SlotReservations");

            migrationBuilder.AlterColumn<Guid>(
                name: "WarehouseSlotId1",
                table: "SlotReservations",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.UpdateData(
                table: "Addresses",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa20"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 29, 14, 49, 40, 491, DateTimeKind.Local).AddTicks(6289));

            migrationBuilder.UpdateData(
                table: "Addresses",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa21"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 29, 14, 49, 40, 491, DateTimeKind.Local).AddTicks(6303));

            migrationBuilder.UpdateData(
                table: "Addresses",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa22"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 29, 14, 49, 40, 491, DateTimeKind.Local).AddTicks(6306));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000001"),
                column: "ConcurrencyStamp",
                value: "710408e1-b37c-44b4-9e27-e6f0aa6fb34f");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000002"),
                column: "ConcurrencyStamp",
                value: "eee8537f-b49f-4875-bb40-f4dab7dac010");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000003"),
                column: "ConcurrencyStamp",
                value: "a284334f-edfb-48eb-8a0d-340bf6d7646a");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000004"),
                column: "ConcurrencyStamp",
                value: "f41527ea-5e0b-42b3-b9bd-f80467a9a692");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222221"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEDU2Er80FuatevHHUHQMJlfXdbPJe1JBnUJYK/CY1Ki9qjsd8qx1F+9KDNU2E8fJpg==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEDU2Er80FuatevHHUHQMJlfXdbPJe1JBnUJYK/CY1Ki9qjsd8qx1F+9KDNU2E8fJpg==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222223"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEDU2Er80FuatevHHUHQMJlfXdbPJe1JBnUJYK/CY1Ki9qjsd8qx1F+9KDNU2E8fJpg==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000001"),
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "UpdatedAt" },
                values: new object[] { "ec149ba1-77dc-4b5c-ad61-b21620d2c995", new DateTime(2025, 10, 29, 7, 49, 40, 444, DateTimeKind.Utc).AddTicks(8982), "AQAAAAIAAYagAAAAEO1lOpQcLJfNMPnIU/D2joJXsIHy69ExhNceiJdzXRlOQVfR/3ig/oaXpgI2EcLqww==", new DateTime(2025, 10, 29, 7, 49, 40, 444, DateTimeKind.Utc).AddTicks(8987) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000002"),
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "UpdatedAt" },
                values: new object[] { "8f069b01-5633-485a-8c09-dcca8ab170e1", new DateTime(2025, 10, 29, 7, 49, 40, 396, DateTimeKind.Utc).AddTicks(6414), "AQAAAAIAAYagAAAAEFzFQ7gyQA/TFsTtczsr3Z53D/lOyYqb681RvT/6EBKeKEmYAmAxGFOxisguc/Vn3Q==", new DateTime(2025, 10, 29, 7, 49, 40, 396, DateTimeKind.Utc).AddTicks(6418) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa01"),
                column: "ConcurrencyStamp",
                value: "ff7feb2d-1ba3-439a-8d6f-4f4ffb763e41");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa02"),
                column: "ConcurrencyStamp",
                value: "498cca23-6092-4b64-84ce-5c4d2e8a7e4a");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 29, 14, 49, 40, 396, DateTimeKind.Local).AddTicks(6708));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 29, 14, 49, 40, 396, DateTimeKind.Local).AddTicks(6798));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 29, 14, 49, 40, 396, DateTimeKind.Local).AddTicks(6800));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000004"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 29, 14, 49, 40, 396, DateTimeKind.Local).AddTicks(6802));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa2-0000-0000-0000-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 29, 14, 49, 40, 396, DateTimeKind.Local).AddTicks(6725));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa2-0000-0000-0000-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 29, 14, 49, 40, 396, DateTimeKind.Local).AddTicks(6803));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa2-0000-0000-0000-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 29, 14, 49, 40, 396, DateTimeKind.Local).AddTicks(6805));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa3-0000-0000-0000-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 29, 14, 49, 40, 396, DateTimeKind.Local).AddTicks(6726));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa3-0000-0000-0000-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 29, 14, 49, 40, 396, DateTimeKind.Local).AddTicks(6806));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa3-0000-0000-0000-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 29, 14, 49, 40, 396, DateTimeKind.Local).AddTicks(6807));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa10"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 29, 14, 49, 40, 491, DateTimeKind.Local).AddTicks(6373));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa11"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 29, 14, 49, 40, 491, DateTimeKind.Local).AddTicks(6377));

            migrationBuilder.UpdateData(
                table: "KycDocuments",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc1"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 23, 7, 49, 40, 491, DateTimeKind.Utc).AddTicks(5879));

            migrationBuilder.UpdateData(
                table: "KycDocuments",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc2"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 23, 7, 49, 40, 491, DateTimeKind.Utc).AddTicks(5883));

            migrationBuilder.UpdateData(
                table: "KycDocuments",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc3"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 23, 7, 49, 40, 491, DateTimeKind.Utc).AddTicks(5886));

            migrationBuilder.UpdateData(
                table: "KycDocuments",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc4"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 24, 7, 49, 40, 491, DateTimeKind.Utc).AddTicks(5889));

            migrationBuilder.UpdateData(
                table: "KycDocuments",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc5"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 24, 7, 49, 40, 491, DateTimeKind.Utc).AddTicks(5891));

            migrationBuilder.UpdateData(
                table: "KycDocuments",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc6"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 14, 7, 49, 40, 491, DateTimeKind.Utc).AddTicks(5893));

            migrationBuilder.UpdateData(
                table: "KycDocuments",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc7"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 14, 7, 49, 40, 491, DateTimeKind.Utc).AddTicks(5895));

            migrationBuilder.UpdateData(
                table: "KycDocuments",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc8"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 14, 7, 49, 40, 491, DateTimeKind.Utc).AddTicks(5897));

            migrationBuilder.UpdateData(
                table: "KycSubmissions",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb1"),
                columns: new[] { "CreatedAt", "SubmittedAt" },
                values: new object[] { new DateTime(2025, 10, 23, 7, 49, 40, 491, DateTimeKind.Utc).AddTicks(5777), new DateTime(2025, 10, 23, 7, 49, 40, 491, DateTimeKind.Utc).AddTicks(5774) });

            migrationBuilder.UpdateData(
                table: "KycSubmissions",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb2"),
                columns: new[] { "CreatedAt", "ReviewedAt", "SubmittedAt" },
                values: new object[] { new DateTime(2025, 10, 24, 7, 49, 40, 491, DateTimeKind.Utc).AddTicks(5787), new DateTime(2025, 10, 25, 7, 49, 40, 491, DateTimeKind.Utc).AddTicks(5784), new DateTime(2025, 10, 24, 7, 49, 40, 491, DateTimeKind.Utc).AddTicks(5783) });

            migrationBuilder.UpdateData(
                table: "KycSubmissions",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb3"),
                columns: new[] { "CreatedAt", "ReviewedAt", "SubmittedAt" },
                values: new object[] { new DateTime(2025, 10, 14, 7, 49, 40, 491, DateTimeKind.Utc).AddTicks(5796), new DateTime(2025, 10, 15, 7, 49, 40, 491, DateTimeKind.Utc).AddTicks(5795), new DateTime(2025, 10, 14, 7, 49, 40, 491, DateTimeKind.Utc).AddTicks(5794) });

            migrationBuilder.UpdateData(
                table: "OrderItems",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa40"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 29, 14, 49, 40, 492, DateTimeKind.Local).AddTicks(1666));

            migrationBuilder.UpdateData(
                table: "OrderItems",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa41"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 29, 14, 49, 40, 492, DateTimeKind.Local).AddTicks(1678));

            migrationBuilder.UpdateData(
                table: "OrderItems",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa42"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 29, 14, 49, 40, 492, DateTimeKind.Local).AddTicks(1682));

            migrationBuilder.UpdateData(
                table: "OrderItems",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa43"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 29, 14, 49, 40, 492, DateTimeKind.Local).AddTicks(1694));

            migrationBuilder.UpdateData(
                table: "PriceRules",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa14"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 29, 14, 49, 40, 491, DateTimeKind.Local).AddTicks(6607));

            migrationBuilder.UpdateData(
                table: "Stores",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa1"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 29, 7, 49, 40, 491, DateTimeKind.Utc).AddTicks(5552), new DateTime(2025, 10, 29, 7, 49, 40, 491, DateTimeKind.Utc).AddTicks(5554) });

            migrationBuilder.UpdateData(
                table: "Stores",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa2"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 29, 7, 49, 40, 491, DateTimeKind.Utc).AddTicks(5652), new DateTime(2025, 10, 29, 7, 49, 40, 491, DateTimeKind.Utc).AddTicks(5652) });

            migrationBuilder.UpdateData(
                table: "Stores",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa3"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 29, 7, 49, 40, 491, DateTimeKind.Utc).AddTicks(5662), new DateTime(2025, 10, 29, 7, 49, 40, 491, DateTimeKind.Utc).AddTicks(5672) });

            migrationBuilder.UpdateData(
                table: "WarehouseSlots",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa61"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 29, 14, 49, 40, 492, DateTimeKind.Local).AddTicks(1867));

            migrationBuilder.UpdateData(
                table: "WarehouseSlots",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa62"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 29, 14, 49, 40, 492, DateTimeKind.Local).AddTicks(1872));

            migrationBuilder.UpdateData(
                table: "WarehouseSlots",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa63"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 29, 14, 49, 40, 492, DateTimeKind.Local).AddTicks(1874));

            migrationBuilder.UpdateData(
                table: "Warehouses",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa60"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 29, 14, 49, 40, 492, DateTimeKind.Local).AddTicks(1813));

            migrationBuilder.AddForeignKey(
                name: "FK_SlotReservations_WarehouseSlots_WarehouseSlotId",
                table: "SlotReservations",
                column: "WarehouseSlotId",
                principalTable: "WarehouseSlots",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SlotReservations_WarehouseSlots_WarehouseSlotId1",
                table: "SlotReservations",
                column: "WarehouseSlotId1",
                principalTable: "WarehouseSlots",
                principalColumn: "Id");
        }
    }
}
