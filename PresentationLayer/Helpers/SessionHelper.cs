using System.Text.Json;

namespace PresentationLayer.Helpers
{
    public static class SessionHelper
    {
        private static readonly JsonSerializerOptions _json = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false
        };

        public static void SetObject<T>(this ISession session, string key, T value)
            => session.SetString(key, JsonSerializer.Serialize(value, _json));

        public static T? GetObject<T>(this ISession session, string key)
            => session.GetString(key) is { } s ? JsonSerializer.Deserialize<T>(s, _json) : default;
    }
}
