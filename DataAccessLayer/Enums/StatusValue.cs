using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Enums
{
    public enum StatusValue
    {
        Pending,
        Canceled,
        Rejected,       
        Approved,
        Available,
        Reserved,
        InUse,
        Maintenance,
        Active,
        InActive,
        Ban,
        Unban,
        Expired,
        Payment,
        Completed,
        PendingKyc,
        Verified,
        Occupied,
        NotSubmitted,
        Draft,
        Sent,
        Success,
        ExpiringSoon,
        Blocked,
        Revised
    }
}
