using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Dtos.RegisterStore
{
    public class KycStatusViewModel
    {
        public RegisterStoreResponse StoreInfo { get; set; }
        public List<KycDocumentInfo> SubmittedDocuments { get; set; } = new List<KycDocumentInfo>();
        public DateTime SubmissionDate { get; set; }
        public int EstimatedProcessingDays { get; set; } = 2;
        public string SupportEmail { get; set; } = "support@storemanager.com";
        public string SupportPhone { get; set; } = "1900-xxxx";
    }

}
