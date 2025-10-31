using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Abstractions.IServices;

namespace PresentationLayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentCallbackController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly ILogger<PaymentCallbackController> _logger;

        public PaymentCallbackController(
            IPaymentService paymentService,
            ILogger<PaymentCallbackController> logger)
        {
            _paymentService = paymentService;
            _logger = logger;
        }

        /// <summary>
        /// Callback từ MoMo
        /// </summary>
        [HttpPost("momo")]
        public async Task<IActionResult> MoMoCallback([FromForm] Dictionary<string, string> parameters)
        {
            try
            {
                _logger.LogInformation("Received MoMo callback: {@Parameters}", parameters);

                var result = await _paymentService.HandlePaymentCallback("momo", parameters);

                if (result.IsSuccess)
                {
                    return Ok(new { message = "success" });
                }

                return BadRequest(new { message = result.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error handling MoMo callback");
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        /// <summary>
        /// Return URL từ MoMo (khi user quay lại website)
        /// </summary>
        [HttpGet("momo/return")]
        public async Task<IActionResult> MoMoReturn([FromQuery] Dictionary<string, string> parameters)
        {
            try
            {
                _logger.LogInformation("Received MoMo return: {@Parameters}", parameters);

                var resultCode = parameters.GetValueOrDefault("resultCode");
                var orderId = parameters.GetValueOrDefault("orderId");

                if (resultCode == "0") // Success
                {
                    // Chuyển đến trang thành công
                    return Redirect($"/Payment/Success?transactionId={orderId}");
                }
                else
                {
                    // Chuyển đến trang thất bại
                    return Redirect($"/Payment/Failed?transactionId={orderId}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error handling MoMo return");
                return Redirect("/Payment/Failed");
            }
        }

        /// <summary>
        /// Callback từ ZaloPay
        /// </summary>
        [HttpPost("zalopay")]
        public async Task<IActionResult> ZaloPayCallback([FromForm] Dictionary<string, string> parameters)
        {
            try
            {
                _logger.LogInformation("Received ZaloPay callback: {@Parameters}", parameters);

                var result = await _paymentService.HandlePaymentCallback("zalopay", parameters);

                if (result.IsSuccess)
                {
                    return Ok(new { return_code = 1, return_message = "success" });
                }

                return Ok(new { return_code = 0, return_message = result.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error handling ZaloPay callback");
                return Ok(new { return_code = 0, return_message = "Internal server error" });
            }
        }

        /// <summary>
        /// Return URL từ ZaloPay
        /// </summary>
        [HttpGet("zalopay/return")]
        public async Task<IActionResult> ZaloPayReturn([FromQuery] Dictionary<string, string> parameters)
        {
            try
            {
                _logger.LogInformation("Received ZaloPay return: {@Parameters}", parameters);

                var status = parameters.GetValueOrDefault("status");
                var appTransId = parameters.GetValueOrDefault("apptransid");

                if (status == "1") // Success
                {
                    return Redirect($"/Payment/Success?transactionId={appTransId}");
                }
                else
                {
                    return Redirect($"/Payment/Failed?transactionId={appTransId}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error handling ZaloPay return");
                return Redirect("/Payment/Failed");
            }
        }

        /// <summary>
        /// Callback từ VNPay
        /// </summary>
        [HttpGet("vnpay")]
        public async Task<IActionResult> VNPayCallback([FromQuery] Dictionary<string, string> parameters)
        {
            try
            {
                _logger.LogInformation("Received VNPay callback: {@Parameters}", parameters);

                var result = await _paymentService.HandlePaymentCallback("vnpay", parameters);

                var responseCode = parameters.GetValueOrDefault("vnp_ResponseCode");
                var txnRef = parameters.GetValueOrDefault("vnp_TxnRef");

                if (responseCode == "00") // Success
                {
                    return Redirect($"/Payment/Success?transactionId={txnRef}");
                }
                else
                {
                    return Redirect($"/Payment/Failed?transactionId={txnRef}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error handling VNPay callback");
                return Redirect("/Payment/Failed");
            }
        }

        /// <summary>
        /// Webhook cho xác nhận chuyển khoản ngân hàng (từ hệ thống internal hoặc third-party)
        /// </summary>
        [HttpPost("bank-transfer")]
        public async Task<IActionResult> BankTransferConfirmation([FromBody] BankTransferWebhookModel model)
        {
            try
            {
                _logger.LogInformation("Received bank transfer confirmation: {@Model}", model);

                // Validate webhook signature (if any)
                if (!ValidateWebhookSignature(model))
                {
                    return Unauthorized();
                }

                var success = await _paymentService.VerifyPayment(model.PaymentId, model.TransactionId);

                if (success)
                {
                    return Ok(new { message = "Confirmed" });
                }

                return BadRequest(new { message = "Verification failed" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error handling bank transfer confirmation");
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        /// <summary>
        /// API để admin xác nhận thanh toán thủ công
        /// </summary>
        [HttpPost("manual-confirm")]
        [Authorize(Roles = "Admin,Store")]
        public async Task<IActionResult> ManualConfirmPayment([FromBody] ManualConfirmModel model)
        {
            try
            {
                var success = await _paymentService.VerifyPayment(model.PaymentId, model.TransactionId);

                if (success)
                {
                    return Ok(new { message = "Payment confirmed successfully" });
                }

                return BadRequest(new { message = "Payment confirmation failed" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in manual payment confirmation");
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        private bool ValidateWebhookSignature(BankTransferWebhookModel model)
        {
            // Implement signature validation logic
            // Thường sẽ so sánh signature được tạo từ data với signature trong request
            return true; // Placeholder
        }
    }

    public class BankTransferWebhookModel
    {
        public Guid PaymentId { get; set; }
        public string TransactionId { get; set; } = null!;
        public decimal Amount { get; set; }
        public string BankCode { get; set; } = null!;
        public DateTime TransactionTime { get; set; }
        public string? Description { get; set; }
        public string? Signature { get; set; }
    }

    public class ManualConfirmModel
    {
        public Guid PaymentId { get; set; }
        public string TransactionId { get; set; } = null!;
        public string? Note { get; set; }
    }
}
