using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Mvc;

namespace PresentationLayer.Models
{
    public class StoreOrderIndexViewModel 
    {
        public Warehouse Warehouse { get; set; }
        public List<Order> Orders { get; set; } = new();
        public int TotalElements { get; set; }

    }
}
