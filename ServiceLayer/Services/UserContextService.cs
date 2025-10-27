using Microsoft.AspNetCore.Http;
using ServiceLayer.Abstractions.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Services
{
    public class UserContextService : IUserContextService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserContextService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string? GetUserId()
        {
            var user = _httpContextAccessor.HttpContext?.User;
            return user?.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        public string? GetUserName()
        {
            var user = _httpContextAccessor.HttpContext?.User;
            return user?.Identity?.Name;
        }
    }
}
