namespace DataAccessLayer.Enums;

public enum MaintenanceStatus
{
    Scheduled = 1,
    InProgress = 2,
    Completed = 3,
    Cancelled = 4,
    Overdue = 5
}
public static partial class EnumExtensions
{
    public static string ToDisplayStringForMaintenanceStatus(this MaintenanceStatus status)
    {
        return status switch
        {
            MaintenanceStatus.Scheduled => "Đã lên lịch",
            MaintenanceStatus.InProgress => "Đang bảo trì",
            MaintenanceStatus.Completed => "Đã hoàn thành",
            MaintenanceStatus.Cancelled => "Đã hủy",
            MaintenanceStatus.Overdue => "Quá hạn",
            _ => status.ToString()
        };
    }
}