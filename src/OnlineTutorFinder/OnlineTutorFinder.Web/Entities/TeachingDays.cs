namespace OnlineTutorFinder.Web.Entities;

public class TeachingDays
{
    public Guid Id { get; set; }
    public DayOfWeek TeachingDay { get; set; }

    public Schedule? Schedule { get; set; }
    public Guid ScheduleId { get; set; }
}
