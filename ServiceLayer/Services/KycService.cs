using DataAccessLayer.Abstractions.IRepositories;
using DataAccessLayer.Entities;
using DataAccessLayer.Enums;
using Microsoft.EntityFrameworkCore;
using ServiceLayer.Abstractions.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Services
{
    public class KycService : IKycService
    {
        private readonly DeliverySytemContext _db;
        private readonly IKycRepository _kycRepository;
        public KycService(DeliverySytemContext db, IKycRepository kycRepository)
        {
            _db = db;
            _kycRepository = kycRepository;
        }
        public async Task<KycSubmission> SubmitAsync(Guid storeId, IEnumerable<(string DocType, string FilePath, string? Hash)> docs, string? note)
        {
            
            var hasOpen = await _db.Set<KycSubmission>()
                .AnyAsync(x => x.StoreId == storeId && (x.Status == KycStatus.Pending || x.Status == KycStatus.NeedChanges));
            if (hasOpen) throw new InvalidOperationException("Đang có KYC chưa xử lý.");

            var sub = new KycSubmission
            {
                Id = Guid.NewGuid(),
                StoreId = storeId,
                Status = KycStatus.Pending,
                AdminNote = note,
                SubmittedAt = DateTime.UtcNow
            };
            _db.Add(sub);

            foreach (var d in docs)
            {
                _db.Add(new KycDocument
                {
                    Id = Guid.NewGuid(),
                    KycSubmissionId = sub.Id,
                    DocType = d.DocType,
                    FilePath = d.FilePath,
                    Hash = d.Hash
                });
            }

            await _db.SaveChangesAsync();
            return sub;
        }

        public async Task ApproveAsync(Guid submissionId, int? maxPerDay, IEnumerable<string>? regions, Guid adminId)
        {
            using var tx = await _db.Database.BeginTransactionAsync();

            var sub = await _db.Set<KycSubmission>()
                               .Include(s => s.Store)
                               .FirstOrDefaultAsync(s => s.Id == submissionId)
                      ?? throw new KeyNotFoundException();

            if (sub.Status is not (KycStatus.Pending or KycStatus.NeedChanges))
                throw new InvalidOperationException("Trạng thái không hợp lệ để duyệt.");

            sub.Status = KycStatus.Approved;
            sub.ReviewedAt = DateTime.UtcNow;
            sub.ReviewedBy = adminId;

            
            var store = sub.Store;
            store.Status = StatusValue.Active;
            store.KycLevel = "Đã xác minh"; 
            store.MaxOrdersPerDay = maxPerDay;
            store.ActiveRegions = regions != null ? string.Join(",", regions) : store.ActiveRegions;

            await _db.SaveChangesAsync();
            await tx.CommitAsync();
        }

        public async Task NeedChangesAsync(Guid submissionId, string note, Guid adminId)
        {
            var sub = await _db.Set<KycSubmission>().FindAsync(submissionId) ?? throw new KeyNotFoundException();
           
            sub.Status = KycStatus.NeedChanges;
            sub.AdminNote = note;
            sub.ReviewedAt = DateTime.UtcNow;
            sub.ReviewedBy = adminId;

            await _db.SaveChangesAsync();
        }

        public async Task RejectAsync(Guid submissionId, string note, Guid adminId)
        {
            using var tx = await _db.Database.BeginTransactionAsync();

            var sub = await _db.Set<KycSubmission>()
                               .Include(s => s.Store)
                               .FirstOrDefaultAsync(s => s.Id == submissionId)
                      ?? throw new KeyNotFoundException();

            sub.Status = KycStatus.Rejected;
            sub.AdminNote = note;
            sub.ReviewedAt = DateTime.UtcNow;
            sub.ReviewedBy = adminId;

            
            sub.Store.Status = StatusValue.InActive;

            await _db.SaveChangesAsync();
            await tx.CommitAsync();
        }

        public async Task<List<KycSubmission>> GetAllAsync(string storeName, KycStatus? status = null)
        {
            return await _kycRepository.GetAllAsync(storeName, status);
        }
    }
}
