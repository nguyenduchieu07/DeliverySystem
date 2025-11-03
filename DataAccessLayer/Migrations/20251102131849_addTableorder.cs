using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class addTableorder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Addresses",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa20"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 2, 20, 18, 45, 81, DateTimeKind.Local).AddTicks(4301));

            migrationBuilder.UpdateData(
                table: "Addresses",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa21"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 2, 20, 18, 45, 81, DateTimeKind.Local).AddTicks(4327));

            migrationBuilder.UpdateData(
                table: "Addresses",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa22"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 2, 20, 18, 45, 81, DateTimeKind.Local).AddTicks(4331));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000001"),
                column: "ConcurrencyStamp",
                value: "c8e2ee45-4bbf-4fae-9cce-2c5e99bee101");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000002"),
                column: "ConcurrencyStamp",
                value: "af90eaf0-fc49-4d58-834b-536510bf4fc0");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000003"),
                column: "ConcurrencyStamp",
                value: "4f0134ad-4074-4f97-a132-33f4ec50c9a3");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000004"),
                column: "ConcurrencyStamp",
                value: "654efa26-4cbc-4622-ad42-4e79bf07750b");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222221"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEOX4tHE88Y1U29oA4Q6/3gl8oS3n2CXhNzQcIYwXhE1RNitl+HXTQ/q3EslQI8zaVw==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEOX4tHE88Y1U29oA4Q6/3gl8oS3n2CXhNzQcIYwXhE1RNitl+HXTQ/q3EslQI8zaVw==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222223"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEOX4tHE88Y1U29oA4Q6/3gl8oS3n2CXhNzQcIYwXhE1RNitl+HXTQ/q3EslQI8zaVw==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000001"),
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "UpdatedAt" },
                values: new object[] { "a330b1a0-d6ed-451f-904b-e84c02706635", new DateTime(2025, 11, 2, 13, 18, 45, 26, DateTimeKind.Utc).AddTicks(9317), "AQAAAAIAAYagAAAAEJckeHmZDk3THsVguR6FE1ai3pUKD3MLcGwe9DfFslA2A2980LV6jYOE0feus5/J1g==", new DateTime(2025, 11, 2, 13, 18, 45, 26, DateTimeKind.Utc).AddTicks(9333) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000002"),
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "UpdatedAt" },
                values: new object[] { "1bf72b5e-c77b-46fb-bf01-9a0553d80e3d", new DateTime(2025, 11, 2, 13, 18, 44, 979, DateTimeKind.Utc).AddTicks(7846), "AQAAAAIAAYagAAAAEETG35RY4uD8pgA2BiPELOFrW3OCdKSuBPugH6QMq8/Z4j8c7SxE3i4QHxiLOskKUw==", new DateTime(2025, 11, 2, 13, 18, 44, 979, DateTimeKind.Utc).AddTicks(7861) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa01"),
                column: "ConcurrencyStamp",
                value: "e595c10f-96b2-461e-9ba9-74380754a017");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa02"),
                column: "ConcurrencyStamp",
                value: "e40eaa53-01bb-49b4-b617-19338e97e1dc");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 2, 20, 18, 44, 979, DateTimeKind.Local).AddTicks(8657));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 2, 20, 18, 44, 979, DateTimeKind.Local).AddTicks(8690));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 2, 20, 18, 44, 979, DateTimeKind.Local).AddTicks(8695));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000004"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 2, 20, 18, 44, 979, DateTimeKind.Local).AddTicks(8697));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa2-0000-0000-0000-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 2, 20, 18, 44, 979, DateTimeKind.Local).AddTicks(8686));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa2-0000-0000-0000-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 2, 20, 18, 44, 979, DateTimeKind.Local).AddTicks(8698));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa2-0000-0000-0000-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 2, 20, 18, 44, 979, DateTimeKind.Local).AddTicks(8700));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa3-0000-0000-0000-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 2, 20, 18, 44, 979, DateTimeKind.Local).AddTicks(8688));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa3-0000-0000-0000-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 2, 20, 18, 44, 979, DateTimeKind.Local).AddTicks(8702));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa3-0000-0000-0000-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 2, 20, 18, 44, 979, DateTimeKind.Local).AddTicks(8704));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa10"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 2, 20, 18, 45, 81, DateTimeKind.Local).AddTicks(4599));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa11"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 2, 20, 18, 45, 81, DateTimeKind.Local).AddTicks(4602));

            migrationBuilder.UpdateData(
                table: "KycDocuments",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc1"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 27, 13, 18, 45, 81, DateTimeKind.Utc).AddTicks(554));

            migrationBuilder.UpdateData(
                table: "KycDocuments",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc2"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 27, 13, 18, 45, 81, DateTimeKind.Utc).AddTicks(557));

            migrationBuilder.UpdateData(
                table: "KycDocuments",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc3"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 27, 13, 18, 45, 81, DateTimeKind.Utc).AddTicks(560));

            migrationBuilder.UpdateData(
                table: "KycDocuments",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc4"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 28, 13, 18, 45, 81, DateTimeKind.Utc).AddTicks(564));

            migrationBuilder.UpdateData(
                table: "KycDocuments",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc5"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 28, 13, 18, 45, 81, DateTimeKind.Utc).AddTicks(567));

            migrationBuilder.UpdateData(
                table: "KycDocuments",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc6"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 18, 13, 18, 45, 81, DateTimeKind.Utc).AddTicks(569));

            migrationBuilder.UpdateData(
                table: "KycDocuments",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc7"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 18, 13, 18, 45, 81, DateTimeKind.Utc).AddTicks(572));

            migrationBuilder.UpdateData(
                table: "KycDocuments",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc8"),
                column: "CreatedAt",
                value: new DateTime(2025, 10, 18, 13, 18, 45, 81, DateTimeKind.Utc).AddTicks(576));

            migrationBuilder.UpdateData(
                table: "KycSubmissions",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb1"),
                columns: new[] { "CreatedAt", "SubmittedAt" },
                values: new object[] { new DateTime(2025, 10, 27, 13, 18, 45, 81, DateTimeKind.Utc).AddTicks(323), new DateTime(2025, 10, 27, 13, 18, 45, 81, DateTimeKind.Utc).AddTicks(322) });

            migrationBuilder.UpdateData(
                table: "KycSubmissions",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb2"),
                columns: new[] { "CreatedAt", "ReviewedAt", "SubmittedAt" },
                values: new object[] { new DateTime(2025, 10, 28, 13, 18, 45, 81, DateTimeKind.Utc).AddTicks(331), new DateTime(2025, 10, 29, 13, 18, 45, 81, DateTimeKind.Utc).AddTicks(327), new DateTime(2025, 10, 28, 13, 18, 45, 81, DateTimeKind.Utc).AddTicks(327) });

            migrationBuilder.UpdateData(
                table: "KycSubmissions",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb3"),
                columns: new[] { "CreatedAt", "ReviewedAt", "SubmittedAt" },
                values: new object[] { new DateTime(2025, 10, 18, 13, 18, 45, 81, DateTimeKind.Utc).AddTicks(346), new DateTime(2025, 10, 19, 13, 18, 45, 81, DateTimeKind.Utc).AddTicks(344), new DateTime(2025, 10, 18, 13, 18, 45, 81, DateTimeKind.Utc).AddTicks(344) });

            migrationBuilder.UpdateData(
                table: "OrderItems",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa40"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 2, 20, 18, 45, 81, DateTimeKind.Local).AddTicks(5000));

            migrationBuilder.UpdateData(
                table: "OrderItems",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa41"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 2, 20, 18, 45, 81, DateTimeKind.Local).AddTicks(5006));

            migrationBuilder.UpdateData(
                table: "OrderItems",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa42"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 2, 20, 18, 45, 81, DateTimeKind.Local).AddTicks(5018));

            migrationBuilder.UpdateData(
                table: "OrderItems",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa43"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 2, 20, 18, 45, 81, DateTimeKind.Local).AddTicks(5051));

            migrationBuilder.UpdateData(
                table: "PriceRules",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa14"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 2, 20, 18, 45, 81, DateTimeKind.Local).AddTicks(4727));

            migrationBuilder.UpdateData(
                table: "Stores",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa1"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 2, 13, 18, 45, 81, DateTimeKind.Utc).AddTicks(226), new DateTime(2025, 11, 2, 13, 18, 45, 81, DateTimeKind.Utc).AddTicks(228) });

            migrationBuilder.UpdateData(
                table: "Stores",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa2"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 2, 13, 18, 45, 81, DateTimeKind.Utc).AddTicks(241), new DateTime(2025, 11, 2, 13, 18, 45, 81, DateTimeKind.Utc).AddTicks(242) });

            migrationBuilder.UpdateData(
                table: "Stores",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa3"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 10, 3, 13, 18, 45, 81, DateTimeKind.Utc).AddTicks(248), new DateTime(2025, 11, 2, 13, 18, 45, 81, DateTimeKind.Utc).AddTicks(258) });

            migrationBuilder.UpdateData(
                table: "WarehouseSlots",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa61"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 2, 20, 18, 45, 81, DateTimeKind.Local).AddTicks(5182));

            migrationBuilder.UpdateData(
                table: "WarehouseSlots",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa62"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 2, 20, 18, 45, 81, DateTimeKind.Local).AddTicks(5187));

            migrationBuilder.UpdateData(
                table: "WarehouseSlots",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa63"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 2, 20, 18, 45, 81, DateTimeKind.Local).AddTicks(5189));

            migrationBuilder.UpdateData(
                table: "Warehouses",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa60"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 2, 20, 18, 45, 81, DateTimeKind.Local).AddTicks(5144));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
        }
    }
}
