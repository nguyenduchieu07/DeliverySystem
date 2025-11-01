using DataAccessLayer.Entities.Common;

namespace DataAccessLayer.Entities
{
    public class OrderWarehouseSlot : BaseEntity<Guid>
    {
        public Guid OrderId { get; set; }
        public Guid WarehouseSlotId { get; set; }

        // Thời gian gán - giúp tracking lịch sử
        public DateTime AssignedAt { get; set; } 
        public DateTime? ReleasedAt { get; set; }

        // Navigation
        public virtual Order Order { get; set; } = null!;
        public virtual WarehouseSlot WarehouseSlot { get; set; } = null!;
    }
}
