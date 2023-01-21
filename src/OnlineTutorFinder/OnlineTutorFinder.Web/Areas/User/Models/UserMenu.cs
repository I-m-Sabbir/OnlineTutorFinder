using OnlineTutorFinder.Web.Models;

namespace OnlineTutorFinder.Web.Areas.User.Models;

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
            new MenuItem
            {
                Name = "My Courses",
                Link = "",
                Icon = "fas fa-book",
                HasChild = true,
                ChildItems = new List<ChildItem>
                {
                    new ChildItem{ Name = "Request Pending", Icon = "fa fa-hourglass-half", Link = "/User/EnrolledCourses/PendingCourses"},
                    new ChildItem{ Name = "Enrolled", Icon = "fa fa-check", Link = "/User/EnrolledCourses/EnrolledCourses"},
                }
            },
        };
    }
}
