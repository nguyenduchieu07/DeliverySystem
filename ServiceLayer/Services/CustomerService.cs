using DataAccessLayer.Abstractions.IRepositories;
using DataAccessLayer.Entities;
using DataAccessLayer.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ServiceLayer.Abstractions.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace ServiceLayer.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IUserRepository _userRepository;
        private readonly IBaseRepository<Customer, Guid> _customerRepo;
        private readonly IStoreRepository _storeRepo;
        private readonly IBaseRepository<Address, Guid> _addressRepo;
        private readonly IBaseRepository<Order, Guid> _orderRepo;
        private readonly DeliverySytemContext _context;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public CustomerService(
            IUserRepository userRepository,
           IStoreRepository storeRepo,
            IBaseRepository<Customer, Guid> customerRepo,
            IBaseRepository<Address, Guid> addressRepo,
            IBaseRepository<Order, Guid> orderRepo,
            DeliverySytemContext context,
            UserManager<User> userManager,
            SignInManager<User> signInManager)
        {
            _userRepository = userRepository;
            _customerRepo = customerRepo;
            _addressRepo = addressRepo;
            _orderRepo = orderRepo;
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _storeRepo = storeRepo;
        }

        public async Task<(bool Success, string Message, User? User)> RegisterCustomerAsync(
            string phoneNumber,
            string password,
            string fullName,
            string email)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Chuẩn hóa số điện thoại
                phoneNumber = phoneNumber.Trim().Replace(" ", "").Replace("-", "");

                // Kiểm tra số điện thoại đã tồn tại
                var existingUserByPhone = await _userManager.Users
                    .FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber);
                if (existingUserByPhone != null)
                {
                    return (false, "Số điện thoại đã được đăng ký", null);
                }

                // Kiểm tra email đã tồn tại (nếu có)
                if (!string.IsNullOrWhiteSpace(email))
                {
                    var existingUserByEmail = await _userManager.FindByEmailAsync(email);
                    if (existingUserByEmail != null)
                    {
                        return (false, "Email đã được đăng ký", null);
                    }
                }

                // Tạo entity User
                var user = new User
                {
                    Id = Guid.NewGuid(),
                    UserName = phoneNumber,
                    PhoneNumber = phoneNumber,
                    Email = email,
                    PhoneNumberConfirmed = false,
                    EmailConfirmed = false,
                    Status = StatusValue.Active,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                // Tạo người dùng với mật khẩu
                var result = await _userManager.CreateAsync(user, password);
                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    return (false, $"Đăng ký thất bại: {errors}", null);
                }

                // Tạo entity Customer với cùng Id
                var customer = new Customer
                {
                    Id = user.Id,
                    FullName = fullName,
                    PhoneNumber = phoneNumber,
                    Email = email,
                    PreferredLang = "vi",
                    Tier = "Basic",
                    KycLevel = "None",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };
                _customerRepo.Add(customer);
                await _context.SaveChangesAsync();

                // Gán vai trò Customer
                await _userManager.AddToRoleAsync(user, "Customer");

                // Commit transaction
                await transaction.CommitAsync();
                return (true, "Đăng ký thành công! Vui lòng đăng nhập.", user);
            }
            catch (Exception ex)
            {
                // Rollback nếu có lỗi
                await transaction.RollbackAsync();
                return (false, $"Lỗi hệ thống: {ex.Message}", null);
            }
        }

        public async Task<(bool Success, string Message)> LoginAsync(
            string phoneOrEmail,
            string password,
            bool rememberMe = false)
        {
            phoneOrEmail = phoneOrEmail.Trim();
            User? user;

            // Tìm người dùng theo email hoặc số điện thoại
            if (phoneOrEmail.Contains("@"))
            {
                user = await _userManager.FindByEmailAsync(phoneOrEmail);
            }
            else
            {
                // Keep leading 0 and compare exactly after trimming spaces/dashes
                var cleanPhone = phoneOrEmail.Replace(" ", "").Replace("-", "");
                user = await _userManager.Users
                    .FirstOrDefaultAsync(u => u.PhoneNumber == cleanPhone || u.UserName == cleanPhone);
            }

            if (user == null)
            {
                return (false, "Số điện thoại hoặc email không tồn tại");
            }

            // Kiểm tra trạng thái tài khoản
            if (user.Status != StatusValue.Active)
            {
                return (false, "Tài khoản của bạn đã bị vô hiệu hóa");
            }

            // Đoạn code mới
            var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");
            var isCustomer = await _userManager.IsInRoleAsync(user, "Customer");

            // Nếu không phải Admin VÀ cũng không phải Customer thì mới chặn
            if (!isAdmin && !isCustomer)
            {
                return (false, "Tài khoản không có quyền đăng nhập vào hệ thống này.");
            }

            // Đăng nhập
            var result = await _signInManager.CheckPasswordSignInAsync(
                user,
                password,
                lockoutOnFailure: false);


            if (result.Succeeded)
            {
                var storeByUser = await _storeRepo.GetStoreByCustomerIdAsync(user.Id);

                if(storeByUser != null)
                {
                    // Kiểm tra xem claim đã có chưa
                    var existingClaims = await _userManager.GetClaimsAsync(user);
                    if (!existingClaims.Any(c => c.Type == "StoreId"))
                    {
                        await _userManager.AddClaimAsync(user, new Claim("StoreId", storeByUser.Id.ToString()));
                    }

                    await _signInManager.SignInAsync(user, isPersistent: rememberMe);
                }
                else 
                    await _signInManager.SignInAsync(user, isPersistent: rememberMe);
                return (true, "Đăng nhập thành công");
            }
            else if (result.IsLockedOut)
            {
                return (false, "Tài khoản đã bị khóa tạm thời");
            }
            else
            {
                return (false, "Mật khẩu không chính xác");
            }
        }

        public async Task<(bool Success, string Message, string? ResetToken)> ForgotPasswordAsync(string email)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    return (false, "Không tìm thấy tài khoản hoặc email chưa được xác nhận.", null);
                }

                var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
                return (true, "Mã đặt lại mật khẩu đã được tạo.", resetToken);
            }
            catch (Exception ex)
            {
                return (false, $"Lỗi hệ thống: {ex.Message}", null);
            }
        }

        public async Task<(bool Success, string Message)> ResetPasswordAsync(string userId, string token, string newPassword)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return (false, "Không tìm thấy tài khoản.");
                }

                var result = await _userManager.ResetPasswordAsync(user, token, newPassword);
                if (result.Succeeded)
                {
                    return (true, "Đặt lại mật khẩu thành công.");
                }

                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return (false, $"Đặt lại mật khẩu thất bại: {errors}");
            }
            catch (Exception ex)
            {
                return (false, $"Lỗi hệ thống: {ex.Message}");
            }
        }

        public async Task<(bool Success, string Message)> ChangePasswordAsync(Guid userId, string currentPassword, string newPassword)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId.ToString());
                if (user == null)
                {
                    return (false, "Không tìm thấy tài khoản.");
                }

                var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
                if (result.Succeeded)
                {
                    await _signInManager.RefreshSignInAsync(user); // Cập nhật phiên đăng nhập
                    return (true, "Đổi mật khẩu thành công.");
                }

                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return (false, $"Đổi mật khẩu thất bại: {errors}");
            }
            catch (Exception ex)
            {
                return (false, $"Lỗi hệ thống: {ex.Message}");
            }
        }

        public async Task<Customer?> GetProfileAsync(Guid userId)
        {
            return await _customerRepo.FindSingleAsync(c => c.Id == userId);
        }

        public async Task UpdateProfileAsync(Guid userId, string fullName, string? email, string? phoneNumber, string? lang, string? tier)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var customer = await _customerRepo.FindSingleAsync(c => c.Id == userId);
                if (customer == null)
                {
                    return;
                }

                // Cập nhật thông tin khách hàng
                customer.FullName = fullName;
                customer.PreferredLang = lang ?? customer.PreferredLang;
                customer.Tier = tier ?? customer.Tier;
                customer.UpdatedAt = DateTime.UtcNow;

                // Cập nhật email và số điện thoại nếu được cung cấp
                if (!string.IsNullOrWhiteSpace(email) || !string.IsNullOrWhiteSpace(phoneNumber))
                {
                    var user = await _userManager.FindByIdAsync(userId.ToString());
                    if (user != null)
                    {
                        if (!string.IsNullOrWhiteSpace(email) && email != user.Email)
                        {
                            var existingUserByEmail = await _userManager.FindByEmailAsync(email);
                            if (existingUserByEmail != null && existingUserByEmail.Id != userId)
                            {
                                throw new Exception("Email đã được sử dụng.");
                            }
                            user.Email = email;
                            customer.Email = email;
                        }

                        if (!string.IsNullOrWhiteSpace(phoneNumber))
                        {
                            var cleanPhone = phoneNumber.Trim().Replace(" ", "").Replace("-", "");
                            var existingUserByPhone = await _userManager.Users
                                .FirstOrDefaultAsync(u => u.PhoneNumber == cleanPhone);
                            if (existingUserByPhone != null && existingUserByPhone.Id != userId)
                            {
                                throw new Exception("Số điện thoại đã được sử dụng.");
                            }
                            user.PhoneNumber = cleanPhone;
                            customer.PhoneNumber = cleanPhone;
                        }

                        var updateResult = await _userManager.UpdateAsync(user);
                        if (!updateResult.Succeeded)
                        {
                            var errors = string.Join(", ", updateResult.Errors.Select(e => e.Description));
                            throw new Exception($"Cập nhật thông tin người dùng thất bại: {errors}");
                        }
                    }
                }

                _customerRepo.Update(customer);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception($"Lỗi khi cập nhật hồ sơ: {ex.Message}");
            }
        }

        public async Task<List<Address>> GetAddressesAsync(Guid userId)
        {
            var addresses = _addressRepo.FindAll(a => a.UserId == userId && a.Active);
            return await addresses.ToListAsync();
        }

        public async Task AddAddressAsync(Guid userId, Address address)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                address.UserId = userId;
                address.Active = true;
                address.CreatedAt = DateTime.UtcNow;
                address.UpdatedAt = DateTime.UtcNow;
                _addressRepo.Add(address);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception($"Lỗi khi thêm địa chỉ: {ex.Message}");
            }
        }

        public async Task DeleteAddressAsync(Guid addressId)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var address = await _addressRepo.FindSingleAsync(a => a.Id == addressId);
                if (address != null)
                {
                    address.Active = false;
                    address.UpdatedAt = DateTime.UtcNow;
                    _addressRepo.Update(address);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception($"Lỗi khi xóa địa chỉ: {ex.Message}");
            }
        }

        public async Task<List<Order>> GetOrdersAsync(Guid userId)
        {
            return await _orderRepo.FindAll(o => o.CustomerId == userId).ToListAsync();
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }
    }
}