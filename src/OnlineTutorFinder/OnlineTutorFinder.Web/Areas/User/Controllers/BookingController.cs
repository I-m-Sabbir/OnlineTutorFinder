using Microsoft.AspNetCore.Mvc;
using OnlineTutorFinder.Web.Areas.User.Models;
using OnlineTutorFinder.Web.Extensions;
using OnlineTutorFinder.Web.Models;
using OnlineTutorFinder.Web.Services;
using OnlineTutorFinder.Web.Services.Membership;

namespace OnlineTutorFinder.Web.Areas.User.Controllers;

public class BookingController : UserBaseController<BookingController>
{
    private readonly IPostService _postService;
    private readonly UserManager _userManager;
    private readonly ICourseEnrollmentService _enrollmentService;
    public BookingController(ILogger<BookingController> logger, IPostService postService, 
        UserManager userManager, ICourseEnrollmentService enrollmentService)
        : base(logger)
    {
        _postService = postService;
        _userManager = userManager;
        _enrollmentService = enrollmentService;
    }
    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> RequestEnroll(Guid id, string returnUlr = "/User/EnrolledCourses/PendingCourses")
    {
        var model = new PostModel();
        try
        {
            if (id == Guid.Empty)
                throw new Exception("No Schedule Selected for Booking.");

            var result = await _postService.GetPosts(x => x.Id == id);
            model.Map(result.FirstOrDefault()!);
            model.ReturnURL = returnUlr;
        }
        catch (Exception ex)
        {
            TempData.Put<ResponseModel>("ResponseMessage", new ResponseModel
            {
                Message = "Something Went Wrong. Please Try Again.",
                Type = ResponseTypes.Danger
            });

            _logger.LogError(ex, ex.Message);
        }

        return View(model);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Enroll(Guid scheduleId, string returnUlr = "/User/EnrolledCourses/PendingCourses")
    {
        try
        {
            var user =  await _userManager.FindByNameAsync(User.Identity!.Name);

            if (user != null)
                await _enrollmentService.RequestEnrollAsync(scheduleId, user.Id);
            else
                throw new Exception("Invalid User..");

            TempData.Put<ResponseModel>("ResponseMessage", new ResponseModel
            {
                Message = "Enroll Request Sent.",
                Type = ResponseTypes.Success
            });
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, ex.Message);

            TempData.Put<ResponseModel>("ResponseMessage", new ResponseModel
            {
                Message = "Enroll Request Can not be Sent.",
                Type = ResponseTypes.Danger
            });
        }

        return Redirect(returnUlr);
    }
}
