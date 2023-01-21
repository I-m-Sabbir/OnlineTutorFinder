using Microsoft.EntityFrameworkCore;
using OnlineTutorFinder.Web.Areas.Teacher.Models.PostModels;
using OnlineTutorFinder.Web.DbContext;
using OnlineTutorFinder.Web.Entities;
using OnlineTutorFinder.Web.Extensions;
using OnlineTutorFinder.Web.Services.DTO;
using System.Linq.Expressions;

namespace OnlineTutorFinder.Web.Services;

public interface IPostService
{
    Task<IList<PostModelDto>> GetPosts(Expression<Func<Schedule, bool>> fiter = null!);
    Task SavePostAsync(AddSubjectScheduleModel model);
}

public class PostService : IPostService
{
    private readonly ApplicationDbContext _context;

    public PostService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IList<PostModelDto>> GetPosts(Expression<Func<Schedule, bool>> fiter = null!)
    {
        IQueryable<Schedule> schedules = _context.Schedules;

        if (fiter != null)
            schedules = schedules.Where(fiter);

        schedules = schedules.Include(x => x.TeachingDays)
            .Include(p => p.Subject).ThenInclude(t => t.Teacher);

        var results = await schedules.ToListAsync();

        var posts = new List<PostModelDto>();

        if (results.Count > 0)
        {
            foreach (var item in results)
            {
                var post = new PostModelDto
                {
                    ScheduleId = item.Id,
                    SubjectId = item.SubjectId,
                    Teacher = $"{item.Subject!.Teacher!.FirstName} {item.Subject.Teacher.LastName}",
                    StartTime = item.StartTime,
                    EndTime = item.Endtime,
                    SubjectName = item.Subject!.Name,
                    TeachingDays = string.Join('-', item.TeachingDays!.Select(x => x.TeachingDay))
                };

                posts.Add(post);
            }
        }

        return posts;
    }

    public async Task SavePostAsync(AddSubjectScheduleModel model)
    {
        try
        {
            var teachingDays = new List<TeachingDays>();
            var days = new List<int>();

            foreach (var item in model.DayOfWeeks)
            {
                teachingDays.Add(new TeachingDays { TeachingDay = item });
                days.Add((int)item);
            }

            var subject = new Subject
            {
                Name = model.SubjectName,
                TeacherId = model.TeacherId,
                SubjectScedules = new List<Schedule>()
                {
                    new Schedule
                    {
                        StartTime = model.StartTime,
                        Endtime = model.EndTime,
                        TeachingDays = teachingDays,
                    }
                }

            };
                            
            string query = $@"
SELECT s.Id
FROM Schedules AS s
INNER JOIN TeachingDays AS td ON td.ScheduleId = s.Id
INNER JOIN Subjects AS sub ON sub.Id = s.SubjectId
INNER JOIN AspNetUsers AS t ON t.Id = sub.TeacherId
WHERE t.Id = '{model.TeacherId}'
AND td.TeachingDay IN ({string.Join(',', days)})
AND s.StartTime BETWEEN '{model.StartTime}' AND '{model.EndTime}'
";
            var count = _context.Schedules.FromSqlRaw<Schedule>(query).Count();

            if (count > 0)
                throw new DuplicateException("An Schedule with Same Day and Time Already Exist.");

            await _context.Subjects.AddAsync(subject);
            await _context.SaveChangesAsync();

        }

        catch (Exception)
        {
            throw;
        }
    }

}
