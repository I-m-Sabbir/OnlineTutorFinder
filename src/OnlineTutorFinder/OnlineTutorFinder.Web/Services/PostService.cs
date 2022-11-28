using Microsoft.EntityFrameworkCore;
using OnlineTutorFinder.Web.Areas.Teacher.Models.PostModels;
using OnlineTutorFinder.Web.DbContext;
using OnlineTutorFinder.Web.Entities;
using System.Linq.Expressions;

namespace OnlineTutorFinder.Web.Services
{
    public interface IPostService
    {
        Task<IList<PostModel>> GetPosts(Expression<Func<Schedule, bool>> fiter = null!);
    }

    public class PostService : IPostService
    {
        private readonly ApplicationDbContext _context;

        public PostService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IList<PostModel>> GetPosts(Expression<Func<Schedule, bool>> fiter = null!)
        {
            IQueryable<Schedule> schedules = _context.Schedules;

            if (fiter != null)
                schedules = schedules.Where(fiter);

            schedules = schedules.Include(x => x.TeachingDays).Include(p => p.Subject);

            var results = await schedules.ToListAsync();

            if (results.Count == 0)
                throw new NullReferenceException();

            var posts = new List<PostModel>();
            foreach (var item in results)
            {
                var post = new PostModel
                {
                    StartTime = item.StartTime,
                    EndTime = item.Endtime,
                    SubjectName = item.Subject!.Name,
                    TeachingDays = string.Join('-', item.TeachingDays!.Select(x => x.TeachingDay))
                };

                posts.Add(post);
            }

            return posts;
        }

        public async Task SavePostAsync(AddSubjectScheduleModel model)
        {
            try
            {
                var teachingDays = new List<TeachingDays>();
                
                foreach(var item in model.DayOfWeeks)
                {
                    teachingDays.Add(new TeachingDays { TeachingDay = item });
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

                await _context.Subjects.AddAsync(subject);
                await _context.SaveChangesAsync();
                
            }

            catch (Exception ex)
            {
                throw;
            }
        }



    }
}
