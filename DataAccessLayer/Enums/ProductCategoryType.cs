// File: DataAccessLayer/Enums/ProductCategoryType.cs
namespace DataAccessLayer.Enums
{
    public enum ProductCategoryType
    {
        Food = 1,           // Thức ăn
        Household = 2,      // Đồ gia dụng
        Fragile = 3,        // Đồ dễ vỡ
        Electronics = 4,    // Đồ điện tử
        Clothing = 5,       // Quần áo
        Books = 6,          // Sách vở
        Furniture = 7,      // Nội thất
        Cosmetics = 8,      // Mỹ phẩm
        Medicine = 9,       // Dược phẩm
        Others = 10         // Khác
    }

    public static class ProductCategoryHelper
    {
        public static string GetDisplayName(this ProductCategoryType category)
        {
            return category switch
            {
                ProductCategoryType.Food => "Thức ăn",
                ProductCategoryType.Household => "Đồ gia dụng",
                ProductCategoryType.Fragile => "Đồ dễ vỡ",
                ProductCategoryType.Electronics => "Đồ điện tử",
                ProductCategoryType.Clothing => "Quần áo",
                ProductCategoryType.Books => "Sách vở",
                ProductCategoryType.Furniture => "Nội thất",
                ProductCategoryType.Cosmetics => "Mỹ phẩm",
                ProductCategoryType.Medicine => "Dược phẩm",
                ProductCategoryType.Others => "Khác",
                _ => "Không xác định"
            };
        }

        public static string GetIcon(this ProductCategoryType category)
        {
            return category switch
            {
                ProductCategoryType.Food => "🍔",
                ProductCategoryType.Household => "🏠",
                ProductCategoryType.Fragile => "⚠️",
                ProductCategoryType.Electronics => "📱",
                ProductCategoryType.Clothing => "👕",
                ProductCategoryType.Books => "📚",
                ProductCategoryType.Furniture => "🛋️",
                ProductCategoryType.Cosmetics => "💄",
                ProductCategoryType.Medicine => "💊",
                ProductCategoryType.Others => "📦",
                _ => "❓"
            };
        }

        public static string GetDescription(this ProductCategoryType category)
        {
            return category switch
            {
                ProductCategoryType.Food => "Thực phẩm, đồ ăn nhanh, đồ uống",
                ProductCategoryType.Household => "Đồ dùng hàng ngày trong gia đình",
                ProductCategoryType.Fragile => "Đồ dễ vỡ, cần cẩn thận khi vận chuyển",
                ProductCategoryType.Electronics => "Thiết bị điện tử, máy móc",
                ProductCategoryType.Clothing => "Quần áo, giày dép, phụ kiện thời trang",
                ProductCategoryType.Books => "Sách, tài liệu, văn phòng phẩm",
                ProductCategoryType.Furniture => "Đồ nội thất, bàn ghế",
                ProductCategoryType.Cosmetics => "Mỹ phẩm, chăm sóc cá nhân",
                ProductCategoryType.Medicine => "Thuốc, dược phẩm, thiết bị y tế",
                ProductCategoryType.Others => "Các loại hàng hóa khác",
                _ => ""
            };
        }
    }
}