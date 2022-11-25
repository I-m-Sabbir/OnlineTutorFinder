using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OnlineTutorFinder.Web.Areas.User.Controllers
{
    [Area("User")]
    [Authorize(Roles = "User")]
    public class DashboardController : UserBaseController<DashboardController>
    {
        public DashboardController(ILogger<DashboardController> logger)
            : base(logger)
        {

        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
