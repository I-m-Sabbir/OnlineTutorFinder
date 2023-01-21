using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OnlineTutorFinder.Web.Areas.Teacher.Controllers;

[Area("Teacher")]
[Authorize(Roles = "Teacher")]
public class TeacherBaseController<TController> : Controller
    where TController : Controller
{
    protected readonly ILogger<TController> _logger;

    public TeacherBaseController(ILogger<TController> logger)
    {
        _logger = logger;
    }

}
