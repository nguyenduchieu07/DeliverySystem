using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Dtos.RegisterStore
{
    public class RegisterStoreRequest
    {
        public Guid UserId { get; set; }    
        // Store Information
        public string StoreName { get; set; }
        public string LegalName { get; set; }
        
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Số giấy phép kinh doanh phải có đúng 10 chữ số")]
        [Display(Name = "Số giấy phép kinh doanh")]
        public string LicenseNumber { get; set; }
        
        [RegularExpression(@"^(\d{10}|\d{13})$", ErrorMessage = "Mã số thuế phải có 10 hoặc 13 chữ số")]
        [Display(Name = "Mã số thuế")]
        public string TaxNumber { get; set; }
        
        [RegularExpression(@"^\d{12}$", ErrorMessage = "Căn cước công dân phải có đúng 12 chữ số")]
        [Display(Name = "Căn cước công dân")]
        public string ID { get; set; }

        // Address Information
        public string AddressLine { get; set; }
        public string Ward { get; set; }
        public string District { get; set; }
        public string City { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
    }
}
