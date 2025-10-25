using DataAccessLayer.Entities;
using DataAccessLayer.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServiceLayer.Abstractions.IServices;
using System;
using System.Security.Claims;

namespace PresentationLayer.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class KycController : Controller
    {
        private readonly DeliverySytemContext _db;
        private readonly IKycService _svc;
        public KycController(DeliverySytemContext db, IKycService svc)
        {
            _db = db;
            _svc = svc;
        }
        public async Task<IActionResult> Index(KycStatus status = KycStatus.Pending, string? q = null)
        {

            var qry = _db.Set<KycSubmission>()
                         .Include(x => x.Store)
                         .Include(x => x.Documents)
                         .Where(x => x.Status == status);
            if (!string.IsNullOrWhiteSpace(q))
                qry = qry.Where(x => x.Store.StoreName.Contains(q));
            var list = await qry.OrderBy(x => x.SubmittedAt).Take(200).ToListAsync();
            return View(list);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Approve(Guid submissionId, int? maxPerDay, string? regionsCsv)
        {
            try
            {
                var regions = string.IsNullOrWhiteSpace(regionsCsv)
                ? null
                : regionsCsv.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                var adminId = new Guid();
                await _svc.ApproveAsync(submissionId, maxPerDay, regions, adminId);
                return RedirectToAction(nameof(Index), new { status = KycStatus.Pending });
            }
            catch(Exception ex)
            {
                TempData["Error"] = "Approve thất bại: " + ex.Message;
                return RedirectToAction(nameof(Index), new { status = KycStatus.Pending });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> NeedChanges(Guid submissionId, string note)
        {
            try
            {
                var adminId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
                await _svc.NeedChangesAsync(submissionId, note, adminId);
                return RedirectToAction(nameof(Index), new { status = KycStatus.Pending });
            }
            catch(Exception ex)
            {
                TempData["Error"] = "Approve thất bại: " + ex.Message;
                return RedirectToAction(nameof(Index), new { status = KycStatus.Pending });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reject(Guid submissionId, string note)
        {
            try
            {
                var adminId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
                await _svc.RejectAsync(submissionId, note, adminId);
                return RedirectToAction(nameof(Index), new { status = KycStatus.Pending });
            }
            catch(Exception ex)
            {
                TempData["Error"] = "Approve thất bại: " + ex.Message;
                return RedirectToAction(nameof(Index), new { status = KycStatus.Pending });
            }
        }
    }
}
