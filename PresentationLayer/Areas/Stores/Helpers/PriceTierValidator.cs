using DataAccessLayer.Entities;

namespace PresentationLayer.Areas.Stores.Helpers
{
    public class PriceTierValidator
    {
        public static IEnumerable<string> ValidateNewTier(ServicePrice newTier, IEnumerable<ServicePrice> existing)
        {
            var errors = new List<string>();

            // Chuẩn hóa ngày về Date (00:00), để tránh lệch giờ
            newTier.ValidFrom = newTier.ValidFrom.Date;
            newTier.ValidTo = newTier.ValidTo.Date;

            // Kiểm tra cơ bản
            if (newTier.ValidTo < newTier.ValidFrom)
                errors.Add("Đến ngày phải ≥ Từ ngày.");

            if (newTier.MinQty.HasValue && newTier.MinQty < 0) errors.Add("MinQty không hợp lệ.");
            if (newTier.MaxQty.HasValue && newTier.MaxQty < 0) errors.Add("MaxQty không hợp lệ.");
            if (newTier.MinQty.HasValue && newTier.MaxQty.HasValue && newTier.MinQty > newTier.MaxQty)
                errors.Add("MinQty phải ≤ MaxQty.");

            // Nếu có lỗi cơ bản, khỏi check tiếp
            if (errors.Count > 0) return errors;

            bool DateOverlap(ServicePrice a, ServicePrice b)
            {
                var aTo = DateTime.MaxValue;
                var bTo = DateTime.MaxValue;
                return a.ValidFrom <= bTo && b.ValidFrom <= aTo;
            }

            bool QtyOverlap(ServicePrice a, ServicePrice b)
            {
                // Null = open
                int aMin = a.MinQty ?? int.MinValue;
                int aMax = a.MaxQty ?? int.MaxValue;
                int bMin = b.MinQty ?? int.MinValue;
                int bMax = b.MaxQty ?? int.MaxValue;
                return aMin <= bMax && bMin <= aMax;
            }

            foreach (var ex in existing)
            {
                // so cùng service
                if (ex.ServiceId != newTier.ServiceId) continue;

                if (DateOverlap(ex, newTier) && QtyOverlap(ex, newTier))
                {
                    static string DateToText(DateTime? d) => d?.ToString("yyyy-MM-dd") ?? "∞";
                    static string QtyToText(int? q, bool isMin) => q?.ToString() ?? (isMin ? "-∞" : "∞");
                    errors.Add(
                        $"Chồng chéo với tier {ex.ValidFrom:yyyy-MM-dd} → {DateToText(ex.ValidTo)}, " +
                        $"qty [{QtyToText(ex.MinQty, true)}..{QtyToText(ex.MaxQty, false)}] ."
                    );
                }
            }

            return errors;
        }


    }
}
