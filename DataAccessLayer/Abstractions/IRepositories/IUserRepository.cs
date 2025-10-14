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
        UserManager<User> UserManager { get; }
        SignInManager<User> SignInManager { get; }
    }
}