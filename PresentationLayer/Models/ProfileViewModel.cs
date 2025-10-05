// PresentationLayer/Models/ProfileViewModel.cs
using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;

namespace PresentationLayer.Models
{
    public class ProfileViewModel
    {
        public Customer? Customer { get; set; }
        public List<Address> Addresses { get; set; } = new List<Address>();
        public List<Order> Orders { get; set; } = new List<Order>();
        public Address NewAddress { get; set; } = new Address();

        // Thêm thuộc tính cho booking kho
        public Address PickupAddress { get; set; } = new Address();
        public Address DropoffAddress { get; set; } = new Address();
        public DateTime? DeliveryDate { get; set; }
        public DateTime? PickupDate { get; set; }
        public string Note { get; set; }
        public string SelectedVehicle { get; set; } = "motorbike";
        public float? DistanceKm { get; set; }
        public int? EtaMinutes { get; set; }
        public bool UseStoredPickupAddress { get; set; }
        public bool UseStoredDropoffAddress { get; set; }
    }
}