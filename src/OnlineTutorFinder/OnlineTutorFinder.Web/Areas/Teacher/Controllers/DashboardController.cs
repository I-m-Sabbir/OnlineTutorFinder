using Microsoft.AspNetCore.Mvc;

namespace OnlineTutorFinder.Web.Areas.Teacher.Controllers;

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
