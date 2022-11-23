using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineTutorFinder.Web.Areas.Admin.Models;

namespace OnlineTutorFinder.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AdminBaseController<Tcontroller> : Controller
        where Tcontroller : Controller
    {
        public AdminBaseController()
        {
            ViewBag.Menu = AdminMenu.Items();
        }
    }
}
