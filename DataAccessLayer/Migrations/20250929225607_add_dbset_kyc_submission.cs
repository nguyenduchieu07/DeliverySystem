using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class add_dbset_kyc_submission : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_KycDocument_KycSubmission_KycSubmissionId",
                table: "KycDocument");

            migrationBuilder.DropForeignKey(
                name: "FK_KycSubmission_Stores_StoreId",
                table: "KycSubmission");

            migrationBuilder.DropPrimaryKey(
                name: "PK_KycSubmission",
                table: "KycSubmission");

            migrationBuilder.DropPrimaryKey(
                name: "PK_KycDocument",
                table: "KycDocument");

            migrationBuilder.RenameTable(
                name: "KycSubmission",
                newName: "KycSubmissions");

            migrationBuilder.RenameTable(
                name: "KycDocument",
                newName: "KycDocuments");

            migrationBuilder.RenameIndex(
                name: "IX_KycSubmission_StoreId",
                table: "KycSubmissions",
                newName: "IX_KycSubmissions_StoreId");

            migrationBuilder.RenameIndex(
                name: "IX_KycSubmission_Status_SubmittedAt",
                table: "KycSubmissions",
                newName: "IX_KycSubmissions_Status_SubmittedAt");

            migrationBuilder.RenameIndex(
                name: "IX_KycDocument_KycSubmissionId",
                table: "KycDocuments",
                newName: "IX_KycDocuments_KycSubmissionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_KycSubmissions",
                table: "KycSubmissions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_KycDocuments",
                table: "KycDocuments",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222221"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAECLj3gMAxv/qFUFuzzN4JNlEO5gsFWhvZNJC2NQSg5AnoC/uJfbVVUuQcPDM0Ebm3w==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAECLj3gMAxv/qFUFuzzN4JNlEO5gsFWhvZNJC2NQSg5AnoC/uJfbVVUuQcPDM0Ebm3w==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222223"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAECLj3gMAxv/qFUFuzzN4JNlEO5gsFWhvZNJC2NQSg5AnoC/uJfbVVUuQcPDM0Ebm3w==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000001"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "816ee09f-f684-46d2-aa0f-6689dfa6c2f8", "AQAAAAIAAYagAAAAEPPz/3MePLWkiaOsteublwmcLK48ZMgxEu7dkrws49mvYwclBYE2DZFGvR604/+Akw==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000002"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "129f014a-c51a-4dc8-bc70-09cdaf2f0a31", "AQAAAAIAAYagAAAAEEpOnhPQSOUesD9OEtDMhiYuJynRQc6Q4/bYS8B7Bya72SQ0GfQMqOqQmW7f0Bqfeg==" });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2025, 9, 30, 5, 56, 4, 358, DateTimeKind.Local).AddTicks(4794));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2025, 9, 30, 5, 56, 4, 358, DateTimeKind.Local).AddTicks(4815));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2025, 9, 30, 5, 56, 4, 358, DateTimeKind.Local).AddTicks(4818));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-0000-0000-0000-000000000004"),
                column: "CreatedAt",
                value: new DateTime(2025, 9, 30, 5, 56, 4, 358, DateTimeKind.Local).AddTicks(4845));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa2-0000-0000-0000-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2025, 9, 30, 5, 56, 4, 358, DateTimeKind.Local).AddTicks(4811));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa2-0000-0000-0000-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2025, 9, 30, 5, 56, 4, 358, DateTimeKind.Local).AddTicks(4847));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa2-0000-0000-0000-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2025, 9, 30, 5, 56, 4, 358, DateTimeKind.Local).AddTicks(4849));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa3-0000-0000-0000-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2025, 9, 30, 5, 56, 4, 358, DateTimeKind.Local).AddTicks(4814));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa3-0000-0000-0000-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2025, 9, 30, 5, 56, 4, 358, DateTimeKind.Local).AddTicks(4852));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa3-0000-0000-0000-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2025, 9, 30, 5, 56, 4, 358, DateTimeKind.Local).AddTicks(4854));

            migrationBuilder.UpdateData(
                table: "KycDocuments",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc1"),
                column: "CreatedAt",
                value: new DateTime(2025, 9, 23, 22, 56, 4, 525, DateTimeKind.Utc).AddTicks(6192));

            migrationBuilder.UpdateData(
                table: "KycDocuments",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc2"),
                column: "CreatedAt",
                value: new DateTime(2025, 9, 23, 22, 56, 4, 525, DateTimeKind.Utc).AddTicks(6195));

            migrationBuilder.UpdateData(
                table: "KycDocuments",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc3"),
                column: "CreatedAt",
                value: new DateTime(2025, 9, 23, 22, 56, 4, 525, DateTimeKind.Utc).AddTicks(6198));

            migrationBuilder.UpdateData(
                table: "KycDocuments",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc4"),
                column: "CreatedAt",
                value: new DateTime(2025, 9, 24, 22, 56, 4, 525, DateTimeKind.Utc).AddTicks(6208));

            migrationBuilder.UpdateData(
                table: "KycDocuments",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc5"),
                column: "CreatedAt",
                value: new DateTime(2025, 9, 24, 22, 56, 4, 525, DateTimeKind.Utc).AddTicks(6210));

            migrationBuilder.UpdateData(
                table: "KycDocuments",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc6"),
                column: "CreatedAt",
                value: new DateTime(2025, 9, 14, 22, 56, 4, 525, DateTimeKind.Utc).AddTicks(6213));

            migrationBuilder.UpdateData(
                table: "KycDocuments",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc7"),
                column: "CreatedAt",
                value: new DateTime(2025, 9, 14, 22, 56, 4, 525, DateTimeKind.Utc).AddTicks(6215));

            migrationBuilder.UpdateData(
                table: "KycDocuments",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc8"),
                column: "CreatedAt",
                value: new DateTime(2025, 9, 14, 22, 56, 4, 525, DateTimeKind.Utc).AddTicks(6218));

            migrationBuilder.UpdateData(
                table: "KycSubmissions",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb1"),
                columns: new[] { "CreatedAt", "SubmittedAt" },
                values: new object[] { new DateTime(2025, 9, 23, 22, 56, 4, 525, DateTimeKind.Utc).AddTicks(4669), new DateTime(2025, 9, 23, 22, 56, 4, 525, DateTimeKind.Utc).AddTicks(4668) });

            migrationBuilder.UpdateData(
                table: "KycSubmissions",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb2"),
                columns: new[] { "CreatedAt", "ReviewedAt", "SubmittedAt" },
                values: new object[] { new DateTime(2025, 9, 24, 22, 56, 4, 525, DateTimeKind.Utc).AddTicks(4679), new DateTime(2025, 9, 25, 22, 56, 4, 525, DateTimeKind.Utc).AddTicks(4673), new DateTime(2025, 9, 24, 22, 56, 4, 525, DateTimeKind.Utc).AddTicks(4673) });

            migrationBuilder.UpdateData(
                table: "KycSubmissions",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb3"),
                columns: new[] { "CreatedAt", "ReviewedAt", "SubmittedAt" },
                values: new object[] { new DateTime(2025, 9, 14, 22, 56, 4, 525, DateTimeKind.Utc).AddTicks(6030), new DateTime(2025, 9, 15, 22, 56, 4, 525, DateTimeKind.Utc).AddTicks(6020), new DateTime(2025, 9, 14, 22, 56, 4, 525, DateTimeKind.Utc).AddTicks(6016) });

            migrationBuilder.UpdateData(
                table: "Stores",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa1"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 29, 22, 56, 4, 525, DateTimeKind.Utc).AddTicks(4542), new DateTime(2025, 9, 29, 22, 56, 4, 525, DateTimeKind.Utc).AddTicks(4542) });

            migrationBuilder.UpdateData(
                table: "Stores",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa2"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 29, 22, 56, 4, 525, DateTimeKind.Utc).AddTicks(4546), new DateTime(2025, 9, 29, 22, 56, 4, 525, DateTimeKind.Utc).AddTicks(4546) });

            migrationBuilder.UpdateData(
                table: "Stores",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa3"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 8, 30, 22, 56, 4, 525, DateTimeKind.Utc).AddTicks(4550), new DateTime(2025, 9, 29, 22, 56, 4, 525, DateTimeKind.Utc).AddTicks(4556) });

            migrationBuilder.AddForeignKey(
                name: "FK_KycDocuments_KycSubmissions_KycSubmissionId",
                table: "KycDocuments",
                column: "KycSubmissionId",
                principalTable: "KycSubmissions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_KycSubmissions_Stores_StoreId",
                table: "KycSubmissions",
                column: "StoreId",
                principalTable: "Stores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_KycDocuments_KycSubmissions_KycSubmissionId",
                table: "KycDocuments");

            migrationBuilder.DropForeignKey(
                name: "FK_KycSubmissions_Stores_StoreId",
                table: "KycSubmissions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_KycSubmissions",
                table: "KycSubmissions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_KycDocuments",
                table: "KycDocuments");

            migrationBuilder.RenameTable(
                name: "KycSubmissions",
                newName: "KycSubmission");

            migrationBuilder.RenameTable(
                name: "KycDocuments",
                newName: "KycDocument");

            migrationBuilder.RenameIndex(
                name: "IX_KycSubmissions_StoreId",
                table: "KycSubmission",
                newName: "IX_KycSubmission_StoreId");

            migrationBuilder.RenameIndex(
                name: "IX_KycSubmissions_Status_SubmittedAt",
                table: "KycSubmission",
                newName: "IX_KycSubmission_Status_SubmittedAt");

            migrationBuilder.RenameIndex(
                name: "IX_KycDocuments_KycSubmissionId",
                table: "KycDocument",
                newName: "IX_KycDocument_KycSubmissionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_KycSubmission",
                table: "KycSubmission",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_KycDocument",
                table: "KycDocument",
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

            migrationBuilder.AddForeignKey(
                name: "FK_KycDocument_KycSubmission_KycSubmissionId",
                table: "KycDocument",
                column: "KycSubmissionId",
                principalTable: "KycSubmission",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_KycSubmission_Stores_StoreId",
                table: "KycSubmission",
                column: "StoreId",
                principalTable: "Stores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
