using Microsoft.AspNetCore.Identity;

namespace OnlineTutorFinder.Web.Entities.Membership
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Gender { get; set; }
        public string? ProfilePicture { get; set; }
        public string? Address { get; set; }
        public bool IsActive { get; set; }

        public IList<Subject>? Subjects { get; set; }
    }
}
