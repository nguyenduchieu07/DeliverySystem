using DataAccessLayer.Entities;
using DataAccessLayer.Enums;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace PresentationLayer.Areas.Admin.ViewComponents
{
    public class WarehouseInfoViewComponent : ViewComponent
    {
        private readonly DeliverySytemContext _db;
        public WarehouseInfoViewComponent(DeliverySytemContext db) => _db = db;

        public async Task<IViewComponentResult> InvokeAsync(Guid warehouseId)
        {
            var warehouse = await _db.Warehouses
                .Include(w => w.Slots)
                .FirstOrDefaultAsync(w => w.Id == warehouseId);

            if (warehouse == null)
            {
                TempData["Error"] = "Warehouse not found!";
                return View();
            }

            warehouse.Slots = warehouse.Slots
                .OrderBy(s => s.Row)
                .ThenBy(s => s.Col).ToList();

            return View(warehouse);
           
        }


    }
}
