using System.ComponentModel.DataAnnotations;
using DataAccessLayer.Abstractions.IRepositories;
using DataAccessLayer.Entities;
using DataAccessLayer.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PresentationLayer.Models;
using ServiceLayer.Abstractions.IServices;
using ServiceLayer.Helpers;

namespace PresentationLayer.Areas.Stores.Controllers
{
    [Area("Stores")]
    public class OrdersController : Controller
    {
        private readonly DeliverySytemContext _context;
        private readonly IOrderService _orderService;
        private readonly IBaseRepository<OrderWarehouseSlot, Guid> _orderWarehouseSlotRepository;
        private readonly ICloudinaryService _cloudinaryService;

        public OrdersController(IOrderService orderService,
            IBaseRepository<OrderWarehouseSlot, Guid> orderWarehouseSlotRepository, DeliverySytemContext context,
            ICloudinaryService cloudinaryService)
        {
            _orderService = orderService;
            _orderWarehouseSlotRepository = orderWarehouseSlotRepository;
            _context = context;
            _cloudinaryService = cloudinaryService;
        }


        public async Task<IActionResult> Index([FromQuery] StatusValue? status)
        {
            var vm = new StoreOrderIndexViewModel();

            var storeId = User.FindFirst("StoreId")?.Value ?? "";

            var orders = await _orderService.GetAllOrdersByStoreIdAsync(Guid.Parse(storeId), status);
            vm.Orders = orders;
            vm.TotalElements = orders.Count;

            return View(vm);
        }

        public class StoreOrderDetailViewModel
        {
            public Order Order { get; set; } = null!;
            public List<OrderItem> OrderItems { get; set; } = new();
            public Customer? Customer { get; set; }
            public Address? PickupAddress { get; set; }
            public Address? DropoffAddress { get; set; }
        }

        public async Task<IActionResult> Detail(Guid id)
        {
            var order = await _orderService.GetOrderInfoByIdAsync(id);

            if (order == null)
                return NotFound();

            var vm = new StoreOrderDetailViewModel
            {
                Order = order,
                OrderItems = order.OrderItems.ToList(),
                Customer = order.Customer,
                PickupAddress = order.PickupAddress,
                DropoffAddress = order.DropoffAddress,
            };

            return View(vm);
        }

        public class UpdateOrderRequest
        {
            [Required(ErrorMessage = "Trạng thái là bắt buộc")]
            public StatusValue Status { get; set; }

            [StringLength(500, ErrorMessage = "Ghi chú không được vượt quá 500 ký tự")]
            public string? Note { get; set; }

            [Required(ErrorMessage = "Ngày gửi hàng là bắt buộc")]
            public DateTime PickupDate { get; set; }

            [Required(ErrorMessage = "Ngày giao hàng là bắt buộc")]
            [DateGreaterThan(nameof(PickupDate), ErrorMessage = "Ngày giao hàng phải sau ngày gửi hàng")]
            public DateTime DeliveryDate { get; set; }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateOrder(Guid orderId, UpdateOrderRequest data)
        {
            if (!ModelState.IsValid)
            {
                var firstError = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .FirstOrDefault();

                return Json(new { success = false, message = firstError });
            }

            var order = await _orderService.GetByIdAsync(orderId);
            if (order == null)
                return Json(new { success = false, message = "Không tìm thấy đơn hàng" });

            OrderWarehouseSlot orderWarehouseSlot;
            try
            {
                orderWarehouseSlot =
                    await _orderWarehouseSlotRepository.FindSingleAsync(x => x.OrderId.Equals(orderId));
            }
            catch (InvalidOperationException invalidEx)
            {
                return Json(new { success = false, message = invalidEx.Message });
            }
            catch (Exception e)
            {
                return Json(new { success = false, message = e.Message });
            }

            if (data.PickupDate > orderWarehouseSlot.AssignedAt)
            {
                return Json(
                    new { success = false, message = "Ngày lấy hàng phải trước hoặc bằng ngày bắt đầu lưu kho" });
            }

            if (data.DeliveryDate > orderWarehouseSlot.ReleasedAt)
            {
                return Json(new { success = false, message = "Ngày giao hàng phải trước ngày kết thúc lưu kho" });
            }

            bool IsValidTransition(StatusValue current, StatusValue next) =>
                current switch
                {
                    StatusValue.Draft => next is StatusValue.Pending or StatusValue.Canceled,
                    StatusValue.Pending => next is StatusValue.Approved or StatusValue.Rejected or StatusValue.Canceled,
                    StatusValue.Approved => next is StatusValue.Completed or StatusValue.Canceled,
                    StatusValue.Completed => false, // completed rồi thì không đổi nữa
                    StatusValue.Rejected => false, // rejected thì không phục hồi
                    StatusValue.Canceled => false, // canceled thì không quay lại
                    _ => false
                };

            var currentStatus = order.Status;
            var newStatus = data.Status;
            if (!IsValidTransition(currentStatus, newStatus))
                return Json(new
                {
                    success = false,
                    message =
                        $"Không thể chuyển từ trạng thái {currentStatus.ToDisplayStringForOrder()} sang {newStatus.ToDisplayStringForOrder()}"
                });

            order.Status = data.Status;
            order.Note = data.Note;
            order.PickupDate = data.PickupDate;
            order.DeliveryDate = data.DeliveryDate;

            _orderService.UpdateOrder(order);

            return Json(new { success = true });
        }

        public class UpdateUpdateOrderWarehouseSlotRequest
        {
            [Required(ErrorMessage = "Ngày lưu kho là bắt buộc")]
            public DateTime AssignedAt { get; set; }

            [Required(ErrorMessage = "Ngày kết thúc là bắt buộc")]
            [DateGreaterThan(nameof(AssignedAt), ErrorMessage = "Ngày kết thúc phải sau ngày lưu kho")]
            public DateTime ReleasedAt { get; set; }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateOrderWarehouseSlot(Guid orderId,
            UpdateUpdateOrderWarehouseSlotRequest data)
        {
            if (!ModelState.IsValid)
            {
                var firstError = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .FirstOrDefault();

                return Json(new { success = false, message = firstError });
            }

            OrderWarehouseSlot orderWarehouseSlot;
            try
            {
                orderWarehouseSlot =
                    await _orderWarehouseSlotRepository.FindSingleAsync(x => x.OrderId.Equals(orderId));
            }
            catch (InvalidOperationException invalidEx)
            {
                return Json(new { success = false, message = invalidEx.Message });
            }
            catch (Exception e)
            {
                return Json(new { success = false, message = e.Message });
            }

            var order = await _orderService.GetByIdAsync(orderId);
            if (order == null) return Json(new { success = false, message = "Không tìm thấy đơn hàng" });

            if (order.PickupDate.HasValue && data.AssignedAt < order.PickupDate)
            {
                return Json(new { success = false, message = "Ngày lưu kho phải sau hoặc bằng ngày lấy hàng" });
            }

            if (order.DeliveryDate.HasValue && data.ReleasedAt < order.DeliveryDate)
            {
                return Json(new { success = false, message = "Ngày kết thúc phải trước hoặc bằng ngày giao hàng" });
            }

            orderWarehouseSlot.AssignedAt = data.AssignedAt;
            orderWarehouseSlot.ReleasedAt = data.ReleasedAt;

            _orderWarehouseSlotRepository.Update(orderWarehouseSlot);

            return Json(new { success = true });
        }

        public class IncidentReportCreateViewModel
        {
            [Required] public Guid OrderId { get; set; }

            [Required] public Guid OrderItemId { get; set; }

            [Required] public ItemReportType ItemReportType { get; set; }

            [Required] [StringLength(1000)] public string Description { get; set; } = string.Empty;

            public bool IsReturned { get; set; }

            public bool IsCompensated { get; set; }

            [Range(0, double.MaxValue)] public decimal? CompensationAmount { get; set; }
        }

        [HttpPost("/Stores/Orders/CreateReport")]
        public async Task<IActionResult> CreateReport([FromForm] IncidentReportCreateViewModel request,
            IFormFile? imageFile)
        {
            try
            {
                var report = new ItemReport
                {
                    Id = Guid.NewGuid(),
                    // OrderId = request.OrderId,
                    OrderItemId = request.OrderItemId,
                    Type = request.ItemReportType,
                    Description = request.Description,
                    IsReturned = request.IsReturned,
                    IsCompensated = request.IsCompensated,
                    CompensationAmount = request.CompensationAmount,
                    CreatedAt = DateTime.UtcNow
                };

                if (imageFile != null && imageFile.Length > 0)
                {
                    report.ImageUrl = await _cloudinaryService.UploadImageFileAsync(imageFile);
                }

                _context.IncidentReports.Add(report);
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Tạo báo cáo thành công", data = report });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Lỗi khi tạo báo cáo", error = ex.Message });
            }
        }

        public partial class HandleReportViewModel
        {
            public Guid OrderItemId { get; set; }
            public ItemReportActionType ActionType { get; set; }
            public string? Note { get; set; }
        }

        [HttpPost("/Stores/Orders/HandleIncident")]
        public async Task<IActionResult> HandleIncident([FromForm] HandleReportViewModel model)
        {
            await using var ts = await _context.Database.BeginTransactionAsync();
            try
            {
                if (!ModelState.IsValid)
                    return Json(new { success = false, message = "Dữ liệu không hợp lệ." });

                var report = await _context.IncidentReports
                    .FirstOrDefaultAsync(x => x.OrderItemId == model.OrderItemId && x.Status != ReportStatus.Done);

                if (report == null)
                    return Json(new { success = false, message = "Không tìm thấy báo cáo." });

                var action = new ItemReportAction
                {
                    Id = Guid.NewGuid(),
                    ItemReportId = report.Id,
                    ActionType = model.ActionType,
                    Note = model.Note,
                    CreatedAt = DateTime.Now
                };

                _context.IncidentActions.Add(action);

                if (model.ActionType == ItemReportActionType.Close)
                {
                    report.Status = ReportStatus.Done;
                    // Không cần _context.IncidentReports.Update(report);
                }

                await _context.SaveChangesAsync();
                await ts.CommitAsync();

                return Json(new { success = true, message = "Xử lý sự cố thành công!" });
            }
            catch (Exception ex)
            {
                await ts.RollbackAsync();
                return Json(new { success = false, message = "Lỗi xử lý sự cố.", error = ex.Message });
            }
        }

     
    }
}