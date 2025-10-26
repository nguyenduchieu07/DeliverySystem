using System.Security.Claims;
using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace PresentationLayer.Areas.Admin.ViewComponents;

public class UserAvatarViewComponent : ViewComponent
{
    private readonly UserManager<User> _userManager;

    public UserAvatarViewComponent(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var adminIdStrClaim = UserClaimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);
        var adminId = Guid.Parse(adminIdStrClaim);
        var username = _userManager.Users.FirstOrDefault(x => x.Id == adminId)?.UserName;
        return View(model: username);
    }
}