using DataAccessLayer.Entities;

namespace PresentationLayer.Models;

public class AdminStoreDetailViewModel
{
    public int ReviewCount { get; set; }

    public Store Store { get; set; }

    public Dictionary<Guid, int> WarehouseSlotCount { get; set; } = new();
    public KycSubmission KycSubmission { get; set; }
}