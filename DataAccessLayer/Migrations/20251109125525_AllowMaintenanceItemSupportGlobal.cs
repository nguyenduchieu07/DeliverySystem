using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class AllowMaintenanceItemSupportGlobal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "StoreId",
                table: "MaintenanceItems",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceItems_StoreId",
                table: "MaintenanceItems",
                column: "StoreId");

            migrationBuilder.AddForeignKey(
                name: "FK_MaintenanceItems_Stores_StoreId",
                table: "MaintenanceItems",
                column: "StoreId",
                principalTable: "Stores",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MaintenanceItems_Stores_StoreId",
                table: "MaintenanceItems");

            migrationBuilder.DropIndex(
                name: "IX_MaintenanceItems_StoreId",
                table: "MaintenanceItems");

            migrationBuilder.DropColumn(
                name: "StoreId",
                table: "MaintenanceItems");
        }
    }
}
