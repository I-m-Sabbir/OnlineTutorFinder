using OnlineTutorFinder.Web.Services.DTO;

namespace OnlineTutorFinder.Web.Areas.Teacher.Models;

public class RequestEnrollmentModel : TeacherBaseModel
{
    public Guid Id { get; set; }
    public string? SubjectName { get; set; }
    public TimeSpan? StartTime { get; set; }
    public TimeSpan? EndTime { get; set; }
    public string TeachingDays { get; set; } = null!;
    public string RequestedBy { get; set; } = null!;
    public string? Address { get; set; }
    public string StartTimeText
    {
        get
        {
            string result = "-";
            if (StartTime.HasValue)
            {
                result = TimeOnly.FromTimeSpan(StartTime.Value).ToString("hh:mm tt");
            }

            return result;
        }
    }

    public string EndTimeText
    {
        get
        {
            string result = "-";
            if (EndTime.HasValue)
            {
                result = TimeOnly.FromTimeSpan(EndTime.Value).ToString("hh:mm tt");
            }

            return result;
        }
    }

    public IList<RequestEnrollmentModel>? RequestEnrollmentModels { get; set; }

    internal void Map(IList<EnrollmentRequestedDto> dtos)
    {
        RequestEnrollmentModels = new List<RequestEnrollmentModel>();

        if (dtos.Count > 0)
        {
            foreach (var item in dtos)
            {
                var model = new RequestEnrollmentModel
                {
                    Id = item.Id,
                    SubjectName = item.SubjectName,
                    StartTime = item.StartTime,
                    EndTime = item.EndTime,
                    TeachingDays = item.TeachingDays,
                    RequestedBy = item.RequestedBy,
                    Address = item.Address
                };

                RequestEnrollmentModels.Add(model);
            }

        }
    }
}
