namespace OnlineTutorFinder.Web.Services.DTO;

public class EnrollmentRequestedDto
{
    public Guid Id { get; set; }
    public string? SubjectName { get; set; }
    public TimeSpan? StartTime { get; set; }
    public TimeSpan? EndTime { get; set; }
    public string TeachingDays { get; set; } = null!;
    public string RequestedBy { get; set; } = null!;
    public string? Address { get; set; }
}
