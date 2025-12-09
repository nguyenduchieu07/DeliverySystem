using DataAccessLayer.Entities;
using DataAccessLayer.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ServiceLayer.Abstractions.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly DeliverySytemContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<PaymentService> _logger;

        public PaymentService(
            DeliverySytemContext context,
            IConfiguration configuration,
            ILogger<PaymentService> logger)
        {
            _context = context;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<PaymentResultViewModel> ProcessPayment(Payment payment, string paymentMethod, string? provider = null)
        {
            try
            {
                payment.Method = paymentMethod;
                payment.Provider = provider;
                payment.ProviderTxnId = GenerateTransactionId(paymentMethod);

                var result = paymentMethod.ToLower() switch
                {
                    "cash" => await ProcessCashPayment(payment),
                    "bank_transfer" => await ProcessBankTransfer(payment),
                    "momo" => await ProcessMoMoPayment(payment),
                    "zalopay" => await ProcessZaloPayPayment(payment),
                    "vnpay" => await ProcessVNPayPayment(payment),
                    _ => new PaymentResultViewModel
                    {
                        IsSuccess = false,
                        Message = "Phương thức thanh toán không được hỗ trợ"
                    }
                };

                _logger.LogInformation("Payment processed: {PaymentId}, Method: {Method}, Success: {Success}",
                    payment.Id, paymentMethod, result.IsSuccess);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing payment {PaymentId}", payment.Id);
                return new PaymentResultViewModel
                {
                    IsSuccess = false,
                    Message = "Có lỗi xảy ra trong quá trình thanh toán"
                };
            }
        }

        public async Task<bool> VerifyPayment(Guid paymentId, string providerTxnId)
        {
            try
            {
                var payment = await _context.Payments
                    .Include(p => p.Order)
                    .FirstOrDefaultAsync(p => p.Id == paymentId);
                if (payment == null) return false;

                // Verify với provider tương ứng
                var isValid = payment.Method?.ToLower() switch
                {
                    "momo" => await VerifyMoMoPayment(payment.ProviderTxnId, providerTxnId),
                    "zalopay" => await VerifyZaloPayPayment(payment.ProviderTxnId, providerTxnId),
                    "vnpay" => await VerifyVNPayPayment(payment.ProviderTxnId, providerTxnId),
                    _ => payment.ProviderTxnId == providerTxnId
                };

                if (isValid)
                {
                    payment.Status = StatusValue.Completed;
                    payment.UpdatedAt = DateTime.Now;

                    // Sau khi thanh toán thành công, tự động gán slot và cập nhật trạng thái đơn hàng
                    if (payment.Order != null)
                    {
                        await AssignSlotToOrderAfterPaymentAsync(payment.Order.Id);
                        
                        // Cập nhật trạng thái đơn hàng thành Approved (đã duyệt)
                        payment.Order.Status = StatusValue.Approved;
                        payment.Order.UpdatedAt = DateTime.Now;
                    }

                    await _context.SaveChangesAsync();
                }

                return isValid;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error verifying payment {PaymentId}", paymentId);
                return false;
            }
        }

        /// <summary>
        /// Tự động gán slot vào đơn hàng sau khi thanh toán thành công
        /// </summary>
        private async Task AssignSlotToOrderAfterPaymentAsync(Guid orderId)
        {
            try
            {
                _logger.LogInformation("AssignSlotToOrderAfterPaymentAsync - Starting for order: {OrderId}", orderId);

                // Kiểm tra xem đã có OrderWarehouseSlot chưa
                var existingAssignment = await _context.OrderWarehouseSlots
                    .FirstOrDefaultAsync(ows => ows.OrderId == orderId);

                if (existingAssignment != null)
                {
                    _logger.LogInformation("AssignSlotToOrderAfterPaymentAsync - OrderWarehouseSlot already exists: {SlotId}", existingAssignment.WarehouseSlotId);
                    
                    // Đã có assignment rồi, chỉ cập nhật slot status
                    var slot = await _context.WarehouseSlots
                        .FirstOrDefaultAsync(s => s.Id == existingAssignment.WarehouseSlotId);
                    
                    if (slot != null && slot.Status == StatusValue.Reserved)
                    {
                        slot.Status = StatusValue.InUse;
                        slot.UpdatedAt = DateTime.Now;
                        _context.WarehouseSlots.Update(slot);
                        _logger.LogInformation("AssignSlotToOrderAfterPaymentAsync - Updated slot {SlotId} status from Reserved to InUse", slot.Id);
                    }
                    return;
                }

                // Tìm slot từ SlotReservation hoặc slot.CurrentOrderId
                var reservation = await _context.SlotReservations
                    .Include(r => r.WarehouseSlot)
                    .FirstOrDefaultAsync(r => r.OrderId == orderId && r.Status == StatusValue.Active);

                if (reservation != null && reservation.WarehouseSlot != null)
                {
                    var slot = reservation.WarehouseSlot;
                    _logger.LogInformation("AssignSlotToOrderAfterPaymentAsync - Found reservation with slot: {SlotId}, Status: {Status}", 
                        slot.Id, slot.Status);

                    // Tạo OrderWarehouseSlot
                    var orderWarehouseSlot = new OrderWarehouseSlot
                    {
                        Id = Guid.NewGuid(),
                        OrderId = orderId,
                        WarehouseSlotId = slot.Id,
                        AssignedAt = DateTime.Now,
                        ReleasedAt = null,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    };
                    _context.OrderWarehouseSlots.Add(orderWarehouseSlot);
                    _logger.LogInformation("AssignSlotToOrderAfterPaymentAsync - Created OrderWarehouseSlot: {OrderWarehouseSlotId}", orderWarehouseSlot.Id);

                    // Cập nhật slot status từ Reserved → InUse
                    if (slot.Status == StatusValue.Reserved)
                    {
                        slot.Status = StatusValue.InUse;
                        slot.UpdatedAt = DateTime.Now;
                        _context.WarehouseSlots.Update(slot);
                        _logger.LogInformation("AssignSlotToOrderAfterPaymentAsync - Updated slot {SlotId} status from Reserved to InUse", slot.Id);
                    }

                    // Cập nhật reservation status
                    reservation.Status = StatusValue.InActive;
                    reservation.UpdatedAt = DateTime.Now;
                    _context.SlotReservations.Update(reservation);
                    _logger.LogInformation("AssignSlotToOrderAfterPaymentAsync - Updated reservation {ReservationId} status to InActive", reservation.Id);
                }
                else
                {
                    _logger.LogWarning("AssignSlotToOrderAfterPaymentAsync - No active reservation found, trying to find slot by CurrentOrderId");
                    
                    // Nếu không có reservation, tìm slot qua CurrentOrderId
                    var slot = await _context.WarehouseSlots
                        .FirstOrDefaultAsync(s => s.CurrentOrderId == orderId);

                    if (slot != null)
                    {
                        _logger.LogInformation("AssignSlotToOrderAfterPaymentAsync - Found slot by CurrentOrderId: {SlotId}, Status: {Status}", 
                            slot.Id, slot.Status);
                        
                        // Tạo OrderWarehouseSlot
                        var orderWarehouseSlot = new OrderWarehouseSlot
                        {
                            Id = Guid.NewGuid(),
                            OrderId = orderId,
                            WarehouseSlotId = slot.Id,
                            AssignedAt = DateTime.Now,
                            ReleasedAt = null,
                            CreatedAt = DateTime.Now,
                            UpdatedAt = DateTime.Now
                        };
                        _context.OrderWarehouseSlots.Add(orderWarehouseSlot);
                        _logger.LogInformation("AssignSlotToOrderAfterPaymentAsync - Created OrderWarehouseSlot: {OrderWarehouseSlotId}", orderWarehouseSlot.Id);

                        // Cập nhật slot status từ Reserved → InUse
                        if (slot.Status == StatusValue.Reserved)
                        {
                            slot.Status = StatusValue.InUse;
                            slot.UpdatedAt = DateTime.Now;
                            _context.WarehouseSlots.Update(slot);
                            _logger.LogInformation("AssignSlotToOrderAfterPaymentAsync - Updated slot {SlotId} status from Reserved to InUse", slot.Id);
                        }
                    }
                    else
                    {
                        _logger.LogWarning("AssignSlotToOrderAfterPaymentAsync - Cannot find slot for order {OrderId} after payment", orderId);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "AssignSlotToOrderAfterPaymentAsync - Error assigning slot to order {OrderId} after payment", orderId);
                // Không throw exception để không làm gián đoạn quá trình verify payment
            }
        }

        public async Task<PaymentResultViewModel> HandlePaymentCallback(string method, Dictionary<string, string> parameters)
        {
            try
            {
                return method.ToLower() switch
                {
                    "momo" => await HandleMoMoCallback(parameters),
                    "zalopay" => await HandleZaloPayCallback(parameters),
                    "vnpay" => await HandleVNPayCallback(parameters),
                    _ => new PaymentResultViewModel
                    {
                        IsSuccess = false,
                        Message = "Callback không được hỗ trợ"
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error handling payment callback for method {Method}", method);
                return new PaymentResultViewModel
                {
                    IsSuccess = false,
                    Message = "Lỗi xử lý callback"
                };
            }
        }

        public string GenerateTransactionId(string method)
        {
            var prefix = method.ToUpper() switch
            {
                "CASH" => "CASH",
                "BANK_TRANSFER" => "BANK",
                "MOMO" => "MOMO",
                "ZALOPAY" => "ZALO",
                "VNPAY" => "VNP",
                _ => "TXN"
            };

            return $"{prefix}_{DateTime.Now:yyyyMMddHHmmss}_{Guid.NewGuid().ToString("N")[..6].ToUpper()}";
        }

        private async Task<PaymentResultViewModel> ProcessCashPayment(Payment payment)
        {
            payment.Status = StatusValue.Pending;

            return new PaymentResultViewModel
            {
                IsSuccess = true,
                Message = "Đã xác nhận thanh toán tiền mặt. Bạn sẽ thanh toán khi nhận hàng."
            };
        }

        private async Task<PaymentResultViewModel> ProcessBankTransfer(Payment payment)
        {
            payment.Status = StatusValue.Pending;

            return new PaymentResultViewModel
            {
                IsSuccess = true,
                Message = "Vui lòng chuyển khoản theo thông tin được cung cấp và gửi ảnh xác nhận."
            };
        }

        private async Task<PaymentResultViewModel> ProcessMoMoPayment(Payment payment)
        {
            try
            {
                // Lấy thông tin cấu hình MoMo
                var partnerCode = _configuration["MoMo:PartnerCode"];
                var accessKey = _configuration["MoMo:AccessKey"];
                var secretKey = _configuration["MoMo:SecretKey"];
                var endpoint = _configuration["MoMo:Endpoint"];
                var returnUrl = _configuration["MoMo:ReturnUrl"];
                var notifyUrl = _configuration["MoMo:NotifyUrl"];

                if (string.IsNullOrEmpty(partnerCode) || string.IsNullOrEmpty(accessKey) || string.IsNullOrEmpty(secretKey))
                {
                    _logger.LogWarning("MoMo configuration is missing");
                    return new PaymentResultViewModel
                    {
                        IsSuccess = false,
                        Message = "Cấu hình MoMo chưa được thiết lập"
                    };
                }

                payment.Status = StatusValue.Pending;

                // Tạo request đến MoMo (đây là ví dụ, cần implement API thực tế)
                var momoRequest = new
                {
                    partnerCode = partnerCode,
                    orderId = payment.ProviderTxnId,
                    orderInfo = $"Thanh toán đơn hàng #{payment.OrderId.ToString("N")[..8].ToUpper()}",
                    amount = (long)payment.Amount,
                    returnUrl = returnUrl,
                    notifyUrl = notifyUrl,
                    requestId = payment.Id.ToString(),
                    extraData = ""
                };

                // Trong thực tế, bạn sẽ gọi MoMo API ở đây
                var paymentUrl = $"{endpoint}?orderId={payment.ProviderTxnId}&amount={payment.Amount}";

                return new PaymentResultViewModel
                {
                    IsSuccess = true,
                    Message = "Đang chuyển hướng đến MoMo...",
                    RedirectUrl = paymentUrl
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing MoMo payment");
                return new PaymentResultViewModel
                {
                    IsSuccess = false,
                    Message = "Lỗi khi tạo thanh toán MoMo"
                };
            }
        }

        private async Task<PaymentResultViewModel> ProcessZaloPayPayment(Payment payment)
        {
            try
            {
                // Tương tự như MoMo
                payment.Status = StatusValue.Pending;

                var appId = _configuration["ZaloPay:AppId"];
                var key1 = _configuration["ZaloPay:Key1"];
                var endpoint = _configuration["ZaloPay:Endpoint"];

                // Implement ZaloPay API call
                var paymentUrl = $"{endpoint}?app_trans_id={payment.ProviderTxnId}&amount={payment.Amount}";

                return new PaymentResultViewModel
                {
                    IsSuccess = true,
                    Message = "Đang chuyển hướng đến ZaloPay...",
                    RedirectUrl = paymentUrl
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing ZaloPay payment");
                return new PaymentResultViewModel
                {
                    IsSuccess = false,
                    Message = "Lỗi khi tạo thanh toán ZaloPay"
                };
            }
        }

        private async Task<PaymentResultViewModel> ProcessVNPayPayment(Payment payment)
        {
            try
            {
                payment.Status = StatusValue.Pending;

                var tmnCode = _configuration["VNPay:TmnCode"];
                var hashSecret = _configuration["VNPay:HashSecret"];
                var endpoint = _configuration["VNPay:Endpoint"];

                // Implement VNPay API call
                var paymentUrl = $"{endpoint}?vnp_TxnRef={payment.ProviderTxnId}&vnp_Amount={payment.Amount * 100}";

                return new PaymentResultViewModel
                {
                    IsSuccess = true,
                    Message = "Đang chuyển hướng đến VNPay...",
                    RedirectUrl = paymentUrl
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing VNPay payment");
                return new PaymentResultViewModel
                {
                    IsSuccess = false,
                    Message = "Lỗi khi tạo thanh toán VNPay"
                };
            }
        }

        private async Task<bool> VerifyMoMoPayment(string orderId, string providerTxnId)
        {
            // Implement MoMo verification logic
            return orderId == providerTxnId;
        }

        private async Task<bool> VerifyZaloPayPayment(string orderId, string providerTxnId)
        {
            // Implement ZaloPay verification logic
            return orderId == providerTxnId;
        }

        private async Task<bool> VerifyVNPayPayment(string orderId, string providerTxnId)
        {
            // Implement VNPay verification logic
            return orderId == providerTxnId;
        }

        private async Task<PaymentResultViewModel> HandleMoMoCallback(Dictionary<string, string> parameters)
        {
            // Xử lý callback từ MoMo
            var orderId = parameters.GetValueOrDefault("orderId");
            var resultCode = parameters.GetValueOrDefault("resultCode");

            if (resultCode == "0") // Success
            {
                var payment = await _context.Payments
                    .Include(p => p.Order)
                    .FirstOrDefaultAsync(p => p.ProviderTxnId == orderId);

                if (payment != null)
                {
                    payment.Status = StatusValue.Completed;
                    payment.UpdatedAt = DateTime.Now;

                    // Sau khi thanh toán thành công, tự động gán slot và cập nhật trạng thái đơn hàng
                    if (payment.Order != null)
                    {
                        await AssignSlotToOrderAfterPaymentAsync(payment.Order.Id);
                        
                        // Cập nhật trạng thái đơn hàng thành Approved (đã duyệt)
                        payment.Order.Status = StatusValue.Approved;
                        payment.Order.UpdatedAt = DateTime.Now;
                    }

                    await _context.SaveChangesAsync();
                }

                return new PaymentResultViewModel
                {
                    IsSuccess = true,
                    Message = "Thanh toán thành công",
                    PaymentId = payment?.Id
                };
            }

            return new PaymentResultViewModel
            {
                IsSuccess = false,
                Message = "Thanh toán thất bại"
            };
        }

        private async Task<PaymentResultViewModel> HandleZaloPayCallback(Dictionary<string, string> parameters)
        {
            // Tương tự như MoMo
            return new PaymentResultViewModel { IsSuccess = false, Message = "Not implemented" };
        }

        private async Task<PaymentResultViewModel> HandleVNPayCallback(Dictionary<string, string> parameters)
        {
            // Xử lý callback từ VNPay
            var responseCode = parameters.GetValueOrDefault("vnp_ResponseCode");
            var txnRef = parameters.GetValueOrDefault("vnp_TxnRef");
            var orderId = parameters.GetValueOrDefault("vnp_OrderInfo"); // VNPay có thể gửi orderId trong OrderInfo

            _logger.LogInformation("VNPay callback - responseCode: {ResponseCode}, txnRef: {TxnRef}", responseCode, txnRef);

            if (responseCode == "00") // Success
            {
                // Tìm payment bằng txnRef (vnp_TxnRef thường là paymentId)
                var payment = await _context.Payments
                    .Include(p => p.Order)
                    .FirstOrDefaultAsync(p => p.Id.ToString() == txnRef || p.ProviderTxnId == txnRef);

                if (payment != null)
                {
                    _logger.LogInformation("VNPay callback - Found payment: {PaymentId}, OrderId: {OrderId}, Current Status: {Status}", 
                        payment.Id, payment.OrderId, payment.Status);

                    payment.Status = StatusValue.Completed;
                    payment.UpdatedAt = DateTime.Now;

                    // Sau khi thanh toán thành công, tự động gán slot và cập nhật trạng thái đơn hàng
                    if (payment.Order != null)
                    {
                        _logger.LogInformation("VNPay callback - Assigning slot to order: {OrderId}, Current Order Status: {OrderStatus}", 
                            payment.Order.Id, payment.Order.Status);

                        await AssignSlotToOrderAfterPaymentAsync(payment.Order.Id);
                        
                        // Cập nhật trạng thái đơn hàng thành Approved (đã duyệt)
                        payment.Order.Status = StatusValue.Approved;
                        payment.Order.UpdatedAt = DateTime.Now;

                        _logger.LogInformation("VNPay callback - Updated order status to: {OrderStatus}", payment.Order.Status);
                    }
                    else
                    {
                        _logger.LogWarning("VNPay callback - Payment {PaymentId} has no Order", payment.Id);
                    }

                    await _context.SaveChangesAsync();
                    _logger.LogInformation("VNPay callback - Successfully saved payment and order updates");

                    return new PaymentResultViewModel
                    {
                        IsSuccess = true,
                        Message = "Thanh toán thành công",
                        PaymentId = payment.Id
                    };
                }
                else
                {
                    _logger.LogWarning("VNPay callback: Payment not found for txnRef: {TxnRef}. Searching all payments...", txnRef);
                    
                    // Debug: List all payments để tìm vấn đề
                    var allPayments = await _context.Payments
                        .Select(p => new { p.Id, p.ProviderTxnId, p.OrderId })
                        .Take(10)
                        .ToListAsync();
                    _logger.LogInformation("VNPay callback - Recent payments: {@Payments}", allPayments);
                }
            }
            else
            {
                _logger.LogWarning("VNPay callback - Payment failed. ResponseCode: {ResponseCode}", responseCode);
            }

            return new PaymentResultViewModel
            {
                IsSuccess = false,
                Message = "Thanh toán thất bại"
            };
        }
    }
}
