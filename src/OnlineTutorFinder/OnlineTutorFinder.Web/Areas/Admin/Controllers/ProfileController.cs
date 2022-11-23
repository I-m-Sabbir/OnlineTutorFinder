using Microsoft.AspNetCore.Mvc;
using OnlineTutorFinder.Web.Areas.Admin.Models;
using OnlineTutorFinder.Web.Services.Membership;

namespace OnlineTutorFinder.Web.Areas.Admin.Controllers
{
    public class ProfileController : AdminBaseController<ProfileController>
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

        public async Task<IActionResult> Edit()
        {
            var user = await _userManager.FindByNameAsync(User.Identity!.Name);
            var model = new ProfileModel();
            model.LoadData(user);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ProfileModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(User.Identity!.Name);

                var updated = model.Update(user);
                var result = await _userManager.UpdateAsync(updated);

                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }

            }

            return View(model);
        }
    }
}
