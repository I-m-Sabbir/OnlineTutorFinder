using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineTutorFinder.Web.Areas.User.Models;
using OnlineTutorFinder.Web.Services.Membership;

namespace OnlineTutorFinder.Web.Areas.User.Controllers
{
    [Area("User")]
    [Authorize(Roles = "User")]
    public class ProfileController : UserBaseController<ProfileController>
    {
        private readonly UserManager _userManager;
        private readonly IWebHostEnvironment _webhostEnvironment;

        public ProfileController(UserManager userManager, ILogger<ProfileController> logger, IWebHostEnvironment webhostEnvironment)
            : base(logger)
        {
            _userManager = userManager;
            _webhostEnvironment = webhostEnvironment;
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
                try
                {
                    if(model.Picture != null)
                        model.PictureUrl = await PictureUpload(model.Picture, user.Id);
                    var updated = model.Update(user);
                    var result = await _userManager.UpdateAsync(updated);

                    if (result.Succeeded)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
                }

            }

            return View(model);
        }

        private async Task<string> PictureUpload(IFormFile? picture, Guid id)
        {
            string path = Path.Combine("/Images/ProfilePictures/", id + "-" + Path.GetFileName(picture!.FileName));
            string absulotePath = Path.Combine(_webhostEnvironment.WebRootPath + path);

            using (Stream stream = new FileStream(absulotePath, FileMode.Create))
            {
                await picture.CopyToAsync(stream);
            }
            return path;
        }
    }
}
