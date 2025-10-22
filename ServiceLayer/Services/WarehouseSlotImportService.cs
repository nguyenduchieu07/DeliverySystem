using ClosedXML.Excel;
using DataAccessLayer;
using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using ServiceLayer.Abstractions.IServices;
using ServiceLayer.Dtos.ExcelImports;
using ServiceLayer.Dtos.Warehouses;
using System;

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
                result.Errors = errors;
                return result;
            }

            var used = ws.RangeUsed();
            if (used == null || used.RowCount() <= 1)
            {
                errors.Add(new ImportError { RowIndex = 1, Field = "Sheet", Message = "Sheet 'Slots' trống hoặc thiếu header." });
                result.Errors = errors;
                return result;
            }

            // map theo cột template
            // A:WarehouseName, B:Code, C:Row, D:Col, E:HeightM, F:LengthM, G:WidthM,
            // H:BasePricePerHour, I:LeaseStart, J:LeaseEnd, K:IsBlocked, L:ImageUrl
            foreach (var r in used.RowsUsed().Skip(1))
            {
                var excelRowIndex = r.RowNumber();
                try
                {
                    var dto = new WarehouseSlotImportRow
                    {
                        WarehouseName = r.Cell(1).GetString().Trim(),
                        Code = r.Cell(2).GetString().Trim(),
                        Row = SafeInt(r.Cell(3)),
                        Col = SafeInt(r.Cell(4)),
                        HeightM = SafeDecimal(r.Cell(5)),
                        LengthM = SafeDecimal(r.Cell(6)),
                        WidthM = SafeDecimal(r.Cell(7)),
                        BasePricePerHour = SafeDecimal(r.Cell(8)),
                        LeaseStart = TryDate(r.Cell(9)),
                        LeaseEnd = TryDate(r.Cell(10)),
                        IsBlocked = TryBool(r.Cell(11)),
                        ImageUrl = NullIfEmpty(r.Cell(12).GetString())
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
            // Required
            if (string.IsNullOrWhiteSpace(dto.WarehouseName))
                errors.Add(new ImportError { RowIndex = excelRow, Field = "WarehouseName", Message = "Bắt buộc." });
            if (string.IsNullOrWhiteSpace(dto.Code))
                errors.Add(new ImportError { RowIndex = excelRow, Field = "Code", Message = "Bắt buộc." });
            if (dto.Row <= 0)
                errors.Add(new ImportError { RowIndex = excelRow, Field = "Row", Message = "Phải là số nguyên dương." });
            if (dto.Col <= 0)
                errors.Add(new ImportError { RowIndex = excelRow, Field = "Col", Message = "Phải là số nguyên dương." });

            // Nếu không bị chặn thì kích thước > 0
            if (!dto.IsBlocked)
            {
                if (dto.HeightM <= 0) errors.Add(new ImportError { RowIndex = excelRow, Field = "HeightM", Message = "Phải > 0 khi IsBlocked = FALSE." });
                if (dto.LengthM <= 0) errors.Add(new ImportError { RowIndex = excelRow, Field = "LengthM", Message = "Phải > 0 khi IsBlocked = FALSE." });
                if (dto.WidthM <= 0) errors.Add(new ImportError { RowIndex = excelRow, Field = "WidthM", Message = "Phải > 0 khi IsBlocked = FALSE." });
            }
            // Giá >= 0
            if (dto.BasePricePerHour < 0)
                errors.Add(new ImportError { RowIndex = excelRow, Field = "BasePricePerHour", Message = "Không được âm." });

            // LeaseStart <= LeaseEnd (nếu có cả hai)
            if (dto.LeaseStart.HasValue && dto.LeaseEnd.HasValue && dto.LeaseStart > dto.LeaseEnd)
                errors.Add(new ImportError { RowIndex = excelRow, Field = "LeaseStart/LeaseEnd", Message = "LeaseStart phải ≤ LeaseEnd." });
        }

        // 3) Validate mức 2: trùng trong chính file (Code & (Row,Col) theo từng WarehouseName)
        var groupByWh = rows.GroupBy(x => x.dto.WarehouseName, StringComparer.OrdinalIgnoreCase);
        foreach (var g in groupByWh)
        {
            // Dup Code
            var dupCode = g.GroupBy(x => x.dto.Code, StringComparer.OrdinalIgnoreCase)
                           .Where(gr => gr.Count() > 1);
            foreach (var d in dupCode)
                foreach (var item in d)
                    errors.Add(new ImportError { RowIndex = item.excelRow, Field = "Code", Message = $"Trùng Code trong file: '{item.dto.Code}'." });

            // Dup (Row,Col)
            var dupPos = g.GroupBy(x => (x.dto.Row, x.dto.Col))
                          .Where(gr => gr.Count() > 1);
            foreach (var d in dupPos)
                foreach (var item in d)
                    errors.Add(new ImportError { RowIndex = item.excelRow, Field = "Row/Col", Message = $"Trùng vị trí (Row,Col) trong file: ({item.dto.Row},{item.dto.Col})." });
        }

        // 4) Validate mức 3: map WarehouseName -> WarehouseId, và check trùng với DB
        var distinctWhNames = rows.Select(x => x.dto.WarehouseName.Trim())
                                  .Where(x => !string.IsNullOrEmpty(x))
                                  .Distinct(StringComparer.OrdinalIgnoreCase)
                                  .ToList();

        var whNameToId = await _db.Warehouses
            .Where(w => distinctWhNames.Contains(w.Name))
            .Select(w => new { w.Name, w.Id })
            .ToListAsync(ct);

        var map = whNameToId
            .GroupBy(x => x.Name, StringComparer.OrdinalIgnoreCase)
            .ToDictionary(g => g.Key, g => g.First().Id, StringComparer.OrdinalIgnoreCase);

        foreach (var (excelRow, dto) in rows)
        {
            if (!map.TryGetValue(dto.WarehouseName, out _))
                errors.Add(new ImportError { RowIndex = excelRow, Field = "WarehouseName", Message = $"Không tồn tại trong DB: '{dto.WarehouseName}'." });
        }

        // Nếu đã có lỗi validate, trả về sớm (không insert)
        if (errors.Any())
        {
            result.Errors = Collate(errors);
            result.Skipped = result.TotalRows - result.Success;
            return result;
        }

        // 5) Tải dữ liệu slot đã có trong DB để check trùng (Code) và (Row,Col) theo từng warehouse
        var whIds = map.Values.Distinct().ToList();
        var existing = await _db.WarehouseSlots
            .Where(s => whIds.Contains(s.WarehouseId))
            .Select(s => new { s.WarehouseId, s.Code, s.Row, s.Col })
            .ToListAsync(ct);

        var existingByWhCode = existing
            .GroupBy(x => x.WarehouseId)
            .ToDictionary(
                g => g.Key,
                g => new HashSet<string>(g.Select(x => x.Code), StringComparer.OrdinalIgnoreCase));

        var existingByWhPos = existing
            .GroupBy(x => x.WarehouseId)
            .ToDictionary(
                g => g.Key,
                g => new HashSet<(int, int)>(g.Select(x => (x.Row, x.Col))));

        // 6) Insert trong transaction
        using var tx = await _db.Database.BeginTransactionAsync(ct);
        try
        {
            foreach (var (excelRow, dto) in rows)
            {
                var whId = map[dto.WarehouseName];

                // Check trùng với DB
                if (existingByWhCode.TryGetValue(whId, out var codes) && codes.Contains(dto.Code))
                {
                    errors.Add(new ImportError { RowIndex = excelRow, Field = "Code", Message = $"Đã tồn tại trong DB: '{dto.Code}'." });
                    continue;
                }
                if (existingByWhPos.TryGetValue(whId, out var positions) && positions.Contains((dto.Row, dto.Col)))
                {
                    errors.Add(new ImportError { RowIndex = excelRow, Field = "Row/Col", Message = $"Vị trí đã tồn tại trong DB: ({dto.Row},{dto.Col})." });
                    continue;
                }

                var entity = new WarehouseSlot
                {
                    Id = Guid.NewGuid(),
                    WarehouseId = whId,
                    Code = dto.Code,
                    Row = dto.Row,
                    Col = dto.Col,
                    HeightM = dto.HeightM,
                    LengthM = dto.LengthM,
                    WidthM = dto.WidthM,
                    BasePricePerHour = dto.BasePricePerHour,
                    LeaseStart = dto.LeaseStart,
                    LeaseEnd = dto.LeaseEnd,
                    IsBlocked = dto.IsBlocked,
                    ImageUrl = dto.ImageUrl,
                    Status = DataAccessLayer.Enums.StatusValue.Active // tuỳ enum của bạn
                };

                _db.WarehouseSlots.Add(entity);

                // update cache để tránh trùng tiếp trong cùng file
                if (!existingByWhCode.ContainsKey(whId)) existingByWhCode[whId] = new(StringComparer.OrdinalIgnoreCase);
                if (!existingByWhPos.ContainsKey(whId)) existingByWhPos[whId] = new();

                existingByWhCode[whId].Add(dto.Code);
                existingByWhPos[whId].Add((dto.Row, dto.Col));

                result.Success++;
            }

            if (errors.Any())
            {
                // có lỗi ở một số dòng: rollback để không insert nửa vời
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
        }

        result.Errors = Collate(errors);
        result.Skipped = result.TotalRows - result.Success;
        return result;
    }

    // -------- helpers ----------
    private static int SafeInt(IXLCell cell)
    {
        if (cell.DataType == XLDataType.Number) return (int)cell.GetDouble();
        var s = cell.GetString().Trim();
        return string.IsNullOrEmpty(s) ? 0 : int.Parse(s);
    }

    private static decimal SafeDecimal(IXLCell cell)
    {
        if (cell.DataType == XLDataType.Number) return (decimal)cell.GetDouble();
        var s = cell.GetString().Trim();
        return string.IsNullOrEmpty(s) ? 0m : decimal.Parse(s);
    }

    private static DateTime? TryDate(IXLCell cell)
    {
        // Hỗ trợ cả kiểu Date trong Excel và text yyyy-mm-dd
        if (cell.DataType == XLDataType.DateTime)
            return cell.GetDateTime();

        var s = cell.GetString().Trim();
        if (string.IsNullOrEmpty(s)) return null;
        if (DateTime.TryParse(s, out var d)) return d;
        return null;
    }

    private static bool TryBool(IXLCell cell)
    {
        if (cell.DataType == XLDataType.Boolean) return cell.GetBoolean();
        var s = cell.GetString().Trim().ToUpperInvariant();
        return s == "TRUE" || s == "1" || s == "YES";
    }

    private static string? NullIfEmpty(string? s)
        => string.IsNullOrWhiteSpace(s) ? null : s!.Trim();

    private static List<ImportError> Collate(List<ImportError> errs)
    {
        // Gom lỗi trùng lặp cùng dòng/cột cho gọn (giữ nguyên chi tiết nếu bạn muốn)
        return errs.OrderBy(e => e.RowIndex).ThenBy(e => e.Field).ToList();
    }
}
