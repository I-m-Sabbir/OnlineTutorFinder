using Microsoft.AspNetCore.Mvc;
using OnlineTutorFinder.Web.Areas.Teacher.Models;
using OnlineTutorFinder.Web.Extensions;
using OnlineTutorFinder.Web.Models;
using OnlineTutorFinder.Web.Services;
using OnlineTutorFinder.Web.Services.Membership;

namespace OnlineTutorFinder.Web.Areas.Teacher.Controllers;

public class CourseEnrollmentController : TeacherBaseController<CourseEnrollmentController>
{
    private readonly ICourseEnrollmentService _courseEnrollmentService;
    private readonly UserManager _userManager;

    public CourseEnrollmentController(ILogger<CourseEnrollmentController> logger, ICourseEnrollmentService courseEnrollmentService, UserManager userManager)
        : base(logger)
    {
        _courseEnrollmentService = courseEnrollmentService;
        _userManager = userManager;

    }

    public async Task<IActionResult> Index()
    {
        var model = new RequestEnrollmentModel();
        try
        {
            var user = await _userManager.FindByNameAsync(User.Identity!.Name);
            if (user == null)
                throw new Exception("Invalid User.");
            else
            {
                var result = await _courseEnrollmentService.GetRequestedEnrollmentsAsync(user.Id);
                model.Map(result);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);

            TempData.Put<ResponseModel>("ResponseMessage", new ResponseModel
            {
                Message = "Something Went Wrong. Please Try Again.",
                Type = ResponseTypes.Danger
            });
        }

        return View(model);
    }

    public async Task<IActionResult> AcceptRequest(Guid id)
    {
        try
        {
            if (id == Guid.Empty)
                throw new Exception("Invalid Request Selected.");

            await _courseEnrollmentService.AcceptEnrollRequest(id);

            TempData.Put<ResponseModel>("ResponseMessage", new ResponseModel
            {
                Message = "Request Accepted.",
                Type = ResponseTypes.Success
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);

            TempData.Put<ResponseModel>("ResponseMessage", new ResponseModel
            {
                Message = ex.Message,
                Type = ResponseTypes.Danger
            });
        }

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> RejectRequest(Guid id)
    {
        try
        {
            if (id == Guid.Empty)
                throw new Exception("Invalid Request Selected.");

            await _courseEnrollmentService.RejectEnrollRequest(id);

            TempData.Put<ResponseModel>("ResponseMessage", new ResponseModel
            {
                Message = "Request Rejected.",
                Type = ResponseTypes.Success
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);

            TempData.Put<ResponseModel>("ResponseMessage", new ResponseModel
            {
                Message = ex.Message,
                Type = ResponseTypes.Danger
            });
        }

        return RedirectToAction(nameof(Index));
    }
}
