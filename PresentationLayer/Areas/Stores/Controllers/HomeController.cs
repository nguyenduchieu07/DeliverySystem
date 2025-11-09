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

            var startDate = from?.Date ?? DateTime.Now.AddDays(-6).Date; // bắt đầu từ 00:00:00
            var endDate = to?.Date.AddDays(1).AddTicks(-1) ?? DateTime.Now; // kết thúc 23:59:59.9999999

            // Lấy payments completed
            var payments = await _db.Orders
                .Where(o => o.CreatedAt.Date >= startDate && o.CreatedAt.Date <= endDate)
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

            var now = DateTime.Now;
            var currentYear = now.Year;
            var currentMonth = now.Month;

            // Lấy tổng các payment đã hoàn thành trong tháng hiện tại
            var totalCompletedPaymentsInMonth = await _db.Orders
                .Where(o => o.CreatedAt.Year == currentYear && o.CreatedAt.Month == currentMonth)
                .SelectMany(o => o.Payments
                    .Where(p => p.Status == StatusValue.Completed)
                    .Select(p => p.Amount))
                .ToListAsync();
            
            var totalInMonth = (double)totalCompletedPaymentsInMonth.Sum();
            var dayCountInMonth = DateTime.DaysInMonth(currentYear, currentMonth);
            var average = dayCountInMonth > 0 ? totalInMonth / dayCountInMonth : 0;

            reports.AverageRevenueMonth = new AverageRevenueMonth
            {
                Total = totalInMonth,
                Average = average,
                Month = currentMonth,
                DayCountInMonth = dayCountInMonth
            };
            
            var vm = new DashboardViewModel
            {
                DashboardDto = reports,
                RevenueLabels = labels,
                RevenueData = data,
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
        public async Task<IActionResult> ExportRevenueReport(DateTime? from = null, DateTime? to = null)
        {
            var startDate = from ?? DateTime.Now.AddDays(-6);
            var endDate = to ?? DateTime.Now;

            using var workbook = new XLWorkbook();

            // ===== Sheet 1: Doanh thu theo quý =====
            var sheetQuarter = workbook.Worksheets.Add("Doanh thu theo quý");
            int year = DateTime.Now.Year;
            var ordersYear = await _db.Orders.Where(o => o.CreatedAt.Year == year).ToListAsync();

            var revenueByQuarter = Enumerable.Range(1, 4)
                .Select(q => new
                {
                    Quarter = q,
                    TotalRevenue = ordersYear
                        .Where(o => (o.CreatedAt.Month - 1) / 3 + 1 == q)
                        .Sum(o => o.TotalAmount)
                })
                .ToList();

            sheetQuarter.Cell(1, 1).Value = "Quý";
            sheetQuarter.Cell(1, 2).Value = "Doanh thu (VNĐ)";
            int rowQ = 2;
            foreach (var q in revenueByQuarter)
            {
                sheetQuarter.Cell(rowQ, 1).Value = $"Quý {q.Quarter}";
                sheetQuarter.Cell(rowQ, 2).Value = q.TotalRevenue;
                sheetQuarter.Cell(rowQ, 2).Style.NumberFormat.Format = "#,##0 ₫";
                rowQ++;
            }

            sheetQuarter.Columns().AdjustToContents();

            // ===== Sheet 2: So sánh doanh thu theo tháng =====
            var sheetCompare = workbook.Worksheets.Add("So sánh tháng");

            var paymentsCompare = await _db.Orders
                .Where(o => o.CreatedAt.Date >= startDate.Date && o.CreatedAt.Date <= endDate.Date)
                .SelectMany(o => o.Payments
                    .Where(p => p.Status == StatusValue.Completed)
                    .Select(p => new { o.CreatedAt, p.Amount }))
                .ToListAsync();

            // Liệt kê đủ 12 tháng
            var monthly = Enumerable.Range(1, 12)
                .Select(m => new
                {
                    Month = m,
                    TotalRevenue = paymentsCompare
                        .Where(p => p.CreatedAt.Month == m)
                        .Sum(p => p.Amount)
                })
                .ToList();

            decimal? prevRevenueMonth = null;
            sheetCompare.Cell(1, 1).Value = "Tháng";
            sheetCompare.Cell(1, 2).Value = "Doanh thu (VNĐ)";
            sheetCompare.Cell(1, 3).Value = "Tỉ lệ tăng trưởng (%)";

            var headerRangeM = sheetCompare.Range(1, 1, 1, 3);
            headerRangeM.Style.Font.Bold = true;
            headerRangeM.Style.Fill.BackgroundColor = XLColor.LightGray;
            headerRangeM.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            int rowM = 2;
            foreach (var m in monthly)
            {
                decimal growth = prevRevenueMonth.HasValue && prevRevenueMonth.Value != 0
                    ? Math.Round((m.TotalRevenue - prevRevenueMonth.Value) / prevRevenueMonth.Value * 100, 2)
                    : 0;

                sheetCompare.Cell(rowM, 1).Value = $"Tháng {m.Month}";
                sheetCompare.Cell(rowM, 2).Value = m.TotalRevenue;
                sheetCompare.Cell(rowM, 3).Value = growth;

                sheetCompare.Cell(rowM, 2).Style.NumberFormat.Format = "#,##0 ₫";
                sheetCompare.Cell(rowM, 3).Style.NumberFormat.Format = "0.00";

                prevRevenueMonth = m.TotalRevenue;
                rowM++;
            }

            sheetCompare.Columns().AdjustToContents();

            // ===== Sheet 3: Doanh thu chi tiết theo ngày =====
            var payments = await _db.Orders
                .Where(o => o.CreatedAt >= startDate && o.CreatedAt <= endDate)
                .SelectMany(o => o.Payments
                    .Where(p => p.Status == StatusValue.Completed)
                    .Select(p => new { o.CreatedAt, o.Id, p.Amount }))
                .ToListAsync();

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

            var sheetDaily = workbook.Worksheets.Add("Chi tiết ngày");
            sheetDaily.Cell(1, 1).Value = "Ngày";
            sheetDaily.Cell(1, 2).Value = "Số đơn hàng";
            sheetDaily.Cell(1, 3).Value = "Số đơn hoàn thành";
            sheetDaily.Cell(1, 4).Value = "Doanh thu (VNĐ)";
            sheetDaily.Cell(1, 5).Value = "Tỉ lệ tăng trưởng (%)";
            sheetDaily.Cell(1, 6).Value = "Giá trị trung bình đơn (VNĐ)";

            var headerRange = sheetDaily.Range(1, 1, 1, 6);
            headerRange.Style.Font.Bold = true;
            headerRange.Style.Fill.BackgroundColor = XLColor.LightGray;
            headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            decimal? prevRevenue = null;
            int rowD = 2;
            foreach (var d in daily)
            {
                var avgOrder = d.TotalOrders > 0 ? d.TotalRevenue / d.TotalOrders : 0;
                decimal growth = prevRevenue.HasValue && prevRevenue.Value != 0
                    ? Math.Round((d.TotalRevenue - prevRevenue.Value) / prevRevenue.Value * 100, 2)
                    : 0;

                sheetDaily.Cell(rowD, 1).Value = d.Date.ToString("dd/MM/yyyy");
                sheetDaily.Cell(rowD, 2).Value = d.TotalOrders;
                sheetDaily.Cell(rowD, 3).Value = d.TotalOrders;
                sheetDaily.Cell(rowD, 4).Value = d.TotalRevenue;
                sheetDaily.Cell(rowD, 5).Value = growth;
                sheetDaily.Cell(rowD, 6).Value = Math.Round(avgOrder, 0);

                prevRevenue = d.TotalRevenue;
                rowD++;
            }

            // Tổng cộng cuối sheet
            sheetDaily.Cell(rowD, 1).Value = "Tổng cộng";
            sheetDaily.Cell(rowD, 2).FormulaA1 = $"SUM(B2:B{rowD - 1})";
            sheetDaily.Cell(rowD, 3).FormulaA1 = $"SUM(C2:C{rowD - 1})";
            sheetDaily.Cell(rowD, 4).FormulaA1 = $"SUM(D2:D{rowD - 1})";
            sheetDaily.Cell(rowD, 6).FormulaA1 = $"IF(B{rowD}>0,D{rowD}/B{rowD},0)";

            sheetDaily.Range(rowD, 1, rowD, 6).Style.Font.Bold = true;
            sheetDaily.Range(rowD, 1, rowD, 6).Style.Fill.BackgroundColor = XLColor.LightGray;

            sheetDaily.Columns().AdjustToContents();

            // ===== Trả file =====
            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            stream.Position = 0;

            string fileName = $"BaoCaoDoanhThu_{startDate:yyyyMMdd}_{endDate:yyyyMMdd}.xlsx";
            return File(stream.ToArray(),
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                fileName);
        }
    }
}