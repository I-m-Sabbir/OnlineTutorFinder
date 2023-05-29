using Microsoft.AspNetCore.Authorization;
using OnlineTutorFinder.Web.Services.DTO;

namespace OnlineTutorFinder.Web.Areas.Teacher.Models.PostModels;

[Authorize(Roles = "Teacher")]
public class PostModel : TeacherBaseModel
{
    public Guid SubjectId { get; set; }
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
                SubjectId = post.SubjectId,
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
}

