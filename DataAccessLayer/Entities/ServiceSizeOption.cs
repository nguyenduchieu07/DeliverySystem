using DataAccessLayer.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Entities
{
    public partial class ServiceSizeOption : BaseEntity<Guid>
    {
        public Guid ServiceId { get; set; }

        public string Code { get; set; } = null!;        // S/M/L/XL
        public string DisplayName { get; set; } = null!; // "M (1.2 m³)"

        public decimal? VolumeM3 { get; set; }
        public decimal? AreaM2 { get; set; }
        public decimal? MaxWeightKg { get; set; }

        // Nếu có giá riêng cho size (override)
        public decimal? PriceOverride { get; set; }

        public virtual Service Service { get; set; } = null!;
    }
}
