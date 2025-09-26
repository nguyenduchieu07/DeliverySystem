using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Abstractions.IServices
{
    public interface IKycService
    {
        Task<KycSubmission> SubmitAsync(Guid storeId, IEnumerable<(string DocType, string FilePath, string? Hash)> docs, string? note);
        Task ApproveAsync(Guid submissionId, int? maxPerDay, IEnumerable<string>? regions, Guid adminId);
        Task NeedChangesAsync(Guid submissionId, string note, Guid adminId);
        Task RejectAsync(Guid submissionId, string note, Guid adminId);
    }
}
