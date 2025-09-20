using DataAccessLayer.Abstractions.IRepositories;
using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositoies
{
    public class UserRepository : BaseRepository<User, Guid>, IUserRepository
    {
        public UserRepository(DeliverySytemContext context) : base(context)
        {
        }
        [FromServices]
        public UserManager<User> UserManager {  get; set; }

        [FromServices]
        public SignInManager<User> SignInManager {  get; set; }
    }
}
