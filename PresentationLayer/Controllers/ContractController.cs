using System.Threading.Tasks;
using DataAccessLayer.Enums;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.Models;
using ServiceLayer.Abstractions.IServices;
using DataAccessLayer.Entities;
using DataAccessLayer.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace PresentationLayer.Controllers
{
    public class ContractController : Controller
    {
        private readonly IContractService _contractService;
        private readonly IOrderService _orderService;
        private readonly DeliverySytemContext _context; 
        
        public ContractController(IContractService contractService, IOrderService orderService, DeliverySytemContext context)
        {
            _contractService = contractService;
            _orderService = orderService;
            _context = context;
        }

        public async Task<IActionResult> GetContract([FromQuery] Guid orderId)
        {
            try
            {
                var orderById = await _orderService.GetByIdAsync(orderId);

                if(orderById!.QuotationId == null)
                    return BadRequest("Báo giá không tồn tại cho đơn hàng này");

                var contract = await _contractService.GenerateContractAsync(orderById.QuotationId ?? Guid.Empty);

                var viewModel = new ContractIndexViewModel
                {
                    Order = orderById,
                    Contract = contract
                };

                return View(viewModel);
            }
            catch (InvalidOperationException invalidEx)
            {
                return BadRequest(invalidEx.Message);
            }
        }


        [HttpPost]
        public async Task<IActionResult> Confirm(Guid contractId, Guid orderId)
        {
            await _contractService.ConfirmContract(contractId, orderId);
            return RedirectToAction("Index", "Payment", new { orderId });
        }

        [HttpPost]
        public async Task<IActionResult> Cancel(Guid contractId, Guid orderId)
        {
            await _contractService.CancleContract(contractId, orderId);
            return RedirectToAction("Index", "Home");
        }

        // GET: Contract/Index
        public async Task<IActionResult> Index(string search, string status, DateTime? fromDate, DateTime? toDate)
        {
            // Query base
            var query = _context.Contracts
                .Include(c => c.Customer)
                .Include(c => c.Store)
                .Include(c => c.Warehouse)
                .Include(c => c.WarehouseSlot)
                .Include(c => c.Quotation)
                .AsQueryable();

            // Filter by search
            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(c =>
                    c.Id.ToString().Contains(search) ||
                    c.Customer.FullName.Contains(search) ||
                    c.Store.StoreName.Contains(search)
                );
            }

            // Filter by status
            if (!string.IsNullOrWhiteSpace(status) && Enum.TryParse<ContractStatus>(status, out var contractStatus))
            {
                query = query.Where(c => c.Status == contractStatus);
            }

            // Filter by date range
            if (fromDate.HasValue)
            {
                query = query.Where(c => c.StartDate >= fromDate.Value);
            }

            if (toDate.HasValue)
            {
                query = query.Where(c => c.EndDate <= toDate.Value);
            }

            // Order by created date descending
            query = query.OrderByDescending(c => c.CreatedAt);

            // Pass filter values to view
            ViewBag.Search = search;
            ViewBag.Status = status;
            ViewBag.FromDate = fromDate?.ToString("yyyy-MM-dd");
            ViewBag.ToDate = toDate?.ToString("yyyy-MM-dd");

            var contracts = await query.ToListAsync();
            return View(contracts);
        }

        // GET: Contract/Details/5
        public async Task<IActionResult> Details(Guid id)
        {
            var contract = await _context.Contracts
                .Include(c => c.Customer)
                    .ThenInclude(cu => cu.User)
                .Include(c => c.Store)
                .Include(c => c.Warehouse)
                    .ThenInclude(w => w.Address)
                .Include(c => c.WarehouseSlot)
                .Include(c => c.Quotation)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (contract == null)
            {
                return NotFound();
            }

            return View(contract);
        }

        // POST: Contract/Renew
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Renew(Guid contractId, DateTime newEndDate)
        {
            var contract = await _context.Contracts.FindAsync(contractId);

            if (contract == null)
            {
                return NotFound();
            }

            if (contract.Status != ContractStatus.Active)
            {
                TempData["Error"] = "Chỉ có thể gia hạn hợp đồng đang hoạt động.";
                return RedirectToAction(nameof(Details), new { id = contractId });
            }

            if (newEndDate <= contract.EndDate)
            {
                TempData["Error"] = "Ngày kết thúc mới phải sau ngày kết thúc hiện tại.";
                return RedirectToAction(nameof(Details), new { id = contractId });
            }

            // Update contract
            contract.EndDate = newEndDate;
            contract.UpdatedAt = DateTime.Now;

            // Recalculate amount if needed based on new duration
            // var additionalDays = (newEndDate - contract.EndDate).Days;
            // contract.TotalAmount += CalculateAdditionalCost(contract, additionalDays);

            await _context.SaveChangesAsync();

            TempData["Success"] = "Gia hạn hợp đồng thành công!";
            return RedirectToAction(nameof(Details), new { id = contractId });
        }

        // POST: Contract/Terminate
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Terminate(Guid contractId, string reason)
        {
            var contract = await _context.Contracts.FindAsync(contractId);

            if (contract == null)
            {
                return NotFound();
            }

            if (contract.Status != ContractStatus.Active)
            {
                TempData["Error"] = "Chỉ có thể chấm dứt hợp đồng đang hoạt động.";
                return RedirectToAction(nameof(Details), new { id = contractId });
            }

            // Update contract status
            contract.Status = ContractStatus.Terminated;
            contract.UpdatedAt = DateTime.Now;

            // You might want to save the reason in a separate table or field
            // contract.TerminationReason = reason;

            // If there's a warehouse slot, free it up
            if (contract.WarehouseSlotId.HasValue)
            {
                var slot = await _context.WarehouseSlots.FindAsync(contract.WarehouseSlotId.Value);
                if (slot != null)
                {
                    slot.Status = StatusValue.Active; // or Available
                    slot.CurrentOrderId = null;
                    slot.LeaseStart = null;
                    slot.LeaseEnd = null;
                }
            }

            await _context.SaveChangesAsync();

            TempData["Success"] = "Chấm dứt hợp đồng thành công!";
            return RedirectToAction(nameof(Index));
        }

        // POST: Contract/GeneratePDF
        [HttpPost]
        public async Task<IActionResult> GeneratePDF(Guid id)
        {
            try
            {
                var contract = await _context.Contracts
                    .Include(c => c.Customer)
                    .Include(c => c.Store)
                    .Include(c => c.Warehouse)
                    .Include(c => c.WarehouseSlot)
                    .Include(c => c.Quotation)
                    .FirstOrDefaultAsync(c => c.Id == id);

                if (contract == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy hợp đồng" });
                }

                // Generate PDF using a PDF library (e.g., iTextSharp, QuestPDF, etc.)
                // Example pseudocode:
                // var pdfBytes = GenerateContractPDF(contract);
                // var fileName = $"Contract_{contract.Id}_{DateTime.Now:yyyyMMdd}.pdf";
                // var filePath = Path.Combine("wwwroot", "contracts", fileName);
                // File.WriteAllBytes(filePath, pdfBytes);

                // Update contract with PDF URL
                // contract.PdfUrl = $"/contracts/{fileName}";
                // await _context.SaveChangesAsync();

                return Json(new
                {
                    success = true,
                    message = "Tạo PDF thành công",
                    pdfUrl = contract.PdfUrl
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // Helper method to check and update expired contracts
        public async Task UpdateExpiredContracts()
        {
            var expiredContracts = await _context.Contracts
                .Where(c => c.Status == ContractStatus.Active && c.EndDate < DateTime.Now)
                .ToListAsync();

            foreach (var contract in expiredContracts)
            {
                contract.Status = ContractStatus.Expired;
                contract.UpdatedAt = DateTime.Now;

                // Free up warehouse slot if exists
                if (contract.WarehouseSlotId.HasValue)
                {
                    var slot = await _context.WarehouseSlots.FindAsync(contract.WarehouseSlotId.Value);
                    if (slot != null)
                    {
                        slot.Status = StatusValue.Active;
                        slot.CurrentOrderId = null;
                        slot.LeaseStart = null;
                        slot.LeaseEnd = null;
                    }
                }
            }

            if (expiredContracts.Any())
            {
                await _context.SaveChangesAsync();
            }
        }
    }
}

