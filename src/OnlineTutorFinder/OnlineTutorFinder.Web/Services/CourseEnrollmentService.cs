using OnlineTutorFinder.Web.DbContext;

namespace OnlineTutorFinder.Web.Services
{
    public interface ICourseEnrollmentService
    {

    }

    public class CourseEnrollmentService : ICourseEnrollmentService
    {
        private readonly ApplicationDbContext _context;

        public CourseEnrollmentService(ApplicationDbContext context)
        {
            _context = context;
        }


    }
}
