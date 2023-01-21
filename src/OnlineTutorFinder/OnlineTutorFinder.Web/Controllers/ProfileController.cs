using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineTutorFinder.Web.Models;
using OnlineTutorFinder.Web.Services.Membership;

namespace OnlineTutorFinder.Web.Controllers;

[Authorize]
public class ProfileController : Controller
{
    private readonly UserManager _userManager;

    public ProfileController(UserManager userManager)
    {
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        var model = new ProfileModel();
        var user = await _userManager.FindByNameAsync(User.Identity!.Name);

        if (user != null)
            model.LoadData(user);

        return View(model);
    }
}
