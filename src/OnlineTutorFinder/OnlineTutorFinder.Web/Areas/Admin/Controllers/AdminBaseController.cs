using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineTutorFinder.Web.Areas.Admin.Models;

namespace OnlineTutorFinder.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AdminBaseController<TController> : Controller
        where TController : Controller
    {
        protected readonly ILogger<TController> _logger;
        public AdminBaseController(ILogger<TController> logger)
        {
            _logger = logger;
        }
    }
}
