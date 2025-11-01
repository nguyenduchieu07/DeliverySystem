namespace DataAccessLayer.Enums;

public partial class EnumExtensions
{
    public static string ToDisplayStringForWarehouseSlot(this StatusValue status)
    {
        return status switch
        {
            StatusValue.Available => "Trống",
            StatusValue.Reserved => "Giữ chỗ", //ra hợp đông status Draft 
            StatusValue.InUse => "Được sử dụng",
            StatusValue.Maintenance => "Đang bảo trì",
            _ => status.ToString()
        };
    }
    
    public static string ToDisplayStringForWarehouse(this StatusValue status)
    {
        return status switch
        {
            StatusValue.Pending => "Chờ duyệt",
            StatusValue.Approved => "Đã duyệt", 
            _ => status.ToString()
        };
    }
    
    public static string ToDisplayStringForOrder(this StatusValue status)
    {
        return status switch
        {
            StatusValue.Draft => "Tạm lưu",
            StatusValue.Pending => "Chờ xử lý", 
            StatusValue.Approved => "Đã duyệt",
            StatusValue.Completed => "Hoàn thành",
            StatusValue.Canceled => "Hủy",
            _ => status.ToString()
        };
    }
}