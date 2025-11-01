using DataAccessLayer.Entities;
using DataAccessLayer.Enums;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.Models;
using ServiceLayer.Abstractions.IServices;

namespace PresentationLayer.Areas.Stores.Controllers
{
    [Area("Stores")]
    public class OrdersController : Controller
    {
        private readonly IOrderService _orderService;
        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        

        public async Task<IActionResult> Index([FromQuery] StatusValue? status)
        {
            var vm = new StoreOrderIndexViewModel();

            var storeId = User.FindFirst("StoreId")?.Value ?? "";

            var orders = await _orderService.GetAllOrdersByStoreIdAsync(Guid.Parse(storeId), status);
            vm.Orders = orders;
            vm.TotalElements = orders.Count;
            
            return View(vm);
        }
        
        public class StoreOrderDetailViewModel
        {
            public Order Order { get; set; } = null!;
            public List<OrderItem> OrderItems { get; set; } = new();
            public Customer? Customer { get; set; }
            public Address? PickupAddress { get; set; }
            public Address? DropoffAddress { get; set; }
            
        }
        
        public async Task<IActionResult> Detail(Guid id)
        {
            var order = await _orderService.GetOrderInfoByIdAsync(id);

            if (order == null)
                return NotFound();

            var vm = new StoreOrderDetailViewModel
            {
                Order = order,
                OrderItems = order.OrderItems.ToList(),
                Customer = order.Customer,
                PickupAddress = order.PickupAddress,
                DropoffAddress = order.DropoffAddress,
                
            };

            return View(vm);
        }
    }
}
