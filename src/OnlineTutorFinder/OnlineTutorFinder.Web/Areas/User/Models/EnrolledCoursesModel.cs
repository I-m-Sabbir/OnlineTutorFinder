using Microsoft.AspNetCore.Authorization;
using OnlineTutorFinder.Web.Services.DTO;

namespace OnlineTutorFinder.Web.Areas.User.Models;

[Authorize(Roles = "User")]
public class EnrolledCoursesModel : UserBaseModel
{
    public string? SubjectName { get; set; }
    public TimeSpan? StartTime { get; set; }
    public TimeSpan? EndTime { get; set; }
    public string? TeacherName { get; set; }
    public string? TeachingDays { get; set; }

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
