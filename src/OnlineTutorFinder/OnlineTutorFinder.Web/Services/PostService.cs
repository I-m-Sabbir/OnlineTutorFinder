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
    Task<AddSubjectScheduleModel> GetPostByIdAsync(Guid id);
    Task UpdatePostAsync(AddSubjectScheduleModel model);
    Task DeletePostAsync(Guid id);
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

            var count = GetSamePostCount(model.TeacherId, days, model.StartTime, model.EndTime, Guid.Empty);

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

    public async Task<AddSubjectScheduleModel> GetPostByIdAsync(Guid id)
    {
        try
        {
            var result = await _context.Subjects.Where(s => s.Id == id).Include(x => x.SubjectScedules)!.ThenInclude(x => x.TeachingDays)
                .Include(x => x.Teacher).FirstOrDefaultAsync();

            var model = new AddSubjectScheduleModel
            {
                SubjectId = result!.Id,
                SubjectName = result!.Name,
                StartTime = result.SubjectScedules!.First().StartTime ?? TimeSpan.MinValue,
                EndTime = result.SubjectScedules!.First().Endtime ?? TimeSpan.MinValue,
                DayOfWeeks = result.SubjectScedules!.First().TeachingDays!.Select(x => x.TeachingDay).ToList(),
            };

            return model;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task UpdatePostAsync(AddSubjectScheduleModel model)
    {
        try
        {
            var days = new List<int>();
            var teachingDays = new List<TeachingDays>();
            
            foreach (var item in model.DayOfWeeks)
            {
                teachingDays.Add(new TeachingDays { TeachingDay = item });
                days.Add((int)item);
            }

            
            var entity = await _context.Subjects.Where(x => x.Id == model.SubjectId).Include(x => x.SubjectScedules)!.ThenInclude(x => x.TeachingDays)
                .Include(x => x.Teacher).FirstOrDefaultAsync();

            var count = GetSamePostCount(entity.TeacherId, days, model.StartTime, model.EndTime, model.SubjectId);
            if (count > 0)
                throw new DuplicateException("Conflicted Time Detected.");

            entity.Name = model.SubjectName;
            entity.SubjectScedules.FirstOrDefault()!.StartTime = model.StartTime;
            entity.SubjectScedules.FirstOrDefault()!.Endtime = model.EndTime;
            entity.SubjectScedules.FirstOrDefault()!.TeachingDays = teachingDays;

            await _context.SaveChangesAsync();
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task DeletePostAsync(Guid id)
    {
        try
        {
            if (id == Guid.Empty)
                throw new Exception("Invalid Post Selected.");

            var subjectEntity = await _context.Subjects.Where(x => x.Id == id).Include(x => x.SubjectScedules).FirstOrDefaultAsync();
            if (subjectEntity == null)
                throw new Exception("Subject Doesn't Exist.");

            var enrollments = await _context.Enrollments.Where(x => x.ScheduleId == subjectEntity.SubjectScedules.FirstOrDefault().Id && x.Status == EnrollStatus.Accepted).ToListAsync();
            if (enrollments.Count > 0)
                throw new DuplicateException("Can Not Delete Due To Existing Enrollments.");

            var enrollmentsToDelete = await _context.Enrollments.Where(x => x.ScheduleId == subjectEntity.SubjectScedules.FirstOrDefault().Id).ToListAsync();
            _context.RemoveRange(enrollmentsToDelete);

            var teachingDaysToDelete = await _context.TeachingDays.Where(x => x.ScheduleId == subjectEntity.SubjectScedules.FirstOrDefault().Id).ToListAsync();
            _context.RemoveRange(teachingDaysToDelete);

            _context.RemoveRange(subjectEntity.SubjectScedules);
            _context.Remove(subjectEntity);

            await _context.SaveChangesAsync();

        }
        catch(DuplicateException)
        {
            throw;
        }
        catch(Exception)
        {
            throw;
        }
    }

    private int GetSamePostCount(Guid teacherId, List<int> teachingdays, TimeSpan startTime, TimeSpan endTime, Guid subjectId)
    {
        int count = 0;

        string query = $@"
SELECT s.Id
FROM Schedules AS s
INNER JOIN TeachingDays AS td ON td.ScheduleId = s.Id
INNER JOIN Subjects AS sub ON sub.Id = s.SubjectId
INNER JOIN AspNetUsers AS t ON t.Id = sub.TeacherId
WHERE t.Id = '{teacherId}'
AND td.TeachingDay IN ({string.Join(',', teachingdays)})
AND s.StartTime BETWEEN '{startTime}' AND '{endTime}'
AND s.SubjectId != '{subjectId}'
";
        count = _context.Schedules.FromSqlRaw<Schedule>(query).Count();

        return count;
    }
}
