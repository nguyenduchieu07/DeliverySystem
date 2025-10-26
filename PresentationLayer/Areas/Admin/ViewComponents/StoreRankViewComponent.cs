using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace PresentationLayer.Areas.Admin.ViewComponents;

public class StoreRankViewComponent : ViewComponent
{
    private readonly DeliverySytemContext _db;

    public StoreRankViewComponent(DeliverySytemContext db)
    {
        _db = db;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        // Lấy top 5 cửa hàng có nhiều đơn hàng nhất
        var topStores = await _db.Set<Order>()
            .Include(o => o.Store) // giả sử Order có navigation Store
            .GroupBy(o => o.StoreId)
            .Select(g => new
            {
                StoreName = g.First().Store.StoreName, // hoặc g.Key nếu bạn chỉ có StoreId
                OrderCount = g.Count()
            })
            .OrderByDescending(x => x.OrderCount)
            .Take(4)
            .ToDictionaryAsync(x => x.StoreName, x => x.OrderCount);

        return View(topStores);
    }
}