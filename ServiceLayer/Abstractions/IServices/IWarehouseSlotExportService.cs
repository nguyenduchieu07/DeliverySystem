using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Abstractions.IServices
{
    public interface IWarehouseSlotExportService
    {
        /// <summary>Tạo file template (sheet Slots, chỉ header & mô tả).</summary>
        Task<byte[]> ExportTemplateAsync(string warehouseName, CancellationToken ct = default);

        /// <summary>Xuất toàn bộ slots của 1 kho ra Excel theo format nhập (không Row/Col).</summary>
        Task<byte[]> ExportWarehouseSlotsAsync(Guid warehouseId, CancellationToken ct = default);

        /// <summary>(Tuỳ chọn) Xuất nhiều kho – mỗi kho 1 sheet tên theo kho.</summary>
        Task<byte[]> ExportMultiWarehousesAsync(Guid[] warehouseIds, CancellationToken ct = default);
    }
}
