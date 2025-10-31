namespace DataAccessLayer.Enums;

public enum ContractStatus
{
    Draft = 0,     
    Active = 1,     
    Expired = 2,   
    Terminating = 3, 
    Terminated = 4   
}

public static class ContractStatusExtensions
{
    public static string ToDisplayString(this ContractStatus status)
    {
        switch (status)
        {
            case ContractStatus.Draft:
                return "Nháp";
            case ContractStatus.Active:
                return "Hoạt động";
            case ContractStatus.Expired:
                return "Hết hạn";
            case ContractStatus.Terminating:
                return "Xem xét hủy";
            case ContractStatus.Terminated:
                default:
                return "Hủy";
        }
    }
}