using DataAccessLayer.Entities;
using ServiceLayer.Dtos.RegisterStore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Abstractions.IServices
{
    public interface IStoreRegistrationService
    {
        Task<RegisterStoreResponse> RegisterStoreAsync(RegisterStoreRequest request);
        Task<KycSubmission> SubmitKycDocumentsAsync(SubmitKycRequest request);
        Task<Store> GetStoreDetailsAsync(Guid storeId);
    }
}
