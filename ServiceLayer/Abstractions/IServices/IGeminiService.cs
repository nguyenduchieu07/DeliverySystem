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
        /// Phân tích ảnh và dữ liệu mẫu để tính toán thể tích và diện tích cần thiết
        /// </summary>
        /// <param name="imageUrl">URL ảnh sản phẩm</param>
        /// <param name="items">Danh sách items với tên, số lượng, trọng lượng</param>
        /// <returns>Kết quả tính toán thể tích và diện tích</returns>
        Task<VolumeCalculationResult> AnalyzeImageAndCalculateVolumeAsync(string imageUrl, List<ItemInfo> items);
    }

    public class ItemInfo
    {
        public string Name { get; set; } = string.Empty;
        public string? Category { get; set; }
        public int Quantity { get; set; }
        public decimal? EstimatedWeightKg { get; set; }
    }
}

