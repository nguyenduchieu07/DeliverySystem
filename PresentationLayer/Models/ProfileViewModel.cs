using DataAccessLayer.Entities;

namespace PresentationLayer.Models
{
    public class ProfileViewModel
    {
        public Customer? Customer { get; set; }
        public List<Address> Addresses { get; set; } = new List<Address>();
        public List<Order> Orders { get; set; } = new List<Order>();
        public Address NewAddress { get; set; } = new Address();
    }
}
