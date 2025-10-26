using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Dtos.Warehouses
{
    public class WarehouseSlotImportRow
    {
        public string WarehouseName { get; set; } = null!;
        public string Code { get; set; } = null!;
        public int Row { get; set; }
        public int Col { get; set; }
        public decimal HeightM { get; set; }
        public decimal LengthM { get; set; }
        public decimal WidthM { get; set; }
        public decimal BasePricePerHour { get; set; }
        public DateTime? LeaseStart { get; set; }
        public DateTime? LeaseEnd { get; set; }
        public bool IsBlocked { get; set; }
        public string? ImageUrl { get; set; }
    }
}
