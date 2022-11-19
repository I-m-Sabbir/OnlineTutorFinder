using Microsoft.AspNetCore.Identity;
using OnlineTutorFinder.Web.Entities.Membership;

namespace OnlineTutorFinder.Web.Data.DataSeed
{
    internal static class ApplicationUserSeed
    {
        internal static ApplicationUser[] Users
        {
            get
            {
                var user = new ApplicationUser
                {
                    Id = Guid.Parse("8e445865-a24d-4543-a6c6-9443d048cdb9"),
                    FirstName = "Super Admin",
                    LastName = "",
                    UserName = "SuperAdmin@gmail.com",
                    NormalizedUserName = "SUPERADMIN@GMAIL.COM",
                    Email = "SuperAdmin@gmail.com",
                    NormalizedEmail = "SUPERADMIN@GMAIL.COM",
                    LockoutEnabled = true,
                    Gender = "Male",
                    SecurityStamp = "8e445865-a24d-4543-a6c6-9443d048cdb8",
                    EmailConfirmed = true,
                };
                var password = new PasswordHasher<ApplicationUser>();
                var hashed = password.HashPassword(user, "Asd123!@#");
                user.PasswordHash = hashed;
                return new ApplicationUser[]
                {
                    user
                };
            }
        }
    }
}
