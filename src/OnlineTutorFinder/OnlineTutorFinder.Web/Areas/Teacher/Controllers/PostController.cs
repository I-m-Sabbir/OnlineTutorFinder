using Microsoft.AspNetCore.Mvc;

namespace OnlineTutorFinder.Web.Areas.Teacher.Controllers
{
    public class PostController : TeacherBaseController<PostController>
    {

        public PostController(ILogger<PostController> logger)
            : base(logger)
        {

        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
