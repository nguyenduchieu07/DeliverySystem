using Microsoft.AspNetCore.Mvc;
using DataAccessLayer.Entities;
using DataAccessLayer.Enums;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using ServiceLayer.Dtos.OrderTracking;


namespace PresentationLayer.Areas.Stores.Controllers
{

    public class OrderController : Controller
    {

        private readonly DeliverySytemContext _context;

        public OrderController(DeliverySytemContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string search, StatusValue? status, DateTime? fromDate, DateTime? toDate)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var baseRows = await _context.Orders
                .AsNoTracking()
                .Include(o => o.Customer)
                .Include(o => o.Store)
                .Include(o => o.PickupAddress)
                .Include(o => o.DropoffAddress)
                .Where(o => o.Customer.Id.ToString() == userId)
                // Filters
                .Where(o => string.IsNullOrEmpty(search)
                            || o.Id.ToString().Contains(search)
                            || o.Store.StoreName.Contains(search))
                .Where(o => !status.HasValue || o.Status == status.Value)
                .Where(o => !fromDate.HasValue || o.CreatedAt >= fromDate.Value)
                .Where(o => !toDate.HasValue || o.CreatedAt <= toDate.Value.AddDays(1))
                .OrderByDescending(o => o.CreatedAt)
                .Select(o => new
                {
                    o.Id,
                    o.CreatedAt,
                    o.Store.StoreName,
                    o.Status,
                    o.TotalAmount,
                    Pickup = new { o.PickupAddress.Ward, o.PickupAddress.District, o.PickupAddress.City },
                    Dropoff = new { o.DropoffAddress.Ward, o.DropoffAddress.District, o.DropoffAddress.City },
                    o.DeliveryDate
                })
                .ToListAsync();

            var orders = baseRows
                .Select(o => new OrderListViewModel
                {
                    Id = o.Id,
                    OrderCode = $"ORD{o.CreatedAt:yyyyMMdd}{o.Id.ToString().Substring(0, 8).ToUpper()}",
                    CreatedAt = o.CreatedAt,
                    StoreName = o.StoreName,
                    Status = o.Status,
                    StatusDisplay = GetStatusDisplay(o.Status),
                    StatusColor = GetStatusColor(o.Status),
                    TotalAmount = o.TotalAmount,
                    PickupAddress = o.Pickup?.Ward is null
                        ? ""
                        : $"{o.Pickup.Ward}, {o.Pickup.District}, {o.Pickup.City}",
                    DropoffAddress = o.Dropoff?.Ward is null
                        ? ""
                        : $"{o.Dropoff.Ward}, {o.Dropoff.District}, {o.Dropoff.City}",
                    DeliveryDate = o.DeliveryDate
                })
                .ToList();

            ViewBag.CurrentSearch = search;
            ViewBag.CurrentStatus = status;
            ViewBag.FromDate = fromDate;
            ViewBag.ToDate = toDate;

            return View(orders);
        }
        private string GetStatusDisplay(StatusValue status)
        {
            return status switch
            {
                StatusValue.Pending => "Chờ xử lý",
                StatusValue.Approved => "Đã duyệt",
                StatusValue.InUse => "Đang lưu kho",
                StatusValue.Reserved => "Đã đặt chỗ",
                StatusValue.Completed => "Hoàn thành",
                StatusValue.Canceled => "Đã hủy",
                StatusValue.Rejected => "Từ chối",
                _ => status.ToString()
            };
        }

        private string GetStatusColor(StatusValue status)
        {
            return status switch
            {
                StatusValue.Pending => "warning",
                StatusValue.Approved => "info",
                StatusValue.InUse => "primary",
                StatusValue.Reserved => "secondary",
                StatusValue.Completed => "success",
                StatusValue.Canceled => "dark",
                StatusValue.Rejected => "danger",
                _ => "secondary"
            };
        }

    }
}
