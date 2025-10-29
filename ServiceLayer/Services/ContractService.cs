using DataAccessLayer.Abstractions.IRepositories;
using DataAccessLayer.Entities;
using DataAccessLayer.Enums;
using Microsoft.EntityFrameworkCore;
using ServiceLayer.Abstractions.IServices;

namespace ServiceLayer.Services;

public class ContractService : IContractService
{
    private DeliverySytemContext _db;
    private readonly IContractRepository _contractRepository;
    private readonly IQuotationRepository _quotationRepository;
    private readonly IBaseRepository<Order, Guid> _orderRepository;
    private readonly IBaseRepository<WarehouseSlot, Guid> _warehouseSlotRepository;
    private readonly IBaseRepository<SlotReservation, Guid> _slotReservationRepository;

    public ContractService(DeliverySytemContext db, IContractRepository contractRepository,
        IQuotationRepository quotationRepository,
        IBaseRepository<Order, Guid> orderRepository, IBaseRepository<WarehouseSlot, Guid> warehouseSlotRepository,
        IBaseRepository<SlotReservation, Guid> slotReservationRepository)
    {
        _db = db;
        _contractRepository = contractRepository;
        _quotationRepository = quotationRepository;
        _orderRepository = orderRepository;
        _warehouseSlotRepository = warehouseSlotRepository;
        _slotReservationRepository = slotReservationRepository;
    }

    public async Task<List<Contract>> GenerateContractsync(Guid quotationId)
    {
        using var ts = await _db.Database.BeginTransactionAsync();

        try
        {
            var quotation = await _quotationRepository.GetQuotationInfo(quotationId);

            if (quotation == null)
                throw new Exception("Quotation not found");

            var newestOrder = quotation.Orders.OrderByDescending(x => x.CreatedAt).First();
            if (newestOrder == null)
                throw new Exception("No orders found for quotation");
            newestOrder.Status = StatusValue.Approved;
            _orderRepository.Update(newestOrder);

            var slotReservations = await _slotReservationRepository.FindAll(s => s.OrderId == newestOrder.Id,
                includeProperties: [o => o.WarehouseSlot]
            ).ToListAsync();
            var contracts = new List<Contract>();
            //create pending contract
            foreach (var slot in slotReservations)
            {
                var contract = new Contract
                {
                    Id = Guid.NewGuid(),
                    QuotationId = quotation.Id,
                    CustomerId = quotation.CustomerId,
                    StoreId = quotation.StoreId ?? Guid.Empty,
                    WarehouseId = slot.WarehouseSlot.WarehouseId,
                    WarehouseSlotId = slot.WarehouseSlotId,
                    TotalAmount = quotation.TotalAmount,
                    StartDate = slot.From,
                    EndDate = slot.To,
                    Status = ContractStatus.Draft,
                };
                contracts.Add(contract);
            }

            await _contractRepository.AddRangeAsync(contracts);
            await ts.CommitAsync();
            return contracts;
        }
        catch (Exception e)
        {
            await ts.RollbackAsync();
            return [];
        }
        
    }
}