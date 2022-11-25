using Microsoft.AspNetCore.Authorization;
using OnlineTutorFinder.Web.Models;

namespace OnlineTutorFinder.Web.Areas.Teacher.Models
{
    [Authorize(Roles = "Teacher")]
    public class TeacherBaseModel
    {
        public static IList<MenuItem>? Menu { get; set; } = TeacherMenu.Items();
    }
}
