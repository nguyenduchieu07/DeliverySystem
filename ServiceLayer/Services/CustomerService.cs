using DataAccessLayer.Abstractions.IRepositories;
using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ServiceLayer.Abstractions.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceLayer.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IUserRepository _userRepository;
        private readonly IBaseRepository<Customer, Guid> _customerRepo;
        private readonly IBaseRepository<Address, Guid> _addressRepo;
        private readonly IBaseRepository<Order, Guid> _orderRepo;
        private readonly DeliverySytemContext _context;

        public CustomerService(
            IUserRepository userRepository,
            IBaseRepository<Customer, Guid> customerRepo,
            IBaseRepository<Address, Guid> addressRepo,
            IBaseRepository<Order, Guid> orderRepo,
            DeliverySytemContext context)
        {
            _userRepository = userRepository;
            _customerRepo = customerRepo;
            _addressRepo = addressRepo;
            _orderRepo = orderRepo;
            _context = context;
        }

        public async Task<(bool Success, string Message, User? User)> RegisterCustomerAsync(
             string phoneNumber,
             string password,
             string fullName,
             string email)
        {
            // Sửa: Sử dụng transaction để đảm bảo tính toàn vẹn
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Validate phone number format (remove spaces, dashes)
                phoneNumber = phoneNumber.Trim().Replace(" ", "").Replace("-", "");

                // Check if phone already exists
                var existingUserByPhone = await _userRepository.UserManager.Users
                    .FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber);
                if (existingUserByPhone != null)
                {
                    return (false, "Số điện thoại đã được đăng ký", null);
                }

                // Check if email already exists (if provided)
                if (!string.IsNullOrWhiteSpace(email))
                {
                    var existingUserByEmail = await _userRepository.UserManager.FindByEmailAsync(email);
                    if (existingUserByEmail != null)
                    {
                        return (false, "Email đã được đăng ký", null);
                    }
                }

                // Create User entity
                var user = new User
                {
                    Id = Guid.NewGuid(),
                    UserName = phoneNumber, // Use phone as username
                    PhoneNumber = phoneNumber,
                    Email = email,
                    PhoneNumberConfirmed = false,
                    EmailConfirmed = false,
                    Status = "Active",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                // Create user with password
                var result = await _userRepository.UserManager.CreateAsync(user, password);
                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    return (false, $"Đăng ký thất bại: {errors}", null);
                }

                // Create Customer entity with same Id
                var customer = new Customer
                {
                    Id = user.Id, // Same ID as User
                    FullName = fullName,
                    PhoneNumber=phoneNumber,
                    Email = email,
                    PreferredLang = "vi",
                    Tier = "Basic",
                    KycLevel = "None",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };
                _customerRepo.Add(customer);
                await _context.SaveChangesAsync();

                // Assign Customer role
                await _userRepository.UserManager.AddToRoleAsync(user, "Customer");

                // Commit transaction nếu thành công
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

            // Find user by phone or email
            User? user;
            if (phoneOrEmail.Contains("@"))
            {
                // Login by email
                user = await _userRepository.UserManager.FindByEmailAsync(phoneOrEmail);
            }
            else
            {
                // Login by phone (remove spaces/dashes)
                var cleanPhone = phoneOrEmail.Replace(" ", "").Replace("-", "");
                user = await _userRepository.UserManager.Users
                    .FirstOrDefaultAsync(u => u.PhoneNumber == cleanPhone);
            }

            if (user == null)
            {
                return (false, "Số điện thoại hoặc email không tồn tại");
            }

            // Check if user is active
            if (user.Status != "Active")
            {
                return (false, "Tài khoản của bạn đã bị vô hiệu hóa");
            }

            // Check if user is a customer
            var isCustomer = await _userRepository.UserManager.IsInRoleAsync(user, "Customer");
            if (!isCustomer)
            {
                return (false, "Tài khoản này không phải là tài khoản khách hàng");
            }

            // Sign in
            var result = await _userRepository.SignInManager.PasswordSignInAsync(
                user,
                password,
                rememberMe,
                lockoutOnFailure: false);

            if (result.Succeeded)
            {
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

        public async Task<Customer?> GetProfileAsync(Guid userId)
        {
            return await _customerRepo.FindSingleAsync(c => c.Id == userId);
        }

        public async Task UpdateProfileAsync(Guid userId, string fullName, string? email, string? phoneNumber, string? lang, string? tier)
        {
            var customer = await _customerRepo.FindSingleAsync(c => c.Id == userId);
            if (customer != null)
            {
                customer.FullName = fullName;
                customer.PreferredLang = lang ?? customer.PreferredLang;
                customer.Tier = tier ?? customer.Tier;

                _customerRepo.Update(customer);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Address>> GetAddressesAsync(Guid userId)
        {
            var addresses = _addressRepo.FindAll(a => a.UserId == userId && a.Active);
            return await addresses.ToListAsync();
        }

        public async Task AddAddressAsync(Guid userId, Address address)
        {
            address.UserId = userId;
            address.Active = true;
            address.CreatedAt = DateTime.UtcNow;
            address.UpdatedAt = DateTime.UtcNow;

            _addressRepo.Add(address);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAddressAsync(Guid addressId)
        {
            var address = await _addressRepo.FindSingleAsync(a => a.Id == addressId);
            if (address != null)
            {
                address.Active = false;
                _addressRepo.Update(address);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Order>> GetOrdersAsync(Guid userId)
        {
            return await _orderRepo.FindAll(o => o.CustomerId == userId).ToListAsync();
        }

        public async Task LogoutAsync()
        {
            await _userRepository.SignInManager.SignOutAsync();
        }
    }
}