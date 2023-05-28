namespace OnlineTutorFinder.Web.Services;

public static class ServiceRegistrations
{
    public static IServiceCollection GetServices(this IServiceCollection services)
    {
        services.AddScoped<IPostService, PostService>();
        services.AddScoped<ICourseEnrollmentService, CourseEnrollmentService>();
        services.AddScoped<IUserManagement, UserManagement>();

        return services;
    }
}
