using Microsoft.AspNetCore.Mvc;

namespace OnlineTutorFinder.Web.Areas.User.Controllers
{
    public class BookingController : UserBaseController<BookingController>
    {
        public BookingController(ILogger<BookingController> logger)
            :base(logger)
        {

        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
