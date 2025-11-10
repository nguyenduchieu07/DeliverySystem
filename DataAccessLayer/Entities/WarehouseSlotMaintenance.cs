using System.ComponentModel.DataAnnotations.Schema;
using DataAccessLayer.Entities.Common;
using DataAccessLayer.Enums;

namespace DataAccessLayer.Entities;

public class WarehouseSlotMaintenance : BaseEntity<Guid>
{
    public Guid WarehouseSlotId { get; set; }
    public Guid MaintenanceItemId { get; set; }

    public DateTime ScheduledStart { get; set; }

    public DateTime ScheduledEnd { get; set; }

    public DateTime? ActualStart { get; set; }

    public DateTime? ActualEnd { get; set; }

    public MaintenanceStatus Status { get; set; }

    public Guid? AssignedStaffId { get; set; }

    public string? Note { get; set; }

    public string? Result { get; set; }

    [ForeignKey(nameof(WarehouseSlotId))] public virtual WarehouseSlot WarehouseSlot { get; set; } = null!;

    [ForeignKey(nameof(MaintenanceItemId))]
    public virtual MaintenanceItem MaintenanceItem { get; set; } = null!;
}