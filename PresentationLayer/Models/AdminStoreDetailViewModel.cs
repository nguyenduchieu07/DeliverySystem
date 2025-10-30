using DataAccessLayer.Entities;

namespace PresentationLayer.Models;

public class AdminStoreDetailViewModel
{
    public int ReviewCount { get; set; }

    public Store Store { get; set; }
    public KycSubmission KycSubmission { get; set; }
}