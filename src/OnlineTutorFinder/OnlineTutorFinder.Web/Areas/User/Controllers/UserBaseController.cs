using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OnlineTutorFinder.Web.Areas.User.Controllers;

[Area("User")]
[Authorize(Roles = "User")]
public class UserBaseController<TController> : Controller
    where TController : Controller
{
    protected readonly ILogger<TController> _logger;
    public UserBaseController(ILogger<TController> logger)
    {
        _logger = logger;
    }
}
