using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace PresentationLayer.Areas.Admin.ViewComponents
{
    public class RevenueByMonthViewComponent : ViewComponent
    {
        private readonly DeliverySytemContext _db;

        public RevenueByMonthViewComponent(DeliverySytemContext db)
        {
            _db = db;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
        
            var dic = new Dictionary<string, decimal>
            {
                { "Jan", 0 }, { "Feb", 0 }, { "Mar", 0 }, { "Apr", 0 },
                { "May", 0 }, { "Jun", 0 }, { "Jul", 0 }, { "Aug", 0 },
                { "Sep", 0 }, { "Oct", 0 }, { "Nov", 0 }, { "Dec", 0 }
            };

         
            var currentYear = DateTime.Now.Year;

         
            var data = await _db.Set<Order>()
                .Where(o => o.CreatedAt.Year == currentYear)
                .GroupBy(o => o.CreatedAt.Month)
                .Select(g => new
                {
                    Month = g.Key,
                    Revenue = g.Sum(x => x.TotalAmount)
                })
                .ToListAsync();

          
            foreach (var item in data)
            {
                var monthName = CultureInfo.InvariantCulture
                    .DateTimeFormat
                    .GetAbbreviatedMonthName(item.Month); 

                if (dic.ContainsKey(monthName))
                    dic[monthName] = item.Revenue;
            }

       
            return View(dic);
        }
    }
}