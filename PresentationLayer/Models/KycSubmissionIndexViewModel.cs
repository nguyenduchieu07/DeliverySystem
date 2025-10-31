using DataAccessLayer.Entities;
using DataAccessLayer.Enums;

namespace PresentationLayer.Models
{
    public class KycSubmissionIndexViewModel
    {
        public IEnumerable<KycSubmission> KycSubmissions { get; set; } = new List<KycSubmission>();
        public KycStatus? Status { get; set; } 
        public string? StoreName { get; set; }  

    }
}
