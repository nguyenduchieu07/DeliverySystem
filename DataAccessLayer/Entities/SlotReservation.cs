using DataAccessLayer.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Entities
{
    public class SlotReservation : BaseEntity<Guid>
    {
        public Guid WarehouseSlotId { get; set; }
        public Guid? OrderId { get; set; }
        public DateTimeOffset ExpiresAt { get; set; } // now + 24h
        public string Status { get; set; } = "Reserved"; // Reserved/Expired/Used
    }
}
