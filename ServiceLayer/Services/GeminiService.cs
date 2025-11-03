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
            // Không set BaseAddress để có thể dùng absolute URL hoặc relative URL tùy vào endpoint
            _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        public async Task<VolumeCalculationResult> AnalyzeImageAndCalculateVolumeAsync(string imageUrl)
        {
            if (string.IsNullOrEmpty(imageUrl))
            {
                throw new ArgumentException("Image URL is required", nameof(imageUrl));
            }

            // Tạo prompt cho Gemini - chỉ đọc ảnh
            var prompt = BuildPrompt();

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
                    maxOutputTokens = 16384, // Tăng giới hạn output token để tránh cắt ngang response
                },
                safetySettings = new[]
                {
                    new { category = "HARM_CATEGORY_HARASSMENT", threshold = "BLOCK_NONE" },
                    new { category = "HARM_CATEGORY_HATE_SPEECH", threshold = "BLOCK_NONE" },
                    new { category = "HARM_CATEGORY_SEXUALLY_EXPLICIT", threshold = "BLOCK_NONE" },
                    new { category = "HARM_CATEGORY_DANGEROUS_CONTENT", threshold = "BLOCK_NONE" }
                }
            };

            // Tạo full URL endpoint với absolute URL - đảm bảo format đúng
            var baseUrl = _config.BaseUrl.TrimEnd('/');
            var endpoint = $"{baseUrl}/models/{_config.ModelName}:generateContent?key={_config.ApiKey}";
            
            // Dùng absolute URL
            var request = new HttpRequestMessage(HttpMethod.Post, endpoint)
            {
                Content = JsonContent.Create(requestBody)
            };

            var response = await _httpClient.SendAsync(request);
            
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"❌ Gemini API HTTP Error: {response.StatusCode} {response.ReasonPhrase}");
                Console.WriteLine($"Request URL: {endpoint}");
                Console.WriteLine($"Error response: {errorContent}");
                
                // Thử parse error response để lấy thông tin chi tiết
                if (!string.IsNullOrEmpty(errorContent))
                {
                    try
                    {
                        var errorJson = JsonSerializer.Deserialize<JsonElement>(errorContent);
                        if (errorJson.TryGetProperty("error", out var error))
                        {
                            if (error.TryGetProperty("message", out var message))
                            {
                                Console.WriteLine($"Error message: {message.GetString()}");
                            }
                            if (error.TryGetProperty("status", out var status))
                            {
                                Console.WriteLine($"Error status: {status.GetString()}");
                            }
                        }
                    }
                    catch { }
                }
                
                throw new HttpRequestException($"Gemini API returned {response.StatusCode}: {response.ReasonPhrase}. Response: {errorContent}");
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            
            GeminiResponse? geminiResponse;
            try
            {
                geminiResponse = JsonSerializer.Deserialize<GeminiResponse>(responseContent, _jsonOptions);
            }
            catch (JsonException jsonEx)
            {
                Console.WriteLine($"❌ Failed to deserialize Gemini response as JSON: {jsonEx.Message}");
                Console.WriteLine($"Response content: {responseContent}");
                throw new InvalidOperationException($"Failed to deserialize Gemini API response: {jsonEx.Message}", jsonEx);
            }

            if (geminiResponse?.Candidates == null || geminiResponse.Candidates.Count == 0)
            {
                Console.WriteLine($"❌ Gemini API did not return valid candidates");
                Console.WriteLine($"Full response: {responseContent}");
                throw new InvalidOperationException("Gemini API did not return a valid response");
            }

            var candidate = geminiResponse.Candidates[0];

            // Kiểm tra xem Content và Parts có tồn tại không
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
                
                // Kiểm tra xem có phải bị block bởi safety filter không
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

            // Parse response từ Gemini (JSON format)
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

        private string BuildPrompt()
        {
            var sb = new StringBuilder();
            sb.AppendLine("Bạn là chuyên gia tính toán không gian kho hàng. Hãy phân tích hình ảnh để đọc thông tin và tính toán:");
            sb.AppendLine();
            sb.AppendLine("**Yêu cầu:**");
            sb.AppendLine("1. Quan sát kỹ hình ảnh và liệt kê TẤT CẢ các đồ vật có trong ảnh (tên đồ vật, số lượng).");
            sb.AppendLine("2. Ước tính kích thước của từng loại đồ vật (Dài x Rộng x Cao, đơn vị mét) dựa trên hình ảnh.");
            sb.AppendLine("3. Tính toán thể tích vật lý chiếm chỗ khi xếp gọn TỐI ƯU NHẤT (có thể xếp chồng, tháo rời các bộ phận, lồng ghép để tiết kiệm không gian).");
            sb.AppendLine("4. Tính diện tích sàn tối thiểu cần thiết (m²) khi xếp gọn nhất, bao gồm cả khoảng trống cần thiết.");
            sb.AppendLine("5. Tính thể tích ô kho cần thiết (m³) với chiều cao trần kho đề xuất khoảng 2.5m.");
            sb.AppendLine();
            sb.AppendLine("**Trả về JSON với format sau (CHỈ TRẢ VỀ JSON, KHÔNG CÓ TEXT KHÁC):**");
            sb.AppendLine("{");
            sb.AppendLine("  \"requiredVolumeM3\": <số thực>, // Thể tích ô kho cần thiết khi xếp gọn nhất (m³)");
            sb.AppendLine("  \"requiredAreaM2\": <số thực>, // Diện tích sàn tối thiểu khi xếp gọn nhất (m²)");
            sb.AppendLine("  \"analysisDetails\": \"<mô tả chi tiết: liệt kê các đồ vật trong ảnh, cách xếp gọn nhất, tối đa 200 từ>\",");
            sb.AppendLine("  \"itemEstimates\": [");
            sb.AppendLine("    {");
            sb.AppendLine("      \"name\": \"<tên đồ vật trong ảnh>\",");
            sb.AppendLine("      \"quantity\": <số lượng trong ảnh>,");
            sb.AppendLine("      \"estimatedVolumeM3\": <thể tích ước tính cho món này, m³>,");
            sb.AppendLine("      \"notes\": \"<ghi chú về kích thước và cách xếp, tối đa 50 từ>\",");
            sb.AppendLine("    }");
            sb.AppendLine("  ]");
            sb.AppendLine("}");
            sb.AppendLine();
            sb.AppendLine("**Lưu ý quan trọng:**");
            sb.AppendLine("- Chỉ đọc thông tin từ hình ảnh, KHÔNG sử dụng thông tin từ bên ngoài.");
            sb.AppendLine("- Liệt kê đầy đủ các đồ vật có trong ảnh.");
            sb.AppendLine("- Tính toán với giả định xếp gọn TỐI ƯU NHẤT (xếp chồng, lồng ghép, tháo rời nếu có thể).");
            sb.AppendLine("- requiredVolumeM3 và requiredAreaM2 phải là giá trị gọn nhất có thể.");
            sb.AppendLine("- analysisDetails tối đa 200 từ, notes tối đa 50 từ để response ngắn gọn.");

            return sb.ToString();
        }

        private async Task<byte[]> DownloadImageAsync(string imageUrl)
        {
            using var httpClient = new HttpClient();
            return await httpClient.GetByteArrayAsync(imageUrl);
        }

        private VolumeCalculationResult ParseGeminiResponse(string textResponse)
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
            
            // Thử parse JSON bình thường trước
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
                // Fallback: Xử lý JSON bị cắt ngang hoặc không hợp lệ
                try
                {
                    // Thử sửa JSON bị cắt ngang bằng cách thêm dấu đóng ngoặc thiếu
                    var fixedJson = FixTruncatedJson(jsonText);
                    
                    var result = JsonSerializer.Deserialize<VolumeCalculationResult>(fixedJson, _jsonOptions);
                    if (result == null)
                    {
                        throw new InvalidOperationException("Failed to deserialize Gemini response after fix");
                    }
                    
                    // Đảm bảo có analysisDetails nếu không có
                    if (string.IsNullOrEmpty(result.AnalysisDetails))
                    {
                        result.AnalysisDetails = textResponse;
                    }
                    
                    return result;
                }
                catch (Exception fixEx)
                {
                    Console.WriteLine($"❌ Failed to parse Gemini JSON response: {ex.Message}");
                    throw new InvalidOperationException($"Failed to parse Gemini JSON response: {ex.Message}. Raw response: {jsonText}", ex);
                }
            }
        }

        private string FixTruncatedJson(string jsonText)
        {
            // Thử sửa JSON bị cắt ngang bằng cách:
            // 1. Đếm số dấu ngoặc mở và đóng
            // 2. Thêm dấu đóng ngoặc thiếu
            // 3. Loại bỏ trailing comma
            // 4. Đóng các string chưa được đóng
            
            var fixedJson = jsonText;
            
            // Đếm ngoặc nhọn
            var openBraces = fixedJson.Count(c => c == '{');
            var closeBraces = fixedJson.Count(c => c == '}');
            var missingBraces = openBraces - closeBraces;
            
            // Đếm ngoặc vuông
            var openBrackets = fixedJson.Count(c => c == '[');
            var closeBrackets = fixedJson.Count(c => c == ']');
            var missingBrackets = openBrackets - closeBrackets;
            
            // Loại bỏ trailing comma trước khi đóng
            fixedJson = Regex.Replace(fixedJson, @",\s*([}\]])", "$1");
            
            // Thêm dấu đóng ngoặc thiếu
            for (int i = 0; i < missingBrackets; i++)
            {
                fixedJson += "]";
            }
            for (int i = 0; i < missingBraces; i++)
            {
                fixedJson += "}";
            }
            
            return fixedJson;
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

