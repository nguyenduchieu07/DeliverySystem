using DataAccessLayer.Entities.Common;
using System;
using System.Collections.Generic;

namespace DataAccessLayer.Entities
{
    public partial class Customer : BaseEntity<Guid>
    {
        public Guid Id { get; set; }

        public string FullName { get; set; } = null!;
        public string Email { get; set; }       // Thêm thuộc tính Email
        public string PhoneNumber { get; set; }

        public string? PreferredLang { get; set; }

        public string? Tier { get; set; }

        public string? KycLevel { get; set; }

        // Xóa CustomerNavigation, giữ mối quan hệ với User
        public virtual User User { get; set; } = null!;

        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
        public virtual ICollection<Quotation> Quotations { get; set; } = new List<Quotation>();
    }
}