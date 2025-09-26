using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class seeding_data_for_admin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ActiveRegions",
                table: "Stores",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "KycNote",
                table: "Stores",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "KycReviewedAt",
                table: "Stores",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "KycReviewedBy",
                table: "Stores",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "KycStatus",
                table: "Stores",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "KycSubmittedAt",
                table: "Stores",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MaxOrdersPerDay",
                table: "Stores",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000001"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "8d05f2a0-fecd-4a6a-b1af-173dcaf1bb74", "AQAAAAIAAYagAAAAEKgYq34yX7lavVvPfnxi3V6aWLIy/AecsSZ59nvU6XQzYHBCzQ0QEaGd0OMrKZ9Ctw==" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "CreatedAt", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "Status", "TwoFactorEnabled", "UpdatedAt", "UserName" },
                values: new object[] { new Guid("aaaaaaa1-0000-0000-0000-000000000002"), 0, "c9967212-78af-4eb5-b7cb-03c4c0a70bb0", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "SystemAdmin@gmail.com", true, false, null, "SystemAdmin@gmail.com", "SystemAdmin", "AQAAAAIAAYagAAAAEMB+s8sXLgkBX4Qe4X2AvY8iqoNKp2KpdtOfbD36JsI9aO3LcVavElDQb6t7q054rw==", null, false, null, "Active", false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "SystemAdmin" });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2025, 9, 26, 19, 6, 49, 721, DateTimeKind.Local).AddTicks(7954));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2025, 9, 26, 19, 6, 49, 721, DateTimeKind.Local).AddTicks(7986));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2025, 9, 26, 19, 6, 49, 721, DateTimeKind.Local).AddTicks(7991));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000004"),
                column: "CreatedAt",
                value: new DateTime(2025, 9, 26, 19, 6, 49, 721, DateTimeKind.Local).AddTicks(7993));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa2-0000-0000-0000-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2025, 9, 26, 19, 6, 49, 721, DateTimeKind.Local).AddTicks(7977));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa2-0000-0000-0000-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2025, 9, 26, 19, 6, 49, 721, DateTimeKind.Local).AddTicks(7996));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa2-0000-0000-0000-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2025, 9, 26, 19, 6, 49, 721, DateTimeKind.Local).AddTicks(7999));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa3-0000-0000-0000-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2025, 9, 26, 19, 6, 49, 721, DateTimeKind.Local).AddTicks(7983));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa3-0000-0000-0000-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2025, 9, 26, 19, 6, 49, 721, DateTimeKind.Local).AddTicks(8001));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa3-0000-0000-0000-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2025, 9, 26, 19, 6, 49, 721, DateTimeKind.Local).AddTicks(8004));

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { new Guid("aaaaaaa1-0000-0000-0000-000000000001"), new Guid("aaaaaaa1-0000-0000-0000-000000000002") });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("aaaaaaa1-0000-0000-0000-000000000001"), new Guid("aaaaaaa1-0000-0000-0000-000000000002") });

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000002"));

            migrationBuilder.DropColumn(
                name: "ActiveRegions",
                table: "Stores");

            migrationBuilder.DropColumn(
                name: "KycNote",
                table: "Stores");

            migrationBuilder.DropColumn(
                name: "KycReviewedAt",
                table: "Stores");

            migrationBuilder.DropColumn(
                name: "KycReviewedBy",
                table: "Stores");

            migrationBuilder.DropColumn(
                name: "KycStatus",
                table: "Stores");

            migrationBuilder.DropColumn(
                name: "KycSubmittedAt",
                table: "Stores");

            migrationBuilder.DropColumn(
                name: "MaxOrdersPerDay",
                table: "Stores");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000001"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "1c297548-3e0f-4417-a380-fe4da6cd691f", "AQAAAAIAAYagAAAAEHjiqHVtDzf62rjKykFI1Wncq+hyA/w7jY6h0UlujMGCz3SPgsXVZVfD+xocg2BwSw==" });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2025, 9, 21, 23, 23, 19, 512, DateTimeKind.Local).AddTicks(3987));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2025, 9, 21, 23, 23, 19, 512, DateTimeKind.Local).AddTicks(4017));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2025, 9, 21, 23, 23, 19, 512, DateTimeKind.Local).AddTicks(4020));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000004"),
                column: "CreatedAt",
                value: new DateTime(2025, 9, 21, 23, 23, 19, 512, DateTimeKind.Local).AddTicks(4022));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa2-0000-0000-0000-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2025, 9, 21, 23, 23, 19, 512, DateTimeKind.Local).AddTicks(4012));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa2-0000-0000-0000-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2025, 9, 21, 23, 23, 19, 512, DateTimeKind.Local).AddTicks(4024));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa2-0000-0000-0000-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2025, 9, 21, 23, 23, 19, 512, DateTimeKind.Local).AddTicks(4028));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa3-0000-0000-0000-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2025, 9, 21, 23, 23, 19, 512, DateTimeKind.Local).AddTicks(4015));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa3-0000-0000-0000-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2025, 9, 21, 23, 23, 19, 512, DateTimeKind.Local).AddTicks(4031));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa3-0000-0000-0000-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2025, 9, 21, 23, 23, 19, 512, DateTimeKind.Local).AddTicks(4033));
        }
    }
}
