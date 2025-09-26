namespace PresentationLayer.Areas.Admin.Models
{
    public class DashboardVM
    {
        public int PendingKyc { get; set; }
        public int ActiveStores { get; set; }
        public int OrdersToday { get; set; }
        public int LowRatingCount { get; set; }
    }
}
