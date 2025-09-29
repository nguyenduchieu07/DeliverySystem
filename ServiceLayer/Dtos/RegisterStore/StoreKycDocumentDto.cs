using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Dtos.RegisterStore
{
    public class StoreKycDocumentDto
    {
        public string DocType { get; set; } // BusinessLicense, TaxCertificate, OwnerID
        public IFormFile File { get; set; }
    }
}
