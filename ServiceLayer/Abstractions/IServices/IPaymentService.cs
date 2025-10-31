using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Abstractions.IServices
{
    public interface IPaymentService
    {
        Task<PaymentResultViewModel> ProcessPayment(Payment payment, string paymentMethod, string? provider = null);
        Task<bool> VerifyPayment(Guid paymentId, string providerTxnId);
        Task<PaymentResultViewModel> HandlePaymentCallback(string method, Dictionary<string, string> parameters);
        string GenerateTransactionId(string method);
    }
}
