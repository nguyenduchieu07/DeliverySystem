using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class AddSlotMaintenanceTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MaintenanceItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EstimatedDurationMinutes = table.Column<int>(type: "int", nullable: false),
                    RequireShutdown = table.Column<bool>(type: "bit", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaintenanceItems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WarehouseSlotMaintenances",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WarehouseSlotId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MaintenanceItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ScheduledStart = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ScheduledEnd = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ActualStart = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ActualEnd = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    AssignedStaffId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Result = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WarehouseSlotMaintenances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WarehouseSlotMaintenances_MaintenanceItems_MaintenanceItemId",
                        column: x => x.MaintenanceItemId,
                        principalTable: "MaintenanceItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WarehouseSlotMaintenances_WarehouseSlots_WarehouseSlotId",
                        column: x => x.WarehouseSlotId,
                        principalTable: "WarehouseSlots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseSlotMaintenances_MaintenanceItemId",
                table: "WarehouseSlotMaintenances",
                column: "MaintenanceItemId");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseSlotMaintenances_WarehouseSlotId",
                table: "WarehouseSlotMaintenances",
                column: "WarehouseSlotId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WarehouseSlotMaintenances");

            migrationBuilder.DropTable(
                name: "MaintenanceItems");
        }
    }
}
