
using ClosedXML.Excel;
using DataAccessLayer;
using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using ServiceLayer.Abstractions.IServices;
using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DataAccessLayer.Enums;

public class WarehouseSlotExportService : IWarehouseSlotExportService
{
    private readonly DeliverySytemContext _db;
    public WarehouseSlotExportService(DeliverySytemContext db) => _db = db;

    // Cấu trúc cột theo thiết kế import MỚI (Có Row/Col)
    // A:WarehouseName, B:Code, C:HeightM, D:LengthM, E:WidthM,
    // F:BasePricePerHour, G:Row, H:Col, I:IsBlocked, J:ImageUrl
    private static readonly string[] HeaderTemplate = new[]
    {
        "WarehouseName","Code","HeightM","LengthM","WidthM",
        "BasePricePerHour","Row","Col","IsBlocked","ImageUrl", "LeaseStart","LeaseEnd","Status"
    };

    private static readonly string[] HeaderExport = new[]
    {
        "WarehouseName","Code","HeightM","LengthM","WidthM",
        "BasePricePerHour","Row","Col", "LeaseStart", "LeaseEnd","IsBlocked","ImageUrl"
    };

    public async Task<byte[]> ExportTemplateAsync(string warehouseName, CancellationToken ct = default)
    {
        using var wb = new XLWorkbook();
        var wsSlots = wb.Worksheets.Add("Slots");

        // Header
        for (int i = 0; i < HeaderTemplate.Length; i++)
            wsSlots.Cell(1, i + 1).Value = HeaderTemplate[i];

        StyleHeader(wsSlots.Range(1, 1, 1, HeaderTemplate.Length));

        // Ghi dòng mô tả (optional)
        wsSlots.Cell(2, 1).Value = $"{warehouseName}";
        wsSlots.Cell(2, 3).Value = "(m)";
        wsSlots.Cell(2, 4).Value = "(m)";
        wsSlots.Cell(2, 5).Value = "(m)";
        wsSlots.Cell(2, 6).Value = "(VND/giờ)";
        wsSlots.Cell(2, 7).Value = "(số hàng)";
        wsSlots.Cell(2, 8).Value = "(số cột)";
        wsSlots.Cell(2, 9).Value = "(TRUE/FALSE)";
        wsSlots.Cell(2, 10).Value = "";
        wsSlots.Cell(2, 11).Value = "(yyyy-MM-dd)";
        wsSlots.Cell(2, 12).Value = "(yyyy-MM-dd)";
        wsSlots.Cell(2, 13).Value = "(Available/InUse/Reserved/Maintenance)";

        #region Xử lý data cho dropdown Status

        var statusCol = 13;
        var lastRow = 100; // ví dụ template hỗ trợ 10 slot
        var statusValues = new[]
        {
            nameof(StatusValue.Available), 
            nameof(StatusValue.InUse), 
            nameof(StatusValue.Reserved), 
            nameof(StatusValue.Maintenance), 
        };
        //Add sheet khác chứa data
        var wsData = wb.Worksheets.Add("Data");
        for (int i = 0; i < statusValues.Length; i++)
        {
            wsData.Cell(i + 1, 1).Value = statusValues[i]; // cột A, dòng 1->n
        }

        wsData.Hide();
        #endregion

        wsSlots.Range(3, statusCol, lastRow, statusCol)
            .SetDataValidation()
            .List(wsData.Range($"A1:A{statusValues.Length}"), true);
        
        AutoFit(wsSlots, HeaderTemplate.Length);
        wsSlots.SheetView.FreezeRows(1);
        wsSlots.RangeUsed().SetAutoFilter();

        using var stream = new System.IO.MemoryStream();
        wb.SaveAs(stream);
        return stream.ToArray();
    }

    public async Task<byte[]> ExportWarehouseSlotsAsync(Guid warehouseId, CancellationToken ct = default)
    {
        var wh = await _db.Warehouses
            .AsNoTracking()
            .Where(w => w.Id == warehouseId)
            .Select(w => new { w.Id, w.Name })
            .FirstOrDefaultAsync(ct);

        if (wh == null)
            throw new InvalidOperationException("Warehouse không tồn tại.");

        var slots = await _db.WarehouseSlots
            .AsNoTracking()
            .Where(s => s.WarehouseId == warehouseId)
            .OrderBy(s => s.Code) // thứ tự xuất: theo Code
            .Select(s => new
            {
                s.Code,
                s.HeightM,
                s.LengthM,
                s.WidthM,
                s.BasePricePerHour,
                s.Row,
                s.Col,
                s.LeaseStart,
                s.LeaseEnd,
                s.IsBlocked,
                s.ImageUrl
            })
            .ToListAsync(ct);

        using var wb = new XLWorkbook();
        var ws = wb.Worksheets.Add("Slots");

        // Header
        for (int i = 0; i < HeaderExport.Length; i++)
            ws.Cell(1, i + 1).Value = HeaderExport[i];
        StyleHeader(ws.Range(1, 1, 1, HeaderExport.Length));

        // Data
        var r = 2;
        foreach (var s in slots)
        {
            ws.Cell(r, 1).Value = wh.Name;                 // WarehouseName
            ws.Cell(r, 2).Value = s.Code;                  // Code
            ws.Cell(r, 3).Value = s.HeightM;         // HeightM
            ws.Cell(r, 4).Value = s.LengthM;         // LengthM
            ws.Cell(r, 5).Value = s.WidthM;         // WidthM
            ws.Cell(r, 6).Value = s.BasePricePerHour; // BasePricePerHour
            ws.Cell(r, 7).Value = s.Col;         // Col
            ws.Cell(r, 8).Value = s.Row; // Row

            if (s.LeaseStart.HasValue) ws.Cell(r, 9).Value = s.LeaseStart.Value;
            if (s.LeaseEnd.HasValue) ws.Cell(r, 10).Value = s.LeaseEnd.Value;

            ws.Cell(r,11).Value = s.IsBlocked;             // IsBlocked
            if (!string.IsNullOrWhiteSpace(s.ImageUrl))
                ws.Cell(r, 12).Value = s.ImageUrl;          // ImageUrl

            // Định dạng
            ws.Cell(r, 3).Style.NumberFormat.Format = "0.00";
            ws.Cell(r, 4).Style.NumberFormat.Format = "0.00";
            ws.Cell(r, 5).Style.NumberFormat.Format = "0.00";
            ws.Cell(r, 6).Style.NumberFormat.Format = "#,##0"; // VND
            ws.Cell(r, 7).Style.NumberFormat.Format = "0"; // Col
            ws.Cell(r, 8).Style.NumberFormat.Format = "0"; // Row
            ws.Cell(r, 9).Style.DateFormat.Format = "yyyy-MM-dd";
            ws.Cell(r, 10).Style.DateFormat.Format = "yyyy-MM-dd";

            r++;
        }

        // Nếu muốn thêm cột Volume (m3) (chỉ xem, không nằm trong format import):
        // ws.Cell(1, 11).Value = "VolumeM3";
        // StyleHeader(ws.Range(1, 11, 1, 11));
        // for (int i = 2; i < r; i++) ws.Cell(i, 11).FormulaA1 = $"C{i}*D{i}*E{i}";

        AutoFit(ws, HeaderExport.Length);
        ws.SheetView.FreezeRows(1);
        if (ws.RangeUsed() != null) ws.RangeUsed().SetAutoFilter();

        using var stream = new System.IO.MemoryStream();
        wb.SaveAs(stream);
        return stream.ToArray();
    }

    public async Task<byte[]> ExportMultiWarehousesAsync(Guid[] warehouseIds, CancellationToken ct = default)
    {
        using var wb = new XLWorkbook();

        var whList = await _db.Warehouses
            .AsNoTracking()
            .Where(w => warehouseIds.Contains(w.Id))
            .Select(w => new { w.Id, w.Name })
            .ToListAsync(ct);

        foreach (var wh in whList)
        {
            var ws = wb.Worksheets.Add(SafeSheetName(wh.Name));

            // Header
            for (int i = 0; i < HeaderExport.Length; i++)
                ws.Cell(1, i + 1).Value = HeaderExport[i];
            StyleHeader(ws.Range(1, 1, 1, HeaderExport.Length));

            var slots = await _db.WarehouseSlots
                .AsNoTracking()
                .Where(s => s.WarehouseId == wh.Id)
                .OrderBy(s => s.Code)
                .Select(s => new
                {
                    s.Code,
                    s.HeightM,
                    s.LengthM,
                    s.WidthM,
                    s.BasePricePerHour,
                    s.Row,
                    s.Col,
                    s.LeaseStart,
                    s.LeaseEnd,
                    s.IsBlocked,
                    s.ImageUrl
                }).ToListAsync(ct);

            var r = 2;
            foreach (var s in slots)
            {
                ws.Cell(r, 1).Value = wh.Name;
                ws.Cell(r, 2).Value = s.Code;
                ws.Cell(r, 3).Value = s.HeightM;
                ws.Cell(r, 4).Value = s.LengthM;
                ws.Cell(r, 5).Value = s.WidthM;
                ws.Cell(r, 6).Value = s.BasePricePerHour;
                ws.Cell(r, 7).Value = s.WidthM;
                ws.Cell(r, 8).Value = s.BasePricePerHour;
                if (s.LeaseStart.HasValue) ws.Cell(r, 9).Value = s.LeaseStart.Value;
                if (s.LeaseEnd.HasValue) ws.Cell(r, 10).Value = s.LeaseEnd.Value;
                ws.Cell(r, 11).Value = s.IsBlocked;
                if (!string.IsNullOrWhiteSpace(s.ImageUrl)) ws.Cell(r, 12).Value = s.ImageUrl;

                ws.Cell(r, 3).Style.NumberFormat.Format = "0.00";
                ws.Cell(r, 4).Style.NumberFormat.Format = "0.00";
                ws.Cell(r, 5).Style.NumberFormat.Format = "0.00";
                ws.Cell(r, 6).Style.NumberFormat.Format = "#,##0";
                ws.Cell(r, 9).Style.DateFormat.Format = "yyyy-MM-dd";
                ws.Cell(r, 10).Style.DateFormat.Format = "yyyy-MM-dd";

                r++;
            }

            AutoFit(ws, HeaderExport.Length);
            ws.SheetView.FreezeRows(1);
            if (ws.RangeUsed() != null) ws.RangeUsed().SetAutoFilter();
        }

        using var stream = new System.IO.MemoryStream();
        wb.SaveAs(stream);
        return stream.ToArray();
    }

    // ---------- helpers ----------
    private static void StyleHeader(IXLRange rng)
    {
        rng.Style.Font.Bold = true;
        rng.Style.Fill.BackgroundColor = XLColor.FromArgb(0xF3, 0xF6, 0xFC);
        rng.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
    }

    private static void AutoFit(IXLWorksheet ws, int cols)
    {
        for (int c = 1; c <= cols; c++)
        {
            var col = ws.Column(c);
            col.AdjustToContents();           
            if (col.Width < 12)
                col.Width = 12;
        }
    }

    private static string SafeSheetName(string name)
    {
        var s = string.Concat(name.Where(ch => !"[]:*?/\\".Contains(ch)));
        if (string.IsNullOrWhiteSpace(s)) s = "Sheet";
        return s.Length <= 31 ? s : s.Substring(0, 31);
    }
}
