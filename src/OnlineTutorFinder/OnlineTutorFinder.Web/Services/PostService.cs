﻿using Microsoft.EntityFrameworkCore;
using OnlineTutorFinder.Web.Areas.Teacher.Models.PostModels;
using OnlineTutorFinder.Web.DbContext;
using OnlineTutorFinder.Web.Entities;
using OnlineTutorFinder.Web.Services.DTO;
using System.Linq.Expressions;

namespace OnlineTutorFinder.Web.Services
{
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

                foreach (var item in model.DayOfWeeks)
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
