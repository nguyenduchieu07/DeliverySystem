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
}
