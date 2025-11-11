namespace DataAccessLayer.Enums;

public enum MaintenanceItemStatus
{
    Active,
    Inactive
}

public static partial class EnumExtensions
{
    public static string ToDisplayStringForMaintenanceItemStatus(this MaintenanceItemStatus status)
    {
        return status switch
        {
            MaintenanceItemStatus.Active => "Khả dụng",
            MaintenanceItemStatus.Inactive => "Không khả dụng",
            _ => status.ToString()
        };
    }
}