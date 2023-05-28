using Microsoft.AspNetCore.Mvc;
using OnlineTutorFinder.Web.Extensions;
using OnlineTutorFinder.Web.Models;
using OnlineTutorFinder.Web.Services;

namespace OnlineTutorFinder.Web.Areas.Admin.Controllers;

public class DashboardController : AdminBaseController<DashboardController>
{
    private readonly IUserManagement _userManagement;

    public DashboardController(ILogger<DashboardController> logger, IUserManagement userManagement)
        : base(logger)
    {
        _userManagement = userManagement;
    }
    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> GetCounts()
    {
        bool isSuccess = false;
        int totalUser = 0;
        int totalTeacher = 0;
        try
        {
            (totalUser, totalTeacher) = await _userManagement.GetUserCountAsync();
            isSuccess = true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);

            TempData.Put<ResponseModel>("ResponseMessage", new ResponseModel
            {
                Message = "Something Went Wrong. Could Not Get Counts. Please Try Again.",
                Type = ResponseTypes.Danger
            });
        }

        return Json(new { isSuccess, totalUser, totalTeacher });
    }
}
