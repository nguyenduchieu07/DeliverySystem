using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Enums
{
    public enum ItemReportType
    {
        Damaged,
        NotAsDescribed,
        MissingItem,
        
        CheckedIn,   // Đã nhập kho
        CheckedOut   // Đã xuất kho
    }

    public static class ItemReportTypeExtensions
    {
        public static string ToStringForType(this ItemReportType itemReportType)
        {
            return itemReportType switch
            {
                ItemReportType.Damaged => "Hư hỏng",
                ItemReportType.NotAsDescribed => "Không như mô tả", 
                ItemReportType.MissingItem => "Mất hàng", 
                
                ItemReportType.CheckedIn => "Nhập kho", 
                ItemReportType.CheckedOut => "Xuất kho", 
                _ => itemReportType.ToString()
            };
        }
    }
}
