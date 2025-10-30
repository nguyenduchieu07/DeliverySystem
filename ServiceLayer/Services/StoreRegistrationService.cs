using DataAccessLayer.Constants;
using DataAccessLayer.Entities;
using DataAccessLayer.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ServiceLayer.Abstractions.IServices;
using ServiceLayer.Dtos.RegisterStore;
using ServiceLayer.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Services
{
    public class StoreRegistrationService : IStoreRegistrationService
    {
        private readonly UserManager<User> _userManager;
        private readonly DeliverySytemContext _context;
        private readonly ICloudinaryService cloudinaryService;
        public StoreRegistrationService(UserManager<User> userManager,
            DeliverySytemContext deliverySytemContext,
            ICloudinaryService cloudinaryService)
        {
            _context = deliverySytemContext;
            _userManager = userManager;
            this.cloudinaryService = cloudinaryService;

        }

        public Task<Store> GetStoreDetailsAsync(Guid storeId)
        {
            throw new NotImplementedException();
        }

        public async Task<RegisterStoreResponse> RegisterStoreAsync(RegisterStoreRequest request)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // 1. Add User
                var user = await _userManager.FindByIdAsync(request.UserId.ToString()) ?? throw new Exception("Not found user");
                var isStore = await _userManager.IsInRoleAsync(user!, UserRoles.STORE);
                if (isStore)
                {
                    throw new Exception($"Failed to create store: user is store");
                }
                await _userManager.AddToRoleAsync(user,UserRoles.STORE);

                // 2. Create Store
                var store = new Store
                {
                    Id = Guid.NewGuid(),
                    OwnerUserId = user.Id,
                    StoreName = request.StoreName,
                    LegalName = request.LegalName,
                    LicenseNumber = request.LicenseNumber,
                    TaxNumber = request.TaxNumber,
                    Status = StatusValue.PendingKyc, // Initial status
                    KycLevel = "None",
                    RatingAvg = 0,
                    RatingCount = 0,
                    UpdatedAt = DateTime.UtcNow
                };
                await _context.Stores.AddAsync(store);


                // 3. Create Store Address
                var address = new Address
                {
                    Id = Guid.NewGuid(),
                    StoreId = store.Id,
                    Label = "Main Office",
                    AddressLine = request.AddressLine,
                    Ward = request.Ward,
                    District = request.District,
                    City = request.City,
                    Latitude = request.Latitude,
                    Longitude = request.Longitude,
                    IsDefault = true,
                    Active = true
                };
                await _context.Addresses.AddAsync(address);


                // 4. Create Wallet for Store
                var wallet = new Wallet
                {
                    Id = Guid.NewGuid(),
                    OwnerType = "Store",
                    OwnerId = store.Id,
                    Currency = "VND",
                    Balance = 0,
                    Status = StatusValue.Active
                };
                await _context.Wallets.AddAsync(wallet);

                // 5. Create initial KYC submission (empty, waiting for documents)
                var kycSubmission = new KycSubmission
                {
                    Id = Guid.NewGuid(),
                    StoreId = store.Id,
                    Status = KycStatus.Pending,
                    SubmittedAt = DateTime.UtcNow
                };
                await _context.KycSubmissions.AddAsync(kycSubmission);

                List<KycDocument> documents = [];
                //add document
                if (!string.IsNullOrEmpty(request.LicenseNumber))
                {
                    var licenseDocument = new KycDocument
                    {
                        Id = Guid.NewGuid(),
                        KycSubmissionId = kycSubmission.Id,
                        DocType = KycDocumentConstant.LicenseNumberKey,
                        FilePath = string.Empty,
                        Hash = Hashor.ToBase64(request.LicenseNumber)
                    };
                    documents.Add(licenseDocument);
                }
                if (!string.IsNullOrEmpty(request.TaxNumber))
                {
                    var taxDocument = new KycDocument
                    {
                        Id = Guid.NewGuid(),
                        KycSubmissionId = kycSubmission.Id,
                        DocType = KycDocumentConstant.TaxNumberKey,
                        FilePath = string.Empty,
                        Hash = Hashor.ToBase64(request.TaxNumber)
                    };
                    documents.Add(taxDocument);
                }

                await _context.KycDocuments.AddRangeAsync(documents);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();


                return new RegisterStoreResponse
                {
                    UserId = user.Id,
                    StoreId = store.Id,
                    StoreName = store.StoreName,
                    Status = store.Status,
                    KycStatus = (StatusValue)kycSubmission.Status,
                    Message = "Store registered successfully. Please submit KYC documents for verification."
                };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

  

        public async Task<KycSubmission> SubmitKycDocumentsAsync(SubmitKycRequest request)
        {
            var store = await _context.Stores.FirstOrDefaultAsync(e => e.Id == request.StoreId);

            if (store == null)
                throw new Exception("Store not found");

            var kycSubmission = await _context.KycSubmissions
                        .Where(k => k.StoreId == request.StoreId && k.Status == KycStatus.Pending)
                        .FirstOrDefaultAsync();


            if (kycSubmission == null)
            {
                kycSubmission = new KycSubmission
                {
                    Id = Guid.NewGuid(),
                    StoreId = request.StoreId,
                    Status = KycStatus.Pending,
                    SubmittedAt = DateTime.UtcNow
                };
                await _context.KycSubmissions.AddAsync(kycSubmission);
            }
            // Upload and save documents
            foreach (var doc in request.Documents)
            {
                var filePath = await cloudinaryService.UploadImageFileAsync(doc.File);
                var fileHash = await CalculateFileHashAsync(doc.File);

                var kycDocument = new KycDocument
                {
                    Id = Guid.NewGuid(),
                    KycSubmissionId = kycSubmission.Id,
                    DocType = doc.DocType,
                    FilePath = filePath,
                    Hash = fileHash
                };
                await _context.KycDocuments.AddAsync(kycDocument);
            }
            await _context.SaveChangesAsync();
            return kycSubmission;
        }
        private async Task<string> CalculateFileHashAsync(IFormFile file)
        {
            using var stream = file.OpenReadStream();
            using var sha256 = System.Security.Cryptography.SHA256.Create();
            var hash = await sha256.ComputeHashAsync(stream);
            return Convert.ToBase64String(hash);
        }

    }
}
