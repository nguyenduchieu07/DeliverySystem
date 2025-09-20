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
            builder.Entity<IdentityRole<Guid>>(entity =>
            {
                entity.HasData(new IdentityRole<Guid>
                {
                    Id = Guid.NewGuid(),
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                });

                entity.HasData(new IdentityRole<Guid>
                {
                    Id = Guid.NewGuid(),
                    Name = "Store",
                    NormalizedName = "STORE"
                });

                entity.HasData(new IdentityRole<Guid>
                {
                    Id = Guid.NewGuid(),
                    Name = "Customer",
                    NormalizedName = "CUSTOMER"
                });

                entity.HasData(new IdentityRole<Guid>
                {
                    Id = Guid.NewGuid(),
                    Name = "StoreStaff",
                    NormalizedName = "StoreStaff"
                });
            });
        }
    }
}
