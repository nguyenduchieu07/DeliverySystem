using DataAccessLayer.Entities;
using DataAccessLayer.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace PresentationLayer.Areas.Admin.ViewComponents;

public class StoresAvailableViewComponent : ViewComponent
{
    private readonly DeliverySytemContext _db;

    public StoresAvailableViewComponent(DeliverySytemContext db)
    {
        _db = db;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var availableStores = await _db.Stores
            .Include(s => s.Warehouses)
            .ThenInclude(w => w.Slots)
            .Where(s => s.Warehouses.Any(w =>
                // w.Status == StatusValue.Active &&
                w.Slots.Any(slot =>
                    slot.IsBlocked == false &&
                    slot.Status == StatusValue.Available)
            ))
            .Select(s => new
            {
                s.Id,
                s.StoreName,
                Warehouses = s.Warehouses.Select(w => new
                {
                    w.Id,
                    w.Name
                })
            })
            .ToListAsync();


        return View(availableStores);
    }
}