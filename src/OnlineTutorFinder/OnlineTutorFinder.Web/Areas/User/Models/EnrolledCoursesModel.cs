using Microsoft.AspNetCore.Authorization;
using OnlineTutorFinder.Web.Services.DTO;

namespace OnlineTutorFinder.Web.Areas.User.Models;

public class EnrolledCoursesModel : UserBaseModel
{
    public Guid Id { get; set; }
    public string? SubjectName { get; set; }
    public TimeSpan? StartTime { get; set; }
    public TimeSpan? EndTime { get; set; }
    public string? TeacherName { get; set; }
    public string? TeachingDays { get; set; }
    public string StartTimeText {
        get {
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
            if(EndTime.HasValue)
            {
                result = TimeOnly.FromTimeSpan(EndTime.Value).ToString("hh:mm tt");
            }

            return result;
        }
    }

    public IList<EnrolledCoursesModel>? EnrolledCourses { get; set; }

    internal void Map(IList<EnrollmentModelDto> dtos)
    {
        if(dtos.Count > 0)
        {
            EnrolledCourses = new List<EnrolledCoursesModel>();
            foreach(var item in dtos)
            {
                var model = new EnrolledCoursesModel
                {
                    Id = item.Id,
                    SubjectName = item.SubjectName,
                    StartTime = item.StartTime,
                    EndTime = item.EndTime,
                    TeacherName = item.TeacherName,
                    TeachingDays = item.TeachingDays
                };

                EnrolledCourses!.Add(model);
            }
        }
    }

}
