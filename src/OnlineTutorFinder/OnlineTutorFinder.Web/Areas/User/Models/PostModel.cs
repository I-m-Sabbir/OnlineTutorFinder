using Microsoft.AspNetCore.Authorization;
using OnlineTutorFinder.Web.Services.DTO;

namespace OnlineTutorFinder.Web.Areas.User.Models;

[Authorize(Roles ="User")]
public class PostModel : UserBaseModel
{
    public Guid ScheduleId { get; set; }
    public string? SubjectName { get; set; }
    public TimeSpan? StartTime { get; set; }
    public TimeSpan? EndTime { get; set; }
    public string? TeachingDays { get; set; }
    public string? Teacher { get; set; }
    public IList<PostModel>? PostModels { get; set; }

    internal void Map(IList<PostModelDto> posts)
    {
        var list = new List<PostModel>();

        foreach (var post in posts)
        {
            var model = new PostModel
            {
                ScheduleId = post.ScheduleId,
                SubjectName = post.SubjectName,
                StartTime = post.StartTime,
                EndTime = post.EndTime,
                TeachingDays = post.TeachingDays,
                Teacher = post.Teacher,
            };
            list.Add(model);
        }

        this.PostModels = list;
    }

    internal void Map(PostModelDto post)
    {
        ScheduleId = post.ScheduleId;
        SubjectName = post.SubjectName;
        StartTime = post.StartTime;
        EndTime = post.EndTime;
        TeachingDays = post.TeachingDays;
        Teacher = post.Teacher;
    }
}
