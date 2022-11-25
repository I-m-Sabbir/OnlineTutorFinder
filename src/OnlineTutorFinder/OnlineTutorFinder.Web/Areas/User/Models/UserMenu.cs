using OnlineTutorFinder.Web.Models;

namespace OnlineTutorFinder.Web.Areas.User.Models
{
    public class UserMenu
    {
        public static IList<MenuItem> Items()
        {
            return new List<MenuItem>
            {
                new MenuItem
                {
                    Name = "Profile",
                    Link = "/User/Profile/Index",
                    Icon = "fas fa-user",
                    HasChild = false,
                },
            };
        }
    }
}
