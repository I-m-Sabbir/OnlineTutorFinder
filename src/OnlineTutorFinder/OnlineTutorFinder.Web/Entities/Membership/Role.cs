using Microsoft.AspNetCore.Identity;

namespace OnlineTutorFinder.Web.Entities.Membership;

public class Role : IdentityRole<Guid>
{
    public Role()
        : base()
    {
    }

    public Role(string roleName)
        : base(roleName)
    {
    }
}
