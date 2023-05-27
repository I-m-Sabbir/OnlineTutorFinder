﻿using Microsoft.AspNetCore.Mvc;
using OnlineTutorFinder.Web.Areas.User.Models;
using OnlineTutorFinder.Web.Extensions;
using OnlineTutorFinder.Web.Models;
using OnlineTutorFinder.Web.Services;
using OnlineTutorFinder.Web.Services.Membership;

namespace OnlineTutorFinder.Web.Areas.User.Controllers;

public class EnrolledCoursesController : UserBaseController<EnrolledCoursesController>
{
    private readonly ICourseEnrollmentService _enrollmentService;
    private readonly UserManager _userManager;
    public EnrolledCoursesController(ILogger<EnrolledCoursesController> logger, ICourseEnrollmentService enrollmentService,
        UserManager userManager)
        : base(logger)
    {
        _enrollmentService = enrollmentService;
        _userManager = userManager;
    }

    public async Task<IActionResult> PendingCourses()
    {
        var model = new EnrolledCoursesModel();
        try
        {
            var user = await _userManager.FindByNameAsync(User.Identity!.Name);
            if (user == null)
                throw new Exception("Invalid User.");
            else
            {
                var result = await _enrollmentService.GetPendingEnrollmentsAsync(user.Id);
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

    public async Task<IActionResult> CencelRequest(Guid id)
    {
        try
        {
            if (id == Guid.Empty)
                throw new Exception("Invalid Request Selected.");

            await _enrollmentService.CencelEnrollmentRequestAsync(id);

            TempData.Put<ResponseModel>("ResponseMessage", new ResponseModel
            {
                Message = "Request Cenceled.",
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

        return RedirectToAction(nameof(PendingCourses));
    }

    public async  Task<IActionResult> EnrolledCourses()
    {
        var model = new EnrolledCoursesModel();
        try
        {
            var user = await _userManager.FindByNameAsync(User.Identity!.Name);
            if (user == null)
                throw new Exception("Invalid User.");
            else
            {
                var result = await _enrollmentService.GetAcceptedEnrollmentsAsync(user.Id);
                model.Map(result);
            }
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
        return View(model);
    }
}
