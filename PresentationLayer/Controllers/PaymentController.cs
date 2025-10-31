using DataAccessLayer.Entities;
using DataAccessLayer.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PresentationLayer.Models;
using System.Security.Claims;

namespace PresentationLayer.Controllers
{
    public class PaymentController : Controller
    {
        private readonly DeliverySytemContext _context; 

        public PaymentController(DeliverySytemContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(Guid orderId)
        {
            var order = await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.PickupAddress)
                .Include(o => o.DropoffAddress)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Service)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null)
            {
                TempData["Error"] = "Không tìm thấy đơn hàng";
                return RedirectToAction("Index", "Home");
            }

            // Kiểm tra quyền truy cập
            var currentUserId =  GetCurrentCustomerId();
            if (order.CustomerId != currentUserId)
            {
                TempData["Error"] = "Bạn không có quyền truy cập đơn hàng này";
                return RedirectToAction("Index", "Home");
            }

            // Kiểm tra trạng thái đơn hàng
            if (order.Status != StatusValue.AwaitingPayment)
            {
                TempData["Error"] = "Đơn hàng chưa được báo giá hoặc đã được xử lý";
                return RedirectToAction("Details", "Order", new { id = orderId });
            }

            var viewModel = new PaymentViewModel
            {
                OrderId = orderId,
                TotalAmount = order.TotalAmount,
                OrderSummary = new OrderSummaryViewModel
                {
                    OrderId = order.Id,
                    CustomerName = order.Customer.FullName,
                    CustomerPhone = order.Customer.PhoneNumber,
                    PickupAddress = order.PickupAddress?.AddressLine,
                    DropoffAddress = order.DropoffAddress?.AddressLine,
                    DeliveryDate = order.DeliveryDate,
                    PickupDate = order.PickupDate,
                    DistanceKm = order.DistanceKm,
                    EtaMinutes = order.EtaMinutes,
                    TotalAmount = order.TotalAmount,
                    Status = order.Status,
                    Note = order.Note,
                    OrderItems = order.OrderItems.Select(oi => new OrderItemSummaryViewModel
                    {
                        ItemName = oi.ItemName,
                        Description = oi.Description,
                        Quantity = oi.Quantity,
                        LengthM = oi.LengthM,
                        WidthM = oi.WidthM,
                        HeightM = oi.HeightM,
                        WeightKg = oi.WeightKg,
                        SizeCode = oi.SizeCode,
                        UnitPrice = oi.UnitPrice,
                        Subtotal = oi.Subtotal
                    }).ToList()
                }
            };

            return View(viewModel);
        }

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Process(PaymentViewModel model)
        {
            

            var order = await _context.Orders.FindAsync(model.OrderId);
            if (order == null)
            {
                return Json(new PaymentResultViewModel
                {
                    IsSuccess = false,
                    Message = "Không tìm thấy đơn hàng"
                });
            }

            // Kiểm tra quyền truy cập
            var currentUserId = GetCurrentCustomerId();
            if (order.CustomerId != currentUserId)
            {
                return Json(new PaymentResultViewModel
                {
                    IsSuccess = false,
                    Message = "Bạn không có quyền truy cập đơn hàng này"
                });
            }

            try
            {
                var payment = new Payment
                {
                    Id = Guid.NewGuid(),
                    OrderId = model.OrderId,
                    Amount = model.TotalAmount,
                    Method = model.PaymentMethod,
                    Provider = model.Provider,
                    Status = StatusValue.Pending,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                // Xử lý theo phương thức thanh toán
                var result = await ProcessPaymentByMethod(payment, model);

                if (result.IsSuccess)
                {
                    _context.Payments.Add(payment);

                    // Cập nhật trạng thái đơn hàng
                    order.Status = payment.Status == StatusValue.Completed
                        ? StatusValue.AwaitingPickup
                        : StatusValue.AwaitingPayment;
                    order.UpdatedAt = DateTime.UtcNow;

                    await _context.SaveChangesAsync();

                    result.PaymentId = payment.Id;
                }

                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(new PaymentResultViewModel
                {
                    IsSuccess = false,
                    Message = "Có lỗi xảy ra trong quá trình thanh toán: " + ex.Message
                });
            }
        }

        // GET: Payment/Success/{paymentId}
        public async Task<IActionResult> Success(Guid paymentId)
        {
            var payment = await _context.Payments
                .Include(p => p.Order)
                    .ThenInclude(o => o.Customer)
                .FirstOrDefaultAsync(p => p.Id == paymentId);

            if (payment == null)
            {
                TempData["Error"] = "Không tìm thấy thông tin thanh toán";
                return RedirectToAction("Index", "Home");
            }

            return View(payment);
        }

        // GET: Payment/Failed/{paymentId}
        public async Task<IActionResult> Failed(Guid paymentId)
        {
            var payment = await _context.Payments
                .Include(p => p.Order)
                .FirstOrDefaultAsync(p => p.Id == paymentId);

            if (payment == null)
            {
                TempData["Error"] = "Không tìm thấy thông tin thanh toán";
                return RedirectToAction("Index", "Home");
            }

            return View(payment);
        }

        // POST: Payment/Retry/{paymentId}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Retry(Guid paymentId)
        {
            var payment = await _context.Payments
                .Include(p => p.Order)
                .FirstOrDefaultAsync(p => p.Id == paymentId);

            if (payment == null)
            {
                return Json(new PaymentResultViewModel
                {
                    IsSuccess = false,
                    Message = "Không tìm thấy thông tin thanh toán"
                });
            }

            // Chuyển hướng về trang thanh toán
            return Json(new PaymentResultViewModel
            {
                IsSuccess = true,
                Message = "Đang chuyển hướng...",
                RedirectUrl = Url.Action("Index", new { orderId = payment.OrderId })
            });
        }

        private async Task<PaymentResultViewModel> ProcessPaymentByMethod(Payment payment, PaymentViewModel model)
        {
            switch (model.PaymentMethod.ToLower())
            {
                case "cash":
                    return await ProcessCashPayment(payment);

                case "bank_transfer":
                    return await ProcessBankTransfer(payment, model.Provider);

                case "momo":
                    return await ProcessMoMoPayment(payment);

                case "zalopay":
                    return await ProcessZaloPayPayment(payment);

                case "vnpay":
                    return await ProcessVNPayPayment(payment);

                default:
                    return new PaymentResultViewModel
                    {
                        IsSuccess = false,
                        Message = "Phương thức thanh toán không được hỗ trợ"
                    };
            }
        }

        private async Task<PaymentResultViewModel> ProcessCashPayment(Payment payment)
        {
            // Thanh toán tiền mặt - chỉ cần tạo record, sẽ thu tiền khi giao hàng
            payment.Status = StatusValue.Pending;
            payment.ProviderTxnId = $"CASH_{DateTime.UtcNow:yyyyMMddHHmmss}";

            return new PaymentResultViewModel
            {
                IsSuccess = true,
                Message = "Đã xác nhận thanh toán tiền mặt. Bạn sẽ thanh toán khi nhận hàng."
            };
        }

        private async Task<PaymentResultViewModel> ProcessBankTransfer(Payment payment, string? provider)
        {
            // Chuyển khoản ngân hàng - tạo thông tin chuyển khoản
            payment.Status = StatusValue.Pending;
            payment.Provider = provider ?? "Manual Bank Transfer";
            payment.ProviderTxnId = $"BANK_{DateTime.UtcNow:yyyyMMddHHmmss}";

            return new PaymentResultViewModel
            {
                IsSuccess = true,
                Message = "Vui lòng chuyển khoản theo thông tin được cung cấp và gửi ảnh xác nhận."
            };
        }

        private async Task<PaymentResultViewModel> ProcessMoMoPayment(Payment payment)
        {
            // Tích hợp MoMo API (giả lập)
            payment.Status = StatusValue.Pending;
            payment.Provider = "MoMo";
            payment.ProviderTxnId = $"MOMO_{Guid.NewGuid().ToString("N")[..10].ToUpper()}";

            // Trong thực tế, bạn sẽ gọi MoMo API ở đây
            var momoPaymentUrl = $"https://test-payment.momo.vn/gw_payment/transactionProcessor?partnerCode=YOUR_PARTNER_CODE&orderId={payment.ProviderTxnId}&amount={payment.Amount}";

            return new PaymentResultViewModel
            {
                IsSuccess = true,
                Message = "Đang chuyển hướng đến MoMo...",
                RedirectUrl = momoPaymentUrl
            };
        }

        private async Task<PaymentResultViewModel> ProcessZaloPayPayment(Payment payment)
        {
            // Tích hợp ZaloPay API (giả lập)
            payment.Status = StatusValue.Pending;
            payment.Provider = "ZaloPay";
            payment.ProviderTxnId = $"ZALO_{Guid.NewGuid().ToString("N")[..10].ToUpper()}";

            return new PaymentResultViewModel
            {
                IsSuccess = true,
                Message = "Đang chuyển hướng đến ZaloPay...",
                RedirectUrl = $"https://zalopay.vn/payment/{payment.ProviderTxnId}"
            };
        }

        private async Task<PaymentResultViewModel> ProcessVNPayPayment(Payment payment)
        {
            // Tích hợp VNPay API (giả lập)
            payment.Status = StatusValue.Pending;
            payment.Provider = "VNPay";
            payment.ProviderTxnId = $"VNP_{Guid.NewGuid().ToString("N")[..10].ToUpper()}";

            return new PaymentResultViewModel
            {
                IsSuccess = true,
                Message = "Đang chuyển hướng đến VNPay...",
                RedirectUrl = $"https://sandbox.vnpayment.vn/paymentv2/vpcpay.html?vnp_TxnRef={payment.ProviderTxnId}"
            };
        }

        private async Task ReloadOrderData(PaymentViewModel model)
        {
            var order = await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.PickupAddress)
                .Include(o => o.DropoffAddress)
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == model.OrderId);

            if (order != null)
            {
                model.OrderSummary = new OrderSummaryViewModel
                {
                    OrderId = order.Id,
                    CustomerName = order.Customer.FullName,
                    CustomerPhone = order.Customer.PhoneNumber,
                    PickupAddress = order.PickupAddress?.AddressLine,
                    DropoffAddress = order.DropoffAddress?.AddressLine,
                    DeliveryDate = order.DeliveryDate,
                    PickupDate = order.PickupDate,
                    DistanceKm = order.DistanceKm,
                    EtaMinutes = order.EtaMinutes,
                    TotalAmount = order.TotalAmount,
                    Status = order.Status,
                    Note = order.Note,
                    OrderItems = order.OrderItems.Select(oi => new OrderItemSummaryViewModel
                    {
                        ItemName = oi.ItemName,
                        Description = oi.Description,
                        Quantity = oi.Quantity,
                        LengthM = oi.LengthM,
                        WidthM = oi.WidthM,
                        HeightM = oi.HeightM,
                        WeightKg = oi.WeightKg,
                        SizeCode = oi.SizeCode,
                        UnitPrice = oi.UnitPrice,
                        Subtotal = oi.Subtotal
                    }).ToList()
                };
            }
        }
    }
}
