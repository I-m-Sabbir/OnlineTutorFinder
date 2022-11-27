using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineTutorFinder.Web.Models.PostModels;

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

        public IActionResult CreateSchedule()
        {
            var model = new AddSubjectScheduleModel();
            return View(model);
        }
    }
}
