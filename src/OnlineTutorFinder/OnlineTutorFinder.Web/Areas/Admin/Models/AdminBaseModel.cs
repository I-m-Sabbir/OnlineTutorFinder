using Microsoft.AspNetCore.Authorization;
using OnlineTutorFinder.Web.Models;

namespace OnlineTutorFinder.Web.Areas.Admin.Models
{
    [Authorize(Roles = "Admin")]
    
    public class AdminBaseModel
    {
        public static IList<MenuItem>? Menu { get; set; } = AdminMenu.Items();
    }
}
