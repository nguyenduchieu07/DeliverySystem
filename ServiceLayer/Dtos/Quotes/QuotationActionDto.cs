using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Dtos.Quotes
{
    public class QuotationActionDto
    {
        public Guid QuotationId { get; set; }
        public string Action { get; set; } // "accept" or "reject"
        public string Note { get; set; }
    }
}
