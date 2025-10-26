using ServiceLayer.Dtos.Quotes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Abstractions.IServices
{
    public interface IQuotationService
    {
        Task<QuoteResultVm> CalculateAndCreateQuotationAsync(QuoteRequestVm req, CancellationToken ct);
        Task<bool> CreateTempReservationAsync(HoldTempVm vm, CancellationToken ct);
        Task<bool> AcceptQuotationAsync(AcceptQuoteVm vm, CancellationToken ct);
        Task<bool> RequestRevisionAsync(RequestRevisionVm vm, CancellationToken ct);
    }
}
