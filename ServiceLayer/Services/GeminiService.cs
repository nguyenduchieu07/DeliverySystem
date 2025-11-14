using DataAccessLayer.Configs;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using ServiceLayer.Abstractions.IServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading;
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

        public async Task<VolumeCalculationResult> CalculateStorageRequirementsAsync(List<ItemInfo> items)
        {
            if (items == null || items.Count == 0)
            {
                throw new ArgumentException("At least one item is required", nameof(items));
            }

            var prompt = BuildPromptForItems(items);
            var parts = new List<object> { new { text = prompt } };

            return await CallGeminiApiAsync(parts);
        }

        public async Task<List<ItemInfo>> DetectItemsFromImagesAsync(List<IFormFile> imageFiles)
        {
            if (imageFiles == null || imageFiles.Count == 0)
            {
                throw new ArgumentException("At least one image file is required", nameof(imageFiles));
            }

            // Convert trực tiếp từ IFormFile sang base64 (KHÔNG upload Cloudinary)
            var imageParts = new List<object>();
            var prompt = BuildPromptForItemDetection(imageFiles.Count);
            imageParts.Add(new { text = prompt });

            foreach (var file in imageFiles)
            {
                if (file == null || file.Length == 0) continue;

                // Đọc file trực tiếp vào memory stream
                using var memoryStream = new MemoryStream();
                await file.CopyToAsync(memoryStream);
                var imageBytes = memoryStream.ToArray();
                var base64Image = Convert.ToBase64String(imageBytes);

                // Xác định MIME type từ ContentType hoặc extension
                var mimeType = file.ContentType ?? "image/jpeg";
                if (string.IsNullOrEmpty(mimeType) || mimeType == "application/octet-stream")
                {
                    var fileName = file.FileName?.ToLower() ?? "";
                    if (fileName.EndsWith(".png"))
                        mimeType = "image/png";
                    else if (fileName.EndsWith(".webp"))
                        mimeType = "image/webp";
                    else
                        mimeType = "image/jpeg";
                }

                imageParts.Add(new
                {
                    inline_data = new
                    {
                        mime_type = mimeType,
                        data = base64Image
                    }
                });
            }

            // Gọi Gemini với config tối ưu cho tốc độ
            var response = await CallGeminiApiForItemDetectionAsync(imageParts);
            return response;
        }

        private async Task<List<ItemInfo>> CallGeminiApiForItemDetectionAsync(List<object> parts)
        {
            var requestBody = new
            {
                contents = new[] { new { parts } },
                generationConfig = new
                {
                    temperature = 0.1, // Thấp để nhanh và chính xác
                    topK = 10, // Giảm để nhanh hơn
                    topP = 0.8, // Giảm để nhanh hơn
                    maxOutputTokens = 8192, // Tăng từ 2048 lên 8192 để xử lý nhiều items hơn
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

            // Sử dụng retry mechanism
            return await RetryWithExponentialBackoffAsync(async () =>
            {
                var request = new HttpRequestMessage(HttpMethod.Post, endpoint)
                {
                    Content = JsonContent.Create(requestBody)
                };

                var response = await _httpClient.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    LogError("Gemini API HTTP Error", response.StatusCode, response.ReasonPhrase, endpoint, errorContent);
                    
                    // Ném exception với status code để retry logic có thể xử lý
                    throw new HttpRequestException($"Gemini API returned {response.StatusCode}: {response.ReasonPhrase}. Response: {errorContent}")
                    {
                        Data = { ["StatusCode"] = response.StatusCode }
                    };
                }

                var responseContent = await response.Content.ReadAsStringAsync();
                var geminiResponse = DeserializeGeminiResponse(responseContent);
                var textResponse = ExtractTextFromResponse(geminiResponse);

                try
                {
                    return ParseItemDetectionResponse(textResponse);
                }
                catch (Exception parseEx)
                {
                    Console.WriteLine($"❌ Failed to parse Gemini item detection response: {parseEx.Message}");
                    Console.WriteLine($"Full response: {textResponse}");
                    throw;
                }
            });
        }

        private string BuildPromptForItemDetection(int imageCount)
        {
            var sb = new StringBuilder();
            sb.AppendLine("Phân tích ảnh, liệt kê CHỈ đồ vật di chuyển được. Bỏ qua: tường, cửa, nền, trần, đèn, ổ cắm, ống nước.");
            if (imageCount > 1)
                sb.AppendLine($"Có {imageCount} ảnh. Cộng dồn số lượng nếu cùng loại.");
            sb.AppendLine("Yêu cầu: Tên + số lượng. Gộp trùng loại. Danh mục: Nội thất/Điện tử/Quần áo/Thực phẩm/Khác.");
            sb.AppendLine(@"Trả về JSON: {""items"":[{""name"":""..."",""quantity"":N,""category"":""...""}]}");
            return sb.ToString();
        }

        private List<ItemInfo> ParseItemDetectionResponse(string textResponse)
        {
            // Loại bỏ markdown code block nếu có (```json ... ```)
            var cleanedResponse = textResponse;
            if (cleanedResponse.Contains("```json"))
            {
                var markdownStart = cleanedResponse.IndexOf("```json") + 7;
                var markdownEnd = cleanedResponse.LastIndexOf("```");
                if (markdownEnd > markdownStart)
                {
                    cleanedResponse = cleanedResponse.Substring(markdownStart, markdownEnd - markdownStart).Trim();
                }
            }
            else if (cleanedResponse.Contains("```"))
            {
                var markdownStart = cleanedResponse.IndexOf("```") + 3;
                var markdownEnd = cleanedResponse.LastIndexOf("```");
                if (markdownEnd > markdownStart)
                {
                    cleanedResponse = cleanedResponse.Substring(markdownStart, markdownEnd - markdownStart).Trim();
                }
            }

            // Tìm JSON object
            var jsonObjectStart = cleanedResponse.IndexOf('{');
            var jsonObjectEnd = cleanedResponse.LastIndexOf('}');

            if (jsonObjectStart == -1)
            {
                throw new InvalidOperationException($"Cannot find JSON object in response. Response: {textResponse}");
            }

            // Nếu không tìm thấy dấu đóng }, có thể JSON bị cắt ngang (MAX_TOKENS)
            if (jsonObjectEnd == -1 || jsonObjectEnd <= jsonObjectStart)
            {
                Console.WriteLine("⚠️ Warning: JSON response appears to be truncated (missing closing brace). Attempting to fix...");
                // Thử sửa JSON bị cắt ngang
                var truncatedJson = cleanedResponse.Substring(jsonObjectStart);
                var fixedJson = FixTruncatedJson(truncatedJson);
                
                try
                {
                    var items = ParseItemDetectionJson(fixedJson);
                    Console.WriteLine($"✅ Successfully parsed truncated JSON. Found {items.Count} items.");
                    return items;
                }
                catch (Exception fixEx)
                {
                    Console.WriteLine($"❌ Failed to parse truncated JSON even after fix: {fixEx.Message}");
                    throw new InvalidOperationException($"Cannot parse Gemini response as JSON (possibly truncated by MAX_TOKENS). Response: {textResponse.Substring(0, Math.Min(500, textResponse.Length))}...");
                }
            }

            var jsonContent = cleanedResponse.Substring(jsonObjectStart, jsonObjectEnd - jsonObjectStart + 1);

            try
            {
                return ParseItemDetectionJson(jsonContent);
            }
            catch (JsonException ex)
            {
                // Thử sửa JSON bị cắt ngang (có thể do MAX_TOKENS)
                Console.WriteLine($"⚠️ Warning: JSON parsing failed, attempting to fix truncated JSON: {ex.Message}");
                try
                {
                    var fixedJson = FixTruncatedJson(jsonContent);
                    var items = ParseItemDetectionJson(fixedJson);
                    Console.WriteLine($"✅ Successfully parsed JSON after fix. Found {items.Count} items.");
                    return items;
                }
                catch (Exception fixEx)
                {
                    Console.WriteLine($"❌ Failed to parse item detection JSON even after fix: {fixEx.Message}");
                    Console.WriteLine($"JSON text (first 500 chars): {jsonContent.Substring(0, Math.Min(500, jsonContent.Length))}...");
                    throw new InvalidOperationException($"Failed to parse item detection response (possibly truncated by MAX_TOKENS): {ex.Message}", ex);
                }
            }
        }

        private List<ItemInfo> ParseItemDetectionJson(string jsonText)
        {
            using var doc = JsonDocument.Parse(jsonText);
            var root = doc.RootElement;

            var items = new List<ItemInfo>();

            if (root.TryGetProperty("items", out var itemsElement) && itemsElement.ValueKind == JsonValueKind.Array)
            {
                foreach (var itemElement in itemsElement.EnumerateArray())
                {
                    var item = new ItemInfo();

                    if (itemElement.TryGetProperty("name", out var nameElement))
                        item.Name = nameElement.GetString() ?? "";

                    if (itemElement.TryGetProperty("quantity", out var quantityElement))
                        item.Quantity = ParseQuantity(quantityElement);

                    if (itemElement.TryGetProperty("category", out var categoryElement))
                        item.Category = categoryElement.GetString();

                    if (!string.IsNullOrWhiteSpace(item.Name) && item.Quantity > 0)
                    {
                        items.Add(item);
                    }
                }
            }

            return items;
        }

        private int ParseQuantity(JsonElement quantityElement)
        {
            if (quantityElement.ValueKind == JsonValueKind.Number)
            {
                if (quantityElement.TryGetInt32(out var q)) return Math.Max(1, q);
                if (quantityElement.TryGetDouble(out var d)) return Math.Max(1, (int)Math.Round(d));
            }
            else if (quantityElement.ValueKind == JsonValueKind.String)
            {
                var str = quantityElement.GetString();
                if (int.TryParse(str, out var parsed)) return Math.Max(1, parsed);
                if (double.TryParse(str, out var parsedDouble)) return Math.Max(1, (int)Math.Round(parsedDouble));
            }
            return 1;
        }

        private async Task<VolumeCalculationResult> CallGeminiApiAsync(List<object> parts)
        {
            var requestBody = new
            {
                contents = new[] { new { parts } },
                generationConfig = new
                {
                    temperature = 0.2, // Thấp để nhanh và chính xác
                    topK = 10, // Giảm để nhanh hơn
                    topP = 0.85, // Giảm để nhanh hơn
                    maxOutputTokens = 3072, // Giảm từ 16384 xuống 3072 để nhanh hơn, vẫn đủ cho JSON
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

            // Sử dụng retry mechanism
            return await RetryWithExponentialBackoffAsync(async () =>
            {
                var request = new HttpRequestMessage(HttpMethod.Post, endpoint)
                {
                    Content = JsonContent.Create(requestBody)
                };

                var response = await _httpClient.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    LogError("Gemini API HTTP Error", response.StatusCode, response.ReasonPhrase, endpoint, errorContent);
                    
                    // Ném exception với status code để retry logic có thể xử lý
                    throw new HttpRequestException($"Gemini API returned {response.StatusCode}: {response.ReasonPhrase}. Response: {errorContent}")
                    {
                        Data = { ["StatusCode"] = response.StatusCode }
                    };
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
            });
        }

        private string BuildPromptForMultipleImages(int imageCount)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Phân tích {imageCount} ảnh, tính thể tích & diện tích kho tối ưu.");
            sb.AppendLine("Yêu cầu: Liệt kê đồ vật (tên, số lượng). Ước tính kích thước (DxRxC mét). Tính thể tích tối ưu (xếp chồng/lồng ghép). Diện tích sàn tối thiểu (m²). Thể tích kho (m³, trần 2.5m).");
            sb.AppendLine("Cộng dồn số lượng nếu cùng loại trong nhiều ảnh.");
            sb.AppendLine(@"Trả về JSON: {""requiredVolumeM3"":N,""requiredAreaM2"":N,""analysisDetails"":""..."",""itemEstimates"":[{""name"":""..."",""quantity"":N,""estimatedVolumeM3"":N,""notes"":""...""}]}");
            return sb.ToString();
        }

        private string BuildPromptForItems(List<ItemInfo> items)
        {
            var sb = new StringBuilder();
            sb.AppendLine("Tính thể tích & diện tích kho tối ưu cho:");
            foreach (var item in items)
            {
                sb.AppendLine($"- {item.Name} x{item.Quantity}" + (!string.IsNullOrWhiteSpace(item.Category) ? $" ({item.Category})" : ""));
            }
            sb.AppendLine("Yêu cầu: Ước tính kích thước (DxRxC mét). Tính thể tích tối ưu (xếp chồng/lồng ghép). Diện tích sàn tối thiểu (m²). Thể tích kho (m³, trần 2.5m).");
            sb.AppendLine(@"Trả về JSON: {""requiredVolumeM3"":N,""requiredAreaM2"":N,""analysisDetails"":""..."",""itemEstimates"":[{""name"":""..."",""quantity"":N,""estimatedVolumeM3"":N,""notes"":""...""}]}");
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
            if (geminiResponse.Candidates == null || geminiResponse.Candidates.Count == 0)
            {
                throw new InvalidOperationException("Gemini API response has no candidates");
            }
            
            var candidate = geminiResponse.Candidates[0];

            if (candidate.Content == null)
            {
                Console.WriteLine("❌ Gemini API response: Content is null");
                Console.WriteLine($"Full Gemini response: {JsonSerializer.Serialize(geminiResponse, new JsonSerializerOptions { WriteIndented = true })}");
                throw new InvalidOperationException($"Gemini API response: Content is null. Finish reason: {candidate.FinishReason ?? "unknown"}");
            }

            // Kiểm tra Parts trước
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

                // Nếu là MAX_TOKENS và không có Parts, không thể parse được
                if (candidate.FinishReason == "MAX_TOKENS")
                {
                    throw new InvalidOperationException($"Gemini API response exceeded token limit (MAX_TOKENS) and no content was returned. Consider reducing the number of items or increasing maxOutputTokens.");
                }

                throw new InvalidOperationException($"Gemini API response does not contain text. Finish reason: {candidate.FinishReason ?? "unknown"}. This might be a temporary API issue or the response exceeded token limits.");
            }

            var textResponse = candidate.Content.Parts[0]?.Text;
            
            // Nếu finish reason là MAX_TOKENS nhưng vẫn có text, log warning và tiếp tục parse
            if (candidate.FinishReason == "MAX_TOKENS" && !string.IsNullOrEmpty(textResponse))
            {
                Console.WriteLine($"⚠️ Warning: Response was truncated (MAX_TOKENS), but attempting to parse available content. Text length: {textResponse.Length} chars");
                // Tiếp tục parse phần đã có, không throw exception
            }
            
            if (string.IsNullOrEmpty(textResponse))
            {
                Console.WriteLine("❌ Gemini API response: Text is null or empty in Parts[0]");
                Console.WriteLine($"Full Gemini response: {JsonSerializer.Serialize(geminiResponse, new JsonSerializerOptions { WriteIndented = true })}");
                Console.WriteLine($"Finish reason: {candidate.FinishReason ?? "null"}");
                
                // Nếu là MAX_TOKENS nhưng không có text, có thể response bị cắt hoàn toàn
                if (candidate.FinishReason == "MAX_TOKENS")
                {
                    throw new InvalidOperationException($"Gemini API response exceeded token limit (MAX_TOKENS) and no text was returned. The response may be too large. Consider reducing the number of images or items.");
                }
                
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
            var fixedJson = jsonText.Trim();

            // Loại bỏ trailing comma trước khi đếm
            fixedJson = Regex.Replace(fixedJson, @",\s*$", "");

            // Đếm ngoặc
            var openBraces = fixedJson.Count(c => c == '{');
            var closeBraces = fixedJson.Count(c => c == '}');
            var missingBraces = openBraces - closeBraces;

            var openBrackets = fixedJson.Count(c => c == '[');
            var closeBrackets = fixedJson.Count(c => c == ']');
            var missingBrackets = openBrackets - closeBrackets;

            // Nếu đang ở giữa một string, đóng string lại
            var lastQuote = fixedJson.LastIndexOf('"');
            var beforeLastQuote = fixedJson.Substring(0, lastQuote > 0 ? lastQuote : 0);
            var openQuotes = beforeLastQuote.Count(c => c == '"');
            if (openQuotes % 2 == 1 && lastQuote > 0)
            {
                // Đang ở giữa string, đóng string lại
                if (!fixedJson.EndsWith("\""))
                {
                    fixedJson += "\"";
                }
            }

            // Loại bỏ trailing comma một lần nữa sau khi đóng string
            fixedJson = Regex.Replace(fixedJson, @",\s*([}\]])", "$1");

            // Thêm dấu đóng ngoặc thiếu (brackets trước, braces sau)
            for (int i = 0; i < missingBrackets; i++)
                fixedJson += "]";

            for (int i = 0; i < missingBraces; i++)
                fixedJson += "}";

            return fixedJson;
        }

        /// <summary>
        /// Retry mechanism với exponential backoff để xử lý các lỗi tạm thời từ Gemini API
        /// </summary>
        private async Task<T> RetryWithExponentialBackoffAsync<T>(Func<Task<T>> operation)
        {
            int attempt = 0;
            Exception? lastException = null;

            while (attempt < _config.MaxRetryAttempts)
            {
                try
                {
                    return await operation();
                }
                catch (HttpRequestException ex) when (IsRetryableError(ex))
                {
                    lastException = ex;
                    attempt++;

                    if (attempt >= _config.MaxRetryAttempts)
                    {
                        Console.WriteLine($"❌ Max retry attempts ({_config.MaxRetryAttempts}) reached. Giving up.");
                        throw;
                    }

                    // Tính toán delay với exponential backoff
                    var delayMs = Math.Min(
                        _config.InitialRetryDelayMs * (int)Math.Pow(2, attempt - 1),
                        _config.MaxRetryDelayMs
                    );

                    // Thêm jitter (random delay) để tránh thundering herd
                    var jitter = new Random().Next(0, delayMs / 4);
                    var totalDelay = delayMs + jitter;

                    Console.WriteLine($"⚠️ Gemini API error (attempt {attempt}/{_config.MaxRetryAttempts}): {ex.Message}");
                    Console.WriteLine($"⏳ Retrying in {totalDelay}ms...");

                    await Task.Delay(totalDelay);
                }
                catch (TaskCanceledException ex) when (ex.InnerException is TimeoutException)
                {
                    lastException = ex;
                    attempt++;

                    if (attempt >= _config.MaxRetryAttempts)
                    {
                        Console.WriteLine($"❌ Max retry attempts ({_config.MaxRetryAttempts}) reached after timeout. Giving up.");
                        throw new HttpRequestException("Request to Gemini API timed out after multiple retries.", ex);
                    }

                    var delayMs = Math.Min(
                        _config.InitialRetryDelayMs * (int)Math.Pow(2, attempt - 1),
                        _config.MaxRetryDelayMs
                    );
                    var jitter = new Random().Next(0, delayMs / 4);
                    var totalDelay = delayMs + jitter;

                    Console.WriteLine($"⚠️ Gemini API timeout (attempt {attempt}/{_config.MaxRetryAttempts})");
                    Console.WriteLine($"⏳ Retrying in {totalDelay}ms...");

                    await Task.Delay(totalDelay);
                }
                catch (Exception ex)
                {
                    // Không retry cho các lỗi không phải lỗi tạm thời
                    Console.WriteLine($"❌ Non-retryable error: {ex.Message}");
                    throw;
                }
            }

            // Nếu đến đây, có nghĩa là đã hết retry attempts
            throw lastException ?? new InvalidOperationException("Unexpected error in retry mechanism");
        }

        /// <summary>
        /// Kiểm tra xem lỗi có thể retry được không
        /// </summary>
        private bool IsRetryableError(HttpRequestException ex)
        {
            // Kiểm tra status code từ exception data
            if (ex.Data.Contains("StatusCode") && ex.Data["StatusCode"] is HttpStatusCode statusCode)
            {
                // Retry cho các lỗi tạm thời:
                // 429: Too Many Requests (rate limit)
                // 500: Internal Server Error
                // 502: Bad Gateway
                // 503: Service Unavailable (overloaded)
                // 504: Gateway Timeout
                return statusCode == HttpStatusCode.TooManyRequests ||
                       statusCode == HttpStatusCode.InternalServerError ||
                       statusCode == HttpStatusCode.BadGateway ||
                       statusCode == HttpStatusCode.ServiceUnavailable ||
                       statusCode == HttpStatusCode.GatewayTimeout;
            }

            // Nếu không có status code, kiểm tra message
            var message = ex.Message.ToLower();
            return message.Contains("503") ||
                   message.Contains("429") ||
                   message.Contains("500") ||
                   message.Contains("502") ||
                   message.Contains("504") ||
                   message.Contains("overloaded") ||
                   message.Contains("unavailable") ||
                   message.Contains("timeout");
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