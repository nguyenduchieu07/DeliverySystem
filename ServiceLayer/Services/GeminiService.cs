using DataAccessLayer.Configs;
using Microsoft.Extensions.Options;
using ServiceLayer.Abstractions.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
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
            _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        public async Task<VolumeCalculationResult> AnalyzeMultipleImagesAndCalculateVolumeAsync(List<string> imageUrls)
        {
            if (imageUrls == null || imageUrls.Count == 0)
            {
                throw new ArgumentException("At least one image URL is required", nameof(imageUrls));
            }

            var prompt = BuildPromptForMultipleImages(imageUrls.Count);

            // Tải tất cả ảnh và convert sang base64
            var imageParts = new List<object> { new { text = prompt } };

            foreach (var imageUrl in imageUrls)
            {
                if (string.IsNullOrEmpty(imageUrl)) continue;

                var imageBytes = await DownloadImageAsync(imageUrl);
                var base64Image = Convert.ToBase64String(imageBytes);

                // Xác định MIME type dựa trên extension
                var mimeType = "image/jpeg";
                if (imageUrl.Contains(".png", StringComparison.OrdinalIgnoreCase))
                    mimeType = "image/png";
                else if (imageUrl.Contains(".webp", StringComparison.OrdinalIgnoreCase))
                    mimeType = "image/webp";

                imageParts.Add(new
                {
                    inline_data = new
                    {
                        mime_type = mimeType,
                        data = base64Image
                    }
                });
            }

            return await CallGeminiApiAsync(imageParts);
        }

        public async Task<VolumeCalculationResult> AnalyzeItemsAndCalculateVolumeAsync(List<ItemInfo> items)
        {
            if (items == null || items.Count == 0)
            {
                throw new ArgumentException("At least one item is required", nameof(items));
            }

            var prompt = BuildPromptForItems(items);
            var parts = new List<object> { new { text = prompt } };

            return await CallGeminiApiAsync(parts);
        }

        private async Task<VolumeCalculationResult> CallGeminiApiAsync(List<object> parts)
        {
            var requestBody = new
            {
                contents = new[] { new { parts } },
                generationConfig = new
                {
                    temperature = 0.4,
                    topK = 20,
                    topP = 1,
                    maxOutputTokens = 16384,
                },
                safetySettings = new[]
                {
                    new { category = "HARM_CATEGORY_HARASSMENT", threshold = "BLOCK_NONE" },
                    new { category = "HARM_CATEGORY_HATE_SPEECH", threshold = "BLOCK_NONE" },
                    new { category = "HARM_CATEGORY_SEXUALLY_EXPLICIT", threshold = "BLOCK_NONE" },
                    new { category = "HARM_CATEGORY_DANGEROUS_CONTENT", threshold = "BLOCK_NONE" }
                }
            };

            var baseUrl = _config.BaseUrl.TrimEnd('/');
            var endpoint = $"{baseUrl}/models/{_config.ModelName}:generateContent?key={_config.ApiKey}";

            var request = new HttpRequestMessage(HttpMethod.Post, endpoint)
            {
                Content = JsonContent.Create(requestBody)
            };

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                LogError("Gemini API HTTP Error", response.StatusCode, response.ReasonPhrase, endpoint, errorContent);
                throw new HttpRequestException($"Gemini API returned {response.StatusCode}: {response.ReasonPhrase}. Response: {errorContent}");
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var geminiResponse = DeserializeGeminiResponse(responseContent);
            var textResponse = ExtractTextFromResponse(geminiResponse);

            try
            {
                return ParseGeminiResponse(textResponse);
            }
            catch (Exception parseEx)
            {
                Console.WriteLine($"❌ Failed to parse Gemini response: {parseEx.Message}");
                Console.WriteLine($"Full response length: {textResponse.Length} chars");
                Console.WriteLine($"Full response: {textResponse}");
                throw;
            }
        }

        private string BuildPromptForMultipleImages(int imageCount)
        {
            var sb = new StringBuilder();
            sb.AppendLine("Bạn là chuyên gia tính toán không gian kho hàng. Hãy phân tích TẤT CẢ các hình ảnh được cung cấp để đọc thông tin và tính toán:");
            sb.AppendLine();
            sb.AppendLine($"Bạn đang xem {imageCount} ảnh. Hãy phân tích TẤT CẢ các đồ vật trong TẤT CẢ các ảnh này.");
            sb.AppendLine();
            sb.AppendLine("**YÊU CẦU:**");
            sb.AppendLine("1. Liệt kê TẤT CẢ đồ vật trong TẤT CẢ các ảnh (tên, số lượng) và ƯỚC TÍNH kích thước (DxRxC, mét).");
            sb.AppendLine("2. Tính toán thể tích vật lý chiếm chỗ TỐI ƯU NHẤT (xếp chồng/lồng ghép/tháo rời) cho TẤT CẢ các đồ vật.");
            sb.AppendLine("3. Tính diện tích sàn TỐI THIỂU cần thiết (m²), bao gồm khoảng trống.");
            sb.AppendLine("4. Tính thể tích ô kho cần thiết (m³) với chiều cao trần đề xuất 2.5m.");
            sb.AppendLine();
            sb.AppendLine("**Lưu ý:** Nếu cùng một loại đồ vật xuất hiện trong nhiều ảnh, hãy cộng dồn số lượng lại.");
            sb.AppendLine();
            sb.AppendLine("**Trả về JSON với format sau (CHỈ TRẢ VỀ JSON, KHÔNG CÓ TEXT KHÁC):**");
            sb.AppendLine(@"{
  ""requiredVolumeM3"": <số thực>,
  ""requiredAreaM2"": <số thực>,
  ""analysisDetails"": ""<mô tả chi tiết: liệt kê đồ vật từ TẤT CẢ các ảnh, cách xếp gọn nhất, tối đa 200 từ>"",
  ""itemEstimates"": [
    {
      ""name"": ""<tên đồ vật>"",
      ""quantity"": <tổng số lượng từ TẤT CẢ các ảnh>,
      ""estimatedVolumeM3"": <thể tích ước tính cho món này, m³>,
      ""notes"": ""<ghi chú kích thước và cách xếp, tối đa 50 từ>""
    }
  ]
}");
            sb.AppendLine("- analysisDetails tối đa 200 từ, notes tối đa 50 từ để response ngắn gọn.");

            return sb.ToString();
        }

        private string BuildPromptForItems(List<ItemInfo> items)
        {
            var sb = new StringBuilder();
            sb.AppendLine("Bạn là chuyên gia tính toán không gian kho hàng. Hãy phân tích danh sách đồ dùng sau để tính toán thể tích và diện tích cần thiết:");
            sb.AppendLine();
            sb.AppendLine("**DANH SÁCH ĐỒ DÙNG:**");
            sb.AppendLine();

            for (int i = 0; i < items.Count; i++)
            {
                var item = items[i];
                sb.AppendLine($"{i + 1}. **{item.Name}**");
                if (!string.IsNullOrWhiteSpace(item.Category))
                    sb.AppendLine($"   - Danh mục: {item.Category}");
                sb.AppendLine($"   - Số lượng: {item.Quantity}");
                sb.AppendLine();
            }

            sb.AppendLine("**YÊU CẦU:**");
            sb.AppendLine("1. Dựa trên tên đồ vật và danh mục, ƯỚC TÍNH kích thước (Dài x Rộng x Cao, mét) cho từng loại đồ vật.");
            sb.AppendLine("2. Tính toán thể tích vật lý chiếm chỗ TỐI ƯU NHẤT (xếp chồng/lồng ghép/tháo rời) cho TẤT CẢ các đồ vật.");
            sb.AppendLine("3. Tính diện tích sàn TỐI THIỂU cần thiết (m²), bao gồm khoảng trống giữa các đồ vật.");
            sb.AppendLine("4. Tính thể tích ô kho cần thiết (m³) với chiều cao trần đề xuất 2.5m.");
            sb.AppendLine();
            sb.AppendLine("**Lưu ý:**");
            sb.AppendLine("- Nếu cùng một loại đồ vật có số lượng > 1, hãy tính toán cách xếp tối ưu (chồng lên nhau, xếp cạnh nhau, v.v.)");
            sb.AppendLine("- Ước tính kích thước dựa trên kiến thức thông thường về loại đồ vật đó");
            sb.AppendLine("- Tính toán bao gồm cả khoảng trống cần thiết để di chuyển và bảo quản");
            sb.AppendLine();
            sb.AppendLine("**Trả về JSON với format sau (CHỈ TRẢ VỀ JSON, KHÔNG CÓ TEXT KHÁC):**");
            sb.AppendLine(@"{
  ""requiredVolumeM3"": <số thực>,
  ""requiredAreaM2"": <số thực>,
  ""analysisDetails"": ""<mô tả chi tiết: cách ước tính kích thước, cách xếp gọn nhất, tối đa 200 từ>"",
  ""itemEstimates"": [
    {
      ""name"": ""<tên đồ vật>"",
      ""quantity"": <số lượng>,
      ""estimatedVolumeM3"": <thể tích ước tính cho món này, m³>,
      ""notes"": ""<ghi chú kích thước ước tính và cách xếp, tối đa 50 từ>""
    }
  ]
}");
            sb.AppendLine("- analysisDetails tối đa 200 từ, notes tối đa 50 từ để response ngắn gọn.");

            return sb.ToString();
        }

        private async Task<byte[]> DownloadImageAsync(string imageUrl)
        {
            return await _httpClient.GetByteArrayAsync(imageUrl);
        }

        private GeminiResponse DeserializeGeminiResponse(string responseContent)
        {
            try
            {
                var geminiResponse = JsonSerializer.Deserialize<GeminiResponse>(responseContent, _jsonOptions);

                if (geminiResponse?.Candidates == null || geminiResponse.Candidates.Count == 0)
                {
                    Console.WriteLine($"❌ Gemini API did not return valid candidates");
                    Console.WriteLine($"Full response: {responseContent}");
                    throw new InvalidOperationException("Gemini API did not return a valid response");
                }

                return geminiResponse;
            }
            catch (JsonException jsonEx)
            {
                Console.WriteLine($"❌ Failed to deserialize Gemini response as JSON: {jsonEx.Message}");
                Console.WriteLine($"Response content: {responseContent}");
                throw new InvalidOperationException($"Failed to deserialize Gemini API response: {jsonEx.Message}", jsonEx);
            }
        }

        private string ExtractTextFromResponse(GeminiResponse geminiResponse)
        {
            var candidate = geminiResponse.Candidates[0];

            if (candidate.Content == null)
            {
                Console.WriteLine("❌ Gemini API response: Content is null");
                Console.WriteLine($"Full Gemini response: {JsonSerializer.Serialize(geminiResponse, new JsonSerializerOptions { WriteIndented = true })}");
                throw new InvalidOperationException($"Gemini API response: Content is null. Finish reason: {candidate.FinishReason ?? "unknown"}");
            }

            if (candidate.Content.Parts == null || candidate.Content.Parts.Count == 0)
            {
                Console.WriteLine("❌ Gemini API response: Parts is null or empty");
                Console.WriteLine($"Full Gemini response: {JsonSerializer.Serialize(geminiResponse, new JsonSerializerOptions { WriteIndented = true })}");
                Console.WriteLine($"Finish reason: {candidate.FinishReason ?? "null"}");

                var blockedRatings = candidate.SafetyRatings?.Where(r => r.Blocked).ToList();
                if (blockedRatings != null && blockedRatings.Count > 0)
                {
                    var blockedCategories = string.Join(", ", blockedRatings.Select(r => r.Category));
                    throw new InvalidOperationException($"Gemini API response was blocked by safety filters: {blockedCategories}. Finish reason: {candidate.FinishReason ?? "unknown"}");
                }

                throw new InvalidOperationException($"Gemini API response does not contain text. Finish reason: {candidate.FinishReason ?? "unknown"}. This might be a temporary API issue or the response exceeded token limits.");
            }

            var textResponse = candidate.Content.Parts[0]?.Text;
            if (string.IsNullOrEmpty(textResponse))
            {
                Console.WriteLine("❌ Gemini API response: Text is null or empty in Parts[0]");
                Console.WriteLine($"Full Gemini response: {JsonSerializer.Serialize(geminiResponse, new JsonSerializerOptions { WriteIndented = true })}");
                Console.WriteLine($"Finish reason: {candidate.FinishReason ?? "null"}");
                throw new InvalidOperationException($"Gemini API response does not contain text. Finish reason: {candidate.FinishReason ?? "unknown"}");
            }

            return textResponse;
        }

        private VolumeCalculationResult ParseGeminiResponse(string textResponse)
        {
            var jsonStart = textResponse.IndexOf('{');
            var jsonEnd = textResponse.LastIndexOf('}');

            if (jsonStart == -1 || jsonEnd == -1 || jsonEnd <= jsonStart)
            {
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

                if (string.IsNullOrEmpty(result.AnalysisDetails))
                {
                    result.AnalysisDetails = textResponse;
                }

                return result;
            }
            catch (JsonException)
            {
                // Fallback: Xử lý JSON bị cắt ngang
                var fixedJson = FixTruncatedJson(jsonText);
                var result = JsonSerializer.Deserialize<VolumeCalculationResult>(fixedJson, _jsonOptions);

                if (result == null)
                {
                    throw new InvalidOperationException("Failed to deserialize Gemini response after fix");
                }

                if (string.IsNullOrEmpty(result.AnalysisDetails))
                {
                    result.AnalysisDetails = textResponse;
                }

                return result;
            }
        }

        private string FixTruncatedJson(string jsonText)
        {
            var fixedJson = jsonText;

            var openBraces = fixedJson.Count(c => c == '{');
            var closeBraces = fixedJson.Count(c => c == '}');
            var missingBraces = openBraces - closeBraces;

            var openBrackets = fixedJson.Count(c => c == '[');
            var closeBrackets = fixedJson.Count(c => c == ']');
            var missingBrackets = openBrackets - closeBrackets;

            // Loại bỏ trailing comma
            fixedJson = Regex.Replace(fixedJson, @",\s*([}\]])", "$1");

            // Thêm dấu đóng ngoặc thiếu
            for (int i = 0; i < missingBrackets; i++)
                fixedJson += "]";

            for (int i = 0; i < missingBraces; i++)
                fixedJson += "}";

            return fixedJson;
        }

        private void LogError(string message, System.Net.HttpStatusCode statusCode, string? reasonPhrase, string endpoint, string errorContent)
        {
            Console.WriteLine($"❌ {message}: {statusCode} {reasonPhrase}");
            Console.WriteLine($"Request URL: {endpoint}");
            Console.WriteLine($"Error response: {errorContent}");

            if (!string.IsNullOrEmpty(errorContent))
            {
                try
                {
                    var errorJson = JsonSerializer.Deserialize<JsonElement>(errorContent);
                    if (errorJson.TryGetProperty("error", out var error))
                    {
                        if (error.TryGetProperty("message", out var msg))
                            Console.WriteLine($"Error message: {msg.GetString()}");
                        if (error.TryGetProperty("status", out var status))
                            Console.WriteLine($"Error status: {status.GetString()}");
                    }
                }
                catch { }
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
            public string? FinishReason { get; set; }
            public List<SafetyRating>? SafetyRatings { get; set; }
        }

        private class SafetyRating
        {
            public string? Category { get; set; }
            public string? Probability { get; set; }
            public bool Blocked { get; set; }
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