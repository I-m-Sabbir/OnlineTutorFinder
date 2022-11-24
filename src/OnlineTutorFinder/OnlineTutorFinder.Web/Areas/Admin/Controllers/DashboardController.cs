using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineTutorFinder.Web.Areas.Admin.Models;

namespace OnlineTutorFinder.Web.Areas.Admin.Controllers
{
    public class DashboardController : AdminBaseController<DashboardController>
    {
        public DashboardController(ILogger<DashboardController> logger)
            :base(logger)
        {
            
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
