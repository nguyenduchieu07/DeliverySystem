using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using PresentationLayer.Areas.Stores.Models;
using ServiceLayer.Abstractions.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            var statisticQuery = _context.Orders.Where(e => e.StoreId == storeId).AsNoTracking();
            var today = DateTime.Today;
            var yesterday = today.AddDays(-1);
            var lastMonth = today.AddMonths(-1);

            var reports = await statisticQuery.Select(e => new DashboardDto
            {
                TotalOrderPending = statisticQuery.Where(e => e.Status.Equals("pending")).Count(),
                OrderToday = new Dtos.OrderToday
                {
                    Total = statisticQuery.Where(e => e.CreatedAt.Date == today).Count(),
                    GrowthRate = (statisticQuery.Where(e => e.CreatedAt.Date == today).Count() - statisticQuery.Where(e => e.CreatedAt.Date == yesterday).Count()) / 100.0 * 100.0
                },

                Revenue = new Dtos.Revenue
                {
                    Total = statisticQuery
                            .Where(e => e.CreatedAt.Month == today.Month
                                      && e.CreatedAt.Year == today.Year)
                            .Sum(e => e.TotalAmount),
                    SaleGrowthRatio = (statisticQuery.Where(e => e.CreatedAt.Month == today.Month && e.CreatedAt.Year == today.Year).Sum(e => e.TotalAmount)

                                      - statisticQuery.Where(e => e.CreatedAt.Month == lastMonth.Month && e.CreatedAt.Year == lastMonth.Year).Sum(e => e.TotalAmount))

                                      / 100 * 100
                },

                AvarageRating = new Dtos.RatingDto
                {
                    Average = statisticQuery
                               .SelectMany(e => e.Feedbacks)
                               .Average(e => e.Rating),
                    Total = statisticQuery.SelectMany(e => e.Feedbacks).Count()
                },

                OrderPendings = statisticQuery.Select(order => new OrderPeding
                {
                    CreatedAt = order.CreatedAt,
                    CustomerName = order.Customer.FullName,
                    Id = order.Id.ToString(),
                    Name = order.OrderItems.FirstOrDefault()!.Service.Name ?? "None",
                    Status = order.Status,
                    Total = order.TotalAmount
                }).ToList(),

               
            }).FirstOrDefaultAsync();
            var warehouse =await _context.Warehouses
                                    .Where(e => e.StoreId == storeId).AsNoTracking()
                                    .Select(e => new WareHouseDashboard
                                    {
                                        Address = $"{e.Address.City}, {e.Address.District} - {e.Address.Ward}",
                                        Slots = e.Slots.Select(slot => new WareHouseSlotDashboard
                                        {
                                            Code = slot.Code,
                                            Id = slot.Id,
                                            Status = slot.Status,
                                        }).ToList()
                                    })
                                    .ToListAsync();
            reports.WareHouseDashboards = warehouse;
            return reports;
        }
    }
}
