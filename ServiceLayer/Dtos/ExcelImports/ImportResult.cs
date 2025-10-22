using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Dtos.ExcelImports
{
    public class ImportResult
    {
        public int TotalRows { get; set; }
        public int Success { get; set; }
        public int Skipped { get; set; }
        public List<ImportError> Errors { get; set; } = new();
    }

    public class ImportError
    {
        public int RowIndex { get; set; }        // số dòng trong Excel (1-based)
        public string Field { get; set; } = "";  // cột gây lỗi, ví dụ "Code"
        public string Message { get; set; } = "";
    }
}
