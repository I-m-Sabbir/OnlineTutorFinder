using OnlineTutorFinder.Web.Entities.Membership;

namespace OnlineTutorFinder.Web.Entities
{
    public class Subject
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }

        public ApplicationUser Teacher { get; set; } = null!;
        public Guid TeacherId { get; set; }
        public IList<Schedule>? SubjectScedules { get; set; }
    }
}
