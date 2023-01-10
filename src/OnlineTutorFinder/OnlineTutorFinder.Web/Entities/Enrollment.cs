using OnlineTutorFinder.Web.Entities.Membership;

namespace OnlineTutorFinder.Web.Entities
{
    public class Enrollment
    {
        public Guid Id { get; set; }
        public DateTime CreateDate { get; set; }
        public ApplicationUser EnrollUser { get; set; } = null!;
        public Guid EnrollUserId { get; set; }
        public Schedule Schedule { get; set; } = null!;
        public Guid ScheduleId { get; set; }
        public EnrollStatus Status { get; set; }
    }

    public enum EnrollStatus
    {
        Pendig = 0,
        Accepted = 1,
        Rejected = 2,
    }
}
