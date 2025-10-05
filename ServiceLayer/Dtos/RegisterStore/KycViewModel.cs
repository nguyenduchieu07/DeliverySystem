using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Dtos.RegisterStore
{
    public class KycViewModel
    {
        public RegisterStoreResponse Response { get; set; }
        public SubmitKycRequest KycRequest { get; set; } = new SubmitKycRequest();
    }
}
