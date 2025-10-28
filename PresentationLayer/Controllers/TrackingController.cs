using DataAccessLayer.Entities;
using DataAccessLayer.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServiceLayer.Dtos.OrderTracking;
using System.Security.Claims;

namespace PresentationLayer.Controllers
{
    [Authorize]
    public class TrackingController : Controller
    {
        private readonly DeliverySytemContext _context;

        public TrackingController(DeliverySytemContext context)
        {
            _context = context;
        }

        // Danh sách đơn hàng đã gửi
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
                    StoreName = o.Store.StoreName,
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
                    PickupAddress = o.Pickup?.Ward is null ? "" : $"{o.Pickup.Ward}, {o.Pickup.District}, {o.Pickup.City}",
                    DropoffAddress = o.Dropoff?.Ward is null ? "" : $"{o.Dropoff.Ward}, {o.Dropoff.District}, {o.Dropoff.City}",
                    DeliveryDate = o.DeliveryDate
                })
                .ToList();

            ViewBag.CurrentSearch = search;
            ViewBag.CurrentStatus = status;
            ViewBag.FromDate = fromDate;
            ViewBag.ToDate = toDate;

            return View(orders);
        }

        // Chi tiết tracking đơn hàng
        [HttpGet]
        public async Task<IActionResult> Detail(Guid id)
        {
            var order = await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.Store)
                .Include(o => o.PickupAddress)
                .Include(o => o.DropoffAddress)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Service)
                .Include(o => o.Quotation)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            var viewModel = new OrderTrackingViewModel
            {
                Id = order.Id,
                OrderCode = $"ORD{order.CreatedAt:yyyyMMdd}{order.Id.ToString().Substring(0, 8).ToUpper()}",
                CustomerName = order.Customer.FullName,
                CustomerPhone = order.Customer.PhoneNumber,
                StoreName = order.Store.StoreName,
                PickupAddress = order.PickupAddress != null ?
                    $"{order.PickupAddress.AddressLine}" : "",
                DropoffAddress = order.DropoffAddress != null ?
                    $"{order.DropoffAddress.AddressLine}" : "",
                CreatedAt = order.CreatedAt,
                PickupDate = order.PickupDate,
                DeliveryDate = order.DeliveryDate,
                Status = order.Status,
                StatusDisplay = GetStatusDisplay(order.Status),
                StatusColor = GetStatusColor(order.Status),
                TotalAmount = order.TotalAmount,
                DistanceKm = order.DistanceKm,
                EtaMinutes = order.EtaMinutes,
                Note = order.Note,
                Items = order.OrderItems.Select(oi => new OrderItemTrackingViewModel
                {
                    ItemName = oi.ItemName,
                    Description = oi.Description,
                    Quantity = oi.Quantity,
                    SizeCode = oi.SizeCode,
                    WeightKg = oi.WeightKg,
                    VolumeM3 = oi.VolumeM3,
                    CategoryName = oi.Service?.Name,
                    UnitPrice = oi.UnitPrice,
                    Subtotal = oi.Subtotal
                }).ToList(),
                TrackingEvents = GetTrackingEvents(order),
                ContractInfo = GetContractInfo(order)
            };


            var st = order.Status;


            if ((st == StatusValue.InUse || st == StatusValue.Completed) && order.PickupDate.HasValue)
            {
                viewModel.CheckInTime = order.PickupDate;
            }
            else
            {

                if ((st == StatusValue.Approved || st == StatusValue.Reserved) && order.UpdatedAt.HasValue)
                    viewModel.CheckInTime = order.UpdatedAt;

            }

            if (st == StatusValue.Completed && order.DeliveryDate.HasValue)
            {
                viewModel.CheckOutTime = order.DeliveryDate;
            }
            else
            {

                if (order.DeliveryDate.HasValue)
                    viewModel.CheckOutTime = order.DeliveryDate;
            }

            if (viewModel.CheckInTime.HasValue && viewModel.CheckOutTime.HasValue
                && viewModel.CheckOutTime < viewModel.CheckInTime)
            {

                viewModel.CheckOutTime = null; 
            }

            return View(viewModel);
        }

        // Track by Order Code
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Track()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Track(string orderCode, string phoneNumber)
        {
            if (string.IsNullOrEmpty(orderCode) || string.IsNullOrEmpty(phoneNumber))
            {
                ModelState.AddModelError("", "Vui lòng nhập mã đơn hàng và số điện thoại");
                return View();
            }

            // Extract order ID from order code
            var orders = await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.Store)
                .Include(o => o.PickupAddress)
                .Include(o => o.DropoffAddress)
                .Where(o => o.Customer.PhoneNumber == phoneNumber)
                .ToListAsync();

            var order = orders.FirstOrDefault(o =>
                $"ORD{o.CreatedAt:yyyyMMdd}{o.Id.ToString().Substring(0, 8).ToUpper()}" == orderCode.ToUpper());

            if (order == null)
            {
                ModelState.AddModelError("", "Không tìm thấy đơn hàng với thông tin đã nhập");
                return View();
            }

            return RedirectToAction(nameof(PublicTracking), new { id = order.Id });
        }

        [AllowAnonymous]
        public async Task<IActionResult> PublicTracking(Guid id)
        {
            var order = await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.Store)
                .Include(o => o.PickupAddress)
                .Include(o => o.DropoffAddress)
                .Include(o => o.OrderItems)
                .Include(o => o.Quotation)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            var viewModel = new OrderTrackingViewModel
            {
                Id = order.Id,
                OrderCode = $"ORD{order.CreatedAt:yyyyMMdd}{order.Id.ToString().Substring(0, 8).ToUpper()}",
                CustomerName = order.Customer.FullName,
                StoreName = order.Store.StoreName,
                Status = order.Status,
                StatusDisplay = GetStatusDisplay(order.Status),
                StatusColor = GetStatusColor(order.Status),
                TrackingEvents = GetTrackingEvents(order),
                DeliveryDate = order.DeliveryDate,
                PickupDate = order.PickupDate
            };

            return View(viewModel);
        }

        // Helper methods
        private static bool IsOneOf(StatusValue s, params StatusValue[] states)
     => states.Contains(s);

        private List<TrackingEventViewModel> GetTrackingEvents(DataAccessLayer.Entities.Order order)
        {
            var events = new List<TrackingEventViewModel>();

            // 1) Luôn có: tạo đơn
            events.Add(new TrackingEventViewModel
            {
                EventTime = order.CreatedAt,
                EventType = "Tạo đơn",
                Description = "Đơn hàng đã được tạo",
                Icon = "fa-plus-circle",
                Color = "success"
            });

            var st = order.Status;

            // 2) Nhánh hủy/từ chối: kết thúc sớm
            if (IsOneOf(st, StatusValue.Canceled, StatusValue.Rejected))
            {
                events.Add(new TrackingEventViewModel
                {
                    EventTime = order.UpdatedAt ?? order.CreatedAt,
                    EventType = st == StatusValue.Canceled ? "Đã hủy" : "Từ chối",
                    Description = st == StatusValue.Canceled ? "Đơn hàng đã bị hủy" : "Đơn hàng bị từ chối",
                    Icon = st == StatusValue.Canceled ? "fa-times-circle" : "fa-ban",
                    Color = st == StatusValue.Canceled ? "dark" : "danger"
                });

                return events.OrderBy(e => e.EventTime).ToList();
            }

            // 3) Chờ xử lý (Pending) – coi như giai đoạn sau CreatedAt
            if (IsOneOf(st, StatusValue.Pending, StatusValue.Approved, StatusValue.Reserved, StatusValue.InUse, StatusValue.Completed))
            {
                events.Add(new TrackingEventViewModel
                {
                    EventTime = order.CreatedAt, // chưa có PendingAt => dùng CreatedAt
                    EventType = "Chờ xử lý",
                    Description = "Đơn hàng đang chờ kho xác nhận",
                    Icon = "fa-clock",
                    Color = "warning"
                });
            }

            // 4) Đã duyệt
            if (IsOneOf(st, StatusValue.Approved, StatusValue.Reserved, StatusValue.InUse, StatusValue.Completed))
            {
                events.Add(new TrackingEventViewModel
                {
                    EventTime = order.UpdatedAt ?? order.CreatedAt, // thiếu ApprovedAt => tạm dùng UpdatedAt
                    EventType = "Đã duyệt",
                    Description = "Kho đã xác nhận đơn hàng",
                    Icon = "fa-check-circle",
                    Color = "info"
                });
            }

            // 5) Đặt chỗ (nếu hệ thống có bước này)
            if (IsOneOf(st, StatusValue.Reserved, StatusValue.InUse, StatusValue.Completed))
            {
                events.Add(new TrackingEventViewModel
                {
                    EventTime = order.UpdatedAt ?? order.CreatedAt, // thiếu ReservedAt
                    EventType = "Đã đặt chỗ",
                    Description = "Đơn hàng đã được đặt chỗ tại kho",
                    Icon = "fa-bookmark",
                    Color = "secondary"
                });
            }

            // 6) Check-in & Lưu kho (InUse)
            if (IsOneOf(st, StatusValue.InUse, StatusValue.Completed))
            {
                // Check-in khi có PickupDate, nếu không thì fallback UpdatedAt
                var checkInTime = order.PickupDate ?? order.UpdatedAt ?? order.CreatedAt;

                events.Add(new TrackingEventViewModel
                {
                    EventTime = checkInTime,
                    EventType = "Check-in",
                    Description = "Hàng hóa đã được nhận vào kho",
                    Location = order.PickupAddress?.AddressLine,
                    Icon = "fa-sign-in-alt",
                    Color = "primary"
                });

                events.Add(new TrackingEventViewModel
                {
                    EventTime = checkInTime, // cùng mốc thời gian; không cộng phút ảo
                    EventType = "Lưu kho",
                    Description = "Hàng hóa đang được lưu trữ an toàn",
                    Icon = "fa-warehouse",
                    Color = "primary"
                });
            }

            // 7) Check-out & Hoàn thành
            if (st == StatusValue.Completed)
            {
                var checkoutTime = order.DeliveryDate ?? order.UpdatedAt ?? order.CreatedAt;

                events.Add(new TrackingEventViewModel
                {
                    EventTime = checkoutTime,
                    EventType = "Check-out",
                    Description = "Hàng hóa đã được trả cho khách",
                    Location = order.DropoffAddress?.AddressLine,
                    Icon = "fa-sign-out-alt",
                    Color = "success"
                });

                events.Add(new TrackingEventViewModel
                {
                    EventTime = checkoutTime,
                    EventType = "Hoàn thành",
                    Description = "Đơn hàng đã hoàn thành",
                    Icon = "fa-flag-checkered",
                    Color = "success"
                });
            }

            // 8) Sort theo thời gian (đưa null xuống sau nếu có)
            return events
                .OrderBy(e => e.EventTime)
                .ThenBy(e => e.EventType) // ổn định
                .ToList();
        }


        private ContractInfoViewModel GetContractInfo(DataAccessLayer.Entities.Order order)
        {
            if (order.Quotation == null)
                return null;

            var now = DateTime.Now;
            var daysRemaining = (order.Quotation.ValidUntil - now).Days;

            return new ContractInfoViewModel
            {
                QuotationId = order.Quotation.Id,
                ValidUntil = order.Quotation.ValidUntil,
                IsExpired = order.Quotation.ValidUntil < now,
                DaysRemaining = daysRemaining > 0 ? daysRemaining : 0,
                ContractStatus = order.Quotation.Status,
                ContractAmount = order.Quotation.TotalAmount
            };
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
