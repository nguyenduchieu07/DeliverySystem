using DataAccessLayer.Entities;
using DataAccessLayer.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace PresentationLayer.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class AccountController : Controller
{
    private readonly DeliverySytemContext _db;
    public AccountController(DeliverySytemContext db) => _db = db;

    public async Task<IActionResult> Index(TargetType? scope = TargetType.Customer, string? q = null,
        StatusValue? status = null)
    {
        if (scope == TargetType.Store)
        {
            var stores = _db.Stores.AsQueryable();
            if (status != null) stores = stores.Where(s => s.Status == status);
            if (!string.IsNullOrWhiteSpace(q)) stores = stores.Where(s => s.StoreName.Contains(q));
            var model = await stores.OrderByDescending(s => s.UpdatedAt).Take(300).ToListAsync();
            ViewBag.Scope = TargetType.Store;
            return View(model);
        }
        else
        {
            var users = _db.Users.AsQueryable();
            if (status != null) users = users.Where(u => u.Status == status);
            if (!string.IsNullOrWhiteSpace(q))
                users = users.Where(u => (u.UserName ?? "").Contains(q) || (u.Email ?? "").Contains(q));
            var model = await users.OrderByDescending(u => u.UpdatedAt).Take(300).ToListAsync();
            ViewBag.Scope = TargetType.Customer;
            return View(model);
        }
    }

    // ---- Store ----
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> BanStore(Guid id)
    {
        var s = await _db.Stores.FindAsync(id);
        if (s == null) return NotFound();
        s.Status = StatusValue.Ban;
        s.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
        TempData["ok"] = $"Store {s.StoreName} banned.";
        return RedirectToAction(nameof(Index), new { scope = "Store" });
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> UnbanStore(Guid id)
    {
        var s = await _db.Stores.FindAsync(id);
        if (s == null) return NotFound();
        s.Status = StatusValue.Active;
        s.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
        TempData["ok"] = $"Store {s.StoreName} unbanned.";
        return RedirectToAction(nameof(Index), new { scope = "Store" });
    }

    // ---- User ----
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> BanUser(Guid id)
    {
        var u = await _db.Users.FindAsync(id);
        if (u == null) return NotFound();
        u.Status = StatusValue.Ban;
        u.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
        TempData["ok"] = $"User {(u.UserName ?? u.Email)} banned.";
        return RedirectToAction(nameof(Index), new { scope = "User", status = "Banned" });
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> UnbanUser(Guid id)
    {
        var u = await _db.Users.FindAsync(id);
        if (u == null) return NotFound();
        u.Status = StatusValue.Active;
        u.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
        TempData["ok"] = $"User {(u.UserName ?? u.Email)} unbanned.";
        return RedirectToAction(nameof(Index), new { scope = "User", status = "Active" });
    }
}