using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Abstractions.IServices;
using ServiceLayer.Dtos.Quotes;

namespace PresentationLayer.Controllers
{
    public class QuoteController : Controller
    {
        private readonly IQuotationService _svc;
        private readonly IFeedbackService _feedbackService;

        public QuoteController(IQuotationService svc, IFeedbackService feedbackService)
        {
            _svc = svc;
            _feedbackService =  feedbackService;    
        }

        // Bước 1–2: Trang form báo giá (UI theo layout của bạn)
        [HttpGet]
        public IActionResult Index()
        {
            var vm = new QuotePageVm
            {
                // default scenario: m³/ngày, FreeDays = 2
                FreeDays = 2,
                VatRate = 0.10m,
                StartDate = new DateTime(2025, 11, 5),
                EndDate = new DateTime(2025, 11, 12)
            };
            return View(vm);
        }

        // Bước 3: Tính giá (AJAX) + lưu Quotation ở trạng thái Sent
        [HttpPost]
        public async Task<IActionResult> Calculate([FromBody] QuoteRequestVm req, CancellationToken ct)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _svc.CalculateAndCreateQuotationAsync(req, ct);
            return Ok(result);
        }

        // Bước 4A: Giữ chỗ tạm (2h)
        [HttpPost]
        public async Task<IActionResult> HoldTemp([FromBody] HoldTempVm vm, CancellationToken ct)
        {
            var ok = await _svc.CreateTempReservationAsync(vm, ct);
            return ok ? Ok() : BadRequest("Không tạo được giữ chỗ tạm.");
        }

        // Bước 4B: Chấp nhận báo giá → Quotation.Accepted + tạo Reservation Firm
        [HttpPost]
        public async Task<IActionResult> Accept([FromBody] AcceptQuoteVm vm, CancellationToken ct)
        {
            var ok = await _svc.AcceptQuotationAsync(vm, ct);
            return ok ? Ok() : BadRequest("Không chấp nhận được báo giá.");
        }

        // Bước 4C: Yêu cầu chỉnh giá (ghi chú)
        [HttpPost]
        public async Task<IActionResult> RequestRevision([FromBody] RequestRevisionVm vm, CancellationToken ct)
        {
            var ok = await _svc.RequestRevisionAsync(vm, ct);
            return ok ? Ok() : BadRequest("Không gửi yêu cầu chỉnh giá được.");
        }

        public class CreateFeedbackDto
        {
            public Guid QuotationId { get; set; }
            public string Comment { get; set; }
            public int Rating { get; set; }
        }
        
        [HttpPost]
        public async Task<IActionResult> Feedback([FromBody] CreateFeedbackDto data)
        {
            var quotationInfo = await _svc.GetByIdAsync(data.QuotationId, CancellationToken.None);
            if(quotationInfo == null) return BadRequest("Thông tin báo giá bị thiếu. Tạo thất bại");

            if(quotationInfo.StoreId == null) return BadRequest("Thông tin báo giá tạm thời đã hết hạn. Không thể đánh giá cho cửa hàng này");

            var feedback = new Feedback
            {
                Id = Guid.NewGuid(),
                FromUserId = quotationInfo.CustomerId,
                ToStoreId = (Guid)quotationInfo.StoreId,
                Rating = data.Rating,
                Comment = data.Comment,
                CreatedAt = DateTime.UtcNow
            };
            
            var rs = await _feedbackService.CreateFeedbackAsync(feedback);
            return rs != null ? Ok() : BadRequest("Không gửi được đánh giá");
        }
        
        
    }
}
