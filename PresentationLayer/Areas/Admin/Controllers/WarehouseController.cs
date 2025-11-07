using DataAccessLayer.Entities;
using DataAccessLayer.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace PresentationLayer.Areas.Admin.Controllers;

[Area("Admin")]
public class WarehouseController : Controller
{
    private readonly DeliverySytemContext _db;
    public WarehouseController(DeliverySytemContext context)
    {
        _db = context;
    }
    
    public async Task<IActionResult> ViewPendingWarehouses()
    {
        var pendingWarehouses = await _db.Warehouses
            .Include(ws => ws.Store)
            .Where(ws => ws.Status == StatusValue.Pending).ToListAsync();

        return View(pendingWarehouses);
    }
    
    [HttpPost]
    public async Task<IActionResult> ApproveSlot(Guid warehouseId)
    {
        var slot = await _db.Warehouses.FindAsync(warehouseId);
        if (slot == null) return NotFound();

        slot.Status = StatusValue.Approved;
        await _db.SaveChangesAsync();
        return RedirectToAction("ViewPendingWarehouses", "Warehouse");
    }

    [HttpPost]
    public async Task<IActionResult> RejectSlot(Guid warehouseId)
    {
        var slot = await _db.Warehouses.FindAsync(warehouseId);
        if (slot == null) return NotFound();

        slot.Status = StatusValue.Rejected;
        await _db.SaveChangesAsync();
        return RedirectToAction("ViewPendingWarehouses", "Warehouse");
    }
}