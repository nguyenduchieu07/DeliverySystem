namespace PresentationLayer.Areas.Stores.Models
{
    public class WareHouseDashboard
    {
        public string Address { get; set; }

        public List<WareHouseSlotDashboard> Slots { get; set; } 
    }
}
