using System.Runtime.CompilerServices;

namespace PresentationLayer.Areas.Stores.Models
{
    public class OrderPeding
    {
        public string Id { get; set; }  

        public string Name { get; set; }

        public string CustomerName { get; set; }

        public string Status { get; set; }

        public DateTime CreatedAt { get; set; }

        public decimal Total {  get; set; }

    }
}
