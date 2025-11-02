using System.Text;
using DataAccessLayer.Abstractions.IRepositories;
using DataAccessLayer.Entities;
using DataAccessLayer.Enums;
using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using ServiceLayer.Abstractions.IServices;

namespace ServiceLayer.Services;

public class ContractService : IContractService
{
    private DeliverySytemContext _db;
    private readonly IContractRepository _contractRepository;
    private readonly IQuotationRepository _quotationRepository;
    private readonly IBaseRepository<Order, Guid> _orderRepository;
    private readonly IBaseRepository<SlotReservation, Guid> _slotReservationRepository;

    private readonly IConverter _converter;
    private readonly IWebHostEnvironment _env;
    
    public ContractService(DeliverySytemContext db, IContractRepository contractRepository,
        IQuotationRepository quotationRepository,
        IBaseRepository<Order, Guid> orderRepository,
        IBaseRepository<SlotReservation, Guid> slotReservationRepository,
        IConverter converter, IWebHostEnvironment env)
    {
        _db = db;
        _contractRepository = contractRepository;
        _quotationRepository = quotationRepository;
        _orderRepository = orderRepository;
        _slotReservationRepository = slotReservationRepository;
        
        _converter = converter;
        _env = env;
        
    }

    public async Task<List<Contract>> GetActiveQuotationContracts(Guid quotationId)
    {
        var activeQuotation = await _quotationRepository.FindSingleAsync(q => q.Id == quotationId);

        if (activeQuotation.Status != StatusValue.Active)
        {
            return [];
        }
        
        var contracts = await _contractRepository.FindAll(x => x.QuotationId == quotationId, [ o=> o.WarehouseSlot]).ToListAsync();
        return contracts;
    }
    
    public async Task<List<Contract>> GenerateContractsAsync(Guid quotationId)
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

            var slotReservations = await _slotReservationRepository.FindAll(s => s.OrderId == newestOrder.Id && s.Status == StatusValue.Reserved,
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
            
            //set slot reservation to InActive for not check again
            slotReservations.ForEach(s =>
            {
                s.Status = StatusValue.InActive;
                _slotReservationRepository.Update(s);
            });
            await ts.CommitAsync();
            
            //get contracts by OrderId 
            var returnContracts = _contractRepository.FindAll(c => c.QuotationId == quotationId, includeProperties: o => o.WarehouseSlot).ToList();
            
            return returnContracts.OrderByDescending(x => x.CreatedAt).ToList();
        }
        catch (Exception e)
        {
            await ts.RollbackAsync();
            return [];
        }
        
    }
    public async Task<Contract> GenerateContractAsync(Guid quotationId)
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

            var returnContract = await _db.Contracts
                     .Include(c => c.Warehouse)
                     .Include(c => c.WarehouseSlot)
                     .Include(c => c.Customer)
                     .Include(c => c.Store)
                    .SingleOrDefaultAsync(c => c.QuotationId == quotationId);
            if (returnContract == null)
            {
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
            }

            //set slot reservation to InActive for not check again
            await ts.CommitAsync();

            //get contracts by OrderId 
            returnContract = await _contractRepository.FindSingleAsync(c => c.QuotationId == quotationId, 
                cancellationToken: default, includeProperties: [o => o.Warehouse, o => o.WarehouseSlot, o => o.Customer, o => o.Store]);

            return returnContract;
        }
        catch (Exception e)
        {
            await ts.RollbackAsync();
            return null;
        }

    }


    public async Task<string> GenerateContractHtmlAsync(Guid contractId, bool forceGenerate = false)
    {
        var contract = await _contractRepository.GetContractWithAllInfoAsync(contractId);

        if (contract == null) throw new Exception("Contract not found");

        // nếu đã có pdf và không ép generate lại -> return
        if (!forceGenerate && !string.IsNullOrEmpty(contract.PdfUrl))
        {
            var webPath = Path.Combine(_env.WebRootPath, contract.PdfUrl.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
            if (File.Exists(webPath))
                return contract.PdfUrl;
        }

        // load template
        var templatePath = Path.Combine(_env.WebRootPath, "templates", "warehouse_contract.html");
        if (!File.Exists(templatePath)) throw new Exception("Contract template missing");

        var html = await File.ReadAllTextAsync(templatePath);

        // build SlotRows HTML
        var slotRowsBuilder = new StringBuilder();
        if (contract.WarehouseSlot != null)
        {
            // single slot case
            slotRowsBuilder.AppendLine(BuildSlotRow(contract.Warehouse, contract.WarehouseSlot, contract.StartDate, contract.EndDate, contract.TotalAmount));
        }

        // replace tokens
        // html = html.Replace("{{CompanyLogoUrl}}", contract.Store?.LogoUrl ?? "");
        html = html.Replace("{{ContractNumber}}", contract.Id.ToString().ToUpper());
        html = html.Replace("{{ContractDate}}", DateTime.UtcNow.ToString("dd/MM/yyyy"));
        html = html.Replace("{{ContractStatus}}", contract.Status.ToDisplayString());
        html = html.Replace("{{StoreName}}", contract.Store?.LegalName ?? "");
        html = html.Replace("{{StoreAddress}}", contract.Store?.Addresses.FirstOrDefault(a => a.IsDefault)?.AddressLine ?? "");
        html = html.Replace("{{StoreEmail}}", contract.Store?.ContactEmail ?? "");
        html = html.Replace("{{StorePhone}}", contract.Store?.ContactPhone ?? "");
        html = html.Replace("{{CustomerName}}", contract.Quotation?.Customer.FullName ?? "");
        html = html.Replace("{{CustomerAddress}}", contract.Quotation?.Customer.User?.Addresses?.FirstOrDefault(a => a.IsDefault)?.AddressLine ?? "");
        html = html.Replace("{{CustomerEmail}}", contract.Quotation?.Customer.Email ?? "");
        html = html.Replace("{{CustomerPhone}}", contract.Quotation?.Customer.PhoneNumber ?? "");
        html = html.Replace("{{QuoteCode}}", contract.Quotation != null ? $"QT-{contract.Quotation.CreatedAt:yyyy}-{contract.Quotation.Id.ToString().Substring(0,6).ToUpper()}" : "");
        html = html.Replace("{{QuotationValidUntil}}", contract.Quotation?.ValidUntil.ToString("dd/MM/yyyy HH:mm") ?? "");
        html = html.Replace("{{Subtotal}}", $"{contract.TotalAmount:N0}");
        html = html.Replace("{{TotalAmount}}", $"{contract.TotalAmount:N0}");
        html = html.Replace("{{VatRate}}", "10");
        html = html.Replace("{{VatAmount}}", $"{Math.Round(contract.TotalAmount * 0.10m):N0}");
        html = html.Replace("{{Total}}", $"{contract.TotalAmount:N0}");
        html = html.Replace("{{StartDate}}", contract.StartDate.ToString("dd/MM/yyyy"));
        html = html.Replace("{{EndDate}}", contract.EndDate.ToString("dd/MM/yyyy"));
        html = html.Replace("{{TermsAndConditions}}", contract.TermsAndConditions ?? "Điều khoản tiêu chuẩn áp dụng.");

        html = html.Replace("{{SlotRows}}", slotRowsBuilder.ToString());

        return html;
    }

    private string BuildSlotRow(Warehouse? wh, WarehouseSlot slot, DateTime start, DateTime end, decimal fee)
    {
        var sb = new StringBuilder();
        sb.AppendLine("<tr>");
        sb.AppendLine($"  <td>{wh?.Name ?? ""}</td>");
        sb.AppendLine($"  <td>{slot.Code}</td>");
        sb.AppendLine($"  <td>{slot.VolumeM3:N2}</td>");
        sb.AppendLine($"  <td>{slot.BasePricePerHour:N0} VND</td>");
        sb.AppendLine($"  <td>{start:dd/MM/yyyy} - {end:dd/MM/yyyy}</td>");
        sb.AppendLine($"  <td>{fee:N0} VND</td>");
        sb.AppendLine("</tr>");
        return sb.ToString();
    }

    public async Task<bool> ConfirmContract(Guid contractId, Guid orderId)
    {
        await using var ts = await _db.Database.BeginTransactionAsync();
        try
        {
            //Kích hoạt hợp đồng
            var contract = await _db.Contracts
                .AsTracking()
                .Include(c => c.Quotation)
                .SingleOrDefaultAsync(x => x.Id == contractId);

            if (contract == null)
                return false;
            
            contract.Quotation.Status = StatusValue.Active;

            contract.Status = ContractStatus.Active;
            _db.Contracts.Update(contract);

            //Cập nhật trạng thái slot & reservation
            var orderSlot = await _db.SlotReservations
                .Include(s => s.WarehouseSlot)
                .AsTracking()
                .FirstOrDefaultAsync(s => s.OrderId == orderId);

            if (orderSlot == null)
                throw new InvalidOperationException("Không tìm thấy slot reservation cho đơn hàng.");

            // Đặt reservation sang Inactive (đã sử dụng)
            orderSlot.Status = StatusValue.InActive;
            _db.SlotReservations.Update(orderSlot);

            // Đặt slot thật sự sang InUse (đang dùng)
            if (orderSlot.WarehouseSlot != null)
            {
                orderSlot.WarehouseSlot.Status = StatusValue.InUse;
                _db.WarehouseSlots.Update(orderSlot.WarehouseSlot);
            }
            await _db.SaveChangesAsync();
            await ts.CommitAsync();

            return true;
        }
        catch (Exception)
        {
            await ts.RollbackAsync();
            return false;
        }
    }


    public async Task<bool> CancleContract(Guid contractId, Guid orderId)
    {
        await using var ts = await _db.Database.BeginTransactionAsync();
        try
        {
            //Hủy hợp đồng
            var contract = await _db.Contracts
                .AsTracking()
                .SingleOrDefaultAsync(x => x.Id == contractId);

            if (contract == null)
                return false;

            contract.Status = ContractStatus.Terminated;
            contract.Quotation.Status = StatusValue.InActive;
            _db.Contracts.Update(contract);

            //Cập nhật trạng thái slot & reservation
            var orderSlot = await _db.SlotReservations
                .Include(s => s.WarehouseSlot)
                .AsTracking()
                .FirstOrDefaultAsync(s => s.OrderId == orderId);

            if (orderSlot == null)
                throw new InvalidOperationException("Không tìm thấy slot reservation cho đơn hàng.");
            orderSlot.Status = StatusValue.InActive;
            _db.SlotReservations.Update(orderSlot);

            if (orderSlot.WarehouseSlot != null)
            {
                orderSlot.WarehouseSlot.Status = StatusValue.Available;
                _db.WarehouseSlots.Update(orderSlot.WarehouseSlot);
            }
            await _db.SaveChangesAsync();
            await ts.CommitAsync();

            return true;
        }
        catch (Exception)
        {
            ts.Rollback();
            return false;
        }
        
    }
}