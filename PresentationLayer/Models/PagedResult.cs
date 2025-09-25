namespace PresentationLayer.Models
{
    public class PagedResult<T> where T : class
    {
        public IReadOnlyList<T> Items { get; set; } = Array.Empty<T>();
        public int Page { get; set; }
        public int Size { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)Math.Max(0, TotalItems) / Math.Max(1, Size));
        public int From => TotalItems == 0 ? 0 : (Page - 1) * Size + 1;
        public int To => Math.Min(Page * Size, TotalItems);
    }
}
