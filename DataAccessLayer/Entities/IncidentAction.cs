using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Entities.Common;
using DataAccessLayer.Enums;

namespace DataAccessLayer.Entities
{
    public partial class IncidentAction : BaseEntity<Guid>
    {
        public Guid IncidentReportId { get; set; }

        /*
         các mức đền bù được triển khai ở FE
        Nhẹ: đền ~30–50% giá trị mặt hàng.
        Nặng: đền ~70–90% giá trị mặt hàng.
         */
        public IncidentActionType ActionType { get; set; }  // e.g. ConfirmDamage, ReturnItem, Compensate, Close

        public string? Note { get; set; }
        public DateTime ActionDate { get; set; } = DateTime.UtcNow;
        public Guid? StaffId { get; set; } // nhân viên kho xử lý

        // Navigation
        public virtual IncidentReport IncidentReport { get; set; } = null!;
        public virtual StoreStaff? Staff { get; set; }
    }
}
