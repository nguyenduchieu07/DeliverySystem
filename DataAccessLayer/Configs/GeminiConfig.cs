namespace DataAccessLayer.Configs
{
    public class GeminiConfig
    {
        public string ApiKey { get; set; } = string.Empty;
        
        // ✅ Dùng model mới nhất - Gemini 2.5 Flash (nhanh, mạnh, hỗ trợ ảnh tốt)
        public string ModelName { get; set; } = "gemini-2.5-flash";
        
        // Hoặc các lựa chọn khác:
        // "gemini-2.5-pro" - Mạnh nhất nhưng chậm hơn
        // "gemini-2.5-flash-lite" - Nhẹ nhất, nhanh nhất
        // "gemini-2.0-flash" - Phiên bản cũ hơn
        
        // ✅ Dùng v1 (không phải v1beta)
        public string BaseUrl { get; set; } = "https://generativelanguage.googleapis.com/v1";
        
        // Retry settings để xử lý lỗi 503 và các lỗi tạm thời
        public int MaxRetryAttempts { get; set; } = 3;
        public int InitialRetryDelayMs { get; set; } = 1000; // 1 giây
        public int MaxRetryDelayMs { get; set; } = 10000; // 10 giây
        public int RequestTimeoutSeconds { get; set; } = 60; // 60 giây
    }
}

