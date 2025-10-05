using DataAccessLayer.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Entities
{
    public class Warehouse : BaseEntity<Guid>
    {
        public Guid StoreId { get; set; }
        public string Name { get; set; } = null!;
        public Guid? AddressRefId { get; set; } 
        public virtual Store Store { get; set; } = null!;

        public Address? Address { get; set; }
        public virtual ICollection<WarehouseSlot> Slots { get; set; } = new List<WarehouseSlot>();
    }
}
