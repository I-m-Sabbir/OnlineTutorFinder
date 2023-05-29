using Microsoft.AspNetCore.Mvc;
using OnlineTutorFinder.Web.Services;
using OnlineTutorFinder.Web.Services.DTO;

namespace OnlineTutorFinder.Web.Areas.Admin.Controllers
{
    public class ManageUserController : AdminBaseController<ManageUserController>
    {
        private readonly IUserManagement _userManagement;

        public ManageUserController(ILogger<ManageUserController> logger, IUserManagement userManagement)
            : base(logger)
        {
            _userManagement = userManagement;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetUsers(int draw, int start, int length)
        {
            bool isSuccess = false;
            string message = string.Empty;
            var data = new List<object>();
            int serial = start + 1;
            int recordsTotal = 0;
            int recordsFiltered = 0;
            string search = HttpContext.Request?.Query["search[value]"].ToString() ?? string.Empty;

            try
            {
                var result = await _userManagement.GetUsers(search, length, start + 1);
                result = result ?? new List<ApplicationUserDto>();
                recordsTotal = result.Count > 0 ? result.FirstOrDefault()!.TotalRecord : 0;
                recordsFiltered = recordsTotal;
                foreach (var item in result)
                {
                    var row = new List<string>
                {
                    serial++.ToString(),
                    $"{item.FirstName} {item.LastName}",
                    item.Email ?? "-",
                    $"{item.Id}"
                };

                    data.Add(row);
                }


                isSuccess = true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                isSuccess = false;
                message = "Something Went Wrong. Please Try Again Later.";
            }

            return Json(new { data, recordsTotal, recordsFiltered, isSuccess, message });
        }
    }
}
