namespace OnlineTutorFinder.Web.Areas.Teacher.Models.PostModels
{
    public class PostModel
    {
        public string? SubjectName { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public string? TeachingDays { get; set; }
    }
}
