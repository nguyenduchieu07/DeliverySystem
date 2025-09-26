namespace PresentationLayer.Areas.Admin.Models
{
    public class KycSubmitVM
    {
        public string? Note { get; set; }
        public IFormFile? LicenseFile { get; set; }
        public IFormFile? TaxFile { get; set; }
        public IFormFile? IdFile { get; set; }
    }
}
