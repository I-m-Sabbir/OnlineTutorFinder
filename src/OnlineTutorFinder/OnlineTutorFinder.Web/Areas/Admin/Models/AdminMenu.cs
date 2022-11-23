using Microsoft.AspNetCore.Http.Features;
using OnlineTutorFinder.Web.Models;

namespace OnlineTutorFinder.Web.Areas.Admin.Models
{
    public class AdminMenu
    {
       public static IList<MenuItem> Items()
        {
            return new List<MenuItem>
            {
                new MenuItem
                {
                    Name = "Profile",
                    Link = "/Profile/Index",
                    Icon = "fas fa-user",
                    HasChild = false,
                },
                
            };
        }
    }
}
