using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using OnlineTutorFinder.Web.Data.DataSeed;
using OnlineTutorFinder.Web.Entities;
using OnlineTutorFinder.Web.Entities.Membership;
using System.Data;
using System.Data.Common;

namespace OnlineTutorFinder.Web.DbContext;

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

        builder.Entity<Enrollment>()
            .HasOne(x => x.EnrollUser)
            .WithMany(p => p.Enrollments)
            .HasForeignKey(k => k.EnrollUserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Entity<Enrollment>()
            .HasOne(x => x.Schedule)
            .WithMany(p => p.Enrollments)
            .HasForeignKey(k => k.ScheduleId)
            .OnDelete(DeleteBehavior.NoAction);

        base.OnModelCreating(builder);
    }

    public DbSet<Subject> Subjects { get; set; }
    public DbSet<Schedule> Schedules { get; set; }
    public DbSet<TeachingDays> TeachingDays { get; set; }
    public DbSet<Enrollment> Enrollments { get; set; }

    //Methods for Executing Query
    internal async Task<(IList<TReturn> result, IDictionary<string, object> outValues)>
            ExecuteQueryAsync<TReturn>(string sql,
            IDictionary<string, object> parameters = null, IDictionary<string, Type> outParameters = null)
            where TReturn : class, new()
    {
        var command = CreateCommand(sql, parameters, outParameters);

        var connectionOpened = false;
        if (command.Connection.State == ConnectionState.Closed)
        {
            await command.Connection.OpenAsync();
            connectionOpened = true;
        }

        IList<TReturn> result = null;
        try
        {
            result = await ExecuteQueryAsync<TReturn>(command);
        }
        finally
        {
            if (connectionOpened)
                await command.Connection.CloseAsync();
        }

        var outValues = CopyOutParams(command, outParameters);

        return (result, outValues);
    }

    private DbCommand CreateCommand(string sql,
            IDictionary<string, object> parameters = null,
            IDictionary<string, Type> outParameters = null)
    {
        var connection = Database.GetDbConnection();
        var command = connection.CreateCommand();
        command.CommandText = sql;
        command.CommandType = CommandType.Text;
        command.CommandTimeout = 300;

        if (parameters != null)
        {
            foreach (var item in parameters)
            {
                command.Parameters.Add(new SqlParameter(item.Key, item.Value));
            }
        }

        if (outParameters != null)
        {
            foreach (var item in outParameters)
            {
                command.Parameters.Add(new SqlParameter(item.Key, item.Value));
            }
        }

        return command;
    }

    private async Task<IList<TReturn>> ExecuteQueryAsync<TReturn>(DbCommand command)
    {
        var reader = await command.ExecuteReaderAsync();
        var result = new List<TReturn>();

        while (await reader.ReadAsync())
        {
            var type = typeof(TReturn);
            var constructor = type.GetConstructor(new Type[] { });
            if (constructor == null)
                throw new InvalidOperationException("An empty contructor is required for the return type");

            var instance = constructor.Invoke(new object[] { });

            for (var i = 0; i < reader.FieldCount; i++)
            {
                var property = type.GetProperty(reader.GetName(i));
                property?.SetValue(instance, ChangeType(property.PropertyType, reader.GetValue(i)));
            }

            result.Add((TReturn)instance);
        }

        return result;
    }

    private object ChangeType(Type propertyType, object itemValue)
    {
        if (itemValue is DBNull)
            return null;

        return itemValue is decimal && propertyType == typeof(double) ?
            Convert.ToDouble(itemValue) : itemValue;
    }

    private IDictionary<string, object> CopyOutParams(DbCommand command,
            IDictionary<string, Type> outParameters)
    {
        Dictionary<string, object> result = null;
        if (outParameters != null)
        {
            result = new Dictionary<string, object>();
            foreach (var item in outParameters)
            {
                result.Add(item.Key, command.Parameters[item.Key].Value);
            }
        }

        return result;
    }
}
