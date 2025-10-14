using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DependencyInjections.Extensions
{
    public static class SeedingDataExtension
    {
        public static void Seeding(this ModelBuilder builder)
        {
            var adminId = Guid.Parse("AAAAAAA1-0000-0000-0000-000000000001");
            var storeId = Guid.Parse("AAAAAAA1-0000-0000-0000-000000000002");
            var storeStaffId = Guid.Parse("AAAAAAA1-0000-0000-0000-000000000003");
            var customerId = Guid.Parse("AAAAAAA1-0000-0000-0000-000000000004");
            builder.Entity<IdentityRole<Guid>>(entity =>
            {

                entity.HasData(new IdentityRole<Guid>
                {
                    Id = adminId,
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                });

                entity.HasData(new IdentityRole<Guid>
                {
                    Id = storeId,
                    Name = "Store",
                    NormalizedName = "STORE"
                });

                entity.HasData(new IdentityRole<Guid>
                {
                    Id = customerId,
                    Name = "Customer",
                    NormalizedName = "CUSTOMER"
                });

                entity.HasData(new IdentityRole<Guid>
                {
                    Id = storeStaffId,
                    Name = "StoreStaff",
                    NormalizedName = "StoreStaff"
                });
            });
        }

        public static void SeedingStoreData(this ModelBuilder modelBuilder)
        {
            var userId = Guid.Parse("AAAAAAA1-0000-0000-0000-000000000001");
            var hasher = new PasswordHasher<User>();
            var temp = new User();
            var hash = hasher.HashPassword(temp, "password123");
            var user = new User
            {
                Id = userId,
                Email = "store1@gmail.com",
                EmailConfirmed = true,
                UserName = "store1",
                NormalizedEmail = "store1@gmail.com",
                NormalizedUserName = "store1",
                PasswordHash = hash,
                Status =Enums.StatusValue.Active
            };
            modelBuilder.Entity<User>().HasData(user);
            modelBuilder.Entity<IdentityUserRole<Guid>>().HasData(new IdentityUserRole<Guid> { RoleId = Guid.Parse("AAAAAAA1-0000-0000-0000-000000000002"), UserId = user.Id });


        }

        public static void SeedingCategoryData(this ModelBuilder modelBuilder)
        {
            var CAT_VANCHUYEN = Guid.Parse("AAAAAAA1-0000-0000-0000-000000000001");
            var CAT_CHUYEN_NHA = Guid.Parse("AAAAAAA1-0000-0000-0000-000000000002");
            var CAT_CHUYEN_VP = Guid.Parse("AAAAAAA1-0000-0000-0000-000000000003");
            var CAT_XE_TAI_KM = Guid.Parse("AAAAAAA1-0000-0000-0000-000000000004");

            var CAT_LUUKHO = Guid.Parse("AAAAAAA2-0000-0000-0000-000000000001");
            var CAT_LK_GIO = Guid.Parse("AAAAAAA2-0000-0000-0000-000000000002");
            var CAT_LK_NGAY = Guid.Parse("AAAAAAA2-0000-0000-0000-000000000003");

            var CAT_DONDEP = Guid.Parse("AAAAAAA3-0000-0000-0000-000000000001");
            var CAT_VS_NHA = Guid.Parse("AAAAAAA3-0000-0000-0000-000000000002");
            var CAT_VS_VP = Guid.Parse("AAAAAAA3-0000-0000-0000-000000000003");

            modelBuilder.Entity<Category>().HasData(

                new Category { Id = CAT_VANCHUYEN, ParentId = null, Name = "Dịch vụ vận chuyển", Slug = "van-chuyen", SortOrder = 1 },
                new Category { Id = CAT_LUUKHO, ParentId = null, Name = "Lưu kho", Slug = "luu-kho", SortOrder = 2 },
                new Category { Id = CAT_DONDEP, ParentId = null, Name = "Dọn dẹp", Slug = "don-dep", SortOrder = 3 },


                new Category { Id = CAT_CHUYEN_NHA, ParentId = CAT_VANCHUYEN, Name = "Chuyển nhà", Slug = "chuyen-nha", SortOrder = 1 },
                new Category { Id = CAT_CHUYEN_VP, ParentId = CAT_VANCHUYEN, Name = "Chuyển văn phòng", Slug = "chuyen-van-phong", SortOrder = 2 },
                new Category { Id = CAT_XE_TAI_KM, ParentId = CAT_VANCHUYEN, Name = "Xe tải theo km", Slug = "xe-tai-theo-km", SortOrder = 3 },

                new Category { Id = CAT_LK_GIO, ParentId = CAT_LUUKHO, Name = "Theo giờ", Slug = "theo-gio", SortOrder = 1 },
                new Category { Id = CAT_LK_NGAY, ParentId = CAT_LUUKHO, Name = "Theo ngày", Slug = "theo-ngay", SortOrder = 2 },


                new Category { Id = CAT_VS_NHA, ParentId = CAT_DONDEP, Name = "Vệ sinh nhà", Slug = "ve-sinh-nha", SortOrder = 1 },
                new Category { Id = CAT_VS_VP, ParentId = CAT_DONDEP, Name = "Vệ sinh văn phòng", Slug = "ve-sinh-van-phong", SortOrder = 2 }
            );
        }

        public static void SeedingAdminData(this ModelBuilder modelBuilder)
        {
            var userId = Guid.Parse("AAAAAAA1-0000-0000-0000-000000000002");
            var hasher = new PasswordHasher<User>();
            var temp = new User();
            var hash = hasher.HashPassword(temp, "password123@");
            var user = new User
            {
                Id = userId,
                Email = "SystemAdmin@gmail.com",
                EmailConfirmed = true,
                UserName = "SystemAdmin",
                NormalizedEmail = "SystemAdmin@gmail.com",
                NormalizedUserName = "SystemAdmin",
                PasswordHash = hash,
                Status = Enums.StatusValue.Active,

            };
            modelBuilder.Entity<User>().HasData(user);
            modelBuilder.Entity<IdentityUserRole<Guid>>().HasData(new IdentityUserRole<Guid> { RoleId = Guid.Parse("AAAAAAA1-0000-0000-0000-000000000001"), UserId = user.Id });

        }


        public static void SeedingDataToTestAdmin(this ModelBuilder b)
        {

            var blueOwnerId = Guid.Parse("22222222-2222-2222-2222-222222222221");
            var freshOwnerId = Guid.Parse("22222222-2222-2222-2222-222222222222");
            var primeOwnerId = Guid.Parse("22222222-2222-2222-2222-222222222223");
            var hasher = new PasswordHasher<User>();
            var hash = hasher.HashPassword(new User(), "password123");
            b.Entity<User>().HasData(
                new User
                {
                    Id = blueOwnerId,
                    UserName = "blueowner",
                    NormalizedUserName = "BLUEOWNER",
                    Email = "owner.blue@demo.local",
                    NormalizedEmail = "OWNER.BLUE@DEMO.LOCAL",
                    EmailConfirmed = true,
                    PasswordHash = hash,
                    SecurityStamp = "sec-blue",
                    ConcurrencyStamp = "con-blue",
                    Status = Enums.StatusValue.Active,
                    CreatedAt = new DateTime(2025, 01, 01),
                    UpdatedAt = new DateTime(2025, 01, 01)
                },
                new User
                {
                    Id = freshOwnerId,
                    UserName = "freshowner",
                    NormalizedUserName = "FRESHOWNER",
                    Email = "owner.fresh@demo.local",
                    NormalizedEmail = "OWNER.FRESH@DEMO.LOCAL",
                    EmailConfirmed = true,
                    PasswordHash = hash,
                    SecurityStamp = "sec-fresh",
                    ConcurrencyStamp = "con-fresh",
                    Status = Enums.StatusValue.Active,
                    CreatedAt = new DateTime(2025, 01, 01),
                    UpdatedAt = new DateTime(2025, 01, 01)
                },
                new User
                {
                    Id = primeOwnerId,
                    UserName = "primeowner",
                    NormalizedUserName = "PRIMEOWNER",
                    Email = "owner.prime@demo.local",
                    NormalizedEmail = "OWNER.PRIME@DEMO.LOCAL",
                    EmailConfirmed = true,
                    PasswordHash = hash,
                    SecurityStamp = "sec-prime",
                    ConcurrencyStamp = "con-prime",
                    Status = Enums.StatusValue.Active,
                    CreatedAt = new DateTime(2025, 01, 01),
                    UpdatedAt = new DateTime(2025, 01, 01)
                });

            b.Entity<IdentityUserRole<Guid>>().HasData(
                new IdentityUserRole<Guid> { UserId = blueOwnerId, RoleId = Guid.Parse("AAAAAAA1-0000-0000-0000-000000000002") },
                new IdentityUserRole<Guid> { UserId = freshOwnerId, RoleId = Guid.Parse("AAAAAAA1-0000-0000-0000-000000000002") },
                new IdentityUserRole<Guid> { UserId = primeOwnerId, RoleId = Guid.Parse("AAAAAAA1-0000-0000-0000-000000000002") }
               );
            var storeBlueId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa1");
            var storeFreshId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa2");
            var storePrimeId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa3");

            b.Entity<Store>().HasData(
                new Store { Id = storeBlueId, OwnerUserId = blueOwnerId, StoreName = "Blue Wash", Status = Enums.StatusValue.InActive, RatingCount= 0,RatingAvg=0, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                new Store { Id = storeFreshId, OwnerUserId = freshOwnerId, StoreName = "Fresh Laundry", Status = Enums.StatusValue.InActive, RatingCount = 0, RatingAvg = 0, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                new Store { Id = storePrimeId, OwnerUserId = primeOwnerId, StoreName = "Prime Cleaners", Status = Enums.StatusValue.Active, RatingCount = 0, RatingAvg = 0, KycLevel = "Verified", MaxOrdersPerDay = 80, ActiveRegions = "HN,HCM", CreatedAt = DateTime.UtcNow.AddDays(-30), UpdatedAt = DateTime.UtcNow }
            );

            var kycBlueId = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb1");
            var kycFreshId = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb2");
            var kycPrimeId = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb3");

            b.Entity<KycSubmission>().HasData(
                new KycSubmission { Id = kycBlueId, StoreId = storeBlueId, Status = Enums.KycStatus.Pending, AdminNote = (string?)null, SubmittedAt = DateTime.UtcNow.AddDays(-6), ReviewedAt = (DateTime?)null, ReviewedBy = (Guid?)null, CreatedAt = DateTime.UtcNow.AddDays(-6) },
                new KycSubmission { Id = kycFreshId, StoreId = storeFreshId, Status = Enums.KycStatus.NeedChanges, AdminNote = "Thiếu giấy tờ thuế / ảnh mờ, vui lòng bổ sung.", SubmittedAt = DateTime.UtcNow.AddDays(-5), ReviewedAt = DateTime.UtcNow.AddDays(-4), ReviewedBy = Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddddddd"), CreatedAt = DateTime.UtcNow.AddDays(-5) },
                new KycSubmission { Id = kycPrimeId, StoreId = storePrimeId, Status = Enums.KycStatus.Approved, AdminNote = "Ok", SubmittedAt = DateTime.UtcNow.AddDays(-15), ReviewedAt = DateTime.UtcNow.AddDays(-14), ReviewedBy = Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddddddd"), CreatedAt = DateTime.UtcNow.AddDays(-15) }
            );

            b.Entity<KycDocument>().HasData(
                new KycDocument { Id = Guid.Parse("cccccccc-cccc-cccc-cccc-ccccccccccc1"), KycSubmissionId = kycBlueId, DocType = "License", FilePath = "/uploads/kyc/blue/license.pdf", CreatedAt = DateTime.UtcNow.AddDays(-6) },
                new KycDocument { Id = Guid.Parse("cccccccc-cccc-cccc-cccc-ccccccccccc2"), KycSubmissionId = kycBlueId, DocType = "ID", FilePath = "/uploads/kyc/blue/id.jpg", CreatedAt = DateTime.UtcNow.AddDays(-6) },
                new KycDocument { Id = Guid.Parse("cccccccc-cccc-cccc-cccc-ccccccccccc3"), KycSubmissionId = kycBlueId, DocType = "Tax", FilePath = "/uploads/kyc/blue/tax.pdf", CreatedAt = DateTime.UtcNow.AddDays(-6) },

                new KycDocument { Id = Guid.Parse("cccccccc-cccc-cccc-cccc-ccccccccccc4"), KycSubmissionId = kycFreshId, DocType = "License", FilePath = "/uploads/kyc/fresh/license.pdf", CreatedAt = DateTime.UtcNow.AddDays(-5) },
                new KycDocument { Id = Guid.Parse("cccccccc-cccc-cccc-cccc-ccccccccccc5"), KycSubmissionId = kycFreshId, DocType = "ID", FilePath = "/uploads/kyc/fresh/id.jpg", CreatedAt = DateTime.UtcNow.AddDays(-5) },

                new KycDocument { Id = Guid.Parse("cccccccc-cccc-cccc-cccc-ccccccccccc6"), KycSubmissionId = kycPrimeId, DocType = "License", FilePath = "/uploads/kyc/prime/license.pdf", CreatedAt = DateTime.UtcNow.AddDays(-15) },
                new KycDocument { Id = Guid.Parse("cccccccc-cccc-cccc-cccc-ccccccccccc7"), KycSubmissionId = kycPrimeId, DocType = "ID", FilePath = "/uploads/kyc/prime/id.jpg", CreatedAt = DateTime.UtcNow.AddDays(-15) },
                new KycDocument { Id = Guid.Parse("cccccccc-cccc-cccc-cccc-ccccccccccc8"), KycSubmissionId = kycPrimeId, DocType = "Tax", FilePath = "/uploads/kyc/prime/tax.pdf", CreatedAt = DateTime.UtcNow.AddDays(-15) }
            );
        }
    }
}
