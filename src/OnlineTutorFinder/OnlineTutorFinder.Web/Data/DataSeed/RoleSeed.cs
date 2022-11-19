using OnlineTutorFinder.Web.Entities.Membership;

namespace OnlineTutorFinder.Web.Data.DataSeed
{
    public static class RoleSeed
    {
        public static Role[] Roles
        {
            get
            {
                return new Role[]
                {
                    new Role{ Id = Guid.Parse("2c5e174e-3b0e-446f-86af-483d56fd7210"), Name = "Admin", NormalizedName = "ADMIN", ConcurrencyStamp =  DateTime.Now.Ticks.ToString()  },
                    new Role{ Id = Guid.Parse("2c5e174e-3b0e-446f-86af-483d56fd7211"), Name = "Teacher", NormalizedName = "TEACHER", ConcurrencyStamp =  DateTime.Now.AddMinutes(1).Ticks.ToString()  },
                    new Role{ Id = Guid.Parse("2c5e174e-3b0e-446f-86af-483d56fd7212"), Name = "User", NormalizedName = "USER", ConcurrencyStamp =  DateTime.Now.AddMinutes(2).Ticks.ToString()  }
                };
            }
        }
    }
}
