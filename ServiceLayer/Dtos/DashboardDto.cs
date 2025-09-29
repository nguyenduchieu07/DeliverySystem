using ServiceLayer.Dtos;

namespace PresentationLayer.Areas.Stores.Models
{
    public class DashboardDto
    {
        public OrderToday? OrderToday { get; set; }

        public int TotalOrderPending { get; set; } = 0;

        public Revenue? Revenue { get; set; }

        public RatingDto? AvarageRating { get; set; }

        public List<OrderPeding> OrderPendings { get; set; } = new List<OrderPeding>();

        public List<WareHouseDashboard> WareHouseDashboards { get; set; } = new List<WareHouseDashboard>();

    }
}
