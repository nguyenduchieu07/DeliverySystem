using DataAccessLayer.Entities;

namespace PresentationLayer.Models
{
    public class DeliveryOrderViewModel
    {
        public Order Order { get; set; }
        public List<Store> NearbyStores { get; set; } = new();
    }
}
