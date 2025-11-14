using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServiceLayer.Abstractions.IServices
{
    public class VolumeCalculationResult
    {
        public decimal RequiredVolumeM3 { get; set; }
        public decimal RequiredAreaM2 { get; set; }
        public string? AnalysisDetails { get; set; }
        public List<ItemEstimate> ItemEstimates { get; set; } = new();
    }

    public class ItemEstimate
    {
        public string Name { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal EstimatedVolumeM3 { get; set; }
        public string? Notes { get; set; }
    }

    public interface IGeminiService
    {
        /// <summary>
        /// Phân tích nhiều ảnh cùng lúc để đọc thông tin và tính toán thể tích và diện tích cần thiết
        /// </summary>
        /// <param name="imageUrls">Danh sách URL ảnh sản phẩm</param>
        /// <returns>Kết quả tính toán thể tích và diện tích từ tất cả các ảnh</returns>
        Task<VolumeCalculationResult> AnalyzeMultipleImagesAndCalculateVolumeAsync(List<string> imageUrls);

        /// <summary>
        /// Tính toán thể tích và diện tích kho cần thiết từ danh sách đồ vật
        /// </summary>
        /// <param name="items">Danh sách đồ vật với tên, danh mục và số lượng</param>
        /// <returns>Kết quả tính toán thể tích (m³) và diện tích (m²) tối ưu</returns>
        Task<VolumeCalculationResult> CalculateStorageRequirementsAsync(List<ItemInfo> items);

        /// <summary>
        /// Phân tích ảnh trực tiếp từ file để nhận diện đồ vật và số lượng (tối ưu tốc độ - không upload Cloudinary)
        /// </summary>
        /// <param name="imageFiles">Danh sách file ảnh</param>
        /// <returns>Danh sách đồ vật được phát hiện</returns>
        Task<List<ItemInfo>> DetectItemsFromImagesAsync(List<IFormFile> imageFiles);
    }

    public class ItemInfo
    {
        public string Name { get; set; } = string.Empty;
        public string? Category { get; set; }
        public int Quantity { get; set; }
        public decimal? EstimatedWeightKg { get; set; }
    }
}