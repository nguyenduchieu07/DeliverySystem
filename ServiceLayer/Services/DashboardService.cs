using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using PresentationLayer.Areas.Stores.Models;
using ServiceLayer.Abstractions.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Status = DataAccessLayer.Enums.StatusValue;
namespace ServiceLayer.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly DeliverySytemContext _context;
        public DashboardService(DeliverySytemContext deliverySytemContext)
        {
            _context = deliverySytemContext;
        }
        public async Task<DashboardDto> GetDashboard(Guid storeId)
        {

            var todayStart = DateTime.Today;
            var tomorrowStart = todayStart.AddDays(1);
            var yesterdayStart = todayStart.AddDays(-1);

            var monthStart = new DateTime(todayStart.Year, todayStart.Month, 1);
            var nextMonthStart = monthStart.AddMonths(1);

            var prevMonthStart = monthStart.AddMonths(-1);

            var orders = _context.Orders
                .AsNoTracking()
                .Where(o => o.StoreId == storeId);


            var agg = await orders
                .GroupBy(_ => 1)
                .Select(g => new
                {
                    PendingCount = g.Sum(o => o.Status == Status.Pending ? 1 : 0),

                    TodayCount = g.Sum(o =>
                        (o.CreatedAt >= todayStart && o.CreatedAt < tomorrowStart) ? 1 : 0),

                    YesterdayCount = g.Sum(o =>
                        (o.CreatedAt >= yesterdayStart && o.CreatedAt < todayStart) ? 1 : 0),

                    RevenueThisMonth = g.Sum(o =>
                        (o.CreatedAt >= monthStart && o.CreatedAt < nextMonthStart) ? o.TotalAmount : 0m),

                    RevenueLastMonth = g.Sum(o =>
                        (o.CreatedAt >= prevMonthStart && o.CreatedAt < monthStart) ? o.TotalAmount : 0m)
                })
                .FirstOrDefaultAsync() ?? new
                {
                    PendingCount = 0,
                    TodayCount = 0,
                    YesterdayCount = 0,
                    RevenueThisMonth = 0m,
                    RevenueLastMonth = 0m
                };

            var ratingAgg = await _context.Feedbacks
                .AsNoTracking()
                .Where(f => f.Order.StoreId == storeId)
                .GroupBy(_ => 1)
                .Select(g => new
                {
                    Avg = (double?)g.Average(f => (double?)f.Rating) ?? 0.0,
                    Total = g.Count()
                })
                .FirstOrDefaultAsync() ?? new { Avg = 0.0, Total = 0 };


            var pendingOrders = await orders
                .Where(o => o.Status == Status.Pending)
                .OrderByDescending(o => o.CreatedAt)
                .Select(o => new OrderPeding
                {
                    CreatedAt = o.CreatedAt,
                    CustomerName = o.Customer.FullName,
                    Id = o.Id.ToString(),

                    Name = o.OrderItems
                                      .OrderBy(oi => oi.Id)
                                      .Select(oi => oi.Service.Name)
                                      .FirstOrDefault() ?? "None",
                    Status = o.Status,
                    Total = o.TotalAmount
                })
                .Take(10)
                .ToListAsync();


            var today = agg.TodayCount;
            var yesterday = agg.YesterdayCount;
            double todayGrowthRate = (yesterday > 0)
                ? ((today - yesterday) / (double)yesterday * 100.0)
                : (today > 0 ? 100.0 : 0.0);

            var thisRev = agg.RevenueThisMonth;
            var lastRev = agg.RevenueLastMonth;
            double saleGrowthRatio = (lastRev > 0m)
                ? (double)((thisRev - lastRev) / lastRev * 100m)
                : (thisRev > 0m ? 100.0 : 0.0);

            var reports = new DashboardDto
            {
                TotalOrderPending = agg.PendingCount,

                OrderToday = new Dtos.OrderToday
                {
                    Total = today,
                    GrowthRate = todayGrowthRate
                },

                Revenue = new Dtos.Revenue
                {
                    Total = thisRev,
                    SaleGrowthRatio = (decimal)saleGrowthRatio
                },

                AvarageRating = new Dtos.RatingDto
                {
                    Average = ratingAgg.Avg,
                    Total = ratingAgg.Total
                },

                OrderPendings = pendingOrders
            };


            var warehouse = await _context.Warehouses
                .AsNoTracking()
                .Where(w => w.StoreId == storeId)
                .Select(w => new WareHouseDashboard
                {
                    Address = (
                        (w.Address.City ?? "") + ", " +
                        (w.Address.District ?? "") + " - " +
                        (w.Address.Ward ?? "")
                    ).Trim(new[] { ' ', ',', '-' }),
                    Slots = w.Slots
                             .OrderBy(s => s.Code)
                             .Select(s => new WareHouseSlotDashboard
                             {
                                 Code = s.Code,
                                 Id = s.Id,
                                 Status = s.Status,
                             })
                             .Take(6)
                             .ToList()
                })
                .Take(3)
                .ToListAsync();

            reports.WareHouseDashboards = warehouse;

            return reports;
        }
    }
}
