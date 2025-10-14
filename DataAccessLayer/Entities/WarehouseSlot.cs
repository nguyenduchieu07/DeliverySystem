using DataAccessLayer.Entities.Common;
using DataAccessLayer.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Entities
{
    public class WarehouseSlot : BaseEntity<Guid>
    {
        public Guid WarehouseId { get; set; }
        public string Code { get; set; } = null!;
        public string? Size { get; set; }
        public string? ImageUrl { get; set; }
        public Guid? CurrentOrderId { get; set; }
        public StatusValue Status { get; set; }
        public virtual Warehouse Warehouse { get; set; } = null!;
    }
}
