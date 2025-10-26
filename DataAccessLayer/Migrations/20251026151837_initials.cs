using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class initials : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Wallets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    OwnerType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    OwnerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false, defaultValue: "VND"),
                    Balance = table.Column<decimal>(type: "decimal(14,2)", nullable: false),
                    Status = table.Column<int>(type: "int", maxLength: 20, nullable: false, defaultValue: 8),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Wallets__84D4F90EC8015006", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PreferredLang = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Tier = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true, defaultValue: "Basic"),
                    KycLevel = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true, defaultValue: "None"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Customer__A4AE64D8B858128B", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Customers_Users",
                        column: x => x.Id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Stores",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    OwnerUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StoreName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    LegalName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    LicenseNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TaxNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Status = table.Column<int>(type: "int", maxLength: 20, nullable: false, defaultValue: 0),
                    KycLevel = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true, defaultValue: "None"),
                    RatingAvg = table.Column<decimal>(type: "decimal(3,2)", nullable: false),
                    RatingCount = table.Column<int>(type: "int", nullable: false),
                    ContactPhone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContactEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LicenseExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsVerified = table.Column<bool>(type: "bit", nullable: false),
                    MaxOrdersPerDay = table.Column<int>(type: "int", nullable: true),
                    ActiveRegions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ServiceTypes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Latitude = table.Column<double>(type: "float", nullable: true),
                    Longitude = table.Column<double>(type: "float", nullable: true),
                    BankAccountNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BankName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(sysdatetime())"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "(sysdatetime())"),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Stores__3B82F101588E2030", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Stores_Users",
                        column: x => x.OwnerUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    StoreId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Label = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    AddressLine = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Ward = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: true),
                    District = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: true),
                    City = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: true),
                    Latitude = table.Column<double>(type: "float", nullable: true),
                    Longitude = table.Column<double>(type: "float", nullable: true),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Addresse__091C2AFB08EA6C9D", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Addresses_Stores",
                        column: x => x.StoreId,
                        principalTable: "Stores",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Addresses_Users",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StoreId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ParentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Icon = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ThumbnailUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    SortOrder = table.Column<int>(type: "int", nullable: true),
                    Path = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Level = table.Column<int>(type: "int", nullable: false),
                    IsLeaf = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                    table.CheckConstraint("CK_Category_Parent_Not_Self", "[ParentId] IS NULL OR [ParentId] <> [Id]");
                    table.ForeignKey(
                        name: "FK_Categories_Categories_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Categories_Stores_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Stores",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "KycSubmissions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StoreId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<int>(type: "int", maxLength: 20, nullable: false),
                    AdminNote = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SubmittedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReviewedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReviewedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KycSubmissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KycSubmissions_Stores_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Stores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Quotations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    StoreId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    ValidUntil = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", maxLength: 20, nullable: false, defaultValue: 20),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(sysdatetime())"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Quotatio__E1975293941AD414", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Quotations_Customers",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Quotations_Stores",
                        column: x => x.StoreId,
                        principalTable: "Stores",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "StoreStaff",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    StoreId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Status = table.Column<int>(type: "int", maxLength: 20, nullable: false, defaultValue: 0),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__StoreSta__96D4AB1701A0B50F", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StoreStaff_Stores",
                        column: x => x.StoreId,
                        principalTable: "Stores",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StoreStaff_Users",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Warehouses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StoreId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AddressRefId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CoverImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MapImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    HeightM = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    LengthM = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    WidthM = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Warehouses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Warehouses_Addresses_AddressRefId",
                        column: x => x.AddressRefId,
                        principalTable: "Addresses",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Warehouses_Stores_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Stores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Services",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    StoreId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Unit = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false, defaultValue: "Job"),
                    BasePrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PricingModel = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(sysdatetime())"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Services__C51BB00A8BC5FA65", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Services_Categories",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Services_Stores",
                        column: x => x.StoreId,
                        principalTable: "Stores",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "KycDocuments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    KycSubmissionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DocType = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    Hash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KycDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KycDocuments_KycSubmissions_KycSubmissionId",
                        column: x => x.KycSubmissionId,
                        principalTable: "KycSubmissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StoreId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuotationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PickupAddressId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DropoffAddressId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeliveryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PickupDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DistanceKm = table.Column<decimal>(type: "decimal(8,2)", nullable: true),
                    EtaMinutes = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<int>(type: "int", maxLength: 30, nullable: false, defaultValue: 19),
                    TotalAmount = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductCategoryIds = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(sysdatetime())"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "(sysdatetime())"),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Orders__C3905BCF19CCE46F", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_Customers",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Orders_Dropoff",
                        column: x => x.DropoffAddressId,
                        principalTable: "Addresses",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Orders_Pickup",
                        column: x => x.PickupAddressId,
                        principalTable: "Addresses",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Orders_Quotations",
                        column: x => x.QuotationId,
                        principalTable: "Quotations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Orders_Stores",
                        column: x => x.StoreId,
                        principalTable: "Stores",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "WarehouseSlots",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WarehouseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Size = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CurrentOrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Row = table.Column<int>(type: "int", nullable: false),
                    Col = table.Column<int>(type: "int", nullable: false),
                    HeightM = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    LengthM = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    WidthM = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    LeaseStart = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LeaseEnd = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsBlocked = table.Column<bool>(type: "bit", nullable: false),
                    BasePricePerHour = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WarehouseSlots", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WarehouseSlots_Warehouses_WarehouseId",
                        column: x => x.WarehouseId,
                        principalTable: "Warehouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PriceRules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ServiceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ValidFrom = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ValidTo = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MinVolumeM3 = table.Column<decimal>(type: "decimal(18,4)", nullable: true),
                    MaxVolumeM3 = table.Column<decimal>(type: "decimal(18,4)", nullable: true),
                    MinAreaM2 = table.Column<decimal>(type: "decimal(18,4)", nullable: true),
                    MaxAreaM2 = table.Column<decimal>(type: "decimal(18,4)", nullable: true),
                    MinQty = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    MaxQty = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    MinDays = table.Column<int>(type: "int", nullable: true),
                    MaxDays = table.Column<int>(type: "int", nullable: true),
                    TimeUnit = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ApplyModel = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PriceRules", x => x.Id);
                    table.CheckConstraint("CK_ServicePriceRule_Ranges", "\r\n            (MinVolumeM3 IS NULL OR MaxVolumeM3 IS NULL OR MinVolumeM3 <= MaxVolumeM3) AND\r\n            (MinAreaM2  IS NULL OR MaxAreaM2  IS NULL OR MinAreaM2  <= MaxAreaM2)  AND\r\n            (MinQty     IS NULL OR MaxQty     IS NULL OR MinQty     <= MaxQty)     AND\r\n            (MinDays    IS NULL OR MaxDays    IS NULL OR MinDays    <= MaxDays)\r\n        ");
                    table.ForeignKey(
                        name: "FK_PriceRules_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ServiceAddons",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ServiceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsPercentage = table.Column<bool>(type: "bit", nullable: false),
                    Value = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceAddons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServiceAddons_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ServiceSizeOptions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ServiceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    VolumeM3 = table.Column<decimal>(type: "decimal(18,4)", nullable: true),
                    AreaM2 = table.Column<decimal>(type: "decimal(18,4)", nullable: true),
                    MaxWeightKg = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PriceOverride = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceSizeOptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServiceSizeOptions_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Feedbacks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    OrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FromUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ToStoreId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(sysdatetime())"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Feedback__6A4BEDD68F15C3E7", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Feedbacks_Orders",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Feedbacks_Stores",
                        column: x => x.ToStoreId,
                        principalTable: "Stores",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Feedbacks_Users",
                        column: x => x.FromUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "OrderItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ServiceId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ItemName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    LengthM = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: true),
                    WidthM = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: true),
                    HeightM = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: true),
                    WeightKg = table.Column<decimal>(type: "decimal(6,2)", precision: 6, scale: 2, nullable: true),
                    SizeCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    Subtotal = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderItems_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderItems_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    OrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    Method = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Provider = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: true),
                    ProviderTxnId = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: true),
                    Status = table.Column<int>(type: "int", maxLength: 20, nullable: false, defaultValue: 0),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(sysdatetime())"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Payments__9B556A3861C7126E", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Payments_Orders",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "WalletTransactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    WalletId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(14,2)", nullable: false),
                    OrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ProviderTxnId = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: true),
                    Status = table.Column<int>(type: "int", maxLength: 20, nullable: false, defaultValue: 21),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(sysdatetime())"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__WalletTr__C19608541DF87B9D", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WalletTransactions_Orders",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_WalletTransactions_Wallets",
                        column: x => x.WalletId,
                        principalTable: "Wallets",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SlotReservations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WarehouseSlotId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ExpiresAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SlotReservations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SlotReservations_WarehouseSlots_WarehouseSlotId",
                        column: x => x.WarehouseSlotId,
                        principalTable: "WarehouseSlots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("aaaaaaa1-0000-0000-0000-000000000001"), null, "Admin", "ADMIN" },
                    { new Guid("aaaaaaa1-0000-0000-0000-000000000002"), null, "Store", "STORE" },
                    { new Guid("aaaaaaa1-0000-0000-0000-000000000003"), null, "StoreStaff", "StoreStaff" },
                    { new Guid("aaaaaaa1-0000-0000-0000-000000000004"), null, "Customer", "CUSTOMER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "CreatedAt", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "Status", "TwoFactorEnabled", "UpdatedAt", "UserName" },
                values: new object[,]
                {
                    { new Guid("22222222-2222-2222-2222-222222222221"), 0, "con-blue", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "owner.blue@demo.local", true, false, null, "OWNER.BLUE@DEMO.LOCAL", "BLUEOWNER", "AQAAAAIAAYagAAAAEAr7uCKsxqq935wf8PdPVAntZKMFQCNtE3IeYgblodpHArCNh7H751/0VxZZPo8eBw==", null, false, "sec-blue", 8, false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "blueowner" },
                    { new Guid("22222222-2222-2222-2222-222222222222"), 0, "con-fresh", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "owner.fresh@demo.local", true, false, null, "OWNER.FRESH@DEMO.LOCAL", "FRESHOWNER", "AQAAAAIAAYagAAAAEAr7uCKsxqq935wf8PdPVAntZKMFQCNtE3IeYgblodpHArCNh7H751/0VxZZPo8eBw==", null, false, "sec-fresh", 8, false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "freshowner" },
                    { new Guid("22222222-2222-2222-2222-222222222223"), 0, "con-prime", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "owner.prime@demo.local", true, false, null, "OWNER.PRIME@DEMO.LOCAL", "PRIMEOWNER", "AQAAAAIAAYagAAAAEAr7uCKsxqq935wf8PdPVAntZKMFQCNtE3IeYgblodpHArCNh7H751/0VxZZPo8eBw==", null, false, "sec-prime", 8, false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "primeowner" },
                    { new Guid("aaaaaaa1-0000-0000-0000-000000000001"), 0, "eb2b2165-4efb-4930-8755-aa058176e0c2", new DateTime(2025, 10, 26, 15, 18, 31, 717, DateTimeKind.Utc).AddTicks(7304), "store1@gmail.com", true, false, null, "store1@gmail.com", "store1", "AQAAAAIAAYagAAAAELiA6LchK/2P0bGnL61zupux89lsGOxB8eUZ7JHYCogQYvnUBotYq9HqNLYQt+qTug==", null, false, null, 8, false, new DateTime(2025, 10, 26, 15, 18, 31, 717, DateTimeKind.Utc).AddTicks(7314), "store1" },
                    { new Guid("aaaaaaa1-0000-0000-0000-000000000002"), 0, "5bb169ac-3b83-4e10-9c56-d2bce5f0736a", new DateTime(2025, 10, 26, 15, 18, 31, 803, DateTimeKind.Utc).AddTicks(7818), "SystemAdmin@gmail.com", true, false, null, "SystemAdmin@gmail.com", "SystemAdmin", "AQAAAAIAAYagAAAAEOn7sYaHvLKxmKD7xC8XPzQA/LppR9W0iDXbRjruNwwfBhdyevVxRrqZhGtubsafqQ==", null, false, null, 8, false, new DateTime(2025, 10, 26, 15, 18, 31, 803, DateTimeKind.Utc).AddTicks(7826), "SystemAdmin" },
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa01"), 0, "62e9d316-be0d-4cc7-8750-11cc9ffd3e1a", new DateTime(2025, 9, 30, 10, 0, 0, 0, DateTimeKind.Utc), "owner@test.local", true, false, null, "OWNER@TEST.LOCAL", "STOREOWNER", "AQAAAA...", null, false, null, 8, false, new DateTime(2025, 9, 30, 10, 0, 0, 0, DateTimeKind.Utc), "storeowner" },
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa02"), 0, "b561968d-162b-4486-91a0-e388afc13cc7", new DateTime(2025, 9, 30, 10, 0, 0, 0, DateTimeKind.Utc), "cust1@test.local", true, false, null, "CUST1@TEST.LOCAL", "CUSTOMER1", "AQAAAA...", null, false, null, 8, false, new DateTime(2025, 9, 30, 10, 0, 0, 0, DateTimeKind.Utc), "customer1" }
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CreatedAt", "DeletedAt", "Description", "Icon", "IsActive", "IsLeaf", "Level", "Name", "ParentId", "Path", "Slug", "SortOrder", "Status", "StoreId", "ThumbnailUrl", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { new Guid("aaaaaaa1-0000-0000-0000-000000000001"), new DateTime(2025, 10, 26, 22, 18, 31, 717, DateTimeKind.Local).AddTicks(8082), null, null, null, true, false, 0, "Dịch vụ vận chuyển", null, null, "van-chuyen", 1, 8, null, null, null, null },
                    { new Guid("aaaaaaa2-0000-0000-0000-000000000001"), new DateTime(2025, 10, 26, 22, 18, 31, 717, DateTimeKind.Local).AddTicks(8103), null, null, null, true, false, 0, "Lưu kho", null, null, "luu-kho", 2, 8, null, null, null, null },
                    { new Guid("aaaaaaa3-0000-0000-0000-000000000001"), new DateTime(2025, 10, 26, 22, 18, 31, 717, DateTimeKind.Local).AddTicks(8105), null, null, null, true, false, 0, "Dọn dẹp", null, null, "don-dep", 3, 8, null, null, null, null },
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa10"), new DateTime(2025, 10, 26, 22, 18, 31, 887, DateTimeKind.Local).AddTicks(9822), null, null, null, true, false, 0, "Moving", null, null, null, null, 8, null, null, null, null },
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa11"), new DateTime(2025, 10, 26, 22, 18, 31, 887, DateTimeKind.Local).AddTicks(9828), null, null, null, true, false, 0, "Storage", null, null, null, null, 8, null, null, null, null }
                });

            migrationBuilder.InsertData(
                table: "Addresses",
                columns: new[] { "Id", "Active", "AddressLine", "City", "CreatedAt", "DeletedAt", "District", "IsDefault", "Label", "Latitude", "Longitude", "StoreId", "UpdatedAt", "UpdatedBy", "UserId", "Ward" },
                values: new object[,]
                {
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa21"), true, "89 Trần Hưng Đạo", "Hà Nội", new DateTime(2025, 10, 26, 22, 18, 31, 887, DateTimeKind.Local).AddTicks(9779), null, "Hoàn Kiếm", true, "Home Pickup", 21.026, 105.84099999999999, null, null, null, new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa02"), "Cửa Nam" },
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa22"), true, "25 Lê Duẩn", "Hồ Chí Minh", new DateTime(2025, 10, 26, 22, 18, 31, 887, DateTimeKind.Local).AddTicks(9782), null, "Q.1", false, "New Apartment", 10.782, 106.7, null, null, null, new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa02"), "Bến Nghé" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { new Guid("aaaaaaa1-0000-0000-0000-000000000002"), new Guid("22222222-2222-2222-2222-222222222221") },
                    { new Guid("aaaaaaa1-0000-0000-0000-000000000002"), new Guid("22222222-2222-2222-2222-222222222222") },
                    { new Guid("aaaaaaa1-0000-0000-0000-000000000002"), new Guid("22222222-2222-2222-2222-222222222223") },
                    { new Guid("aaaaaaa1-0000-0000-0000-000000000002"), new Guid("aaaaaaa1-0000-0000-0000-000000000001") },
                    { new Guid("aaaaaaa1-0000-0000-0000-000000000001"), new Guid("aaaaaaa1-0000-0000-0000-000000000002") }
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CreatedAt", "DeletedAt", "Description", "Icon", "IsActive", "IsLeaf", "Level", "Name", "ParentId", "Path", "Slug", "SortOrder", "Status", "StoreId", "ThumbnailUrl", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { new Guid("aaaaaaa1-0000-0000-0000-000000000002"), new DateTime(2025, 10, 26, 22, 18, 31, 717, DateTimeKind.Local).AddTicks(8106), null, null, null, true, false, 0, "Chuyển nhà", new Guid("aaaaaaa1-0000-0000-0000-000000000001"), null, "chuyen-nha", 1, 8, null, null, null, null },
                    { new Guid("aaaaaaa1-0000-0000-0000-000000000003"), new DateTime(2025, 10, 26, 22, 18, 31, 717, DateTimeKind.Local).AddTicks(8111), null, null, null, true, false, 0, "Chuyển văn phòng", new Guid("aaaaaaa1-0000-0000-0000-000000000001"), null, "chuyen-van-phong", 2, 8, null, null, null, null },
                    { new Guid("aaaaaaa1-0000-0000-0000-000000000004"), new DateTime(2025, 10, 26, 22, 18, 31, 717, DateTimeKind.Local).AddTicks(8113), null, null, null, true, false, 0, "Xe tải theo km", new Guid("aaaaaaa1-0000-0000-0000-000000000001"), null, "xe-tai-theo-km", 3, 8, null, null, null, null },
                    { new Guid("aaaaaaa2-0000-0000-0000-000000000002"), new DateTime(2025, 10, 26, 22, 18, 31, 717, DateTimeKind.Local).AddTicks(8114), null, null, null, true, false, 0, "Theo giờ", new Guid("aaaaaaa2-0000-0000-0000-000000000001"), null, "theo-gio", 1, 8, null, null, null, null },
                    { new Guid("aaaaaaa2-0000-0000-0000-000000000003"), new DateTime(2025, 10, 26, 22, 18, 31, 717, DateTimeKind.Local).AddTicks(8116), null, null, null, true, false, 0, "Theo ngày", new Guid("aaaaaaa2-0000-0000-0000-000000000001"), null, "theo-ngay", 2, 8, null, null, null, null },
                    { new Guid("aaaaaaa3-0000-0000-0000-000000000002"), new DateTime(2025, 10, 26, 22, 18, 31, 717, DateTimeKind.Local).AddTicks(8118), null, null, null, true, false, 0, "Vệ sinh nhà", new Guid("aaaaaaa3-0000-0000-0000-000000000001"), null, "ve-sinh-nha", 1, 8, null, null, null, null },
                    { new Guid("aaaaaaa3-0000-0000-0000-000000000003"), new DateTime(2025, 10, 26, 22, 18, 31, 717, DateTimeKind.Local).AddTicks(8119), null, null, null, true, false, 0, "Vệ sinh văn phòng", new Guid("aaaaaaa3-0000-0000-0000-000000000001"), null, "ve-sinh-van-phong", 2, 8, null, null, null, null }
                });

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "Id", "CreatedAt", "DeletedAt", "Email", "FullName", "KycLevel", "PhoneNumber", "PreferredLang", "Tier", "UpdatedAt", "UpdatedBy" },
                values: new object[] { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa02"), new DateTime(2025, 9, 30, 10, 0, 0, 0, DateTimeKind.Utc), null, "a@gmail.com", "Nguyễn Văn A", "None", "0123456789", "vi", "Basic", null, null });

            migrationBuilder.InsertData(
                table: "Stores",
                columns: new[] { "Id", "ActiveRegions", "BankAccountNumber", "BankName", "ContactEmail", "ContactPhone", "CreatedAt", "DeletedAt", "IsVerified", "Latitude", "LegalName", "LicenseExpiryDate", "LicenseNumber", "Longitude", "MaxOrdersPerDay", "OwnerUserId", "RatingAvg", "RatingCount", "ServiceTypes", "Status", "StoreName", "TaxNumber", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa1"), null, null, null, null, null, new DateTime(2025, 10, 26, 15, 18, 31, 887, DateTimeKind.Utc).AddTicks(9300), null, false, null, null, null, null, null, null, new Guid("22222222-2222-2222-2222-222222222221"), 0m, 0, null, 9, "Blue Wash", null, new DateTime(2025, 10, 26, 15, 18, 31, 887, DateTimeKind.Utc).AddTicks(9301), null },
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa2"), null, null, null, null, null, new DateTime(2025, 10, 26, 15, 18, 31, 887, DateTimeKind.Utc).AddTicks(9308), null, false, null, null, null, null, null, null, new Guid("22222222-2222-2222-2222-222222222222"), 0m, 0, null, 9, "Fresh Laundry", null, new DateTime(2025, 10, 26, 15, 18, 31, 887, DateTimeKind.Utc).AddTicks(9317), null }
                });

            migrationBuilder.InsertData(
                table: "Stores",
                columns: new[] { "Id", "ActiveRegions", "BankAccountNumber", "BankName", "ContactEmail", "ContactPhone", "CreatedAt", "DeletedAt", "IsVerified", "KycLevel", "Latitude", "LegalName", "LicenseExpiryDate", "LicenseNumber", "Longitude", "MaxOrdersPerDay", "OwnerUserId", "RatingAvg", "RatingCount", "ServiceTypes", "Status", "StoreName", "TaxNumber", "UpdatedAt", "UpdatedBy" },
                values: new object[] { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa3"), "HN,HCM", null, null, null, null, new DateTime(2025, 9, 26, 15, 18, 31, 887, DateTimeKind.Utc).AddTicks(9323), null, false, "Verified", null, null, null, null, null, 80, new Guid("22222222-2222-2222-2222-222222222223"), 0m, 0, null, 8, "Prime Cleaners", null, new DateTime(2025, 10, 26, 15, 18, 31, 887, DateTimeKind.Utc).AddTicks(9329), null });

            migrationBuilder.InsertData(
                table: "Addresses",
                columns: new[] { "Id", "Active", "AddressLine", "City", "CreatedAt", "DeletedAt", "District", "IsDefault", "Label", "Latitude", "Longitude", "StoreId", "UpdatedAt", "UpdatedBy", "UserId", "Ward" },
                values: new object[] { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa20"), true, "12 Nguyễn Huệ", "Hồ Chí Minh", new DateTime(2025, 10, 26, 22, 18, 31, 887, DateTimeKind.Local).AddTicks(9770), null, "Q.1", true, "Store HQ", 10.772, 106.70399999999999, new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa3"), null, null, null, "Bến Nghé" });

            migrationBuilder.InsertData(
                table: "KycSubmissions",
                columns: new[] { "Id", "AdminNote", "CreatedAt", "DeletedAt", "ReviewedAt", "ReviewedBy", "Status", "StoreId", "SubmittedAt", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb1"), null, new DateTime(2025, 10, 20, 15, 18, 31, 887, DateTimeKind.Utc).AddTicks(9396), null, null, null, 0, new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa1"), new DateTime(2025, 10, 20, 15, 18, 31, 887, DateTimeKind.Utc).AddTicks(9394), null, null },
                    { new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb2"), "Thiếu giấy tờ thuế / ảnh mờ, vui lòng bổ sung.", new DateTime(2025, 10, 21, 15, 18, 31, 887, DateTimeKind.Utc).AddTicks(9403), null, new DateTime(2025, 10, 22, 15, 18, 31, 887, DateTimeKind.Utc).AddTicks(9399), new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd"), 1, new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa2"), new DateTime(2025, 10, 21, 15, 18, 31, 887, DateTimeKind.Utc).AddTicks(9398), null, null },
                    { new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb3"), "Ok", new DateTime(2025, 10, 11, 15, 18, 31, 887, DateTimeKind.Utc).AddTicks(9409), null, new DateTime(2025, 10, 12, 15, 18, 31, 887, DateTimeKind.Utc).AddTicks(9407), new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd"), 2, new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa3"), new DateTime(2025, 10, 11, 15, 18, 31, 887, DateTimeKind.Utc).AddTicks(9406), null, null }
                });

            migrationBuilder.InsertData(
                table: "Orders",
                columns: new[] { "Id", "CreatedAt", "CustomerId", "DeletedAt", "DeliveryDate", "DistanceKm", "DropoffAddressId", "EtaMinutes", "Note", "PickupAddressId", "PickupDate", "ProductCategoryIds", "QuotationId", "Status", "StoreId", "TotalAmount", "UpdatedAt", "UpdatedBy" },
                values: new object[] { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa30"), new DateTime(2025, 8, 15, 9, 0, 0, 0, DateTimeKind.Utc), new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa02"), null, null, 7.2m, new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa22"), 55, "August order", new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa21"), null, null, null, 14, new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa3"), 2200000m, new DateTime(2025, 8, 15, 9, 0, 0, 0, DateTimeKind.Utc), null });

            migrationBuilder.InsertData(
                table: "Orders",
                columns: new[] { "Id", "CreatedAt", "CustomerId", "DeletedAt", "DeliveryDate", "DistanceKm", "DropoffAddressId", "EtaMinutes", "Note", "PickupAddressId", "PickupDate", "ProductCategoryIds", "QuotationId", "StoreId", "TotalAmount", "UpdatedAt", "UpdatedBy" },
                values: new object[] { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa31"), new DateTime(2025, 9, 29, 14, 30, 0, 0, DateTimeKind.Utc), new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa02"), null, null, 5.1m, new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa22"), 45, "Yesterday pending", new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa21"), null, null, null, new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa3"), 1500000m, new DateTime(2025, 9, 29, 14, 30, 0, 0, DateTimeKind.Utc), null });

            migrationBuilder.InsertData(
                table: "Orders",
                columns: new[] { "Id", "CreatedAt", "CustomerId", "DeletedAt", "DeliveryDate", "DistanceKm", "DropoffAddressId", "EtaMinutes", "Note", "PickupAddressId", "PickupDate", "ProductCategoryIds", "QuotationId", "Status", "StoreId", "TotalAmount", "UpdatedAt", "UpdatedBy" },
                values: new object[] { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa32"), new DateTime(2025, 9, 30, 8, 0, 0, 0, DateTimeKind.Utc), new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa02"), null, null, 3.4m, new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa22"), 35, "Today completed", new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa21"), null, null, null, 14, new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa3"), 2000000m, new DateTime(2025, 9, 30, 9, 0, 0, 0, DateTimeKind.Utc), null });

            migrationBuilder.InsertData(
                table: "Orders",
                columns: new[] { "Id", "CreatedAt", "CustomerId", "DeletedAt", "DeliveryDate", "DistanceKm", "DropoffAddressId", "EtaMinutes", "Note", "PickupAddressId", "PickupDate", "ProductCategoryIds", "QuotationId", "StoreId", "TotalAmount", "UpdatedAt", "UpdatedBy" },
                values: new object[] { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa33"), new DateTime(2025, 9, 30, 9, 0, 0, 0, DateTimeKind.Utc), new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa02"), null, null, 9.0m, new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa22"), 70, "Today pending", new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa21"), null, null, null, new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa3"), 3100000m, new DateTime(2025, 9, 30, 9, 40, 0, 0, DateTimeKind.Utc), null });

            migrationBuilder.InsertData(
                table: "Services",
                columns: new[] { "Id", "BasePrice", "CategoryId", "CreatedAt", "DeletedAt", "Description", "IsActive", "Name", "PricingModel", "Status", "StoreId", "Unit", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa12"), 1500000m, new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa10"), new DateTime(2025, 8, 15, 9, 0, 0, 0, DateTimeKind.Utc), null, "Local moving inside city", true, "House Moving (City)", 3, 0, new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa3"), "Job", null, null },
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa13"), 300000m, new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa10"), new DateTime(2025, 8, 15, 9, 0, 0, 0, DateTimeKind.Utc), null, "Boxes & packing", true, "Packing Service", 3, 0, new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa3"), "Package", null, null }
                });

            migrationBuilder.InsertData(
                table: "Feedbacks",
                columns: new[] { "Id", "Comment", "CreatedAt", "DeletedAt", "FromUserId", "OrderId", "Rating", "ToStoreId", "UpdatedAt", "UpdatedBy" },
                values: new object[] { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa50"), "Very smooth job!", new DateTime(2025, 9, 30, 10, 0, 0, 0, DateTimeKind.Utc), null, new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa02"), new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa32"), 5, new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa3"), null, null });

            migrationBuilder.InsertData(
                table: "KycDocuments",
                columns: new[] { "Id", "CreatedAt", "DeletedAt", "DocType", "FilePath", "Hash", "KycSubmissionId", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc1"), new DateTime(2025, 10, 20, 15, 18, 31, 887, DateTimeKind.Utc).AddTicks(9497), null, "License", "/uploads/kyc/blue/license.pdf", null, new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb1"), null, null },
                    { new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc2"), new DateTime(2025, 10, 20, 15, 18, 31, 887, DateTimeKind.Utc).AddTicks(9501), null, "ID", "/uploads/kyc/blue/id.jpg", null, new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb1"), null, null },
                    { new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc3"), new DateTime(2025, 10, 20, 15, 18, 31, 887, DateTimeKind.Utc).AddTicks(9514), null, "Tax", "/uploads/kyc/blue/tax.pdf", null, new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb1"), null, null },
                    { new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc4"), new DateTime(2025, 10, 21, 15, 18, 31, 887, DateTimeKind.Utc).AddTicks(9516), null, "License", "/uploads/kyc/fresh/license.pdf", null, new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb2"), null, null },
                    { new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc5"), new DateTime(2025, 10, 21, 15, 18, 31, 887, DateTimeKind.Utc).AddTicks(9518), null, "ID", "/uploads/kyc/fresh/id.jpg", null, new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb2"), null, null },
                    { new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc6"), new DateTime(2025, 10, 11, 15, 18, 31, 887, DateTimeKind.Utc).AddTicks(9520), null, "License", "/uploads/kyc/prime/license.pdf", null, new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb3"), null, null },
                    { new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc7"), new DateTime(2025, 10, 11, 15, 18, 31, 887, DateTimeKind.Utc).AddTicks(9522), null, "ID", "/uploads/kyc/prime/id.jpg", null, new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb3"), null, null },
                    { new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc8"), new DateTime(2025, 10, 11, 15, 18, 31, 887, DateTimeKind.Utc).AddTicks(9594), null, "Tax", "/uploads/kyc/prime/tax.pdf", null, new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb3"), null, null }
                });

            migrationBuilder.InsertData(
                table: "OrderItems",
                columns: new[] { "Id", "CreatedAt", "DeletedAt", "Description", "HeightM", "ItemName", "LengthM", "OrderId", "Quantity", "ServiceId", "SizeCode", "Subtotal", "UnitPrice", "UpdatedAt", "UpdatedBy", "WeightKg", "WidthM" },
                values: new object[,]
                {
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa40"), new DateTime(2025, 10, 26, 22, 18, 31, 888, DateTimeKind.Local).AddTicks(153), null, null, null, "Moving Service", null, new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa30"), 1, new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa12"), null, 2200000m, 2200000m, null, null, null, null },
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa41"), new DateTime(2025, 10, 26, 22, 18, 31, 888, DateTimeKind.Local).AddTicks(161), null, null, null, "Moving Service", null, new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa31"), 1, new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa12"), null, 1200000m, 1200000m, null, null, null, null },
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa42"), new DateTime(2025, 10, 26, 22, 18, 31, 888, DateTimeKind.Local).AddTicks(163), null, null, null, "Moving Service", null, new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa31"), 1, new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa13"), null, 300000m, 300000m, null, null, null, null },
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa43"), new DateTime(2025, 10, 26, 22, 18, 31, 888, DateTimeKind.Local).AddTicks(165), null, null, null, "Moving Service", null, new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa32"), 1, new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa12"), null, 2000000m, 2000000m, null, null, null, null }
                });

            migrationBuilder.InsertData(
                table: "PriceRules",
                columns: new[] { "Id", "ApplyModel", "CreatedAt", "DeletedAt", "MaxAreaM2", "MaxDays", "MaxQty", "MaxVolumeM3", "MinAreaM2", "MinDays", "MinQty", "MinVolumeM3", "Price", "ServiceId", "TimeUnit", "UpdatedAt", "UpdatedBy", "ValidFrom", "ValidTo" },
                values: new object[] { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa14"), 3, new DateTime(2025, 10, 26, 22, 18, 31, 888, DateTimeKind.Local).AddTicks(29), null, null, null, 10m, null, null, null, 3m, null, 280000m, new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa13"), 0, null, null, new DateTime(2025, 8, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 12, 31, 0, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.InsertData(
                table: "Warehouses",
                columns: new[] { "Id", "AddressRefId", "CoverImageUrl", "CreatedAt", "DeletedAt", "HeightM", "LengthM", "MapImageUrl", "Name", "Status", "StoreId", "UpdatedAt", "UpdatedBy", "WidthM" },
                values: new object[] { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa60"), new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa20"), null, new DateTime(2025, 10, 26, 22, 18, 31, 888, DateTimeKind.Local).AddTicks(254), null, 0m, 0m, null, "Main Warehouse", 0, new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa3"), null, null, 0m });

            migrationBuilder.InsertData(
                table: "WarehouseSlots",
                columns: new[] { "Id", "BasePricePerHour", "Code", "Col", "CreatedAt", "CurrentOrderId", "DeletedAt", "HeightM", "ImageUrl", "IsBlocked", "LeaseEnd", "LeaseStart", "LengthM", "Row", "Size", "Status", "UpdatedAt", "UpdatedBy", "WarehouseId", "WidthM" },
                values: new object[,]
                {
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa61"), 0m, "A1", 0, new DateTime(2025, 10, 26, 22, 18, 31, 888, DateTimeKind.Local).AddTicks(296), null, null, 0m, null, false, null, null, 0m, 0, null, 4, null, null, new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa60"), 0m },
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa62"), 0m, "A2", 0, new DateTime(2025, 10, 26, 22, 18, 31, 888, DateTimeKind.Local).AddTicks(300), new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa31"), null, 0m, null, false, null, null, 0m, 0, null, 5, null, null, new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa60"), 0m },
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa63"), 0m, "B1", 0, new DateTime(2025, 10, 26, 22, 18, 31, 888, DateTimeKind.Local).AddTicks(302), null, null, 0m, null, false, null, null, 0m, 0, null, 4, null, null, new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa60"), 0m }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_StoreId",
                table: "Addresses",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_UserId",
                table: "Addresses",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_ParentId",
                table: "Categories",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_ParentId_SortOrder",
                table: "Categories",
                columns: new[] { "ParentId", "SortOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_Categories_Path",
                table: "Categories",
                column: "Path");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_StoreId_Slug",
                table: "Categories",
                columns: new[] { "StoreId", "Slug" },
                unique: true,
                filter: "[StoreId] IS NOT NULL AND [Slug] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Feedbacks_FromUserId",
                table: "Feedbacks",
                column: "FromUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Feedbacks_OrderId",
                table: "Feedbacks",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Feedbacks_ToStoreId",
                table: "Feedbacks",
                column: "ToStoreId");

            migrationBuilder.CreateIndex(
                name: "IX_KycDocuments_KycSubmissionId",
                table: "KycDocuments",
                column: "KycSubmissionId");

            migrationBuilder.CreateIndex(
                name: "IX_KycSubmissions_Status_SubmittedAt",
                table: "KycSubmissions",
                columns: new[] { "Status", "SubmittedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_KycSubmissions_StoreId",
                table: "KycSubmissions",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_OrderId",
                table: "OrderItems",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_ServiceId",
                table: "OrderItems",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CustomerId",
                table: "Orders",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_DropoffAddressId",
                table: "Orders",
                column: "DropoffAddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_PickupAddressId",
                table: "Orders",
                column: "PickupAddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_QuotationId",
                table: "Orders",
                column: "QuotationId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_StoreId",
                table: "Orders",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_OrderId",
                table: "Payments",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_PriceRules_ServiceId_MinVolumeM3_MaxVolumeM3_MinDays_MaxDays",
                table: "PriceRules",
                columns: new[] { "ServiceId", "MinVolumeM3", "MaxVolumeM3", "MinDays", "MaxDays" });

            migrationBuilder.CreateIndex(
                name: "IX_PriceRules_ServiceId_ValidFrom_ValidTo",
                table: "PriceRules",
                columns: new[] { "ServiceId", "ValidFrom", "ValidTo" });

            migrationBuilder.CreateIndex(
                name: "IX_Quotations_CustomerId",
                table: "Quotations",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Quotations_StoreId",
                table: "Quotations",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceAddons_ServiceId_Name",
                table: "ServiceAddons",
                columns: new[] { "ServiceId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Services_CategoryId",
                table: "Services",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Services_StoreId",
                table: "Services",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceSizeOptions_ServiceId_Code",
                table: "ServiceSizeOptions",
                columns: new[] { "ServiceId", "Code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SlotReservations_OrderId",
                table: "SlotReservations",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_SlotReservations_WarehouseSlotId_ExpiresAt",
                table: "SlotReservations",
                columns: new[] { "WarehouseSlotId", "ExpiresAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Stores_OwnerUserId",
                table: "Stores",
                column: "OwnerUserId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreStaff_UserId",
                table: "StoreStaff",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "UQ_StoreStaff",
                table: "StoreStaff",
                columns: new[] { "StoreId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WalletTransactions_OrderId",
                table: "WalletTransactions",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_WalletTransactions_WalletId",
                table: "WalletTransactions",
                column: "WalletId");

            migrationBuilder.CreateIndex(
                name: "IX_Warehouses_AddressRefId",
                table: "Warehouses",
                column: "AddressRefId");

            migrationBuilder.CreateIndex(
                name: "IX_Warehouses_StoreId_Name",
                table: "Warehouses",
                columns: new[] { "StoreId", "Name" });

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseSlots_WarehouseId_Code",
                table: "WarehouseSlots",
                columns: new[] { "WarehouseId", "Code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseSlots_WarehouseId_Status",
                table: "WarehouseSlots",
                columns: new[] { "WarehouseId", "Status" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Feedbacks");

            migrationBuilder.DropTable(
                name: "KycDocuments");

            migrationBuilder.DropTable(
                name: "OrderItems");

            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "PriceRules");

            migrationBuilder.DropTable(
                name: "ServiceAddons");

            migrationBuilder.DropTable(
                name: "ServiceSizeOptions");

            migrationBuilder.DropTable(
                name: "SlotReservations");

            migrationBuilder.DropTable(
                name: "StoreStaff");

            migrationBuilder.DropTable(
                name: "WalletTransactions");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "KycSubmissions");

            migrationBuilder.DropTable(
                name: "Services");

            migrationBuilder.DropTable(
                name: "WarehouseSlots");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Wallets");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Warehouses");

            migrationBuilder.DropTable(
                name: "Quotations");

            migrationBuilder.DropTable(
                name: "Addresses");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "Stores");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}
