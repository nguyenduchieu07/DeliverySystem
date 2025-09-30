using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Dtos.RegisterStore
{
    public class KycDocumentInfo
    {
        public string DocType { get; set; }
        public string DisplayName { get; set; }
        public string FileName { get; set; }
        public DateTime UploadDate { get; set; }
        public string Status { get; set; } // Pending, Approved, Rejected
        public string FileSize { get; set; }
    }
}
