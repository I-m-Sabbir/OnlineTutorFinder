using Microsoft.AspNetCore.Mvc;
using OnlineTutorFinder.Web.Areas.Teacher.Models.PostModels;

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

        public IActionResult CreatePost()
        {
            var model = new AddSubjectScheduleModel();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePost(AddSubjectScheduleModel model)
        {
            return View();
        }
    }
}
