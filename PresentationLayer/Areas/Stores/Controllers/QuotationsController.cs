using DataAccessLayer.Entities;
using DataAccessLayer.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServiceLayer.Abstractions.IServices;
using System;

namespace PresentationLayer.Areas.Stores.Controllers
{
    [Area("Stores")]
    public class QuotationsController : Controller
    {
        private readonly DeliverySytemContext _db;
        private readonly IUserContextService _context;
        public QuotationsController(DeliverySytemContext db,
            IUserContextService userContextService)
        {
            _db = db;
            _context = userContextService;
        }

        public async Task<IActionResult> Index(string? tab)
        {
            var id = Guid.Parse(_context.GetUserId()!);
            var storeId = await _db.Stores.Where(e => e.OwnerUserId == id).Select(e => e.Id).FirstOrDefaultAsync(); 
            var q = _db.Quotations.AsNoTracking().Where(x => x.StoreId == storeId);

            ViewBag.Draft = await _db.Quotations.Where(x => x.Status == StatusValue.Draft && x.ValidUntil < DateTime.UtcNow)
               .OrderBy(x => x.ValidUntil)
               .Select(x => new { x.Id, x.TotalAmount, x.ValidUntil, x.Status, Customer = x.Customer.FullName })
               .ToListAsync();

            ViewBag.Waiting = await q.Where(x => x.Status == StatusValue.Sent && x.ValidUntil >= DateTime.UtcNow)
                .OrderBy(x => x.ValidUntil)
                .Select(x => new { x.Id, x.TotalAmount, x.ValidUntil, x.Status, Customer = x.Customer.FullName })
                .ToListAsync();

            ViewBag.Revised = await q.Where(x => x.Status == StatusValue.Revised)
                .OrderByDescending(x => x.UpdatedAt)
                .Select(x => new { x.Id, x.TotalAmount, x.ValidUntil, x.Status, Customer = x.Customer.FullName })
                .ToListAsync();

            ViewBag.Accepted = await q.Where(x => x.Status == StatusValue.Active)
                .OrderByDescending(x => x.UpdatedAt)
                .Select(x => new { x.Id, x.TotalAmount, x.ValidUntil, x.Status, Customer = x.Customer.FullName })
                .ToListAsync();

            ViewBag.Tab = tab ?? "waiting";
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Detail(Guid id)
        {
            var idUser = Guid.Parse(_context.GetUserId()!);
            var storeId = await _db.Stores.Where(e => e.OwnerUserId == idUser)
                .Select(e => e.Id).FirstOrDefaultAsync(); 
            var qt = await _db.Quotations
                .Include(x => x.Customer)
                .Include(x => x.Orders).ThenInclude(o => o.OrderItems)
                .FirstOrDefaultAsync(x => x.Id == id && (x.Status == StatusValue.Draft ? true : x.StoreId == storeId));
            if (qt == null) return NotFound();
            return View(qt);
        }

        [HttpPost]
        public async Task<IActionResult> Approve(Guid id)
        {
            var idUser = Guid.Parse(_context.GetUserId()!);
            var storeId = await _db.Stores
                    .Where(e => e.OwnerUserId == idUser)
                    .Select(e => e.Id).FirstOrDefaultAsync();
            var qt = await _db.Quotations.FirstOrDefaultAsync(x => x.Id == id && x.StoreId == storeId);
            if (qt == null) return NotFound();
            if (qt.Status is not (StatusValue.Sent or StatusValue.Revised))
                return BadRequest("Sai trạng thái.");
            qt.Status = StatusValue.Active;    // coi như 'Approved'
            qt.UpdatedAt = DateTime.UtcNow;
            await _db.SaveChangesAsync();
            return Ok();
        }

        public sealed class ReviseQuotationDto { public decimal? PercentDiscount { get; set; } public decimal? AbsoluteDiscount { get; set; } public string? Note { get; set; } }

        [HttpPost]
        public async Task<IActionResult> Revise(Guid id, [FromBody] ReviseQuotationDto dto)
        {
            var idUser = Guid.Parse(_context.GetUserId()!);
            var storeId = await _db.Stores
                    .Where(e => e.OwnerUserId == idUser)
                    .Select(e => e.Id).FirstOrDefaultAsync();
            var qt = await _db.Quotations.FirstOrDefaultAsync(x => x.Id == id && x.StoreId == storeId);
            if (qt == null) return NotFound();

            if (dto.PercentDiscount is > 0) qt.TotalAmount = Math.Max(0, qt.TotalAmount - Math.Round(qt.TotalAmount * dto.PercentDiscount.Value / 100m, 0));
            if (dto.AbsoluteDiscount is > 0) qt.TotalAmount = Math.Max(0, qt.TotalAmount - dto.AbsoluteDiscount.Value);
            qt.ValidUntil = DateTime.UtcNow.AddHours(48);
            qt.Status = StatusValue.Revised;
            qt.UpdatedAt = DateTime.UtcNow;
            await _db.SaveChangesAsync();
            return Ok(new { qt.TotalAmount, qt.ValidUntil });
        }

        public sealed class SuggestQuotationDto { public decimal? PercentDiscount { get; set; } public decimal? AbsoluteDiscount { get; set; } public string? Note { get; set; } }

        [HttpPost]
        public async Task<IActionResult> Suggest(Guid id, [FromBody] SuggestQuotationDto dto)
        {
            var idUser = Guid.Parse(_context.GetUserId()!);
            var storeId = await _db.Stores
                    .Where(e => e.OwnerUserId == idUser)
                    .Select(e => e.Id).FirstOrDefaultAsync();
            var qt = await _db.Quotations.FirstOrDefaultAsync(x => x.Id == id && x.StoreId == storeId);
            if (qt == null) return NotFound();

            if (dto.PercentDiscount is > 0) qt.TotalAmount = Math.Max(0, qt.TotalAmount - Math.Round(qt.TotalAmount * dto.PercentDiscount.Value / 100m, 0));
            if (dto.AbsoluteDiscount is > 0) qt.TotalAmount = Math.Max(0, qt.TotalAmount - dto.AbsoluteDiscount.Value);
            qt.ValidUntil = DateTime.UtcNow.AddHours(48);
            qt.Status = StatusValue.Draft;
            qt.UpdatedAt = DateTime.UtcNow;
            await _db.SaveChangesAsync();
            return Ok(new { qt.TotalAmount, qt.ValidUntil });
        }

        public sealed class RejectQuotationDto { public string? Reason { get; set; } }

        [HttpPost]
        public async Task<IActionResult> Reject(Guid id, [FromBody] RejectQuotationDto _)
        {
            var idUser = Guid.Parse(_context.GetUserId()!);
            var storeId = await _db.Stores
                    .Where(e => e.OwnerUserId == idUser)
                    .Select(e => e.Id).FirstOrDefaultAsync();
            var qt = await _db.Quotations.FirstOrDefaultAsync(x => x.Id == id && x.StoreId == storeId);
            if (qt == null) return NotFound();
            qt.Status = StatusValue.InActive;
            qt.UpdatedAt = DateTime.UtcNow;
            await _db.SaveChangesAsync();
            return Ok();
        }
    }
}
