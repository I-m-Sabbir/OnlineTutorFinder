using Microsoft.AspNetCore.Mvc;
using OnlineTutorFinder.Web.Services;

namespace OnlineTutorFinder.Web.Areas.Admin.Controllers;

public class ManageTeacherController : AdminBaseController<ManageTeacherController>
{
    private readonly IUserManagement _userManagement;

    public ManageTeacherController(ILogger<ManageTeacherController> logger, IUserManagement userManagement)
        : base(logger)
    {
        _userManagement = userManagement;
    }

    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> GetTeachers(int pageSize, int pageIndex, string searchText)
    {
        bool isSuccess = false;
        string message = string.Empty;

        try
        {
            var data = await _userManagement.GetTeachers(searchText, pageSize, pageIndex);


            isSuccess = true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            isSuccess = false;
            message = "Something Went Wrong. Please Try Again Later.";
        }

        return Json(new { isSuccess, message });
    }

}
