using DataAccessLayer.Constants;
using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServiceLayer.Abstractions.IServices;
using System.Security.Claims;
using ClosedXML.Excel;
using DataAccessLayer.Enums;
using Microsoft.AspNetCore.Identity;
using PresentationLayer.Areas.Stores.Models;

namespace PresentationLayer.Areas.Stores.Controllers
{
    [Area("Stores")]
    [Authorize(Roles = $"{UserRoles.STORE}, {UserRoles.STORESTAFF}")]
    public class HomeController : Controller
    {
        private readonly IDashboardService _dashboardService;
        private readonly DeliverySytemContext _db;
        private readonly UserManager<User> _userManager;

        public HomeController(IDashboardService dashboardService, DeliverySytemContext db,
            UserManager<User> userManager)
        {
            _dashboardService = dashboardService;
            _db = db;
            _userManager = userManager;
        }


        public async Task<IActionResult> Index(DateTime? from = null, DateTime? to = null)
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var id = Guid.Parse(userId!);

            var user = await _userManager.FindByIdAsync(userId);

            var isStoreRole = await _userManager.IsInRoleAsync(user, UserRoles.STORE);
            var isStoreStaffRole = await _userManager.IsInRoleAsync(user, UserRoles.STORESTAFF);

            Store? store = null;
            if (isStoreRole)
            {
                store = await _db.Stores.Where(e => e.OwnerUserId == id).FirstOrDefaultAsync();
            }
            else if (isStoreStaffRole)
            {
                var staffStore = await _db.StoreStaffs.FirstAsync(x => x.UserId == id);
                store = await _db.Stores.Where(e => e.Id == staffStore.StoreId).FirstOrDefaultAsync();
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }


            if (store.Status == DataAccessLayer.Enums.StatusValue.Pending)
            {
                return BadRequest("Store is pending to approved by admin");
            }

            var reports = await _dashboardService.GetDashboard(store.Id);
            if (reports == null)
            {
                reports = new Models.DashboardDto();
            }

            var startDate = from ?? DateTime.Now.AddDays(-6); // default 7 ngày
            var endDate = to ?? DateTime.Now;

            // Lấy payments completed
            var payments = await _db.Orders
                .Where(o => o.CreatedAt >= startDate && o.CreatedAt <= endDate)
                .SelectMany(o => o.Payments
                    .Where(p => p.Status == StatusValue.Completed)
                    .Select(p => new { o.CreatedAt, p.Amount }))
                .ToListAsync();

            List<string> labels;
            List<decimal> data;


            var daily = payments
                .GroupBy(x => x.CreatedAt.Date)
                .Select(g => new
                {
                    Date = g.Key,
                    Total = g.Sum(x => x.Amount)
                })
                .OrderBy(x => x.Date)
                .ToList();

            labels = daily.Select(x => x.Date.ToString("dd/MM")).ToList();
            data = daily.Select(x => x.Total).ToList();

            var vm = new DashboardViewModel
            {
                DashboardDto = reports,
                RevenueLabels = labels,
                RevenueData = data
            };

            return View(vm);
        }

        [HttpGet("/Stores/GetRevenueChangesBy2Months")] //In %
        public IActionResult GetRevenueChangesBy2Months(int month1, int month2)
        {
            // Lấy doanh thu từng tháng từ DB
            decimal revenue1 = _db.Orders
                .Where(o => o.CreatedAt.Month == month1)
                .Sum(o => o.TotalAmount);

            decimal revenue2 = _db.Orders
                .Where(o => o.CreatedAt.Month == month2)
                .Sum(o => o.TotalAmount);

            decimal percentChange = revenue1 == 0 ? 100 : Math.Round((revenue2 - revenue1) / revenue1 * 100, 2);

            return Json(new
            {
                revenues = new[] { revenue1, revenue2 }, // dùng cho chart
                changes = new[] { percentChange } // dùng cho hiển thị %
            });
        }

        [HttpGet("/Stores/GetRevenueByQuarter")]
        public async Task<IActionResult> GetRevenueByQuarter(int year)
        {
            var orders = await _db.Orders
                .Where(o => o.CreatedAt.Year == year)
                .ToListAsync();

            var revenueByQuarter = orders
                .GroupBy(o => (o.CreatedAt.Month - 1) / 3 + 1)
                .Select(g => new
                {
                    Quarter = g.Key,
                    TotalRevenue = g.Sum(o => o.TotalAmount)
                })
                .ToList();

            // Đảm bảo có đủ 4 quý
            var allQuarters = Enumerable.Range(1, 4)
                .Select(q => new
                {
                    Quarter = q,
                    TotalRevenue = revenueByQuarter.FirstOrDefault(r => r.Quarter == q)?.TotalRevenue ?? 0
                })
                .ToList();

            return Json(allQuarters);
        }
        
        
        [HttpGet("/Stores/ExportRevenueReport")]
        public async Task<IActionResult> ExportRevenueReport(DateTime from, DateTime to)
        {
            // Lấy dữ liệu từ DB
            var payments = await _db.Orders
                .Where(o => o.CreatedAt >= from && o.CreatedAt <= to)
                .SelectMany(o => o.Payments
                    .Where(p => p.Status == StatusValue.Completed)
                    .Select(p => new { o.CreatedAt, o.Id, p.Amount }))
                .ToListAsync();

            // Group theo ngày
            var daily = payments
                .GroupBy(x => x.CreatedAt.Date)
                .Select(g => new
                {
                    Date = g.Key,
                    TotalRevenue = g.Sum(x => x.Amount),
                    TotalOrders = g.Select(x => x.Id).Distinct().Count()
                })
                .OrderBy(x => x.Date)
                .ToList();

            // Tính thêm các cột
            var rows = new List<dynamic>();
            decimal? prevRevenue = null;
            foreach (var d in daily)
            {
                var avgOrder = d.TotalOrders > 0 ? d.TotalRevenue / d.TotalOrders : 0;
                decimal growth = prevRevenue.HasValue && prevRevenue.Value != 0
                    ? Math.Round((d.TotalRevenue - prevRevenue.Value) / prevRevenue.Value * 100, 2)
                    : 0;

                rows.Add(new
                {
                    Ngay = d.Date.ToString("dd/MM/yyyy"),
                    SoDonHang = d.TotalOrders,
                    SoDonHoanThanh = d.TotalOrders, // nếu cần tách pending thì thay đổi
                    DoanhThu = d.TotalRevenue,
                    TyLeTangTruong = growth,
                    GiaTriTBDon = Math.Round(avgOrder, 0)
                });

                prevRevenue = d.TotalRevenue;
            }

            using var wb = new XLWorkbook();
            var ws = wb.Worksheets.Add("DoanhThu");

            // Header
            ws.Cell(1, 1).Value = "Ngày";
            ws.Cell(1, 2).Value = "Số đơn hàng";
            ws.Cell(1, 3).Value = "Số đơn hoàn thành";
            ws.Cell(1, 4).Value = "Doanh thu (VNĐ)";
            ws.Cell(1, 5).Value = "Tỉ lệ tăng trưởng (%)";
            ws.Cell(1, 6).Value = "Giá trị trung bình đơn (VNĐ)";

            // Format header
            var headerRange = ws.Range(1, 1, 1, 6);
            headerRange.Style.Font.Bold = true;
            headerRange.Style.Fill.BackgroundColor = XLColor.LightGray;
            headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            // Fill data
            int row = 2;
            foreach (var r in rows)
            {
                ws.Cell(row, 1).Value = r.Ngay;
                ws.Cell(row, 2).Value = r.SoDonHang;
                ws.Cell(row, 3).Value = r.SoDonHoanThanh;
                ws.Cell(row, 4).Value = r.DoanhThu;
                ws.Cell(row, 5).Value = r.TyLeTangTruong;
                ws.Cell(row, 6).Value = r.GiaTriTBDon;
                row++;
            }

            // Format cột số
            ws.Column(4).Style.NumberFormat.Format = "#,##0";
            ws.Column(5).Style.NumberFormat.Format = "0.00";
            ws.Column(6).Style.NumberFormat.Format = "#,##0";

            // Tổng cộng cuối sheet
            ws.Cell(row, 1).Value = "Tổng cộng";
            ws.Cell(row, 2).FormulaA1 = $"SUM(B2:B{row - 1})";
            ws.Cell(row, 3).FormulaA1 = $"SUM(C2:C{row - 1})";
            ws.Cell(row, 4).FormulaA1 = $"SUM(D2:D{row - 1})";
            ws.Cell(row, 6).FormulaA1 = $"IF(B{row}>0,D{row}/B{row},0)";

            ws.Range(row, 1, row, 6).Style.Font.Bold = true;
            ws.Range(row, 1, row, 6).Style.Fill.BackgroundColor = XLColor.LightGray;

            ws.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            wb.SaveAs(stream);
            stream.Position = 0;

            return File(stream.ToArray(),
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                $"BaoCaoDoanhThu_{from:yyyyMMdd}_{to:yyyyMMdd}.xlsx");
        }
    }
}