using DataAccessLayer.Entities.Common;
using DataAccessLayer.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Entities
{
    public class KycSubmission : BaseEntity<Guid>
    {
        public Guid StoreId { get; set; }
        public KycStatus Status { get; set; } // Pending | NeedChanges | Approved | Rejected
        public string? AdminNote { get; set; }

        public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ReviewedAt { get; set; }
        public Guid? ReviewedBy { get; set; }

        public virtual Store Store { get; set; } = null!;
        public virtual ICollection<KycDocument> Documents { get; set; } = new List<KycDocument>();
    }
}
