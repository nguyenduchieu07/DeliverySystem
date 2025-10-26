using DataAccessLayer.Entities.Common;
using DataAccessLayer.Enums;
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
        public string? CoverImageUrl { get; set; }   // ảnh bìa
        public string? MapImageUrl { get; set; }     // sơ đồ (optional)
        public virtual Store Store { get; set; } = null!;
        public Address? Address { get; set; }
        public StatusValue Status { get; set; }
        // NEW: kích thước kho (m)
        public decimal HeightM { get; set; }
        public decimal LengthM { get; set; }
        public decimal WidthM { get; set; }
        public decimal VolumeM3 => Math.Round(HeightM * LengthM * WidthM, 3);

        public virtual ICollection<WarehouseSlot> Slots { get; set; } = new List<WarehouseSlot>();
    }
}
