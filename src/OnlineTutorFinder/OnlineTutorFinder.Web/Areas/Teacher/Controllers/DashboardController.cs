using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OnlineTutorFinder.Web.Areas.Teacher.Controllers
{
    [Area("Teacher")]
    [Authorize(Roles = "Teacher")]
    public class DashboardController : TeacherBaseController<DashboardController>
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
