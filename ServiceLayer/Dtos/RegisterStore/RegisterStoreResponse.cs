using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Dtos.RegisterStore
{
    public class RegisterStoreResponse
    {
        public Guid UserId { get; set; }
        public Guid StoreId { get; set; }
        public string StoreName { get; set; }
        public string Status { get; set; }
        public string KycStatus { get; set; }
        public string Message { get; set; }
    }
}
