using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PresentationLayer.Models;

namespace PresentationLayer.Controllers
{
    public class BookingController : Controller
    {
        private readonly DeliverySytemContext _db;
        public BookingController(DeliverySytemContext deliverySytemContext)
        {
            _db = deliverySytemContext;
        }
        public async Task<IActionResult> Index()
        {
            var userId = "1";


            var vm = new BookingRequestVM
            {
                PickupDate = DateTime.UtcNow.Date.AddDays(1),
                PickupTime = "09:00",
                AddressOptions = await _db.Addresses
            //.Where(a => a.UserId == userId && a.Active)
            .OrderByDescending(a => a.IsDefault)
            .Select(a => new AddressOptionVM
            {
                Id = a.Id,
                Label = string.IsNullOrWhiteSpace(a.Label) ? a.AddressLine : a.Label,
                Full = a.AddressLine + ", " + (a.Ward ?? "") + ", " + (a.District ?? "") + ", " + (a.City ?? "")
            })
            .ToListAsync(),
                Items = { new BookingItemVM() }
            };


            ViewBag.Services = await _db.Services
            .Where(s => s.IsActive)
            .OrderBy(s => s.Name)
            .Select(s => new { s.Id, s.Name, s.Unit, s.BasePrice })
            .ToListAsync();


            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var userId = "1";


            var vm = new BookingRequestVM
            {
                PickupDate = DateTime.UtcNow.Date.AddDays(1),
                PickupTime = "09:00",
                AddressOptions = await _db.Addresses
            //.Where(a => a.UserId == userId && a.Active)
            .OrderByDescending(a => a.IsDefault)
            .Select(a => new AddressOptionVM
            {
                Id = a.Id,
                Label = string.IsNullOrWhiteSpace(a.Label) ? a.AddressLine : a.Label,
                Full = a.AddressLine + ", " + (a.Ward ?? "") + ", " + (a.District ?? "") + ", " + (a.City ?? "")
            })
            .ToListAsync(),
                Items = { new BookingItemVM() }
            };


            ViewBag.Services = await _db.Services
            .Where(s => s.IsActive)
            .OrderBy(s => s.Name)
            .Select(s => new { s.Id, s.Name, s.Unit, s.BasePrice })
            .ToListAsync();


            return View(vm);
        
        }
      

        public async Task<IActionResult> Success(Guid id)
        {
            var order = await _db.Orders
                .Include(o => o.PickupAddress)
                .Include(o => o.DropoffAddress)
                .Include(o => o.OrderItems).ThenInclude(oi => oi.Service)
                .FirstOrDefaultAsync(o => o.Id == id);
            if (order == null) return NotFound();
            return View(order);
        }

        private async Task RehydrateServices()
        {
            ViewBag.Services = await _db.Services
                .Where(s => s.IsActive)
                .OrderBy(s => s.Name)
                .Select(s => new { s.Id, s.Name, s.Unit, s.BasePrice })
                .ToListAsync();
        }

        private async Task RehydrateAddresses(Guid userId, BookingRequestVM vm)
        {
            vm.AddressOptions = await _db.Addresses
                .Where(a => a.UserId == userId && a.Active)
                .OrderByDescending(a => a.IsDefault)
                .Select(a => new AddressOptionVM
                {
                    Id = a.Id,
                    Label = string.IsNullOrWhiteSpace(a.Label) ? a.AddressLine : a.Label,
                    Full = a.AddressLine + ", " + (a.Ward ?? "") + ", " + (a.District ?? "") + ", " + (a.City ?? "")
                })
                .ToListAsync();
        }
    }
}
