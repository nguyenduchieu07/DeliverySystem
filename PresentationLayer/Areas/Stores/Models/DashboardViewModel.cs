namespace PresentationLayer.Areas.Stores.Models;

public class DashboardViewModel
{
    public DashboardDto DashboardDto { get; set; }
    // Dữ liệu cho biểu đồ doanh thu
    public List<string> RevenueLabels { get; set; } = new(); // Ngày/tháng hiển thị
    public List<decimal> RevenueData { get; set; } = new();  // Doanh thu tương ứng
}