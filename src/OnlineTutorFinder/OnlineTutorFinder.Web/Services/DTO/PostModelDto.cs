namespace OnlineTutorFinder.Web.Services.DTO
{
    public class PostModelDto
    {
        public Guid SubjectId { get; set; }
        public string? SubjectName { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public string? TeachingDays { get; set; }
        public string? Teacher { get; set; }

        public IList<PostModelDto>? PostModels { get; set; }
    }
}
