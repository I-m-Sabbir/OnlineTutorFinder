using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OnlineTutorFinder.Web.Data.DataSeed;
using OnlineTutorFinder.Web.Entities;
using OnlineTutorFinder.Web.Entities.Membership;
using System.Reflection.Emit;

namespace OnlineTutorFinder.Web.DbContext
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, Role, Guid,
        UserClaim, UserRole, UserLogin, RoleClaim, UserToken>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            #region Seed
            builder.Entity<ApplicationUser>()
                .HasData(ApplicationUserSeed.Users);

            builder.Entity<Role>()
                .HasData(RoleSeed.Roles);

            builder.Entity<UserRole>()
                .HasData(UserRoleSeed.UserRole);
            #endregion

            builder.Entity<ApplicationUser>()
                .HasMany(x => x.Subjects)
                .WithOne(p => p.Teacher)
                .HasForeignKey(f => f.TeacherId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Subject>()
                .HasMany(x => x.SubjectScedules)
                .WithOne(s => s.Subject)
                .HasForeignKey(p => p.SubjectId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Schedule>()
                .HasMany(x => x.TeachingDays)
                .WithOne(s => s.Schedule)
                .HasForeignKey(p => p.ScheduleId)
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(builder);
        }

        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<TeachingDays> TeachingDays { get; set; }
    }
}
