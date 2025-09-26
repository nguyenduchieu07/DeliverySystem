using PresentationLayer.Models;

namespace PresentationLayer.Areas.Stores.Models
{
    public class ServiceListVM
    {
        public ServiceListFilterVM Filter { get; set; } = new();
        public PagedResult<ServiceListItemVM> PageData { get; set; } = new();
    }
}
