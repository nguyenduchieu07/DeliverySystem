using DataAccessLayer.Entities;
using DataAccessLayer.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PresentationLayer.Models;
using ServiceLayer.Abstractions.IServices;
using System;
using System.Security.Claims;
using DataAccessLayer.Constants;
using ServiceLayer.Extensions;

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

        public async Task<IActionResult> Index(KycStatus? status = null, string? storeName = null)
        {
            var vm = new KycSubmissionIndexViewModel();

            var list = await _svc.GetAllAsync(storeName, status);
            vm.KycSubmissions = list;
            vm.StoreName = storeName;
            vm.Status = status;

            return View(vm);
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
                var adminIdStrClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var adminId = Guid.Parse(adminIdStrClaim);

                var kycDocuments = await _db.KycDocuments.Where(x => x.KycSubmissionId == submissionId).ToListAsync();

                foreach (var type in KycDocumentConstant.Types)
                {
                    var document = kycDocuments.FirstOrDefault(d => d.DocType == type.Key);

                    if (document == null)
                    {
                        TempData["Error"] = $"Chấp thuận thất bại do thiếu tài liệu {type.Key}.";
                        return RedirectToAction(nameof(Index));
                    }
                }

                await _svc.ApproveAsync(submissionId, maxPerDay, regions, adminId);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Chấp thuận thất bại: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> NeedChanges(Guid submissionId, string note)
        {
            try
            {
                var adminIdStrClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var adminId = Guid.Parse(adminIdStrClaim);
                await _svc.NeedChangesAsync(submissionId, note, adminId);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Chính sửa thất bại: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reject(Guid submissionId, string note)
        {
            try
            {
                var adminId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
                var submission = await _db.KycSubmissions.FirstOrDefaultAsync(x => x.StoreId == submissionId);
                if (submission != null && submission.Status == KycStatus.Approved)
                {
                    
                    TempData["Error"] = $"Chấp thuận thất bại do đơn đã được duyệt.";
                    return RedirectToAction(nameof(Index));
                }
                await _svc.RejectAsync(submissionId, note, adminId);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Từ chối thất bại: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }
    }
}