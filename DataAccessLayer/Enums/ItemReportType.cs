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
}
