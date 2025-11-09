using DataAccessLayer.Entities.Common;
using DataAccessLayer.Enums;

namespace DataAccessLayer.Entities;

public class MaintenanceItem : BaseEntity<Guid>
{
    public string Name { get; set; } = null!;
    
    public string? Description { get; set; }
    
    public int EstimatedDurationMinutes { get; set; }
    
    public bool RequireShutdown { get; set; }
    
    public StatusValue Status { get; set; }

    public virtual ICollection<WarehouseSlotMaintenance> SlotMaintenances { get; set; } = new List<WarehouseSlotMaintenance>();
}