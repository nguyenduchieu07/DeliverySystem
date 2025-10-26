using DataAccessLayer.Entities;
using DataAccessLayer.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Abstractions.IServices
{
    public interface IKycService
    {
        Task<List<KycSubmission>> GetAllAsync(string storeName, KycStatus? status = null);
        Task<KycSubmission> SubmitAsync(Guid storeId, IEnumerable<(string DocType, string FilePath, string? Hash)> docs, string? note);
        Task ApproveAsync(Guid submissionId, int? maxPerDay, IEnumerable<string>? regions, Guid adminId);
        Task NeedChangesAsync(Guid submissionId, string note, Guid adminId);
        Task RejectAsync(Guid submissionId, string note, Guid adminId);
    }
}
