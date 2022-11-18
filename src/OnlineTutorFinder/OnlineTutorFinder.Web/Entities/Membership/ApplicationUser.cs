using Microsoft.AspNetCore.Identity;

namespace OnlineTutorFinder.Web.Entities.Membership
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
    }
}
