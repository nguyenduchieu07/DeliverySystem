using DataAccessLayer.Configs;
using Microsoft.Extensions.Options;
using ServiceLayer.Abstractions.IServices;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ServiceLayer.Services
{
    public class GeminiService : IGeminiService
    {
        private readonly HttpClient _httpClient;
        private readonly GeminiConfig _config;
        private readonly JsonSerializerOptions _jsonOptions;

        public GeminiService(HttpClient httpClient, IOptions<GeminiConfig> config)
        {
            _httpClient = httpClient;
            _config = config.Value;
            _httpClient.BaseAddress = new Uri(_config.BaseUrl);
            _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        public async Task<VolumeCalculationResult> AnalyzeImageAndCalculateVolumeAsync(string imageUrl, List<ItemInfo> items)
        {
            if (string.IsNullOrEmpty(imageUrl))
            {
                throw new ArgumentException("Image URL is required", nameof(imageUrl));
            }

            // Tạo prompt cho Gemini
            var prompt = BuildPrompt(items);

            // Tải ảnh và convert sang base64 (hoặc dùng image URL trực tiếp nếu Gemini hỗ trợ)
            var imageBytes = await DownloadImageAsync(imageUrl);
            var base64Image = Convert.ToBase64String(imageBytes);

            // Gọi Gemini API
            var requestBody = new
            {
                contents = new[]
                {
                    new
                    {
                        parts = new object[]
                        {
                            new { text = prompt },
                            new
                            {
                                inline_data = new
                                {
                                    mime_type = "image/jpeg",
                                    data = base64Image
                                }
                            }
                        }
                    }
                },
                generationConfig = new
                {
                    temperature = 0.4,
                    topK = 32,
                    topP = 1,
                    maxOutputTokens = 2048,
                }
            };

            var request = new HttpRequestMessage(HttpMethod.Post, $"/models/{_config.ModelName}:generateContent?key={_config.ApiKey}")
            {
                Content = JsonContent.Create(requestBody)
            };

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            var geminiResponse = JsonSerializer.Deserialize<GeminiResponse>(responseContent, _jsonOptions);

            if (geminiResponse?.Candidates == null || geminiResponse.Candidates.Count == 0)
            {
                throw new InvalidOperationException("Gemini API did not return a valid response");
            }

            var textResponse = geminiResponse.Candidates[0].Content?.Parts?[0]?.Text;
            if (string.IsNullOrEmpty(textResponse))
            {
                throw new InvalidOperationException("Gemini API response does not contain text");
            }

            // Parse response từ Gemini (JSON format)
            return ParseGeminiResponse(textResponse, items);
        }

        private string BuildPrompt(List<ItemInfo> items)
        {
            var sb = new StringBuilder();
            sb.AppendLine("Bạn là chuyên gia tính toán không gian kho hàng. Hãy phân tích ảnh và danh sách đồ vật dưới đây để tính toán:");
            sb.AppendLine();
            sb.AppendLine("**Danh sách đồ vật:**");
            foreach (var item in items)
            {
                sb.AppendLine($"- {item.Name}: {item.Quantity} cái" + 
                             (item.Category != null ? $" (Loại: {item.Category})" : "") + 
                             (item.EstimatedWeightKg.HasValue ? $", Trọng lượng ước tính: {item.EstimatedWeightKg} kg" : ""));
            }
            sb.AppendLine();
            sb.AppendLine("**Yêu cầu:**");
            sb.AppendLine("1. Phân tích ảnh để xác định kích thước ước tính của từng loại đồ vật (Dài x Rộng x Cao, đơn vị mét).");
            sb.AppendLine("2. Tính toán thể tích vật lý chiếm chỗ khi xếp gọn (có thể xếp chồng, tháo rời các bộ phận).");
            sb.AppendLine("3. Tính diện tích sàn tối thiểu cần thiết (bao gồm lối đi và khoảng trống).");
            sb.AppendLine("4. Tính thể tích ô kho cần thiết (với chiều cao trần kho đề xuất khoảng 2.5m).");
            sb.AppendLine();
            sb.AppendLine("**Trả về JSON với format sau:**");
            sb.AppendLine("{");
            sb.AppendLine("  \"requiredVolumeM3\": <số thực>, // Thể tích ô kho cần thiết (m³)");
            sb.AppendLine("  \"requiredAreaM2\": <số thực>, // Diện tích sàn tối thiểu (m²)");
            sb.AppendLine("  \"analysisDetails\": \"<chi tiết phân tích>\",");
            sb.AppendLine("  \"itemEstimates\": [");
            sb.AppendLine("    {");
            sb.AppendLine("      \"name\": \"<tên đồ vật>\",");
            sb.AppendLine("      \"quantity\": <số lượng>,");
            sb.AppendLine("      \"estimatedVolumeM3\": <thể tích ước tính cho món này, m³>,");
            sb.AppendLine("      \"notes\": \"<ghi chú về cách xếp>\",");
            sb.AppendLine("    }");
            sb.AppendLine("  ]");
            sb.AppendLine("}");
            sb.AppendLine();
            sb.AppendLine("Lưu ý: Khi tính toán, cần xem xét việc xếp chồng, tháo rời các bộ phận để tối ưu không gian. Ví dụ: ghế có thể xếp chồng, bàn có thể tháo mặt bàn khỏi khung.");

            return sb.ToString();
        }

        private async Task<byte[]> DownloadImageAsync(string imageUrl)
        {
            using var httpClient = new HttpClient();
            return await httpClient.GetByteArrayAsync(imageUrl);
        }

        private VolumeCalculationResult ParseGeminiResponse(string textResponse, List<ItemInfo> items)
        {
            // Gemini có thể trả về JSON kèm markdown hoặc chỉ text
            // Tìm JSON block trong response
            var jsonStart = textResponse.IndexOf('{');
            var jsonEnd = textResponse.LastIndexOf('}');
            
            if (jsonStart == -1 || jsonEnd == -1 || jsonEnd <= jsonStart)
            {
                // Nếu không tìm thấy JSON, thử parse toàn bộ response
                throw new InvalidOperationException($"Cannot parse Gemini response as JSON. Response: {textResponse}");
            }

            var jsonText = textResponse.Substring(jsonStart, jsonEnd - jsonStart + 1);
            
            try
            {
                var result = JsonSerializer.Deserialize<VolumeCalculationResult>(jsonText, _jsonOptions);
                if (result == null)
                {
                    throw new InvalidOperationException("Failed to deserialize Gemini response");
                }
                
                // Đảm bảo có analysisDetails nếu không có
                if (string.IsNullOrEmpty(result.AnalysisDetails))
                {
                    result.AnalysisDetails = textResponse;
                }
                
                return result;
            }
            catch (JsonException ex)
            {
                throw new InvalidOperationException($"Failed to parse Gemini JSON response: {ex.Message}. Raw response: {jsonText}", ex);
            }
        }

        // Gemini API response models
        private class GeminiResponse
        {
            public List<Candidate>? Candidates { get; set; }
        }

        private class Candidate
        {
            public Content? Content { get; set; }
        }

        private class Content
        {
            public List<Part>? Parts { get; set; }
        }

        private class Part
        {
            public string? Text { get; set; }
        }
    }
}

