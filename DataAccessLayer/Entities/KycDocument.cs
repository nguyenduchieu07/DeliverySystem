using DataAccessLayer.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Entities
{
    public class KycDocument : BaseEntity<Guid>
    {
        public Guid KycSubmissionId { get; set; }
        public string DocType { get; set; } = null!;    
        public string FilePath { get; set; } = null!;    
        public string? Hash { get; set; }                

        public virtual KycSubmission KycSubmission { get; set; } = null!;
    }
}
