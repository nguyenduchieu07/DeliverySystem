using System.Threading.Tasks;
using DataAccessLayer.Enums;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.Models;
using ServiceLayer.Abstractions.IServices;

namespace PresentationLayer.Controllers
{
    public class ContractController : Controller
    {
        private readonly IContractService _contractService;
        private readonly IOrderService _orderService;
        public ContractController(IContractService contractService, IOrderService orderService)
        {
            _contractService = contractService;
            _orderService = orderService;
        }

        public async Task<IActionResult> Index([FromQuery] Guid orderId)
        {
            try
            {
                var orderById = await _orderService.GetByIdAsync(orderId);

                if(orderById!.QuotationId == null)
                    return BadRequest("Báo giá không tồn tại cho đơn hàng này");

                var contract = await _contractService.GenerateContractAsync(orderById.QuotationId ?? Guid.Empty);

                var viewModel = new ContractIndexViewModel
                {
                    Order = orderById,
                    Contract = contract
                };

                return View(viewModel);
            }
            catch (InvalidOperationException invalidEx)
            {
                return BadRequest(invalidEx.Message);
            }
        }


        [HttpPost]
        public async Task<IActionResult> Confirm(Guid contractId, Guid orderId)
        {
            await _contractService.ConfirmContract(contractId, orderId);
            return RedirectToAction("Index", "Home"); //SẼ CHUYỂN SANG TRANG THANH TOÁN VỚI ORERID TƯƠNG ỨNG
        }

        [HttpPost]
        public async Task<IActionResult> Cancel(Guid contractId, Guid orderId)
        {
            await _contractService.CancleContract(contractId, orderId);
            return RedirectToAction("Index", "Home");
        }
    }
}
