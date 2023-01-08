using Microsoft.EntityFrameworkCore.Storage;
using OnlineTutorFinder.Web.Entities.Membership;

namespace OnlineTutorFinder.Web.Entities
{
    public class Schedule
    {
        public Guid Id { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? Endtime { get; set; }

        public Subject? Subject { get; set; }
        public Guid SubjectId { get; set; }

        public IList<TeachingDays>? TeachingDays { get; set; }
    }
}
