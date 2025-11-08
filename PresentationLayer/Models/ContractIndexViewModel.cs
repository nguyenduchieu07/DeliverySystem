using DataAccessLayer.Entities;

namespace PresentationLayer.Models
{
    public class ContractIndexViewModel
    {
        public Order Order { get; set; }
        public Contract? Contract { get; set; }
        
        public List<OrderItem> Items { get; set; }
    }
}
