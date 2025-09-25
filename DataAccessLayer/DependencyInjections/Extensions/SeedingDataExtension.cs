using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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
            var hash = hasher.HashPassword(temp,"password123");
            var user = new User
            {
                Id = userId,
                Email = "store1@gmail.com",
                EmailConfirmed = true,
                UserName = "store1",
                NormalizedEmail = "store1@gmail.com",
                NormalizedUserName = "store1",
                PasswordHash = hash,
                Status = "Active"
            };
            modelBuilder.Entity<User>().HasData(user);
            modelBuilder.Entity<IdentityUserRole<Guid>>().HasData(new IdentityUserRole<Guid> { RoleId = Guid.Parse("AAAAAAA1-0000-0000-0000-000000000002"), UserId = user.Id});


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
    }
}
