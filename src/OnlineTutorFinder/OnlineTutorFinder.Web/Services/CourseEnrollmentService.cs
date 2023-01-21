using Microsoft.EntityFrameworkCore;
using OnlineTutorFinder.Web.DbContext;
using OnlineTutorFinder.Web.Entities;
using OnlineTutorFinder.Web.Services.DTO;

namespace OnlineTutorFinder.Web.Services;

public interface ICourseEnrollmentService
{
    Task RequestEnrollAsync(Guid scheduleId, Guid userId);
    Task<IList<EnrollmentModelDto>> GetPendingEnrollmentsAsync(Guid userId);
}

public class CourseEnrollmentService : ICourseEnrollmentService
{
    private readonly ApplicationDbContext _context;

    public CourseEnrollmentService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task RequestEnrollAsync(Guid scheduleId, Guid userId)
    {
        try
        {
            if (scheduleId == Guid.Empty && userId == Guid.Empty)
                throw new Exception("No Schedule or Member Found.");

            var entity = new Enrollment
            {
                ScheduleId = scheduleId,
                EnrollUserId = userId,
                CreateDate = DateTime.Now,
                Status = EnrollStatus.Pendig
            };

            await _context.Enrollments.AddAsync(entity);
            await _context.SaveChangesAsync();
        }
        catch(Exception ex)
        {
            throw;
        }
    }

    public async Task<IList<EnrollmentModelDto>> GetPendingEnrollmentsAsync(Guid userId)
    {
        try
        {
            var entityModel = _context.Enrollments.Where(x => x.EnrollUserId == userId && x.Status == EnrollStatus.Pendig)
                .Include(x => x.Schedule).ThenInclude(c => c.Subject).ThenInclude(p => p.Teacher)
                .Include(x => x.Schedule).ThenInclude(c => c.TeachingDays)
                .ToList();

            var list = new List<EnrollmentModelDto>();

            if(entityModel.Count > 0)
            {
                foreach(var item in entityModel)
                {
                    var model = new EnrollmentModelDto
                    {
                        SubjectName = item.Schedule.Subject!.Name,
                        StartTime = item.Schedule.StartTime,
                        EndTime = item.Schedule.Endtime,
                        TeacherName = $"{item.Schedule.Subject.Teacher.FirstName} {item.Schedule.Subject.Teacher.LastName}",
                        TeachingDays = String.Join(", ", item.Schedule.TeachingDays!.Select(x => x.TeachingDay)),
                    };
                    list.Add(model);
                }
            }

            return list;
        }
        catch(Exception ex)
        {
            throw;
        }
    }

}
