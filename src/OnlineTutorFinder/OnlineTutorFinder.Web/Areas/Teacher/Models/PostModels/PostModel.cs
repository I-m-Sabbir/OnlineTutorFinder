using Microsoft.AspNetCore.Authorization;

namespace OnlineTutorFinder.Web.Areas.Teacher.Models.PostModels
{
    [Authorize(Roles = "Teacher")]
    public class PostModel : TeacherBaseModel
    {
        public string? SubjectName { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public string? TeachingDays { get; set; }
        public string? Teacher { get; set; }

        public IList<PostModel>? PostModels { get; set; }
    }
}
