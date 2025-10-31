using DataAccessLayer.Entities;
using DataAccessLayer.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServiceLayer.Dtos.Quotes;
using System.Security.Claims;

namespace PresentationLayer.Controllers
{
    public class QuotationController : Controller
    {
        private readonly DeliverySytemContext _context;

        public QuotationController(DeliverySytemContext context)
        {
            _context = context;
        }

        // GET: /Quotation/Index
        // Hiển thị danh sách báo giá của khách hàng
        public async Task<IActionResult> Index(string status = "all")
        {
            var customerId = GetCurrentCustomerId();
            if (customerId == Guid.Empty)
            {
                return RedirectToAction("Login", "Account");
            }

            var query = _context.Quotations
                .Include(q => q.Store)
                .Where(q => q.CustomerId == customerId);

            // Lọc theo trạng thái
            query = status.ToLower() switch
            {
                "pending" => query.Where(q => q.Status == StatusValue.Pending),
                "approved" => query.Where(q => q.Status == StatusValue.Approved),
                "rejected" => query.Where(q => q.Status == StatusValue.Rejected),
                "expired" => query.Where(q => q.Status == StatusValue.Expired || q.ValidUntil < DateTime.Now),
                _ => query
            };

            var quotations = await query
                .OrderByDescending(q => q.CreatedAt)
                .Select(q => new QuotationListDto
                {
                    Id = q.Id,
                    StoreName = q.Store.StoreName,
                    TotalAmount = q.TotalAmount,
                    ValidUntil = q.ValidUntil,
                    Status = q.Status,
                    CreatedAt = q.CreatedAt,
                    StoreRatingAvg = q.Store.RatingAvg,
                    StoreRatingCount = q.Store.RatingCount,
                    StoreContactPhone = q.Store.ContactPhone,
                    ServiceCount = q.Store.Services.Count
                })
                .ToListAsync();

            ViewBag.CurrentFilter = status;
            ViewBag.Title = "Danh Sách Báo Giá";

            return View(quotations);
        }

        public async Task<IActionResult> Detail(Guid id)
        {
            var customerId = GetCurrentCustomerId();
            if (customerId == Guid.Empty)
            {
                return RedirectToAction("Login", "Account");
            }

            var quotation = await _context.Quotations
                .Include(q => q.Store)
                    .ThenInclude(s => s.Services)
                        .ThenInclude(svc => svc.Category)
                .Include(q => q.Store)
                    .ThenInclude(s => s.Services)
                        .ThenInclude(svc => svc.Addons)
                .Include(q => q.Customer)
                .Include(q => q.Orders)
                .FirstOrDefaultAsync(q => q.Id == id && q.CustomerId == customerId);

            if (quotation == null)
            {
                TempData["Error"] = "Không tìm thấy báo giá";
                return RedirectToAction(nameof(Index));
            }

            var detailDto = new QuotationDetailDto
            {
                Id = quotation.Id,
                StoreId = (Guid)quotation.StoreId,
                CustomerId = quotation.CustomerId,
                TotalAmount = quotation.TotalAmount,
                ValidUntil = quotation.ValidUntil,
                Status = quotation.Status,
                CreatedAt = quotation.CreatedAt,

                StoreName = quotation.Store.StoreName,
                StoreContactPhone = quotation.Store.ContactPhone,
                StoreContactEmail = quotation.Store.ContactEmail,
                StoreRatingAvg = quotation.Store.RatingAvg,
                StoreRatingCount = quotation.Store.RatingCount,

                CustomerName = quotation.Customer.FullName,
                CustomerPhone = quotation.Customer.PhoneNumber,
                CustomerEmail = quotation.Customer.Email,

                Services = quotation.Store.Services.Select(s => new QuotationServiceDto
                {
                    ServiceId = s.Id,
                    ServiceName = s.Name,
                    Description = s.Description,
                    Unit = s.Unit,
                    BasePrice = s.BasePrice,
                    Quantity = 1, // Có thể lấy từ OrderItem nếu có
                    Subtotal = s.BasePrice,
                    CategoryName = s.Category?.Name,
                    Addons = s.Addons.Select(a => a.Name).ToList()
                }).ToList(),

                RelatedOrderId = quotation.Orders.FirstOrDefault()?.Id
            };

            ViewBag.Title = "Chi Tiết Báo Giá";
            return View(detailDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Accept(Guid id, string note)
        {
            var customerId = GetCurrentCustomerId();
            if (customerId == Guid.Empty)
                return Json(new { success = false, message = "Vui lòng đăng nhập" });

            var quotation = await _context.Quotations
                .FirstOrDefaultAsync(q => q.Id == id && q.CustomerId == customerId);

            if (quotation == null)
                return Json(new { success = false, message = "Không tìm thấy báo giá" });

            if (quotation.Status != StatusValue.Pending)
                return Json(new { success = false, message = "Báo giá đã được xử lý" });

            if (quotation.ValidUntil < DateTime.Now)
                return Json(new { success = false, message = "Báo giá đã hết hạn" });

            // Cập nhật báo giá
            quotation.Status = StatusValue.Approved;
            quotation.UpdatedAt = DateTime.Now;

            // Tạo đơn hàng ở trạng thái chờ thanh toán
            var order = new Order
            {
                Id = Guid.NewGuid(),
                CustomerId = customerId,
                StoreId = quotation.StoreId ?? Guid.Empty,
                QuotationId = quotation.Id,
                TotalAmount = quotation.TotalAmount,
                Status = StatusValue.AwaitingPayment, // 👈 quan trọng để qua Payment
                Note = note,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            var redirectUrl = Url.Action("Index", "Payment", new { orderId = order.Id });

            return Json(new
            {
                success = true,
                message = "Chấp nhận thành công, chuyển sang thanh toán.",
                orderId = order.Id,
                redirectUrl
            });
        }


        // POST: /Quotation/Reject
        // Từ chối báo giá
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reject(Guid id, string note)
        {
            var customerId = GetCurrentCustomerId();
            if (customerId == Guid.Empty)
            {
                return Json(new { success = false, message = "Vui lòng đăng nhập" });
            }

            var quotation = await _context.Quotations
                .FirstOrDefaultAsync(q => q.Id == id && q.CustomerId == customerId);

            if (quotation == null)
            {
                return Json(new { success = false, message = "Không tìm thấy báo giá" });
            }

            if (quotation.Status != StatusValue.Pending)
            {
                return Json(new { success = false, message = "Báo giá đã được xử lý" });
            }

            quotation.Status = StatusValue.Rejected;
            quotation.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();

            TempData["Success"] = "Đã từ chối báo giá";
            return Json(new { success = true, message = "Từ chối thành công" });
        }

        // Helper: Lấy CustomerId từ User đang đăng nhập
        private Guid GetCurrentCustomerId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (Guid.TryParse(userIdClaim, out var userId))
            {
                var customer = _context.Customers.FirstOrDefault(c => c.Id == userId);
                return customer?.Id ?? Guid.Empty;
            }
            return Guid.Empty;
        }
    }
}
