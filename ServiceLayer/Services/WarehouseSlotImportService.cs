using ClosedXML.Excel;
using DataAccessLayer;
using DataAccessLayer.Entities;
using DataAccessLayer.Enums;
using Microsoft.EntityFrameworkCore;
using ServiceLayer.Abstractions.IServices;
using ServiceLayer.Dtos.ExcelImports;
using ServiceLayer.Dtos.Warehouses;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

public class WarehouseSlotImportService : IWarehouseSlotImportService
{
    private readonly DeliverySytemContext _db;
    public WarehouseSlotImportService(DeliverySytemContext db) => _db = db;

    public async Task<ImportResult> ImportAsync(Stream excelStream, CancellationToken ct = default)
    {
        var result = new ImportResult();
        var rows = new List<(int excelRow, WarehouseSlotImportRow dto)>(); // giữ số dòng để báo lỗi
        var errors = new List<ImportError>();

        // 1) Đọc Excel
        using (var wb = new XLWorkbook(excelStream))
        {
            var ws = wb.Worksheet("Slots");
            if (ws == null)
            {
                errors.Add(new ImportError { RowIndex = 1, Field = "Sheet", Message = "Không tìm thấy sheet 'Slots'." });
                result.Errors = errors.OrderBy(e => e.RowIndex).ThenBy(e => e.Field).ToList();
                return result;
            }

            var used = ws.RangeUsed();
            if (used == null || used.RowCount() <= 1)
            {
                errors.Add(new ImportError { RowIndex = 1, Field = "Sheet", Message = "Sheet 'Slots' trống hoặc thiếu header." });
                result.Errors = errors.OrderBy(e => e.RowIndex).ThenBy(e => e.Field).ToList();
                return result;
            }

            // A:WarehouseName(1), B:Code(2), C:HeightM(3), D:LengthM(4), E:WidthM(5),
            // F:BasePricePerHour(6), G:LeaseStart(7), H:LeaseEnd(8), I:IsBlocked(9), J:ImageUrl(10)
            foreach (var r in used.RowsUsed().Skip(1))
            {
                var excelRowIndex = r.RowNumber();
                try
                {
                    var dto = new WarehouseSlotImportRow
                    {
                        WarehouseName = r.Cell(1).GetString().Trim(),
                        Code = r.Cell(2).GetString().Trim(),
                        HeightM = SafeDecimal(r.Cell(3)),
                        LengthM = SafeDecimal(r.Cell(4)),
                        WidthM = SafeDecimal(r.Cell(5)),
                        BasePricePerHour = SafeDecimal(r.Cell(6)),
                        LeaseStart = TryDate(r.Cell(7)),
                        LeaseEnd = TryDate(r.Cell(8)),
                        IsBlocked = TryBool(r.Cell(9)),
                        ImageUrl = NullIfEmpty(r.Cell(10).GetString())
                    };
                    rows.Add((excelRowIndex, dto));
                }
                catch (Exception ex)
                {
                    errors.Add(new ImportError { RowIndex = excelRowIndex, Field = "Row", Message = $"Lỗi đọc dòng: {ex.Message}" });
                }
            }
        }

        result.TotalRows = rows.Count;

        // 2) Validate mức 1: required + kiểu + ràng buộc logic cơ bản
        foreach (var (excelRow, dto) in rows)
        {
            if (string.IsNullOrWhiteSpace(dto.WarehouseName))
                errors.Add(new ImportError { RowIndex = excelRow, Field = "WarehouseName", Message = "Bắt buộc." });
            if (string.IsNullOrWhiteSpace(dto.Code))
                errors.Add(new ImportError { RowIndex = excelRow, Field = "Code", Message = "Bắt buộc." });

            if (!dto.IsBlocked)
            {
                if (dto.HeightM <= 0) errors.Add(new ImportError { RowIndex = excelRow, Field = "HeightM", Message = "Phải > 0 khi IsBlocked = FALSE." });
                if (dto.LengthM <= 0) errors.Add(new ImportError { RowIndex = excelRow, Field = "LengthM", Message = "Phải > 0 khi IsBlocked = FALSE." });
                if (dto.WidthM <= 0) errors.Add(new ImportError { RowIndex = excelRow, Field = "WidthM", Message = "Phải > 0 khi IsBlocked = FALSE." });
            }

            if (dto.BasePricePerHour < 0)
                errors.Add(new ImportError { RowIndex = excelRow, Field = "BasePricePerHour", Message = "Không được âm." });

            if (dto.LeaseStart.HasValue && dto.LeaseEnd.HasValue && dto.LeaseStart > dto.LeaseEnd)
                errors.Add(new ImportError { RowIndex = excelRow, Field = "LeaseStart/LeaseEnd", Message = "LeaseStart phải ≤ LeaseEnd." });
        }

        // 3) Validate mức 2: trùng Code trong chính file (theo từng WarehouseName)
        var groupByWh = rows.GroupBy(x => x.dto.WarehouseName, StringComparer.OrdinalIgnoreCase);
        foreach (var g in groupByWh)
        {
            var dupCode = g.GroupBy(x => x.dto.Code, StringComparer.OrdinalIgnoreCase)
                           .Where(gr => gr.Count() > 1);
            foreach (var d in dupCode)
                foreach (var item in d)
                    errors.Add(new ImportError { RowIndex = item.excelRow, Field = "Code", Message = $"Trùng Code trong file: '{item.dto.Code}'." });
        }

        // 4) Map WarehouseName -> WarehouseId; check tồn tại
        var distinctWhNames = rows.Select(x => x.dto.WarehouseName.Trim())
                                  .Where(x => !string.IsNullOrEmpty(x))
                                  .Distinct(StringComparer.OrdinalIgnoreCase)
                                  .ToList();

        var whNameToIdList = await _db.Warehouses
            .Where(w => distinctWhNames.Contains(w.Name))
            .Select(w => new { w.Name, w.Id })
            .ToListAsync(ct);

        var whNameToId = whNameToIdList
            .GroupBy(x => x.Name, StringComparer.OrdinalIgnoreCase)
            .ToDictionary(g => g.Key, g => g.First().Id, StringComparer.OrdinalIgnoreCase);

        foreach (var (excelRow, dto) in rows)
        {
            if (!whNameToId.TryGetValue(dto.WarehouseName, out _))
                errors.Add(new ImportError { RowIndex = excelRow, Field = "WarehouseName", Message = $"Không tồn tại trong DB: '{dto.WarehouseName}'." });
        }

        if (errors.Any())
        {
            result.Errors = Collate(errors);
            result.Skipped = result.TotalRows - result.Success;
            return result;
        }

        // 5) Tải dữ liệu slot đã có trong DB để check trùng Code (KHÔNG dùng Row/Col nữa)
        var whIds = whNameToId.Values.Distinct().ToList();
        var existing = await _db.WarehouseSlots
            .Where(s => whIds.Contains(s.WarehouseId))
            .Select(s => new { s.WarehouseId, s.Code })
            .ToListAsync(ct);

        var existingByWhCode = existing
            .GroupBy(x => x.WarehouseId)
            .ToDictionary(
                g => g.Key,
                g => new HashSet<string>(g.Select(x => x.Code), StringComparer.OrdinalIgnoreCase));

        // 6) Insert trong transaction (all-or-nothing). Không động tới Row/Col.
        using var tx = await _db.Database.BeginTransactionAsync(ct);
        try
        {
            foreach (var (excelRow, dto) in rows)
            {
                var whId = whNameToId[dto.WarehouseName];

                // Check trùng Code với DB
                if (existingByWhCode.TryGetValue(whId, out var codes) && codes.Contains(dto.Code))
                {
                    errors.Add(new ImportError { RowIndex = excelRow, Field = "Code", Message = $"Đã tồn tại trong DB: '{dto.Code}'." });
                    continue;
                }

                var entity = new WarehouseSlot
                {
                    Id = Guid.NewGuid(),
                    WarehouseId = whId,
                    Code = dto.Code,
                    Row = 0, // sẽ được set ở bước reindex
                    Col = 0, // sẽ được set ở bước reindex
                    HeightM = dto.HeightM,
                    LengthM = dto.LengthM,
                    WidthM = dto.WidthM,
                    BasePricePerHour = dto.BasePricePerHour,
                    LeaseStart = dto.LeaseStart, 
                    LeaseEnd = dto.LeaseEnd,
                    IsBlocked = dto.IsBlocked,
                    ImageUrl = dto.ImageUrl,
                    Status = StatusValue.Available
                };

                _db.WarehouseSlots.Add(entity);

                if (!existingByWhCode.ContainsKey(whId))
                    existingByWhCode[whId] = new(StringComparer.OrdinalIgnoreCase);
                existingByWhCode[whId].Add(dto.Code);

                result.Success++;
            }

            if (errors.Any())
            {
                await tx.RollbackAsync(ct);
                result.Errors = Collate(errors);
                result.Skipped = result.TotalRows - result.Success;
                result.Success = 0;
                return result;
            }

            await _db.SaveChangesAsync(ct);
            await tx.CommitAsync(ct);
        }
        catch (Exception ex)
        {
            await tx.RollbackAsync(ct);
            errors.Add(new ImportError { RowIndex = 0, Field = "Transaction", Message = $"Lỗi giao dịch: {ex.Message}" });
            result.Errors = Collate(errors);
            result.Skipped = result.TotalRows - result.Success;
            return result;
        }

        var affectedWarehouseIds = whIds;
        foreach (var wid in affectedWarehouseIds.Distinct())
        {
            await ReindexWarehouseSlotsAsync(wid);
        }

        return result;
    }

    // -------- helpers ----------
    private static decimal SafeDecimal(IXLCell cell)
    {
        if (cell.DataType == XLDataType.Number) return (decimal)cell.GetDouble();

        var s = cell.GetString().Trim();
        if (string.IsNullOrEmpty(s)) return 0m;

        // hỗ trợ dấu phẩy/chấm phổ biến
        if (decimal.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out var vInv))
            return vInv;
        if (decimal.TryParse(s, NumberStyles.Any, CultureInfo.GetCultureInfo("vi-VN"), out var vVi))
            return vVi;

        // fallback
        return decimal.Parse(s);
    }

    private static DateTime? TryDate(IXLCell cell)
    {
        if (cell.DataType == XLDataType.DateTime)
            return cell.GetDateTime();

        // ClosedXML có thể lưu ngày ở dạng số (serial)
        if (cell.DataType == XLDataType.Number)
        {
            var d = DateTime.FromOADate(cell.GetDouble());
            return d;
        }

        var s = cell.GetString().Trim();
        if (string.IsNullOrEmpty(s)) return null;

        if (DateTime.TryParse(s, out var d1)) return d1;

        // ISO
        if (DateTime.TryParseExact(s, new[] { "yyyy-MM-dd", "yyyy/MM/dd", "dd/MM/yyyy", "MM/dd/yyyy" },
            CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out var d2)) return d2;

        return null;
    }

    private static bool TryBool(IXLCell cell)
    {
        if (cell.DataType == XLDataType.Boolean) return cell.GetBoolean();
        var s = cell.GetString().Trim().ToUpperInvariant();
        return s == "TRUE" || s == "1" || s == "YES" || s == "Y";
    }

    private static string? NullIfEmpty(string? s)
        => string.IsNullOrWhiteSpace(s) ? null : s!.Trim();

    private static List<ImportError> Collate(List<ImportError> errs)
        => errs.OrderBy(e => e.RowIndex).ThenBy(e => e.Field).ToList();

    public static class WarehouseLayoutOptions
    {
        public const int MaxColsPerRow = 30;
    }

    public async Task ReindexWarehouseSlotsAsync(Guid warehouseId)
    {
        var slots = await _db.Set<WarehouseSlot>()
            .Where(s => s.WarehouseId == warehouseId)
            .OrderBy(s => s.Code) // có thể đổi thành CreatedAt nếu muốn
            .ToListAsync();

        int maxCols = WarehouseLayoutOptions.MaxColsPerRow;
        for (int i = 0; i < slots.Count; i++)
        {
            slots[i].Row = (i / maxCols) + 1;
            slots[i].Col = (i % maxCols) + 1;
        }
        await _db.SaveChangesAsync();
    }
}
