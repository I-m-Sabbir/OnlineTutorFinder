using OnlineTutorFinder.Web.Models;

namespace OnlineTutorFinder.Web.Areas.Teacher.Models
{
    public class TeacherMenu
    {
        public static IList<MenuItem> Items()
        {
            return new List<MenuItem>
            {
                new MenuItem
                {
                    Name = "Profile",
                    Link = "/Teacher/Profile/Index",
                    Icon = "fas fa-user",
                    HasChild = false,
                },
                new MenuItem
                {
                    Name = "Tution",
                    Link = "/Teacher/Post/Index",
                    Icon = "fas fa-book",
                    HasChild = false,
                },
            };
        }
    }
}
