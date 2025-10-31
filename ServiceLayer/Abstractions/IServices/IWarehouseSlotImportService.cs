using ServiceLayer.Dtos.ExcelImports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Abstractions.IServices
{
    public interface IWarehouseSlotImportService
    {
        Task<ImportResult> ImportAsync(Stream excelStream, CancellationToken ct = default);
    }
}
