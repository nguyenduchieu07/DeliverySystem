using DataAccessLayer.Enums;

namespace PresentationLayer.Areas.Stores.Models
{
    public class WareHouseSlotDashboard
    {
        public Guid Id { get; set; }

        public string Code { get; set; }

        public StatusValue Status { get; set; }

    }
}
