using DataAccessLayer.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Entities
{
    public partial class ServiceAddon : BaseEntity<Guid>
    {
        public Guid ServiceId { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }

        // true = % trên giá cơ bản; false = số tiền cố định
        public bool IsPercentage { get; set; }
        public decimal Value { get; set; }

        public virtual Service Service { get; set; } = null!;
    }
}
