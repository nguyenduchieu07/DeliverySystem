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
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public UserRepository(DeliverySytemContext context, UserManager<User> userManager, SignInManager<User> signInManager)
            : base(context)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
        }

        // Loại bỏ [FromServices] vì không cần thiết
        public UserManager<User> UserManager => _userManager;
        public SignInManager<User> SignInManager => _signInManager;
    }
}