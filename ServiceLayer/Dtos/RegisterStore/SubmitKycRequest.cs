using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Dtos.RegisterStore
{
    public class SubmitKycRequest
    {
        public Guid StoreId { get; set; }
        public List<StoreKycDocumentDto> Documents { get; set; } = new List<StoreKycDocumentDto>();
    }
}
