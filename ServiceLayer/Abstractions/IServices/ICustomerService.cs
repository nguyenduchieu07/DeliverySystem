using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServiceLayer.Abstractions.IServices
{
    public interface ICustomerService
    {
        /// <summary>
        /// Register new customer account
        /// </summary>
        /// <returns>Tuple: (Success, Message, User)</returns>
        Task<(bool Success, string Message, User? User)> RegisterCustomerAsync(
            string phoneNumber,
            string password,
            string fullName,
            string email); // Thay đổi thành bắt buộc

        /// <summary>
        /// Login with phone or email
        /// </summary>
        /// <returns>Tuple: (Success, Message)</returns>
        Task<(bool Success, string Message)> LoginAsync(
            string phoneOrEmail,
            string password,
            bool rememberMe = false);

        /// <summary>
        /// Logout current user
        /// </summary>
        Task LogoutAsync();

        /// <summary>
        /// Get customer profile by userId
        /// </summary>
        Task<Customer?> GetProfileAsync(Guid userId);

        /// <summary>
        /// Update customer profile information
        /// </summary>
        Task UpdateProfileAsync(Guid userId, string fullName, string? email, string? phoneNumber, string? lang, string? tier);

        /// <summary>
        /// Get all active addresses of customer
        /// </summary>
        Task<List<Address>> GetAddressesAsync(Guid userId);

        /// <summary>
        /// Add new address for customer
        /// </summary>
        Task AddAddressAsync(Guid userId, Address address);

        /// <summary>
        /// Soft delete address (set Active = false)
        /// </summary>
        Task DeleteAddressAsync(Guid addressId);

        /// <summary>
        /// Get order history of customer
        /// </summary>
        Task<List<Order>> GetOrdersAsync(Guid userId);
    }
}