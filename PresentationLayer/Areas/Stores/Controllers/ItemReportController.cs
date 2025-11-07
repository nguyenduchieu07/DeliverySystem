using DataAccessLayer.Constants;
using DataAccessLayer.Entities;
using DataAccessLayer.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace PresentationLayer.Areas.Stores.Controllers;

[Area("Stores")]
[Authorize(Roles = $"{UserRoles.STORE}, {UserRoles.STORESTAFF}")]
public class ItemReportController : Controller
{
    private readonly DeliverySytemContext _context;
    public ItemReportController(DeliverySytemContext context)
    {
        _context = context;
    }
    // GET
    public async Task<IActionResult> Index()
    {
        var reports = await _context.IncidentReports
            .Include(r => r.OrderItem)
            .ThenInclude(oi => oi.Order)
            .Where(r => r.Status == ReportStatus.Pending)
            .ToListAsync();

        return View(reports);
    }
    
    public async Task<IActionResult> Detail(Guid id)
    {
        var report = await _context.IncidentReports
            .Include(r => r.OrderItem)
            .ThenInclude(oi => oi.Order)
            .Include(r => r.Actions).ThenInclude(a => a.Staff)
            .FirstOrDefaultAsync(r => r.Id == id);

        if (report == null) return NotFound();

        return View(report);
    }
    
    public partial class HandleReportViewModel
    {
        public Guid OrderItemId { get; set; }
        public ItemReportActionType ActionType { get; set; }
        public string? Note { get; set; }
    }

    
    [HttpPost("/ItemReport/Actions")]
        public async Task<IActionResult> HandleIncident([FromForm] HandleReportViewModel model)
        {
            await using var ts = await _context.Database.BeginTransactionAsync();
            try
            {
                var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                var role = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;
                if (!ModelState.IsValid)
                    return Json(new { success = false, message = "Dữ liệu không hợp lệ." });

                var report = await _context.IncidentReports
                    .FirstOrDefaultAsync(x => x.OrderItemId == model.OrderItemId && x.Status != ReportStatus.Done && x.Status != ReportStatus.CheckIn && x.Status != ReportStatus.CheckOut);

                if (report == null)
                    return Json(new { success = false, message = "Không tìm thấy báo cáo." });

                var action = new ItemReportAction
                {
                    Id = Guid.NewGuid(),
                    ItemReportId = report.Id,
                    ActionType = model.ActionType,
                    Note = model.Note,
                    CreatedAt = DateTime.Now,
                   
                };
                if (Guid.TryParse(userId, out var userIdGuid) && role == TargetType.StoreStaff.ToString())
                {
                    var staff = await _context.StoreStaffs.FirstAsync(s => s.UserId == userIdGuid);
                    action.StaffId = staff.Id;
                }

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