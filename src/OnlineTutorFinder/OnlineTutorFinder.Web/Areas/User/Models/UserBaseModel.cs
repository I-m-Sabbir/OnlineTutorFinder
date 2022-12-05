using Microsoft.AspNetCore.Authorization;
using OnlineTutorFinder.Web.Models;

namespace OnlineTutorFinder.Web.Areas.User.Models
{
    [Authorize(Roles = "User")]
    public class UserBaseModel
    {
        public static IList<MenuItem>? Menu { get; set; } = UserMenu.Items();
    }
}
