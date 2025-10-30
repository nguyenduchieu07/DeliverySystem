using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using status = DataAccessLayer.Enums.StatusValue;
namespace DataAccessLayer.DependencyInjections.Extensions
{
    public static class DatabaseDashboardSeeding
    {
        public static void SeedingDashboards(this ModelBuilder modelBuilder)
        {
            var storeId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa3");
            var ownerUserId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa01");
            var customerUser = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa02");
            var customerId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa03");

            var catMoveId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa10");
            var catStoreId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa11");
            var svcMoveId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa12");
            var svcPackId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa13");

            var addrStoreId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa20");
            var addrPickupId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa21");
            var addrDropId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa22");

            var orderLastM = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa30");
            var orderYest = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa31");
            var orderToday1 = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa32");
            var orderToday2 = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa33");

            var oi1 = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa40");
            var oi2 = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa41");
            var oi3 = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa42");
            var oi4 = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa43");

            var feedbackId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa50");

            var whId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa60");
            var slotA1 = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa61");
            var slotA2 = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa62");
            var slotB1 = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa63");

            // ====== Stable dates (Asia/Bangkok, demo) ======
            // Today = 2025-09-30; Yesterday = 2025-09-29; Last month = any day in 2025-08
            var dtToday = new DateTime(2025, 09, 30, 10, 00, 00, DateTimeKind.Utc);
            var dtYesterday = new DateTime(2025, 09, 29, 14, 30, 00, DateTimeKind.Utc);
            var dtLastMonth = new DateTime(2025, 08, 15, 09, 00, 00, DateTimeKind.Utc);

            // ====== Users (Identity) ======
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = ownerUserId,
                    UserName = "storeowner",
                    NormalizedUserName = "STOREOWNER",
                    Email = "owner@test.local",
                    NormalizedEmail = "OWNER@TEST.LOCAL",
                    EmailConfirmed = true,
                    Status = status.Active,
                    CreatedAt = dtToday,
                    UpdatedAt = dtToday,
                    PasswordHash = "AQAAAA..." // demo hash
                },
                new User
                {
                    Id = customerUser,
                    UserName = "customer1",
                    NormalizedUserName = "CUSTOMER1",
                    Email = "cust1@test.local",
                    NormalizedEmail = "CUST1@TEST.LOCAL",
                    EmailConfirmed = true,
                    Status = status.Active,
                    CreatedAt = dtToday,
                    UpdatedAt = dtToday,
                    PasswordHash = "AQAAAA..."
                }
            );



            // ====== Customer (shadow FK -> CustomerNavigationId) ======
            modelBuilder.Entity<Customer>().HasData(new Customer
            {
                Id = customerUser,
                FullName = "Nguyễn Văn A",
                PhoneNumber = "0123456789",
                Email = "a@gmail.com",
                PreferredLang = "vi",
                Tier = "Basic",
                KycLevel = "None",
                CreatedAt = dtToday,
            });

            // ====== Addresses (store + pickup/dropoff) ======
            modelBuilder.Entity<Address>().HasData(
                new Address
                {
                    Id = addrStoreId,
                    StoreId = storeId,
                    Label = "Store HQ",
                    AddressLine = "12 Nguyễn Huệ",
                    Ward = "Bến Nghé",
                    District = "Q.1",
                    City = "Hồ Chí Minh",
                    Latitude = 10.772,
                    Longitude = 106.704,
                    IsDefault = true,
                    Active = true
                },
                new Address
                {
                    Id = addrPickupId,
                    UserId = customerUser,
                    Label = "Home Pickup",
                    AddressLine = "89 Trần Hưng Đạo",
                    Ward = "Cửa Nam",
                    District = "Hoàn Kiếm",
                    City = "Hà Nội",
                    Latitude = 21.026,
                    Longitude = 105.841,
                    IsDefault = true,
                    Active = true
                },
                new Address
                {
                    Id = addrDropId,
                    UserId = customerUser,
                    Label = "New Apartment",
                    AddressLine = "25 Lê Duẩn",
                    Ward = "Bến Nghé",
                    District = "Q.1",
                    City = "Hồ Chí Minh",
                    Latitude = 10.782,
                    Longitude = 106.700,
                    IsDefault = false,
                    Active = true
                }
            );

            // ====== Categories & Services ======
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = catMoveId, Name = "Moving" },
                new Category { Id = catStoreId, Name = "Storage" }
            );

            modelBuilder.Entity<Service>().HasData(
                new Service
                {
                    Id = svcMoveId,
                    StoreId = storeId,
                    CategoryId = catMoveId,
                    Name = "House Moving (City)",
                    Description = "Local moving inside city",
                    Unit = "Job",
                    BasePrice = 1500000m,
                    IsActive = true,
                    CreatedAt = dtLastMonth
                },
                new Service
                {
                    Id = svcPackId,
                    StoreId = storeId,
                    CategoryId = catMoveId,
                    Name = "Packing Service",
                    Description = "Boxes & packing",
                    Unit = "Package",
                    BasePrice = 300000m,
                    IsActive = true,
                    CreatedAt = dtLastMonth
                }
            );

            // (Optional) ServicePrice – không bắt buộc cho dashboard, seed 1 dòng minh họa
            modelBuilder.Entity<ServicePriceRule>().HasData(
                new ServicePriceRule
                {
                    Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa14"),
                    ServiceId = svcPackId,
                    ValidFrom = new DateTime(2025, 08, 01, 0, 0, 0, DateTimeKind.Utc),
                    ValidTo = new DateTime(2025, 12, 31, 0, 0, 0, DateTimeKind.Utc),
                    Price = 280000m,
                    MinQty = 3,
                    MaxQty = 10
                }
            );

            // ====== Orders (last month, yesterday, today, with different statuses) ======
            modelBuilder.Entity<Order>().HasData(
                new Order
                {
                    Id = orderLastM,
                    CustomerId = customerUser,
                    StoreId = storeId,
                    PickupAddressId = addrPickupId,
                    DropoffAddressId = addrDropId,
                    DistanceKm = 7.2m,
                    EtaMinutes = 55,
                    Status = status.Completed,
                    TotalAmount = 2_200_000m,
                    Note = "August order",
                    CreatedAt = dtLastMonth,
                    UpdatedAt = dtLastMonth
                },
                new Order
                {
                    Id = orderYest,
                    CustomerId = customerUser,
                    StoreId = storeId,
                    PickupAddressId = addrPickupId,
                    DropoffAddressId = addrDropId,
                    DistanceKm = 5.1m,
                    EtaMinutes = 45,
                    Status = status.Pending,
                    TotalAmount = 1_500_000m,
                    Note = "Yesterday pending",
                    CreatedAt = dtYesterday,
                    UpdatedAt = dtYesterday
                },
                new Order
                {
                    Id = orderToday1,
                    CustomerId = customerUser,
                    StoreId = storeId,
                    PickupAddressId = addrPickupId,
                    DropoffAddressId = addrDropId,
                    DistanceKm = 3.4m,
                    EtaMinutes = 35,
                    Status = status.Completed,
                    TotalAmount = 2_000_000m,
                    Note = "Today completed",
                    CreatedAt = dtToday.AddHours(-2),
                    UpdatedAt = dtToday.AddHours(-1)
                },
                new Order
                {
                    Id = orderToday2,
                    CustomerId = customerUser,
                    StoreId = storeId,
                    PickupAddressId = addrPickupId,
                    DropoffAddressId = addrDropId,
                    DistanceKm = 9.0m,
                    EtaMinutes = 70,
                    Status = status.Pending,
                    TotalAmount = 3_100_000m,
                    Note = "Today pending",
                    CreatedAt = dtToday.AddHours(-1),
                    UpdatedAt = dtToday.AddMinutes(-20)
                }
            );

            // ====== OrderItems (để dashboard lấy tên service) ======
            modelBuilder.Entity<OrderItem>().HasData(
                new OrderItem { Id = oi1, OrderId = orderLastM, ItemName = "Moving Service", ServiceId = svcMoveId, Quantity = 1, UnitPrice = 2_200_000m, Subtotal = 2_200_000m },
                new OrderItem { Id = oi2, OrderId = orderYest, ItemName = "Moving Service", ServiceId = svcMoveId, Quantity = 1, UnitPrice = 1_200_000m, Subtotal = 1_200_000m },
                new OrderItem { Id = oi3, OrderId = orderYest, ItemName = "Moving Service", ServiceId = svcPackId, Quantity = 1, UnitPrice = 300_000m, Subtotal = 300_000m },
                new OrderItem { Id = oi4, OrderId = orderToday1, ItemName = "Moving Service", ServiceId = svcMoveId, Quantity = 1, UnitPrice = 2_000_000m, Subtotal = 2_000_000m }
            // orderToday2 cố tình không thêm item để test fallback "None" nếu anh muốn,
            // nếu muốn có tên dịch vụ thì thêm 1 item nữa tương tự.
            );

            // ====== Feedback (để tính rating) ======
            modelBuilder.Entity<Feedback>().HasData(new Feedback
            {
                Id = feedbackId,
                OrderId = orderToday1,
                FromUserId = customerUser,
                ToStoreId = storeId,
                Rating = 5,
                Comment = "Very smooth job!",
                CreatedAt = dtToday
            });

            // ====== Warehouse & Slots (dashboard hiển thị 3 slots) ======
            modelBuilder.Entity<Warehouse>().HasData(new Warehouse
            {
                Id = whId,
                StoreId = storeId,
                Name = "Main Warehouse",
                AddressRefId = addrStoreId
            });

            modelBuilder.Entity<WarehouseSlot>().HasData(
                new WarehouseSlot { Id = slotA1, WarehouseId = whId, Code = "A1", Status = status.Available, CurrentOrderId = null },
                new WarehouseSlot { Id = slotA2, WarehouseId = whId, Code = "A2", Status = status.Reserved, CurrentOrderId = orderYest },
                new WarehouseSlot { Id = slotB1, WarehouseId = whId, Code = "B1", Status = status.Available, CurrentOrderId = null }
            );
        }
    }
}
