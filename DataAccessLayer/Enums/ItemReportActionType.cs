using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Enums
{
    public enum ItemReportActionType
    {
        ConfirmDamage, //xác nhận hư hỏng
        ReturnItem,
        Compensate, //đền bù
        Close //đóng 
    }
    
    public static class  ItemReportActionTypeExtensions 
    {
        public static string ToDisplayString(this ItemReportActionType itemReportActionType)
        {
            return itemReportActionType switch
            {
                ItemReportActionType.ConfirmDamage => "Xác nhận hư hỏng",
                ItemReportActionType.ReturnItem => "Hoàn hàng", 
                ItemReportActionType.Compensate => "Bồi thường", 
                ItemReportActionType.Close => "Đóng báo cáo", 
                _ => itemReportActionType.ToString()
            };
        }
    }
}
