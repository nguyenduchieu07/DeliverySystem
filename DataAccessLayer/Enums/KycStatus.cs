using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Enums
{
    public enum KycStatus
    {
        Pending, NeedChanges, Approved, Rejected
    }

    public static partial class EnumExtensions
    {
        public static string ToDisplayStringForKycStatus(this KycStatus status)
        {
            return status switch
            {
                KycStatus.Pending => "Chưa giải quyết",
                KycStatus.NeedChanges => "Cần thay đổi",
                KycStatus.Approved => "Tán thành",
                KycStatus.Rejected => "Từ chối",
                _ => status.ToString()
            };
        }
    }
}
