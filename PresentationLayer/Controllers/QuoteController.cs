using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Abstractions.IServices;
using ServiceLayer.Dtos.Quotes;
using PresentationLayer.Models;
using Microsoft.EntityFrameworkCore;
using DataAccessLayer.Enums;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace PresentationLayer.Controllers
{
    public class QuoteController : Controller
    {
        private readonly IQuotationService _svc;
        private readonly IFeedbackService _feedbackService;
        private readonly DeliverySytemContext _db;
        private readonly ICustomerService _customerService;
        private readonly IDeliveryService _deliveryService;
        private readonly IOrderService _orderService;
        private readonly ICloudinaryService _cloudinaryService;
        private readonly IGeminiService _geminiService;

        public QuoteController(
            IQuotationService svc, 
            IFeedbackService feedbackService,
            DeliverySytemContext db,
            ICustomerService customerService,
            IDeliveryService deliveryService,
            IOrderService orderService,
            ICloudinaryService cloudinaryService,
            IGeminiService geminiService)
        {
            _svc = svc;
            _feedbackService = feedbackService;
            _db = db;
            _customerService = customerService;
            _deliveryService = deliveryService;
            _orderService = orderService;
            _cloudinaryService = cloudinaryService;
            _geminiService = geminiService;
        }

        // Trang form đặt lưu kho (gộp booking + quote)
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var addressOptions = await _db.Addresses
                .OrderByDescending(a => a.IsDefault)
                .Select(a => new AddressOptionVM
                {
                    Id = a.Id,
                    Label = string.IsNullOrWhiteSpace(a.Label) ? a.AddressLine : a.Label,
                    Full = a.AddressLine + ", " + (a.Ward ?? "") + ", " + (a.District ?? "") + ", " + (a.City ?? ""),
                    Latitude = a.Latitude,
                    Longitude = a.Longitude
                })
                .ToListAsync();

            var defaultAddress = await _db.Addresses
                .OrderByDescending(a => a.IsDefault)
                .FirstOrDefaultAsync();

            var vm = new BookingRequestVM
            {
                StorageStartDate = DateTime.UtcNow.Date.AddDays(1),
                StorageEndDate = DateTime.UtcNow.Date.AddDays(30),
                AddressOptions = addressOptions,
                DropoffAddressId = defaultAddress?.Id,
                DropoffLatitude = defaultAddress?.Latitude,
                DropoffLongitude = defaultAddress?.Longitude,
                DropoffAddressText = defaultAddress == null ? null : ($"{defaultAddress.AddressLine}, {defaultAddress.Ward}, {defaultAddress.District}, {defaultAddress.City}")?.Replace("  ", " ")
            };

            // Không prefill items - để user tự nhập hoặc dùng nút "Load dữ liệu mẫu"
            vm.Items = new List<BookingItemVM>();

            // Prefill customer info from profile if available
            var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!string.IsNullOrEmpty(userIdClaim) && Guid.TryParse(userIdClaim, out Guid userId))
            {
                var customer = await _customerService.GetProfileAsync(userId);
                if (customer != null)
                {
                    vm.CustomerFullName = customer.FullName;
                    vm.CustomerEmail = customer.Email;
                    vm.CustomerPhone = customer.PhoneNumber;
                }
            }

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> CreateWarehouseOrder([FromForm] CreateWarehouseOrderViewModel viewModel, IFormFile? productImage)
        {
            if (viewModel == null)
            {
                return BadRequest(new { success = false, message = "Dữ liệu không hợp lệ." });
            }

            if (viewModel.PickupAddress == null || viewModel.WarehouseArea == null)
            {
                return BadRequest(new { success = false, message = "Địa chỉ nhận hàng và khu vực tìm kho là bắt buộc." });
            }

            if (string.IsNullOrWhiteSpace(viewModel.PickupAddress.AddressLine))
            {
                return BadRequest(new { success = false, message = "Địa chỉ nhận hàng không được để trống. Vui lòng nhập địa chỉ hoặc chọn vị trí hiện tại." });
            }

            if (string.IsNullOrWhiteSpace(viewModel.WarehouseArea.AddressLine))
            {
                return BadRequest(new { success = false, message = "Khu vực tìm kho không được để trống. Vui lòng chọn kho từ danh sách." });
            }

            if (viewModel.StorageEndDate <= viewModel.StorageStartDate)
            {
                return BadRequest(new { success = false, message = "Ngày xuất kho phải sau ngày nhập kho." });
            }

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out Guid userId))
            {
                return Unauthorized(new { success = false, message = "Bạn cần đăng nhập để đặt hàng." });
            }

            try
            {
                Console.WriteLine("=== Starting CreateWarehouseOrder ===");
                Console.WriteLine($"Received WarehouseId: {viewModel.WarehouseId}");
                Console.WriteLine($"PickupAddress: {viewModel.PickupAddress?.AddressLine}");
                Console.WriteLine($"WarehouseArea: {viewModel.WarehouseArea?.AddressLine}");
                
                // Upload ảnh nếu có
                string? imageUrl = null;
                VolumeCalculationResult? volumeResult = null;
                if (productImage != null && productImage.Length > 0)
                {
                    Console.WriteLine("Uploading product image...");
                    imageUrl = await _cloudinaryService.UploadImageFileAsync(productImage);
                    Console.WriteLine($"Image uploaded: {imageUrl}");
                    
                    // Gọi Gemini để phân tích ảnh và tính thể tích
                    try
                    {
                        Console.WriteLine("Calling Gemini API...");
                        var items = viewModel.Items?.Select(i => new ItemInfo
                        {
                            Name = i.Name,
                            Category = i.Category,
                            Quantity = i.Quantity,
                            EstimatedWeightKg = i.EstimatedWeightKg
                        }).ToList() ?? new List<ItemInfo>();
                        
                        volumeResult = await _geminiService.AnalyzeImageAndCalculateVolumeAsync(imageUrl, items);
                        Console.WriteLine($"Gemini result: Volume={volumeResult.RequiredVolumeM3} m³, Area={volumeResult.RequiredAreaM2} m²");
                    }
                    catch (Exception geminiEx)
                    {
                        Console.WriteLine("Gemini analysis error: " + geminiEx.Message);
                        Console.WriteLine("Gemini stack trace: " + geminiEx.StackTrace);
                        // Continue without Gemini result - sẽ dùng fallback
                    }
                }
                else
                {
                    Console.WriteLine("No product image provided, skipping Gemini analysis");
                }

                // Tìm warehouse từ WarehouseId (ưu tiên) hoặc WarehouseArea
                Guid warehouseId;
                Guid storeId;
                Warehouse? warehouseFromDb = null;
                
                // Ưu tiên: Tìm warehouse bằng ID nếu có
                if (viewModel.WarehouseId.HasValue)
                {
                    Console.WriteLine($"Looking for warehouse by ID: {viewModel.WarehouseId.Value}");
                    warehouseFromDb = await _db.Warehouses
                        .Include(w => w.Address)
                        .Where(w => w.Id == viewModel.WarehouseId.Value)
                        .FirstOrDefaultAsync();
                    
                    if (warehouseFromDb == null)
                    {
                        Console.WriteLine($"Warehouse with ID {viewModel.WarehouseId.Value} not found in database");
                        return BadRequest(new { success = false, message = "Kho hàng đã chọn không tồn tại." });
                    }
                    Console.WriteLine($"Found warehouse by ID: {warehouseFromDb.Name} (Status: {warehouseFromDb.Status})");
                }
                else
                {
                    Console.WriteLine("No WarehouseId provided, trying to find by address or coordinates");
                }
                
                // Fallback: Tìm warehouse bằng địa chỉ nếu không có ID
                if (warehouseFromDb == null)
                {
                    var warehouseAddress = viewModel.WarehouseArea.AddressLine;
                    Console.WriteLine($"Trying to find warehouse by address: {warehouseAddress}");
                    warehouseFromDb = await _db.Warehouses
                        .Include(w => w.Address)
                        .Where(w => w.Address != null && 
                                    (w.Address.AddressLine.Contains(warehouseAddress) || 
                                     w.Name.Contains(warehouseAddress)))
                        .FirstOrDefaultAsync();
                    
                    if (warehouseFromDb != null)
                    {
                        Console.WriteLine($"Found warehouse by address: {warehouseFromDb.Name}");
                    }
                }
                
                // Fallback: Tìm kho gần nhất với tọa độ
                if (warehouseFromDb == null)
                {
                    Console.WriteLine($"Trying to find nearest store by coordinates: {viewModel.WarehouseArea.Latitude}, {viewModel.WarehouseArea.Longitude}");
                    try
                    {
                        storeId = await _deliveryService.FindNearestStoreAsync(
                            viewModel.WarehouseArea.Latitude ?? 0,
                            viewModel.WarehouseArea.Longitude ?? 0
                        );
                        
                        Console.WriteLine($"Found nearest store ID: {storeId}");
                        
                        warehouseFromDb = await _db.Warehouses
                            .Where(w => w.StoreId == storeId && w.Status == StatusValue.Approved)
                            .FirstOrDefaultAsync();
                        
                        if (warehouseFromDb != null)
                        {
                            Console.WriteLine($"Found warehouse by nearest store: {warehouseFromDb.Name}");
                        }
                        else
                        {
                            Console.WriteLine($"No approved warehouse found for store ID: {storeId}");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error finding nearest store: {ex.Message}");
                    }
                }
                
                if (warehouseFromDb == null)
                {
                    Console.WriteLine("Failed to find any warehouse. All fallback methods exhausted.");
                    return BadRequest(new { success = false, message = "Không tìm thấy kho hàng phù hợp. Vui lòng kiểm tra lại kho đã chọn hoặc thử chọn kho khác." });
                }
                
                warehouseId = warehouseFromDb.Id;
                storeId = warehouseFromDb.StoreId;

                // Tìm slot phù hợp dựa trên thể tích/diện tích
                var requiredVolume = volumeResult?.RequiredVolumeM3 ?? 5m; // Fallback: 5 m³
                var requiredArea = volumeResult?.RequiredAreaM2 ?? 3m; // Fallback: 3 m²
                
                Console.WriteLine($"Looking for slots in warehouse {warehouseId}");
                Console.WriteLine($"Required volume: {requiredVolume} m³, Required area: {requiredArea} m²");
                
                // Kiểm tra tổng số slot trong warehouse
                var allSlotsInWarehouse = await _db.WarehouseSlots
                    .Where(s => s.WarehouseId == warehouseId)
                    .CountAsync();
                Console.WriteLine($"Total slots in warehouse: {allSlotsInWarehouse}");
                
                var availableSlotsCount = await _db.WarehouseSlots
                    .Where(s => s.WarehouseId == warehouseId && !s.IsBlocked && s.CurrentOrderId == null)
                    .CountAsync();
                Console.WriteLine($"Available slots (not blocked, not reserved): {availableSlotsCount}");
                
                // Tính toán thể tích và diện tích trực tiếp trong LINQ (không dùng computed property VolumeM3)
                // Load vào memory trước để có thể tính toán thể tích
                var allSlots = await _db.WarehouseSlots
                    .Where(s => s.WarehouseId == warehouseId &&
                               !s.IsBlocked &&
                               s.CurrentOrderId == null)
                    .ToListAsync();
                
                // Lọc và sắp xếp trong memory
                var suitableSlots = allSlots
                    .Where(s => (s.HeightM * s.LengthM * s.WidthM) >= requiredVolume && // Tính thể tích trực tiếp
                               (s.LengthM * s.WidthM) >= requiredArea) // Diện tích sàn
                    .OrderBy(s => s.HeightM * s.LengthM * s.WidthM) // Ưu tiên slot nhỏ nhất phù hợp
                    .ThenBy(s => s.BasePricePerHour)
                    .ToList();

                Console.WriteLine($"Found {suitableSlots.Count} suitable slots");
                
                // Tính thể tích thực tế của các slot phù hợp để log
                foreach (var slot in suitableSlots.Take(3))
                {
                    var actualVolume = slot.HeightM * slot.LengthM * slot.WidthM;
                    var actualArea = slot.LengthM * slot.WidthM;
                    Console.WriteLine($"  Slot {slot.Code}: Volume={actualVolume:F2} m³, Area={actualArea:F2} m², Price={slot.BasePricePerHour} VND/h");
                }

                if (!suitableSlots.Any())
                {
                    // Thử tìm slot gần nhất về thể tích/diện tích (không cần đúng yêu cầu)
                    var allAvailableSlots = await _db.WarehouseSlots
                        .Where(s => s.WarehouseId == warehouseId && !s.IsBlocked && s.CurrentOrderId == null)
                        .ToListAsync();
                    
                    var closestSlots = allAvailableSlots
                        .OrderBy(s => Math.Abs((double)((s.HeightM * s.LengthM * s.WidthM) - requiredVolume)))
                        .ThenBy(s => Math.Abs((double)((s.LengthM * s.WidthM) - requiredArea)))
                        .Take(5)
                        .ToList();
                    
                    if (closestSlots.Any())
                    {
                        Console.WriteLine($"Found {closestSlots.Count} closest slots (not exact match)");
                        var closest = closestSlots.First();
                        var closestVolume = closest.HeightM * closest.LengthM * closest.WidthM;
                        Console.WriteLine($"Closest slot: Code={closest.Code}, Volume={closestVolume:F2} m³, Area={closest.LengthM * closest.WidthM:F2} m²");
                    }
                    
                    return BadRequest(new { 
                        success = false, 
                        message = $"Không tìm thấy ô kho phù hợp trong kho '{warehouseFromDb.Name}'. Yêu cầu tối thiểu: {requiredVolume:F2} m³ thể tích, {requiredArea:F2} m² diện tích.",
                        warehouseId = warehouseId.ToString(),
                        warehouseName = warehouseFromDb.Name,
                        totalSlots = allSlotsInWarehouse,
                        availableSlots = availableSlotsCount,
                        requiredVolume = requiredVolume,
                        requiredArea = requiredArea
                    });
                }

                var selectedSlot = suitableSlots.First();
                var storageDuration = (viewModel.StorageEndDate - viewModel.StorageStartDate).TotalHours;
                var storageDays = Math.Ceiling(storageDuration / 24.0);
                var baseSlotPrice = selectedSlot.BasePricePerHour * (decimal)storageDuration;
                Console.WriteLine($"Storage duration: {storageDuration:F1} hours ({storageDays} days), Base slot price: {baseSlotPrice:F0} VND");
                
                // Tính giá cho các dịch vụ đặc biệt
                var addonPrices = new Dictionary<string, decimal>
                {
                    { "🧊 Kho mát", 50000m }, // 50,000 VND/ngày
                    { "💧 Chống ẩm", 30000m }, // 30,000 VND/ngày
                    { "🔒 An ninh cao", 40000m }, // 40,000 VND/ngày
                    { "🛡️ Bảo hiểm hàng hóa", 100000m }, // 100,000 VND (một lần)
                    { "🏢 Kho có thang máy", 20000m }, // 20,000 VND/ngày
                    { "📹 Giám sát 24/7", 60000m } // 60,000 VND/ngày
                };
                
                // Dịch vụ tính theo ngày
                var dailyAddons = new HashSet<string> { "🧊 Kho mát", "💧 Chống ẩm", "🔒 An ninh cao", "🏢 Kho có thang máy", "📹 Giám sát 24/7" };
                
                var totalAddonPrice = 0m;
                var addonDetails = new List<object>();
                
                if (viewModel.SpecialRequirements != null && viewModel.SpecialRequirements.Any())
                {
                    Console.WriteLine($"Calculating addon prices for {viewModel.SpecialRequirements.Count} services");
                    foreach (var requirement in viewModel.SpecialRequirements)
                    {
                        if (addonPrices.ContainsKey(requirement))
                        {
                            var addonPrice = addonPrices[requirement];
                            var serviceTotal = dailyAddons.Contains(requirement) 
                                ? addonPrice * (decimal)storageDays 
                                : addonPrice; // Bảo hiểm tính một lần
                            
                            totalAddonPrice += serviceTotal;
                            addonDetails.Add(new
                            {
                                name = requirement,
                                unitPrice = addonPrice,
                                isDaily = dailyAddons.Contains(requirement),
                                quantity = dailyAddons.Contains(requirement) ? (int)storageDays : 1,
                                total = serviceTotal
                            });
                            
                            Console.WriteLine($"  {requirement}: {(dailyAddons.Contains(requirement) ? $"{addonPrice:F0} VND/ngày × {storageDays} ngày" : $"{addonPrice:F0} VND (một lần)")} = {serviceTotal:F0} VND");
                        }
                    }
                }
                
                var totalPrice = baseSlotPrice + totalAddonPrice;
                Console.WriteLine($"Total addon price: {totalAddonPrice:F0} VND");
                Console.WriteLine($"Total price (slot + addons): {totalPrice:F0} VND");

                var order = new Order
                {
                    Id = Guid.NewGuid(),
                    CustomerId = userId,
                    StoreId = storeId,
                    PickupAddress = new Address
                    {
                        Id = Guid.NewGuid(),
                        AddressLine = viewModel.PickupAddress.AddressLine,
                        Latitude = viewModel.PickupAddress.Latitude,
                        Longitude = viewModel.PickupAddress.Longitude,
                        City = "Hà Nội"
                    },
                    DropoffAddress = new Address
                    {
                        Id = Guid.NewGuid(),
                        AddressLine = viewModel.WarehouseArea.AddressLine,
                        Latitude = viewModel.WarehouseArea.Latitude,
                        Longitude = viewModel.WarehouseArea.Longitude,
                        City = "Hà Nội"
                    },
                    DeliveryDate = viewModel.StorageStartDate,
                    PickupDate = viewModel.StorageEndDate,
                    Note = viewModel.Note ?? string.Empty,
                    ProductImageUrl = imageUrl, // Lưu URL ảnh tổng
                    Status = StatusValue.Pending,
                    TotalAmount = totalPrice, // Giá đã tính từ slot + các dịch vụ đặc biệt
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    OrderItems = new List<OrderItem>()
                };

                if (viewModel.SpecialRequirements != null && viewModel.SpecialRequirements.Any())
                {
                    order.Note += "\n\n📋 Yêu cầu đặc biệt:\n" + string.Join("\n", viewModel.SpecialRequirements.Select(r => "• " + r));
                }

                if (viewModel.Items != null && viewModel.Items.Any())
                {
                    foreach (var itemVm in viewModel.Items)
                    {
                        if (string.IsNullOrWhiteSpace(itemVm.Name) || itemVm.Quantity <= 0)
                        {
                            continue;
                        }

                        order.OrderItems.Add(new OrderItem
                        {
                            Id = Guid.NewGuid(),
                            OrderId = order.Id,
                            ItemName = itemVm.Name.Trim(),
                            Description = itemVm.Category,
                            Quantity = itemVm.Quantity,
                            WeightKg = itemVm.EstimatedWeightKg,
                            UnitPrice = 0m,
                            Subtotal = 0m
                        });
                    }
                }

                Console.WriteLine("Creating order in database...");
                var createdOrder = await _deliveryService.CreateOrderAsync(order);
                Console.WriteLine($"Order created: {createdOrder.Id}");
                
                // Gán slot cho order (có thể lưu vào một bảng trung gian hoặc note)
                Console.WriteLine($"Reserving slot {selectedSlot.Code} for order {createdOrder.Id}");
                selectedSlot.CurrentOrderId = createdOrder.Id;
                await _db.SaveChangesAsync();
                Console.WriteLine("Slot reserved successfully");

                // Tính toán chi tiết giá (đã tính addons ở trên)
                var storageDaysForDisplay = Math.Ceiling((viewModel.StorageEndDate - viewModel.StorageStartDate).TotalDays);
                var subtotal = totalPrice; // Đã bao gồm cả addons
                var vatAmount = subtotal * 0.1m; // VAT 10%
                var grandTotal = subtotal + vatAmount;

                // Trả về báo giá chi tiết
                return Json(new
                {
                    success = true,
                    orderId = createdOrder.Id,
                    message = "Đơn hàng đã được tạo thành công!",
                    quote = new
                    {
                        // Thông tin kho
                        warehouseName = warehouseFromDb.Name,
                        warehouseAddress = warehouseFromDb.Address?.AddressLine ?? viewModel.WarehouseArea.AddressLine ?? "N/A",
                        
                        // Thông tin slot
                        slotCode = selectedSlot.Code,
                        slotVolumeM3 = Math.Round(selectedSlot.HeightM * selectedSlot.LengthM * selectedSlot.WidthM, 2), // Tính trực tiếp thay vì dùng VolumeM3 property
                        slotAreaM2 = Math.Round(selectedSlot.LengthM * selectedSlot.WidthM, 2),
                        slotDimensions = $"{selectedSlot.LengthM:F2}m × {selectedSlot.WidthM:F2}m × {selectedSlot.HeightM:F2}m",
                        
                        // Yêu cầu tính toán từ Gemini
                        requiredVolumeM3 = Math.Round(requiredVolume, 2),
                        requiredAreaM2 = Math.Round(requiredArea, 2),
                        analysisDetails = volumeResult?.AnalysisDetails,
                        itemEstimates = volumeResult?.ItemEstimates,
                        
                        // Thông tin thời gian
                        storageStartDate = viewModel.StorageStartDate.ToString("dd/MM/yyyy"),
                        storageEndDate = viewModel.StorageEndDate.ToString("dd/MM/yyyy"),
                        storageDurationHours = Math.Round(storageDuration, 1),
                        storageDurationDays = storageDays,
                        
                        // Bảng giá
                        baseSlotPrice = Math.Round(baseSlotPrice, 0),
                        pricePerHour = Math.Round(selectedSlot.BasePricePerHour, 0),
                        addonDetails = addonDetails, // Chi tiết các dịch vụ đặc biệt
                        totalAddonPrice = Math.Round(totalAddonPrice, 0),
                        subtotal = Math.Round(subtotal, 0),
                        vatAmount = Math.Round(vatAmount, 0),
                        vatRate = 10,
                        totalAmount = Math.Round(grandTotal, 0)
                    }
                });
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine("InvalidOperationException in CreateWarehouseOrder: " + ex.Message);
                Console.WriteLine("Stack trace: " + ex.StackTrace);
                return BadRequest(new { 
                    success = false, 
                    message = "Không tìm thấy kho hàng khả dụng trong khu vực này.",
                    detail = ex.Message,
                    stackTrace = ex.StackTrace
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in CreateWarehouseOrder: " + ex.Message);
                Console.WriteLine("Inner exception: " + ex.InnerException?.Message);
                Console.WriteLine("Stack trace: " + ex.StackTrace);
                return StatusCode(500, new
                {
                    success = false,
                    message = "Có lỗi xảy ra khi tạo đơn hàng. Vui lòng thử lại sau.",
                    detail = ex.Message,
                    innerDetail = ex.InnerException?.Message,
                    stackTrace = ex.StackTrace
                });
            }
        }

        [HttpGet]
        public async Task<IActionResult> NearbyWarehouses(double lat, double lng, int take = 10)
        {
            var warehouses = await _db.Warehouses
                .Include(w => w.Address)
                .Include(w => w.Store)
                .Where(w => w.Address != null && w.Address.Latitude != null && w.Address.Longitude != null)
                .Select(w => new
                {
                    w.Id,
                    w.Name,
                    StoreName = w.Store.StoreName,
                    Latitude = w.Address!.Latitude!.Value,
                    Longitude = w.Address!.Longitude!.Value,
                    AddressLine = w.Address!.AddressLine,
                    Ward = w.Address!.Ward,
                    District = w.Address!.District,
                    City = w.Address!.City
                })
                .ToListAsync();

            static double ToRad(double d) => d * Math.PI / 180.0;
            var results = warehouses
                .Select(w =>
                {
                    var R = 6371.0;
                    var dLat = ToRad(w.Latitude - lat);
                    var dLng = ToRad(w.Longitude - lng);
                    var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) + Math.Cos(ToRad(lat)) * Math.Cos(ToRad(w.Latitude)) * Math.Sin(dLng / 2) * Math.Sin(dLng / 2);
                    var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
                    var dist = R * c;
                    return new
                    {
                        w.Id,
                        w.Name,
                        w.StoreName,
                        w.Latitude,
                        w.Longitude,
                        distanceKm = Math.Round(dist, 2),
                        full = string.Join(", ", new[] { w.AddressLine, w.Ward, w.District, w.City }.Where(s => !string.IsNullOrWhiteSpace(s)))
                    };
                })
                .OrderBy(x => x.distanceKm)
                .Take(take)
                .ToList();

            return Json(results);
        }

        // Bước 3: Tính giá (AJAX) + lưu Quotation ở trạng thái Sent
        [HttpPost]
        public async Task<IActionResult> Calculate([FromBody] QuoteRequestVm req, CancellationToken ct)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _svc.CalculateAndCreateQuotationAsync(req, ct);
            return Ok(result);
        }

        // Bước 4A: Giữ chỗ tạm (2h)
        [HttpPost]
        public async Task<IActionResult> HoldTemp([FromBody] HoldTempVm vm, CancellationToken ct)
        {
            var ok = await _svc.CreateTempReservationAsync(vm, ct);
            return ok ? Ok() : BadRequest("Không tạo được giữ chỗ tạm.");
        }

        // Bước 4B: Chấp nhận báo giá → Quotation.Accepted + tạo Reservation Firm
        [HttpPost]
        public async Task<IActionResult> Accept([FromBody] AcceptQuoteVm vm, CancellationToken ct)
        {
            var result = await _svc.AcceptQuotationAsync(vm, ct);
            if (!result.Success || result.OrderId is null)
                return BadRequest("Không chấp nhận được báo giá.");

            var redirectUrl = Url.Action("Index", "Payment", new { orderId = result.OrderId.Value });
            return Ok(new { success = true, orderId = result.OrderId, redirectUrl });
        }


        // Bước 4C: Yêu cầu chỉnh giá (ghi chú)
        [HttpPost]
        public async Task<IActionResult> RequestRevision([FromBody] RequestRevisionVm vm, CancellationToken ct)
        {
            var ok = await _svc.RequestRevisionAsync(vm, ct);
            return ok ? Ok() : BadRequest("Không gửi yêu cầu chỉnh giá được.");
        }

        public class CreateFeedbackDto
        {
            public Guid QuotationId { get; set; }
            public string Comment { get; set; }
            public int Rating { get; set; }
        }
        
        [HttpPost]
        public async Task<IActionResult> Feedback([FromBody] CreateFeedbackDto data)
        {
            var quotationInfo = await _svc.GetByIdAsync(data.QuotationId, CancellationToken.None);
            if(quotationInfo == null) return BadRequest("Thông tin báo giá bị thiếu. Tạo thất bại");

            if(quotationInfo.StoreId == null) return BadRequest("Thông tin báo giá tạm thời đã hết hạn. Không thể đánh giá cho cửa hàng này");

            var feedback = new Feedback
            {
                Id = Guid.NewGuid(),
                FromUserId = quotationInfo.CustomerId,
                ToStoreId = (Guid)quotationInfo.StoreId,
                Rating = data.Rating,
                Comment = data.Comment,
                CreatedAt = DateTime.UtcNow
            };
            
            var rs = await _feedbackService.CreateFeedbackAsync(feedback);
            return rs != null ? Ok() : BadRequest("Không gửi được đánh giá");
        }
        
        
    }
}
