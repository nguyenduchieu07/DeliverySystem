using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Enums
{
    public enum ReportStatus
    {
        Pending,
        Done,
        
        CheckIn,
        CheckOut
    }
    
    public static class ReportStatusExtensions
    {
        public static string ToStringForReport(this  ReportStatus status)
        {
            return status switch
            {
                ReportStatus.Pending => "Chờ xử lý",
                ReportStatus.Done => "Đã xử lý", 
                _ => status.ToString()
            };
        }
    }
}
