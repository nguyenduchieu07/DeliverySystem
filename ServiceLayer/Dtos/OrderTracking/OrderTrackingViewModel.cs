using DataAccessLayer.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Dtos.OrderTracking
{
    public class OrderTrackingViewModel
    {
        public Guid Id { get; set; }
        public string OrderCode { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public string StoreName { get; set; }

        // Địa chỉ
        public string PickupAddress { get; set; }
        public string DropoffAddress { get; set; }

        // Thời gian
        public DateTime CreatedAt { get; set; }
        public DateTime? PickupDate { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public DateTime? CheckInTime { get; set; }
        public DateTime? CheckOutTime { get; set; }

        // Trạng thái
        public StatusValue Status { get; set; }
        public string StatusDisplay { get; set; }
        public string StatusColor { get; set; }

        // Chi tiết
        public decimal TotalAmount { get; set; }
        public decimal? DistanceKm { get; set; }
        public int? EtaMinutes { get; set; }
        public string Note { get; set; }

        // Danh sách hàng hóa
        public List<OrderItemTrackingViewModel> Items { get; set; }

        // Timeline tracking
        public List<TrackingEventViewModel> TrackingEvents { get; set; }

        // Contract info
        public ContractInfoViewModel ContractInfo { get; set; }
    }

    public class OrderItemTrackingViewModel
    {
        public string ItemName { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public string SizeCode { get; set; }
        public decimal? WeightKg { get; set; }
        public decimal? VolumeM3 { get; set; }
        public string CategoryName { get; set; }
        public decimal? UnitPrice { get; set; }
        public decimal? Subtotal { get; set; }
    }

    public class TrackingEventViewModel
    {
        public DateTime EventTime { get; set; }
        public string EventType { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public string Icon { get; set; }
        public string Color { get; set; }
    }

    public class ContractInfoViewModel
    {
        public Guid? QuotationId { get; set; }
        public DateTime? ValidUntil { get; set; }
        public bool IsExpired { get; set; }
        public int DaysRemaining { get; set; }
        public StatusValue ContractStatus { get; set; }
        public decimal ContractAmount { get; set; }
    }

    public class OrderListViewModel
    {
        public Guid Id { get; set; }
        public string OrderCode { get; set; }
        public DateTime CreatedAt { get; set; }
        public string StoreName { get; set; }
        public StatusValue Status { get; set; }
        public string StatusDisplay { get; set; }
        public string StatusColor { get; set; }
        public decimal TotalAmount { get; set; }
        public string PickupAddress { get; set; }
        public string DropoffAddress { get; set; }
        public DateTime? DeliveryDate { get; set; }
    }
}
