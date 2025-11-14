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
using DocumentFormat.OpenXml.Vml;
using Org.BouncyCastle.Ocsp;
using DocumentFormat.OpenXml.Wordprocessing;
using Humanizer;
using DataAccessLayer.Constants;
using Microsoft.AspNetCore.Authorization;

namespace PresentationLayer.Controllers
{
    [Authorize(Roles = UserRoles.CUSTOMER)]
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
                StorageStartDate = DateTime.Now.Date.AddDays(1),
                StorageEndDate = DateTime.Now.Date.AddDays(30),
                AddressOptions = addressOptions,
                DropoffAddressId = defaultAddress?.Id,
                DropoffLatitude = defaultAddress?.Latitude,
                DropoffLongitude = defaultAddress?.Longitude,
                DropoffAddressText = defaultAddress == null ? null : ($"{defaultAddress.AddressLine}, {defaultAddress.Ward}, {defaultAddress.District}, {defaultAddress.City}")?.Replace("  ", " ")
            };

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
        public async Task<IActionResult> CreateWarehouseOrder([FromForm] CreateWarehouseOrderViewModel viewModel, IFormFile? productImage, List<IFormFile>? productImages)
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

            var transaction = await _db.Database.BeginTransactionAsync();
            try
            {
                // Ưu tiên: Gửi danh sách items lên AI để tính toán
                VolumeCalculationResult? volumeResult = null;
                string? geminiError = null;
                List<string>? imageUrls = null;

                // Nếu có items, gửi items lên AI (ưu tiên cao nhất)
                if (viewModel.Items != null && viewModel.Items.Any(i => !string.IsNullOrWhiteSpace(i.Name) && i.Quantity > 0))
                {
                    try
                    {
                        var itemsForAI = viewModel.Items
                            .Where(i => !string.IsNullOrWhiteSpace(i.Name) && i.Quantity > 0)
                            .Select(i => new ItemInfo
                            {
                                Name = i.Name.Trim(),
                                Category = i.Category?.Trim(),
                                Quantity = i.Quantity
                            })
                            .ToList();

                        volumeResult = await _geminiService.AnalyzeItemsAndCalculateVolumeAsync(itemsForAI);

                        if (volumeResult == null)
                        {
                            geminiError = "Gemini API trả về null result";
                        }
                        else
                        {
                            Console.WriteLine($"✅ AI đã phân tích {itemsForAI.Count} items và tính được: {volumeResult.RequiredVolumeM3:F2} m³, {volumeResult.RequiredAreaM2:F2} m²");
                        }
                    }
                    catch (Exception geminiEx)
                    {
                        Console.WriteLine($"Gemini analysis error (items): {geminiEx.Message}");
                        volumeResult = null;
                        geminiError = geminiEx.Message;
                    }
                }

                // Fallback 1: Nếu không có items hoặc AI lỗi, thử dùng nhiều ảnh
                if (volumeResult == null)
                {
                    var filesToProcess = new List<IFormFile>();

                    if (productImage != null && productImage.Length > 0)
                    {
                        filesToProcess.Add(productImage);
                    }

                    if (productImages != null && productImages.Any())
                    {
                        filesToProcess.AddRange(productImages.Where(f => f != null && f.Length > 0));
                    }

                    if (filesToProcess.Any())
                    {
                        try
                        {
                            // Upload tất cả ảnh lên Cloudinary
                            imageUrls = new List<string>();
                            foreach (var file in filesToProcess)
                            {
                                var imageUrl = await _cloudinaryService.UploadImageFileAsync(file);
                                imageUrls.Add(imageUrl);
                            }

                            // Gọi Gemini để phân tích nhiều ảnh
                            volumeResult = await _geminiService.AnalyzeMultipleImagesAndCalculateVolumeAsync(imageUrls);

                            if (volumeResult == null)
                            {
                                geminiError = "Gemini API trả về null result";
                            }
                            else
                            {
                                Console.WriteLine($"✅ AI đã phân tích {imageUrls.Count} ảnh và tính được: {volumeResult.RequiredVolumeM3:F2} m³, {volumeResult.RequiredAreaM2:F2} m²");
                            }
                        }
                        catch (Exception geminiEx)
                        {
                            Console.WriteLine($"Gemini analysis error (images): {geminiEx.Message}");
                            volumeResult = null;
                            geminiError = geminiEx.Message;
                        }
                    }
                }

                // Tìm warehouse từ WarehouseId (ưu tiên) hoặc WarehouseArea
                Guid warehouseId;
                Guid storeId;
                Warehouse? warehouseFromDb = null;

                // Ưu tiên: Tìm warehouse bằng ID nếu có
                if (viewModel.WarehouseId.HasValue)
                {
                    warehouseFromDb = await _db.Warehouses
                        .Include(w => w.Address)
                        .Where(w => w.Id == viewModel.WarehouseId.Value)
                        .FirstOrDefaultAsync();

                    if (warehouseFromDb == null)
                    {
                        return BadRequest(new { success = false, message = "Kho hàng đã chọn không tồn tại." });
                    }
                }

                // Fallback: Tìm warehouse bằng địa chỉ nếu không có ID
                if (warehouseFromDb == null)
                {
                    var warehouseAddress = viewModel.WarehouseArea.AddressLine;
                    warehouseFromDb = await _db.Warehouses
                        .Include(w => w.Address)
                        .Where(w => w.Address != null &&
                                    (w.Address.AddressLine.Contains(warehouseAddress) ||
                                     w.Name.Contains(warehouseAddress)))
                        .FirstOrDefaultAsync();
                }

                // Fallback: Tìm 3 kho hàng gần nhất với tọa độ
                if (warehouseFromDb == null)
                {
                    try
                    {
                        var lat = viewModel.WarehouseArea.Latitude ?? 0;
                        var lng = viewModel.WarehouseArea.Longitude ?? 0;

                        var warehouses = await _db.Warehouses
                            .Include(w => w.Address)
                            .Where(w => w.Address != null &&
                                       w.Address.Latitude != null &&
                                       w.Address.Longitude != null &&
                                       w.Status == StatusValue.Approved)
                            .ToListAsync();

                        static double ToRad(double d) => d * Math.PI / 180.0;
                        var warehousesWithDistance = warehouses
                            .Select(w =>
                            {
                                var R = 6371.0;
                                var wLat = w.Address!.Latitude!.Value;
                                var wLng = w.Address!.Longitude!.Value;
                                var dLat = ToRad(wLat - lat);
                                var dLng = ToRad(wLng - lng);
                                var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                                       Math.Cos(ToRad(lat)) * Math.Cos(ToRad(wLat)) *
                                       Math.Sin(dLng / 2) * Math.Sin(dLng / 2);
                                var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
                                var distanceKm = R * c;
                                return new { Warehouse = w, DistanceKm = distanceKm };
                            })
                            .OrderBy(x => x.DistanceKm)
                            .Take(3)
                            .ToList();

                        warehouseFromDb = warehousesWithDistance.FirstOrDefault()?.Warehouse;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error finding nearest warehouses: {ex.Message}");
                    }
                }

                if (warehouseFromDb == null)
                {
                    return BadRequest(new { success = false, message = "Không tìm thấy kho hàng phù hợp. Vui lòng kiểm tra lại kho đã chọn hoặc thử chọn kho khác." });
                }

                warehouseId = warehouseFromDb.Id;
                storeId = warehouseFromDb.StoreId;

                // Tìm slot phù hợp dựa trên thể tích/diện tích
                var requiredVolume = volumeResult?.RequiredVolumeM3 ?? 5m;
                var requiredArea = volumeResult?.RequiredAreaM2 ?? 3m;

                // Tính toán thể tích và diện tích trực tiếp trong LINQ (không dùng computed property VolumeM3)
                // Load vào memory trước để có thể tính toán thể tích
                var allSlots = await _db.WarehouseSlots
                    .Where(s => s.WarehouseId == warehouseId &&
                               !s.IsBlocked &&
                               s.CurrentOrderId == null)
                    .ToListAsync();

                // Lọc và sắp xếp trong memory
                var suitableSlots = allSlots
                    .Where(s => (s.HeightM * s.LengthM * s.WidthM) >= requiredVolume &&
                               (s.LengthM * s.WidthM) >= requiredArea)
                    .OrderBy(s => s.HeightM * s.LengthM * s.WidthM)
                    .ThenBy(s => s.BasePricePerHour)
                    .ToList();

                if (!suitableSlots.Any())
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = $"Không tìm thấy ô kho phù hợp trong kho '{warehouseFromDb.Name}'. Yêu cầu tối thiểu: {requiredVolume:F2} m³ thể tích, {requiredArea:F2} m² diện tích."
                    });
                }

                var selectedSlot = suitableSlots.First();
                var storageDuration = (viewModel.StorageEndDate - viewModel.StorageStartDate).TotalHours;
                var storageDays = Math.Ceiling(storageDuration / 24.0);
                var baseSlotPrice = selectedSlot.BasePricePerHour * (decimal)storageDuration;

                // Tính giá cho các dịch vụ đặc biệt
                var addonPrices = new Dictionary<string, decimal>
                {
                    { "🧊 Kho mát", 50000m },
                    { "💧 Chống ẩm", 30000m },
                    { "🔒 An ninh cao", 40000m },
                    { "🛡️ Bảo hiểm hàng hóa", 100000m },
                    { "🏢 Kho có thang máy", 20000m },
                    { "📹 Giám sát 24/7", 60000m }
                };

                var dailyAddons = new HashSet<string> { "🧊 Kho mát", "💧 Chống ẩm", "🔒 An ninh cao", "🏢 Kho có thang máy", "📹 Giám sát 24/7" };

                var totalAddonPrice = 0m;
                var addonDetails = new List<object>();

                if (viewModel.SpecialRequirements != null && viewModel.SpecialRequirements.Any())
                {
                    foreach (var requirement in viewModel.SpecialRequirements)
                    {
                        if (addonPrices.ContainsKey(requirement))
                        {
                            var addonPrice = addonPrices[requirement];
                            var serviceTotal = dailyAddons.Contains(requirement)
                                ? addonPrice * (decimal)storageDays
                                : addonPrice;

                            totalAddonPrice += serviceTotal;
                            addonDetails.Add(new
                            {
                                name = requirement,
                                unitPrice = addonPrice,
                                isDaily = dailyAddons.Contains(requirement),
                                quantity = dailyAddons.Contains(requirement) ? (int)storageDays : 1,
                                total = serviceTotal
                            });
                        }
                    }
                }

                var totalPrice = baseSlotPrice + totalAddonPrice;

                var storageDaysForDisplay = Math.Ceiling((viewModel.StorageEndDate - viewModel.StorageStartDate).TotalDays);
                var subtotal = totalPrice;
                var vatAmount = subtotal * 0.1m;
                var grandTotal = subtotal + vatAmount;

                // Tạo Quotation trước (chưa có Order)
                var VALIDITY_FOR_QUOTATION_HOUR = 24;
                var validUntil = DateTime.Now.AddHours(VALIDITY_FOR_QUOTATION_HOUR);
                var quotation = new Quotation
                {
                    Id = Guid.NewGuid(),
                    StoreId = storeId,
                    CustomerId = userId,
                    TotalAmount = grandTotal,
                    ValidUntil = validUntil,
                    Status = StatusValue.Sent,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };
                _db.Quotations.Add(quotation);

                // Tạo địa chỉ nhận hàng (pickup) cho đơn hàng (nếu người dùng cung cấp)
                Address? pickupAddress = null;
                if (viewModel.PickupAddress != null && !string.IsNullOrWhiteSpace(viewModel.PickupAddress.AddressLine))
                {
                    pickupAddress = new Address
                    {
                        Id = Guid.NewGuid(),
                        AddressLine = viewModel.PickupAddress.AddressLine,
                        Latitude = viewModel.PickupAddress.Latitude,
                        Longitude = viewModel.PickupAddress.Longitude,
                        City = viewModel.PickupAddress.City,
                        District = viewModel.PickupAddress.District,
                        Ward = viewModel.PickupAddress.Ward,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };
                    _db.Addresses.Add(pickupAddress);
                }

                // Địa chỉ kho (drop-off) lấy từ warehouse; fallback từ request
                Address? dropoffAddress = null;
                if (warehouseFromDb.Address != null)
                {
                    dropoffAddress = new Address
                    {
                        Id = Guid.NewGuid(),
                        AddressLine = warehouseFromDb.Address.AddressLine,
                        Latitude = warehouseFromDb.Address.Latitude,
                        Longitude = warehouseFromDb.Address.Longitude,
                        City = warehouseFromDb.Address.City,
                        District = warehouseFromDb.Address.District,
                        Ward = warehouseFromDb.Address.Ward,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };
                    _db.Addresses.Add(dropoffAddress);
                }
                else if (viewModel.WarehouseArea != null && !string.IsNullOrWhiteSpace(viewModel.WarehouseArea.AddressLine))
                {
                    dropoffAddress = new Address
                    {
                        Id = Guid.NewGuid(),
                        AddressLine = viewModel.WarehouseArea.AddressLine,
                        Latitude = viewModel.WarehouseArea.Latitude,
                        Longitude = viewModel.WarehouseArea.Longitude,
                        City = viewModel.WarehouseArea.City,
                        District = viewModel.WarehouseArea.District,
                        Ward = viewModel.WarehouseArea.Ward,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };
                    _db.Addresses.Add(dropoffAddress);
                }

                // Tạo đơn hàng ở trạng thái chờ xác nhận để hiển thị trên màn hình theo dõi
                var provisionalOrder = new Order
                {
                    Id = Guid.NewGuid(),
                    QuotationId = quotation.Id,
                    CustomerId = quotation.CustomerId,
                    StoreId = storeId,
                    PickupAddress = pickupAddress,
                    PickupAddressId = pickupAddress?.Id,
                    DropoffAddress = dropoffAddress,
                    DropoffAddressId = dropoffAddress?.Id,
                    DeliveryDate = viewModel.StorageStartDate,
                    PickupDate = viewModel.StorageEndDate,
                    Status = StatusValue.Pending,
                    TotalAmount = quotation.TotalAmount,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    Note = "Đơn hàng chờ xác nhận báo giá"
                };

                if (viewModel.Items != null && viewModel.Items.Any())
                {
                    foreach (var itemVm in viewModel.Items)
                    {
                        if (string.IsNullOrWhiteSpace(itemVm.Name) || itemVm.Quantity <= 0)
                        {
                            continue;
                        }

                        provisionalOrder.OrderItems.Add(new OrderItem
                        {
                            Id = Guid.NewGuid(),
                            OrderId = provisionalOrder.Id,
                            ItemName = itemVm.Name.Trim(),
                            Description = itemVm.Category,
                            Quantity = itemVm.Quantity,
                            UnitPrice = 0m,
                            Subtotal = 0m,
                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = DateTime.UtcNow
                        });
                    }
                }

                _db.Orders.Add(provisionalOrder);

                // Tạo SlotReservation để giữ chỗ trong 24h
                var slotReservation = new SlotReservation
                {
                    Id = Guid.NewGuid(),
                    OrderId = provisionalOrder.Id,
                    WarehouseSlotId = selectedSlot.Id,
                    ExpiresAt = validUntil,
                    Status = StatusValue.Active,
                    From = viewModel.StorageStartDate,
                    To = viewModel.StorageEndDate
                };
                _db.SlotReservations.Add(slotReservation);

                selectedSlot.Status = StatusValue.Reserved;
                selectedSlot.CurrentOrderId = provisionalOrder.Id;
                _db.WarehouseSlots.Update(selectedSlot);

                await _db.SaveChangesAsync();
                await transaction.CommitAsync();

                // Trả về báo giá chi tiết
                return Json(new
                {
                    success = true,
                    message = "Báo giá đã được tạo thành công! Slot đã được giữ chỗ trong 24 giờ.",
                    quotationId = quotation.Id,
                    quote = new
                    {
                        warehouseName = warehouseFromDb.Name,
                        warehouseAddress = warehouseFromDb.Address?.AddressLine ?? viewModel.WarehouseArea.AddressLine ?? "N/A",

                        slotId = selectedSlot.Id.ToString(),
                        slotCode = selectedSlot.Code,
                        slotVolumeM3 = Math.Round(selectedSlot.HeightM * selectedSlot.LengthM * selectedSlot.WidthM, 2),
                        slotAreaM2 = Math.Round(selectedSlot.LengthM * selectedSlot.WidthM, 2),
                        slotDimensions = $"{selectedSlot.LengthM:F2}m × {selectedSlot.WidthM:F2}m × {selectedSlot.HeightM:F2}m",

                        requiredVolumeM3 = volumeResult != null ? Math.Round(volumeResult.RequiredVolumeM3, 2) : (decimal?)null,
                        requiredAreaM2 = volumeResult != null ? Math.Round(volumeResult.RequiredAreaM2, 2) : (decimal?)null,
                        analysisDetails = volumeResult?.AnalysisDetails,
                        itemEstimates = volumeResult?.ItemEstimates,
                        geminiAnalysisAvailable = volumeResult != null,
                        hasProductImages = imageUrls != null && imageUrls.Any(),
                        imageUrls = imageUrls,
                        geminiError = geminiError,

                        storageStartDate = viewModel.StorageStartDate.ToString("dd/MM/yyyy"),
                        storageEndDate = viewModel.StorageEndDate.ToString("dd/MM/yyyy"),
                        storageDurationHours = Math.Round(storageDuration, 1),
                        storageDurationDays = storageDays,

                        baseSlotPrice = Math.Round(baseSlotPrice, 0),
                        pricePerHour = Math.Round(selectedSlot.BasePricePerHour, 0),
                        addonDetails = addonDetails,
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
                transaction.Rollback();
                Console.WriteLine("InvalidOperationException in CreateWarehouseOrder: " + ex.Message);
                Console.WriteLine("Stack trace: " + ex.StackTrace);
                return BadRequest(new
                {
                    success = false,
                    message = "Không tìm thấy kho hàng khả dụng trong khu vực này.",
                    detail = ex.Message,
                    stackTrace = ex.StackTrace
                });
            }
            catch (Exception ex)
            {
                transaction.Rollback();
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

        // API để gán slot vào order sau khi người dùng xác nhận
        [HttpPost]
        public async Task<IActionResult> AssignSlotToOrder([FromBody] AssignSlotToOrderRequest request)
        {
            if (request == null || request.OrderId == Guid.Empty || request.SlotId == Guid.Empty)
            {
                return BadRequest(new { success = false, message = "OrderId và SlotId là bắt buộc." });
            }

            try
            {
                var order = await _orderService.GetByIdAsync(request.OrderId);
                if (order == null)
                {
                    return NotFound(new { success = false, message = "Không tìm thấy đơn hàng." });
                }

                var slot = await _db.WarehouseSlots
                    .FirstOrDefaultAsync(s => s.Id == request.SlotId);

                if (slot == null)
                {
                    return NotFound(new { success = false, message = "Không tìm thấy ô kho." });
                }

                if (slot.IsBlocked)
                {
                    return BadRequest(new { success = false, message = "Ô kho này đã bị khóa." });
                }

                if (slot.CurrentOrderId != null && slot.CurrentOrderId != request.OrderId)
                {
                    return BadRequest(new { success = false, message = "Ô kho này đã được gán cho đơn hàng khác." });
                }

                var existingAssignment = await _db.OrderWarehouseSlots
                    .FirstOrDefaultAsync(ows => ows.OrderId == request.OrderId &&
                                                 ows.WarehouseSlotId == request.SlotId &&
                                                 ows.DeletedAt == null);

                if (existingAssignment != null)
                {
                    if (existingAssignment.ReleasedAt == null)
                    {
                        return BadRequest(new { success = false, message = "Ô kho này đã được gán cho đơn hàng này rồi." });
                    }
                }

                var orderWarehouseSlot = new OrderWarehouseSlot
                {
                    Id = Guid.NewGuid(),
                    OrderId = request.OrderId,
                    WarehouseSlotId = request.SlotId,
                    AssignedAt = DateTime.Now,
                    ReleasedAt = null,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };

                _db.OrderWarehouseSlots.Add(orderWarehouseSlot);

                slot.CurrentOrderId = request.OrderId;
                slot.Status = DataAccessLayer.Enums.StatusValue.Reserved;

                await _db.SaveChangesAsync();

                return Json(new
                {
                    success = true,
                    message = "Đã gán ô kho thành công!",
                    slotCode = slot.Code,
                    orderId = request.OrderId
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in AssignSlotToOrder: " + ex.Message);
                Console.WriteLine("Stack trace: " + ex.StackTrace);
                return StatusCode(500, new
                {
                    success = false,
                    message = "Có lỗi xảy ra khi gán ô kho. Vui lòng thử lại sau.",
                    detail = ex.Message
                });
            }
        }

        // API để xác nhận quotation và tạo Order
        [HttpPost]
        public async Task<IActionResult> ConfirmQuotation([FromBody] ConfirmQuotationRequest request)
        {
            if (request == null || request.QuotationId == Guid.Empty || request.SlotId == Guid.Empty)
            {
                return BadRequest(new { success = false, message = "QuotationId và SlotId là bắt buộc." });
            }

            try
            {
                using var transaction = await _db.Database.BeginTransactionAsync();

                // ===== LOAD DATA =====
                var quotation = await _db.Quotations
                    .Include(q => q.Orders)
                    .FirstOrDefaultAsync(q => q.Id == request.QuotationId);

                if (quotation == null)
                    return NotFound(new { success = false, message = "Không tìm thấy báo giá." });

                var order = await _db.Orders
                    .Include(o => o.OrderItems)
                    .Include(o => o.PickupAddress)
                    .Include(o => o.DropoffAddress)
                    .FirstOrDefaultAsync(o => o.QuotationId == quotation.Id);

                if (order == null)
                {
                    order = new Order
                    {
                        Id = Guid.NewGuid(),
                        QuotationId = quotation.Id,
                        CustomerId = quotation.CustomerId,
                        StoreId = quotation.StoreId ?? Guid.Empty,
                        Status = StatusValue.Pending,
                        TotalAmount = quotation.TotalAmount,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                        Note = "Order auto-created from quotation confirmation"
                    };
                    _db.Orders.Add(order);
                    await _db.SaveChangesAsync(); // Save ID
                }

                var reservation = await _db.SlotReservations
                    .FirstOrDefaultAsync(r => r.WarehouseSlotId == request.SlotId
                                           && r.Status == StatusValue.Active
                                           && r.ExpiresAt > DateTimeOffset.Now
                                           && (r.OrderId == null || r.OrderId == order.Id));

                if (reservation == null)
                    return BadRequest(new { success = false, message = "Slot reservation đã hết hạn hoặc không tồn tại." });

                var slot = await _db.WarehouseSlots
                    .Include(s => s.Warehouse)
                        .ThenInclude(w => w.Address)
                    .FirstOrDefaultAsync(s => s.Id == request.SlotId);

                if (slot == null || slot.Warehouse == null)
                    return BadRequest(new { success = false, message = "Không tìm thấy thông tin kho hàng." });

                // ===== UPDATE PICKUP ADDRESS =====
                if (request.PickupAddress != null && !string.IsNullOrWhiteSpace(request.PickupAddress.AddressLine))
                {
                    if (order.PickupAddress == null)
                    {
                        var newPickup = new Address
                        {
                            Id = Guid.NewGuid(),
                            AddressLine = request.PickupAddress.AddressLine,
                            Latitude = request.PickupAddress.Latitude,
                            Longitude = request.PickupAddress.Longitude,
                            City = request.PickupAddress.City ?? "Hà Nội",
                            District = request.PickupAddress.District,
                            Ward = request.PickupAddress.Ward,
                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = DateTime.UtcNow
                        };
                        _db.Addresses.Add(newPickup);
                        order.PickupAddressId = newPickup.Id;
                    }
                    else
                    {
                        order.PickupAddress.AddressLine = request.PickupAddress.AddressLine;
                        order.PickupAddress.Latitude = request.PickupAddress.Latitude;
                        order.PickupAddress.Longitude = request.PickupAddress.Longitude;
                        order.PickupAddress.City = request.PickupAddress.City ?? order.PickupAddress.City;
                        order.PickupAddress.District = request.PickupAddress.District ?? order.PickupAddress.District;
                        order.PickupAddress.Ward = request.PickupAddress.Ward ?? order.PickupAddress.Ward;
                        order.PickupAddress.UpdatedAt = DateTime.UtcNow;
                        _db.Addresses.Update(order.PickupAddress);
                    }
                }

                // ===== UPDATE DROPOFF ADDRESS =====
                var warehouseAddress = slot.Warehouse.Address;
                if (warehouseAddress != null)
                {
                    if (order.DropoffAddress == null)
                    {
                        var newDrop = new Address
                        {
                            Id = Guid.NewGuid(),
                            AddressLine = warehouseAddress.AddressLine,
                            Latitude = warehouseAddress.Latitude,
                            Longitude = warehouseAddress.Longitude,
                            City = warehouseAddress.City ?? "Hà Nội",
                            District = warehouseAddress.District,
                            Ward = warehouseAddress.Ward,
                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = DateTime.UtcNow
                        };
                        _db.Addresses.Add(newDrop);
                        order.DropoffAddressId = newDrop.Id;
                    }
                    else
                    {
                        order.DropoffAddress.AddressLine = warehouseAddress.AddressLine;
                        order.DropoffAddress.Latitude = warehouseAddress.Latitude;
                        order.DropoffAddress.Longitude = warehouseAddress.Longitude;
                        order.DropoffAddress.City = warehouseAddress.City ?? order.DropoffAddress.City;
                        order.DropoffAddress.District = warehouseAddress.District ?? order.DropoffAddress.District;
                        order.DropoffAddress.Ward = warehouseAddress.Ward ?? order.DropoffAddress.Ward;
                        order.DropoffAddress.UpdatedAt = DateTime.UtcNow;
                        _db.Addresses.Update(order.DropoffAddress);
                    }
                }

                // ===== UPDATE ORDER FIELDS =====
                order.DeliveryDate = request.DeliveryDate ?? reservation.From;
                order.PickupDate = request.PickupDate ?? reservation.To;
                order.Status = StatusValue.AwaitingPayment;
                order.TotalAmount = quotation.TotalAmount;
                order.UpdatedAt = DateTime.Now;

                // ===== UPDATE ORDER ITEMS =====
                if (request.Items != null)
                {
                    _db.OrderItems.RemoveRange(order.OrderItems);

                    foreach (var itemVm in request.Items)
                    {
                        if (string.IsNullOrWhiteSpace(itemVm.Name) || itemVm.Quantity <= 0) continue;

                        order.OrderItems.Add(new OrderItem
                        {
                            Id = Guid.NewGuid(),
                            OrderId = order.Id,
                            ItemName = itemVm.Name.Trim(),
                            Description = itemVm.Category,
                            Quantity = itemVm.Quantity,
                            UnitPrice = 0m,
                            Subtotal = 0m,
                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = DateTime.UtcNow
                        });
                    }
                }

                // ===== CLEAR TRACKING TO AVOID CONFLICT =====
                _db.ChangeTracker.Clear();

                // ----- UPDATE RESERVATION SAFELY -----
                reservation.OrderId = order.Id;
                reservation.UpdatedAt = DateTime.Now;

                _db.SlotReservations.Attach(reservation);
                _db.Entry(reservation).Property(r => r.OrderId).IsModified = true;
                _db.Entry(reservation).Property(r => r.UpdatedAt).IsModified = true;

                // ----- UPDATE SLOT SAFELY -----
                slot.CurrentOrderId = order.Id;
                slot.Status = StatusValue.Reserved;
                slot.UpdatedAt = DateTime.Now;

                _db.WarehouseSlots.Attach(slot);
                _db.Entry(slot).Property(s => s.CurrentOrderId).IsModified = true;
                _db.Entry(slot).Property(s => s.Status).IsModified = true;
                _db.Entry(slot).Property(s => s.UpdatedAt).IsModified = true;

                // ----- UPDATE QUOTATION SAFELY -----
                quotation.Status = StatusValue.Active;
                quotation.UpdatedAt = DateTime.Now;

                _db.Quotations.Attach(quotation);
                _db.Entry(quotation).Property(q => q.Status).IsModified = true;
                _db.Entry(quotation).Property(q => q.UpdatedAt).IsModified = true;

                // ----- UPDATE ORDER (AFTER CLEAR TRACKING) -----
                _db.Orders.Attach(order);
                _db.Entry(order).Property(o => o.Status).IsModified = true;
                _db.Entry(order).Property(o => o.DeliveryDate).IsModified = true;
                _db.Entry(order).Property(o => o.PickupDate).IsModified = true;
                _db.Entry(order).Property(o => o.TotalAmount).IsModified = true;
                _db.Entry(order).Property(o => o.UpdatedAt).IsModified = true;

                // ===== SAVE ALL =====
                await _db.SaveChangesAsync();
                await transaction.CommitAsync();

                return Json(new
                {
                    success = true,
                    message = "Đã xác nhận báo giá và tạo đơn hàng thành công!",
                    orderId = order.Id,
                    slotCode = slot.Code
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in ConfirmQuotation: " + ex.Message);
                Console.WriteLine("Stack trace: " + ex.StackTrace);

                return StatusCode(500, new
                {
                    success = false,
                    message = "Có lỗi xảy ra khi xác nhận báo giá. Vui lòng thử lại sau.",
                    detail = ex.Message
                });
            }
        }

        public class AssignSlotToOrderRequest
        {
            public Guid OrderId { get; set; }
            public Guid SlotId { get; set; }
        }

        public class ConfirmQuotationRequest
        {
            public Guid QuotationId { get; set; }
            public Guid SlotId { get; set; }
            public AddressViewModel? PickupAddress { get; set; }
            public AddressViewModel? DropoffAddress { get; set; }
            public DateTime? DeliveryDate { get; set; }
            public DateTime? PickupDate { get; set; }
            public List<OrderItemViewModel>? Items { get; set; }
        }

        // API endpoint để AI đọc ảnh và trả về danh sách sản phẩm (TỐI ƯU TỐC ĐỘ - không upload Cloudinary)
        [HttpPost]
        public async Task<IActionResult> AnalyzeProductImage(IFormFile? productImage, List<IFormFile>? productImages = null)
        {
            var filesToProcess = new List<IFormFile>();

            if (productImage != null && productImage.Length > 0)
            {
                filesToProcess.Add(productImage);
            }

            if (productImages != null && productImages.Any())
            {
                filesToProcess.AddRange(productImages.Where(f => f != null && f.Length > 0));
            }

            if (filesToProcess.Count == 0)
            {
                return BadRequest(new { success = false, message = "Vui lòng chọn ít nhất một ảnh để phân tích." });
            }

            try
            {
                // ✅ TỐI ƯU: Gọi trực tiếp từ file, KHÔNG upload Cloudinary
                var items = await _geminiService.DetectItemsFromImagesAsync(filesToProcess);

                return Json(new
                {
                    success = true,
                    items = items,
                    message = items.Any()
                        ? $"Đã phát hiện {items.Count} loại sản phẩm từ {filesToProcess.Count} ảnh."
                        : $"Không phát hiện được sản phẩm trong {filesToProcess.Count} ảnh. Vui lòng thử lại với ảnh khác."
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error analyzing product image(s): {ex.Message}");
                return StatusCode(500, new
                {
                    success = false,
                    message = "Có lỗi xảy ra khi phân tích ảnh. Vui lòng thử lại sau.",
                    detail = ex.Message
                });
            }
        }

        private string? ExtractCategoryFromName(string name)
        {
            var lowerName = name.ToLower();
            if (lowerName.Contains("bàn") || lowerName.Contains("ghế") || lowerName.Contains("tủ") || lowerName.Contains("giường"))
                return "Nội thất";
            if (lowerName.Contains("tivi") || lowerName.Contains("máy") || lowerName.Contains("điện"))
                return "Điện tử";
            if (lowerName.Contains("quần") || lowerName.Contains("áo") || lowerName.Contains("giày"))
                return "Thời trang";
            if (lowerName.Contains("sách") || lowerName.Contains("vở") || lowerName.Contains("bút"))
                return "Văn phòng phẩm";
            return "Khác";
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

        [HttpPost]
        public async Task<IActionResult> Calculate([FromBody] QuoteRequestVm req, CancellationToken ct)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _svc.CalculateAndCreateQuotationAsync(req, ct);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> HoldTemp([FromBody] HoldTempVm vm, CancellationToken ct)
        {
            var ok = await _svc.CreateTempReservationAsync(vm, ct);
            return ok ? Ok() : BadRequest("Không tạo được giữ chỗ tạm.");
        }

        [HttpPost]
        public async Task<IActionResult> Accept([FromBody] AcceptQuoteVm vm, CancellationToken ct)
        {
            var result = await _svc.AcceptQuotationAsync(vm, ct);
            if (!result.Success || result.OrderId is null)
                return BadRequest("Không chấp nhận được báo giá.");

            var redirectUrl = Url.Action("Index", "Payment", new { orderId = result.OrderId.Value });
            return Ok(new { success = true, orderId = result.OrderId, redirectUrl });
        }

        [HttpPost]
        public async Task<IActionResult> RequestRevision([FromBody] RequestRevisionVm vm, CancellationToken ct)
        {
            var ok = await _svc.RequestRevisionAsync(vm, ct);
            return ok ? Ok() : BadRequest("Không gửi yêu cầu chỉnh giá được.");
        }

        public class CreateFeedbackDto
        {
            public Guid QuotationId { get; set; }
            public string? Comment { get; set; }
            public int Rating { get; set; }
        }

        [HttpPost]
        public async Task<IActionResult> Feedback([FromBody] CreateFeedbackDto data)
        {
            var quotationInfo = await _svc.GetByIdAsync(data.QuotationId, CancellationToken.None);
            if (quotationInfo == null) return BadRequest("Thông tin báo giá bị thiếu. Tạo thất bại");

            if (quotationInfo.StoreId == null) return BadRequest("Thông tin báo giá tạm thời đã hết hạn. Không thể đánh giá cho cửa hàng này");

            var feedback = new Feedback
            {
                Id = Guid.NewGuid(),
                FromUserId = quotationInfo.CustomerId,
                ToStoreId = (Guid)quotationInfo.StoreId,
                Rating = data.Rating,
                Comment = data.Comment,
                CreatedAt = DateTime.Now
            };

            var rs = await _feedbackService.CreateFeedbackAsync(feedback);
            return rs != null ? Ok() : BadRequest("Không gửi được đánh giá");
        }
    }
}