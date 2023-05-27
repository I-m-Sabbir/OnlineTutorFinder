using Microsoft.EntityFrameworkCore;
using OnlineTutorFinder.Web.DbContext;
using OnlineTutorFinder.Web.Entities;
using OnlineTutorFinder.Web.Extensions;
using OnlineTutorFinder.Web.Services.DTO;

namespace OnlineTutorFinder.Web.Services;

public interface ICourseEnrollmentService
{
    Task RequestEnrollAsync(Guid scheduleId, Guid userId);
    Task<IList<EnrollmentModelDto>> GetPendingEnrollmentsAsync(Guid userId);
    Task<IList<EnrollmentModelDto>> GetAcceptedEnrollmentsAsync(Guid userId);
    Task<IList<EnrollmentRequestedDto>> GetRequestedEnrollmentsAsync(Guid teacherId);
    Task CencelEnrollmentRequestAsync(Guid id);
    Task AcceptEnrollRequest(Guid id);
    Task RejectEnrollRequest(Guid id);
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

            var count = await _context.Enrollments.Where(e => e.ScheduleId == scheduleId && e.EnrollUserId == userId).CountAsync();
            if (count > 0)
                throw new DuplicateException("Already Requested.");

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
        catch (DuplicateException)
        {
            throw;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<IList<EnrollmentModelDto>> GetPendingEnrollmentsAsync(Guid userId)
    {
        try
        {
            if (userId == Guid.Empty)
                throw new Exception("Invalid User.");

            var entityModel = await _context.Enrollments.Where(x => x.EnrollUserId == userId && x.Status == EnrollStatus.Pendig)
                .Include(x => x.Schedule).ThenInclude(c => c.Subject).ThenInclude(p => p.Teacher)
                .Include(x => x.Schedule).ThenInclude(c => c.TeachingDays)
                .ToListAsync();

            var list = new List<EnrollmentModelDto>();

            if (entityModel.Count > 0)
            {
                foreach (var item in entityModel)
                {
                    var model = new EnrollmentModelDto
                    {
                        Id = item.Id,
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
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<IList<EnrollmentModelDto>> GetAcceptedEnrollmentsAsync(Guid userId)
    {
        try
        {
            if (userId == Guid.Empty)
                throw new Exception("Invalid User.");

            var entityModel = await _context.Enrollments.Where(x => x.EnrollUserId == userId && x.Status == EnrollStatus.Accepted)
                .Include(x => x.Schedule).ThenInclude(c => c.Subject).ThenInclude(p => p.Teacher)
                .Include(x => x.Schedule).ThenInclude(c => c.TeachingDays)
                .ToListAsync();

            var list = new List<EnrollmentModelDto>();

            if (entityModel.Count > 0)
            {
                foreach (var item in entityModel)
                {
                    var model = new EnrollmentModelDto
                    {
                        Id = item.Id,
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
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<IList<EnrollmentRequestedDto>> GetRequestedEnrollmentsAsync(Guid teacherId)
    {
        try
        {
            if (teacherId == Guid.Empty)
                throw new Exception("Invalid Teacher.");

            var result = await _context.Enrollments.Where(x => x.Schedule!.Subject!.TeacherId == teacherId && x.Status == EnrollStatus.Pendig)
                .Include(x => x.Schedule).ThenInclude(s => s.Subject)
                .Include(x => x.Schedule).ThenInclude(s => s.TeachingDays)
                .Include(x => x.EnrollUser).ToListAsync();

            var list = new List<EnrollmentRequestedDto>();
            if (result.Count > 0)
            {
                foreach (var item in result)
                {
                    var model = new EnrollmentRequestedDto
                    {
                        Id = item.Id,
                        SubjectName = item.Schedule.Subject!.Name,
                        StartTime = item.Schedule.StartTime,
                        EndTime = item.Schedule.Endtime,
                        TeachingDays = string.Join(", ", item.Schedule.TeachingDays!.Select(t => t.TeachingDay)),
                        RequestedBy = $"{item.EnrollUser.FirstName} {item.EnrollUser.LastName}",
                        Address = item.EnrollUser.Address
                    };

                    list.Add(model);
                }

            }

            return list;
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public async Task CencelEnrollmentRequestAsync(Guid id)
    {
        try
        {
            if (id == Guid.Empty)
                throw new Exception("Invalid Request Selected.");

            var result = await _context.Enrollments.Where(x => x.Id == id && x.Status == EnrollStatus.Pendig).FirstOrDefaultAsync();

            if (result == null)
                throw new Exception("Invalid Request Selected.");

            _context.Remove(result);
            await _context.SaveChangesAsync();
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task AcceptEnrollRequest(Guid id)
    {
        try
        {
            if (id == Guid.Empty)
                throw new Exception("Invalid Enroll Request Selected");

            var result = await _context.Enrollments.Where(x => x.Id == id && x.Status == EnrollStatus.Pendig).FirstOrDefaultAsync();
            if (result == null)
                throw new Exception("Invalid Enrollment Request Selected.");

            result.Status = EnrollStatus.Accepted;

            if (_context.Entry(result).State == EntityState.Detached)
                _context.Enrollments.Attach(result);
            _context.Entry(result).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task RejectEnrollRequest(Guid id)
    {
        try
        {
            if (id == Guid.Empty)
                throw new Exception("Invalid Enroll Request Selected");

            var result = await _context.Enrollments.Where(x => x.Id == id && x.Status == EnrollStatus.Pendig).FirstOrDefaultAsync();
            if (result == null)
                throw new Exception("Invalid Enrollment Request Selected.");

            result.Status = EnrollStatus.Rejected;

            if (_context.Entry(result).State == EntityState.Detached)
                _context.Enrollments.Attach(result);
            _context.Entry(result).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
        catch (Exception)
        {
            throw;
        }
    }

}
