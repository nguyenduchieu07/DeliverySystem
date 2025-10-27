using DataAccessLayer.Entities;
using DataAccessLayer.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace PresentationLayer.Controllers
{
    [Route("api/warehouses")]
    public class WarehouseController : Controller
    {
        private readonly DeliverySytemContext _db;
        public WarehouseController(DeliverySytemContext db) => _db = db;

        // ====== 1) Lấy Slots theo Warehouse ======
        [HttpGet("{id:guid}/slots")]
        public async Task<IActionResult> GetSlots(Guid id, CancellationToken ct)
        {
            var slots = await _db.WarehouseSlots
                .AsNoTracking()
                .Where(s => s.WarehouseId == id)
                .Select(s => new
                {
                    id = s.Id,
                    code = s.Code,
                    row = s.Row,
                    col = s.Col,
                    size = s.Size,
                    volumeM3 = s.VolumeM3,
                    basePricePerHour = s.BasePricePerHour,
                    status = s.IsBlocked ? "blocked"
                            : (s.CurrentOrderId != null ? "occupied" : "available"),
                    
                    // status = s.IsBlocked ? "blocked"
                    //         : (s.CurrentOrderId != null ? "occupied"
                    //         : (s.TempReservationExpiresAt != null && s.TempReservationExpiresAt > DateTime.UtcNow ? "reserved" : "available")),
                    imageUrl = s.ImageUrl
                })
                .ToListAsync(ct);

            return Ok(slots);
        }

        // ====== 2) Search ngắn gọn (limit) — đồng nhất field cho FE ======
        [HttpGet]
        public async Task<IActionResult> Search([FromQuery] string? q, [FromQuery] int limit = 20, CancellationToken ct = default)
        {
            q = (q ?? "").Trim().ToLower();

            var query = _db.Warehouses
                .AsNoTracking()
                .Include(w => w.Address)
                .Include(w => w.Store)
                .Include(w => w.Slots)
                .Where(w => w.Status == StatusValue.Approved &&
                            w.Address != null &&
                            w.Address.Latitude.HasValue &&
                            w.Address.Longitude.HasValue);

            if (!string.IsNullOrEmpty(q))
            {
                query = query.Where(w =>
                    w.Name.ToLower().Contains(q) ||
                    (w.Address!.City ?? "").ToLower().Contains(q) ||
                    (w.Address!.District ?? "").ToLower().Contains(q) ||
                    (w.Store.StoreName ?? "").ToLower().Contains(q)
                );
            }

            var items = await query
                .OrderBy(w => w.Name)
                .Take(limit)
                .Select(w => new
                {
                    id = w.Id,
                    name = w.Name,
                    storeId = w.StoreId,
                    storeName = w.Store != null ? w.Store.StoreName : null,
                    city = w.Address!.City,
                    district = w.Address!.District,
                    address = w.Address!.AddressLine,
                    coverImageUrl = w.CoverImageUrl,
                    volumeM3 = w.VolumeM3,
                    totalSlots = w.Slots.Count(s => !s.IsBlocked),
                    availableSlots = w.Slots.Count(s => !s.IsBlocked && s.CurrentOrderId == null),
                    minPricePerHour = w.Slots.Where(s => !s.IsBlocked)
                                             .Select(s => (decimal?)s.BasePricePerHour)
                                             .Min() ?? 0m,
                    lat = w.Address!.Latitude!.Value,
                    lng = w.Address!.Longitude!.Value
                })
                .ToListAsync(ct);

            return Ok(items);
        }

        // ====== 3) Nearby theo bán kính ======
        [HttpGet("nearby")]
        public async Task<IActionResult> Nearby(
            [FromQuery] double lat,
            [FromQuery] double lng,
            [FromQuery] double radiusKm = 20,
            [FromQuery] int limit = 50,
            CancellationToken ct = default)
        {
            if (lat is < -90 or > 90 || lng is < -180 or > 180)
                return BadRequest("Invalid coordinates.");
            radiusKm = Math.Clamp(radiusKm, 1, 100);
            limit = Math.Clamp(limit, 1, 200);

            // Bbox filter để giảm tải DB
            var cosLat = Math.Cos(lat * Math.PI / 180.0);
            cosLat = Math.Max(cosLat, 0.01); // tránh chia 0 ở cực
            var latDelta = radiusKm / 111.0;
            var lngDelta = radiusKm / (111.0 * cosLat);
            var minLat = lat - latDelta;
            var maxLat = lat + latDelta;
            var minLng = lng - lngDelta;
            var maxLng = lng + lngDelta;

            var baseQuery = _db.Warehouses
                .AsNoTracking()
                .Include(w => w.Address)
                .Include(w => w.Slots)
                .Include(w => w.Store)
                .Where(w =>
                    w.Status == StatusValue.Approved &&
                    w.Address != null &&
                    w.Address.Latitude.HasValue &&
                    w.Address.Longitude.HasValue &&
                    w.Address.Latitude >= minLat && w.Address.Latitude <= maxLat &&
                    w.Address.Longitude >= minLng && w.Address.Longitude <= maxLng);

            var raw = await baseQuery
                .Select(w => new
                {
                    w.Id,
                    w.Name,
                    w.StoreId,
                    StoreName = w.Store != null ? w.Store.StoreName : null,
                    AddressLine = w.Address!.AddressLine,
                    City = w.Address!.City,
                    District = w.Address!.District,
                    Lat = w.Address!.Latitude!.Value,
                    Lng = w.Address!.Longitude!.Value,
                    TotalSlots = w.Slots.Count(s => !s.IsBlocked),
                    AvailableSlots = w.Slots.Count(s => !s.IsBlocked && s.CurrentOrderId == null),
                    MinPricePerHour = w.Slots.Where(s => !s.IsBlocked)
                                             .Select(s => (decimal?)s.BasePricePerHour)
                                             .Min() ?? 0m
                })
                .ToListAsync(ct);

            static double HaversineKm(double lat1, double lon1, double lat2, double lon2)
            {
                const double R = 6371.0;
                double dLat = (lat2 - lat1) * Math.PI / 180.0;
                double dLon = (lon2 - lon1) * Math.PI / 180.0;
                double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                           Math.Cos(lat1 * Math.PI / 180.0) * Math.Cos(lat2 * Math.PI / 180.0) *
                           Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
                double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
                return R * c;
            }

            var items = raw
                .Select(w => new
                {
                    id = w.Id,
                    name = w.Name,
                    storeId = w.StoreId,
                    storeName = w.StoreName,
                    address = w.AddressLine,
                    city = w.City,
                    district = w.District,
                    lat = w.Lat,
                    lng = w.Lng,
                    totalSlots = w.TotalSlots,
                    availableSlots = w.AvailableSlots,
                    minPricePerHour = w.MinPricePerHour,
                    distanceKm = HaversineKm(lat, lng, w.Lat, w.Lng)
                })
                .Where(x => x.distanceKm <= radiusKm)
                .OrderBy(x => x.distanceKm)
                .ThenBy(x => x.name)
                .Take(limit)
                .ToList();

            return Ok(items);
        }

        // ====== 4) Search phân trang (/api/warehouses/search) ======
        [HttpGet("search")]
        public async Task<IActionResult> All(
            [FromQuery] double? lat,
            [FromQuery] double? lng,
            [FromQuery] string? q,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 100,
            CancellationToken ct = default)
        {
            page = Math.Max(1, page);
            pageSize = Math.Clamp(pageSize, 1, 200);

            if (lat.HasValue && (lat.Value < -90 || lat.Value > 90))
                return BadRequest("Invalid latitude.");
            if (lng.HasValue && (lng.Value < -180 || lng.Value > 180))
                return BadRequest("Invalid longitude.");

            var baseQuery = _db.Warehouses
                .AsNoTracking()
                .Include(w => w.Address)
                .Include(w => w.Slots)
                .Include(w => w.Store)
                .Where(w =>
                    w.Status == StatusValue.Approved &&
                    w.Address != null &&
                    w.Address.Latitude.HasValue &&
                    w.Address.Longitude.HasValue);

            if (!string.IsNullOrWhiteSpace(q))
            {
                var ql = q.Trim().ToLower();
                baseQuery = baseQuery.Where(w =>
                    w.Name.ToLower().Contains(ql) ||
                    (w.Address!.City ?? "").ToLower().Contains(ql) ||
                    (w.Address!.District ?? "").ToLower().Contains(ql) ||
                    (w.Store.StoreName ?? "").ToLower().Contains(ql)
                );
            }

            var totalCount = await baseQuery.CountAsync(ct);

            var raw = await baseQuery
                .Select(w => new
                {
                    w.Id,
                    w.Name,
                    w.StoreId,
                    StoreName = w.Store != null ? w.Store.StoreName : null,
                    AddressLine = w.Address!.AddressLine,
                    City = w.Address!.City,
                    District = w.Address!.District,
                    Lat = w.Address!.Latitude!.Value,
                    Lng = w.Address!.Longitude!.Value,
                    TotalSlots = w.Slots.Count(s => !s.IsBlocked),
                    AvailableSlots = w.Slots.Count(s => !s.IsBlocked && s.CurrentOrderId == null),
                    MinPricePerHour = w.Slots.Where(s => !s.IsBlocked)
                                             .Select(s => (decimal?)s.BasePricePerHour)
                                             .Min() ?? 0m
                })
                .ToListAsync(ct);

            static double? HaversineKm(double? la1, double? lo1, double la2, double lo2)
            {
                if (!la1.HasValue || !lo1.HasValue) return null;
                const double R = 6371.0;
                double dLat = (la2 - la1.Value) * Math.PI / 180.0;
                double dLon = (lo2 - lo1.Value) * Math.PI / 180.0;
                double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                           Math.Cos(la1.Value * Math.PI / 180.0) * Math.Cos(la2 * Math.PI / 180.0) *
                           Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
                double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
                return R * c;
            }

            var projected = raw.Select(w => new
            {
                id = w.Id,
                name = w.Name,
                storeId = w.StoreId,
                storeName = w.StoreName,
                address = w.AddressLine,
                city = w.City,
                district = w.District,
                lat = w.Lat,
                lng = w.Lng,
                totalSlots = w.TotalSlots,
                availableSlots = w.AvailableSlots,
                minPricePerHour = w.MinPricePerHour,
                distanceKm = HaversineKm(lat, lng, w.Lat, w.Lng)
            });

            IOrderedEnumerable<dynamic> ordered;
            if (lat.HasValue && lng.HasValue)
                ordered = projected.OrderBy(x => x.distanceKm ?? double.MaxValue).ThenBy(x => x.name);
            else
                ordered = projected.OrderBy(x => x.name);

            var items = ordered
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return Ok(new
            {
                page,
                pageSize,
                total = totalCount,
                items
            });
        }
    }
}
