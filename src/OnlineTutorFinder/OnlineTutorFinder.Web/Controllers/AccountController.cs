using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineTutorFinder.Web.Entities.Membership;
using OnlineTutorFinder.Web.Models.AccountModels;

namespace OnlineTutorFinder.Web.Controllers;

public class AccountController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly ILogger<AccountController> _logger;

    public AccountController(UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        ILogger<AccountController> logger)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _logger = logger;
    }

    public IActionResult Register(string? returnUrl = null)
    {
        var model = new RegisterModel();
        model.ReturnUrl = returnUrl;
        
        return View(model);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterModel model)
    {
        //model.ReturnUrl ??= Url.Content("~/");
        
        if (ModelState.IsValid)
        {
            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Gender = model.Gender,
                IsActive = true,
            };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                _logger.LogInformation("User created a new account with password.");

                if(model.RegisterAs == "Teacher")
                    await _userManager.AddToRoleAsync(user, "Teacher");
                else if(model.RegisterAs == "User")
                    await _userManager.AddToRoleAsync(user, "User");

                //var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                //code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                await _signInManager.SignInAsync(user, isPersistent: false);

                if (model.ReturnUrl != null)
                    return LocalRedirect(model.ReturnUrl);
                else
                    return RedirectToAction(nameof(DashboardRedirect));

            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        return View(model);
    }

    public async Task<IActionResult> Login(string? returnUrl = null)
    {
        var model = new LoginModel();
        returnUrl ??= Url.Content("~/");

        await _signInManager.SignOutAsync();
        model.ReturnUrl = returnUrl;
        return View(model);
    }


    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginModel model)
    {
        model.ReturnUrl = model.ReturnUrl == "/" ? null : model.ReturnUrl;

        if (ModelState.IsValid)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password,
                model.RememberMe, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null && user.IsActive)
                {
                    if (!string.IsNullOrWhiteSpace(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }

                    return RedirectToAction(nameof(DashboardRedirect));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "User Unavailable");
                    return View(model);
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Email or Password is not valid");
                return View(model);
            }
        }
        else
        {
            ModelState.AddModelError(string.Empty, "Email or Password is not valid");
            return View(model);
        }
    }


    [Authorize]
    public async Task<IActionResult> DashboardRedirect()
    {
        var user = await _userManager.GetUserAsync(User);
        var role = await _userManager.GetRolesAsync(user);

        if (role.FirstOrDefault() == "Admin")
            return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
        else if (role.FirstOrDefault() == "Teacher")
            return RedirectToAction("Index", "Dashboard", new { area = "Teacher" });
        else if (role.FirstOrDefault() == "User")
            return RedirectToAction("Index", "Dashboard", new { area = "User" });
        else
            return RedirectToAction("Index", "Home");

    }

    public IActionResult AccssDenied()
    {
        return View();
    }
}
