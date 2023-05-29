using Microsoft.AspNetCore.Mvc;
using OnlineTutorFinder.Web.Areas.Teacher.Models;
using OnlineTutorFinder.Web.Areas.Teacher.Models.PostModels;
using OnlineTutorFinder.Web.Extensions;
using OnlineTutorFinder.Web.Models;
using OnlineTutorFinder.Web.Services;
using OnlineTutorFinder.Web.Services.Membership;

namespace OnlineTutorFinder.Web.Areas.Teacher.Controllers;

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
        var model = new PostModel();
        try
        {
            var user = await _userManager.GetUserAsync(User);
            var list = await _postService.GetPosts(x => x.Subject!.TeacherId == user.Id);
            model.Map(list);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
        return View(model);
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
                var user = await _userManager.GetUserAsync(User);
                model.TeacherId = user.Id;
                await _postService.SavePostAsync(model);

                TempData.Put<ResponseModel>("ResponseMessage", new ResponseModel
                {
                    Message = "SuccessFully Posted.",
                    Type = ResponseTypes.Success
                });

                return RedirectToAction(nameof(Index));
            }
            catch (DuplicateException dx)
            {
                TempData.Put<ResponseModel>("ResponseMessage", new ResponseModel
                {
                    Message = dx.Message,
                    Type = ResponseTypes.Danger
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                TempData.Put<ResponseModel>("ResponseMessage", new ResponseModel
                {
                    Message = "Something Went Wrong! Can not Post.",
                    Type = ResponseTypes.Danger
                });
            }
        }

        return View(model);
    }

    public async Task<IActionResult> Edit(Guid id)
    {
        var model = new AddSubjectScheduleModel();
        try
        {
            if (id == Guid.Empty)
                throw new Exception("Invalid Post Selected.");

            model = await _postService.GetPostByIdAsync(id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }

        return View(model);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(AddSubjectScheduleModel model)
    {
        if (ModelState.IsValid)
        {
            try
            {
                await _postService.UpdatePostAsync(model);

                TempData.Put<ResponseModel>("ResponseMessage", new ResponseModel
                {
                    Message = "Successfully Updated.",
                    Type = ResponseTypes.Success
                });

                return RedirectToAction(nameof(Index));
            }
            catch(DuplicateException ex)
            {
                TempData.Put<ResponseModel>("ResponseMessage", new ResponseModel
                {
                    Message = ex.Message,
                    Type = ResponseTypes.Danger
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                TempData.Put<ResponseModel>("ResponseMessage", new ResponseModel
                {
                    Message = "Can not Update. Please Try Again.",
                    Type = ResponseTypes.Danger
                });
            }
        }
        return View(model);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            if (id == Guid.Empty)
                throw new Exception("Invalid Post Selected.");

            await _postService.DeletePostAsync(id);

            TempData.Put<ResponseModel>("ResponseMessage", new ResponseModel
            {
                Message = "Deleted Successfully.",
                Type = ResponseTypes.Success
            });
        }
        catch (DuplicateException ex)
        {
            TempData.Put<ResponseModel>("ResponseMessage", new ResponseModel
            {
                Message = ex.Message,
                Type = ResponseTypes.Danger
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
