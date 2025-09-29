using DataAccessLayer.Entities;
using DataAccessLayer.Enums;
using Microsoft.AspNetCore.Identity;
using ServiceLayer.Abstractions.IServices;
using ServiceLayer.Dtos.RegisterStore;
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
        public StoreRegistrationService(UserManager<User> userManager, DeliverySytemContext deliverySytemContext)
        {
            _context = deliverySytemContext;
            _userManager = userManager;
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
                var user = new User
                {
                    Id = Guid.NewGuid(),
                    UserName = request.Email,
                    Email = request.Email,
                    PhoneNumber = request.PhoneNumber,
                    Status = "Active",
                    UpdatedAt = DateTime.UtcNow
                };

                // check add user success
                var createUserResult = await _userManager.CreateAsync(user, request.Password);
                if (!createUserResult.Succeeded)
                {
                    throw new Exception($"Failed to create user: {string.Join(", ", createUserResult.Errors.Select(e => e.Description))}");
                }


                // 2. Create Store
                var store = new Store
                {
                    Id = Guid.NewGuid(),
                    OwnerUserId = user.Id,
                    StoreName = request.StoreName,
                    LegalName = request.LegalName,
                    LicenseNumber = request.LicenseNumber,
                    TaxNumber = request.TaxNumber,
                    Status = "PendingKyc", // Initial status
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
                    Status = "Active"
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


                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                

                return new RegisterStoreResponse
                {
                    UserId = user.Id,
                    StoreId = store.Id,
                    StoreName = store.StoreName,
                    Status = store.Status,
                    KycStatus = kycSubmission.Status.ToString(),
                    Message = "Store registered successfully. Please submit KYC documents for verification."
                };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();

            }
        }

        public Task<KycSubmission> SubmitKycDocumentsAsync(SubmitKycRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
