using OnlineTutorFinder.Web.Services.Membership;

namespace OnlineTutorFinder.Web.Services;

public interface IUserManagement
{
    Task<(int totalUsers, int totalTeachers)> GetUserCountAsync();
}

public class UserManagement : IUserManagement
{
    private readonly UserManager _userManager;
    private readonly RoleManager _roleManager;

    public UserManagement(UserManager userManager, RoleManager roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<(int totalUsers, int totalTeachers)> GetUserCountAsync()
    {
        try
        {
            var totalUsers = await _userManager.GetUsersInRoleAsync("User");
            var totalTeachers = await _userManager.GetUsersInRoleAsync("Teacher");

            return (totalUsers.Count, totalTeachers.Count);
        }
        catch (Exception)
        {
            throw;
        }
    }
}
