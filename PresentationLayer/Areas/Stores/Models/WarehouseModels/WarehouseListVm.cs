using DataAccessLayer.Enums;

namespace PresentationLayer.Areas.Stores.Models.WarehouseModels
{
    public class WarehouseListVm
    {
        // Input (filters)
        public Guid StoreId { get; set; }
        public string? Q { get; set; }              // search theo tên / địa chỉ
        public Guid? AddressId { get; set; }        // filter theo địa chỉ (optional)
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        // Output
        public int Total { get; set; }
        public IReadOnlyList<Row> Items { get; set; } = Array.Empty<Row>();
        public IReadOnlyList<(Guid Id, string Name)> AddressOptions { get; set; } = Array.Empty<(Guid, string)>();

        public int TotalPages => (int)Math.Ceiling((double)Total / Math.Max(1, PageSize));

        public class Row
        {
            public Guid Id { get; set; }
            public string Name { get; set; } = default!;
            public string AddressText { get; set; } = "";
            public int SlotCount { get; set; }
            public StatusValue Status { get; set; }
            public DateTimeOffset CreatedAt { get; set; }
            public string? CoverImageUrl { get; set; }
        }
    }
}
