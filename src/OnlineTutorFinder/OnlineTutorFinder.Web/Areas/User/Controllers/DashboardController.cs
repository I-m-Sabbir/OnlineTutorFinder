using Microsoft.AspNetCore.Mvc;
using OnlineTutorFinder.Web.Areas.User.Models;
using OnlineTutorFinder.Web.Services;

namespace OnlineTutorFinder.Web.Areas.User.Controllers;

public class DashboardController : UserBaseController<DashboardController>
{
    private readonly IPostService _postService;

    public DashboardController(ILogger<DashboardController> logger, IPostService postService)
        : base(logger)
    {
        _postService = postService;
    }

    public async Task<IActionResult> Index()
    {
        var model = new PostModel();
        try
        {
            var list = await _postService.GetPosts();
            model.Map(list);
            model.ReturnURL = $"{Url.Action(nameof(Index),"DashBoard")}";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }

        return View(model);
    }
}
