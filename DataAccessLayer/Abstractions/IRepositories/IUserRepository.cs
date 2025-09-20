using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Abstractions.IRepositories
{
    public interface IUserRepository : IBaseRepository<User, Guid>
    {
        public UserManager<User> UserManager { get; }

        public SignInManager<User> SignInManager { get; }
    }
}
