using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Dtos.Quotes
{
    public class QuotationServiceDto
    {
        public Guid ServiceId { get; set; }
        public string ServiceName { get; set; }
        public string Description { get; set; }
        public string Unit { get; set; }
        public decimal BasePrice { get; set; }
        public int Quantity { get; set; }
        public decimal Subtotal { get; set; }

        // Thông tin chi tiết
        public string CategoryName { get; set; }
        public List<string> Addons { get; set; } = new();
        public string SizeOption { get; set; }
    }
}
