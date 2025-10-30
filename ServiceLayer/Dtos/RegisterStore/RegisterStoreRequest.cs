using System;
using System.Collections.Generic;
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
        public string LicenseNumber { get; set; }
        public string TaxNumber { get; set; }
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
