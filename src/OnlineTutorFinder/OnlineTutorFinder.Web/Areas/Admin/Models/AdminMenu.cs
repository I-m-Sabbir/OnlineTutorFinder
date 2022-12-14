using Microsoft.AspNetCore.Authorization;
using OnlineTutorFinder.Web.Models;

namespace OnlineTutorFinder.Web.Areas.Admin.Models
{
    [Authorize(Roles = "Teacher")]
    public class AdminMenu
    {
       public static IList<MenuItem> Items()
        {
            return new List<MenuItem>
            {
                new MenuItem
                {
                    Name = "Profile",
                    Link = "/Admin/Profile/Index",
                    Icon = "fas fa-user",
                    HasChild = false,
                },
                
            };
        }
    }
}
