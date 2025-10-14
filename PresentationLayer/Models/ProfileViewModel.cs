using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;

namespace PresentationLayer.Models
{
    public class ProfileViewModel
    {
        public Customer Customer { get; set; }
        public List<AddressViewModel> Addresses { get; set; }
        public AddressViewModel NewAddress { get; set; }
        public List<Order> Orders { get; set; }

        // Thuộc tính cho order
        public string PickupAddress { get; set; }
        public string DropoffAddress { get; set; }
        public double DistanceKm { get; set; }
        public int EtaMinutes { get; set; }
        public DateTime DeliveryDate { get; set; }
        public DateTime PickupDate { get; set; }
        public string Note { get; set; }
        public string VehicleType { get; set; }
        public Guid? ServiceCategoryId { get; set; }
        public List<OrderItemViewModel> Items { get; set; }

        public ProfileViewModel()
        {
            Addresses = new List<AddressViewModel>();
            NewAddress = new AddressViewModel();
            Orders = new List<Order>();
            Items = new List<OrderItemViewModel>();
        }
    }
}