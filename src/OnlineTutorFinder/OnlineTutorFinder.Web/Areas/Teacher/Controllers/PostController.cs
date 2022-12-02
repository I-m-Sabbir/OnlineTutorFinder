using Microsoft.AspNetCore.Mvc;
using OnlineTutorFinder.Web.Areas.Teacher.Models.PostModels;
using OnlineTutorFinder.Web.Services;
using OnlineTutorFinder.Web.Services.Membership;

namespace OnlineTutorFinder.Web.Areas.Teacher.Controllers
{
    public class PostController : TeacherBaseController<PostController>
    {
        private readonly UserManager _userManager;
        private readonly IPostService _postService;

        public PostController(ILogger<PostController> logger, UserManager userManager,
            IPostService postService)
            : base(logger)
        {
            _userManager = userManager;
            _postService = postService;
        }

        public async Task<IActionResult> Index()
        {
            var list = new PostModel();
            try
            {
                list.PostModels = await _postService.GetPosts();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            return View(list);
        }

        public IActionResult CreatePost()
        {
            var model = new AddSubjectScheduleModel();
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePost(AddSubjectScheduleModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _userManager.FindByNameAsync(User.Identity!.Name);
                    model.TeacherId = user.Id;
                    await _postService.SavePostAsync(model);

                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);

                }
            }
            return View();
        }
    }
}
