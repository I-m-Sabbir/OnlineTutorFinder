using Microsoft.Data.SqlClient;
using Microsoft.Data.SqlClient.DataClassification;
using Microsoft.EntityFrameworkCore;
using OnlineTutorFinder.Web.DbContext;
using OnlineTutorFinder.Web.Services.DTO;
using OnlineTutorFinder.Web.Services.Membership;
using System.Data.Common;
using System.Data;

namespace OnlineTutorFinder.Web.Services;

public interface IUserManagement
{
    Task<(int totalUsers, int totalTeachers)> GetUserCountAsync();
    Task<IList<ApplicationUserDto>> GetTeachers(string searchText, int pageSize = 10, int pageIndex = 1);
}

public class UserManagement : IUserManagement
{
    private readonly UserManager _userManager;
    private readonly RoleManager _roleManager;
    private readonly ApplicationDbContext _context;

    public UserManagement(UserManager userManager, RoleManager roleManager, ApplicationDbContext context)
    {
        _userManager = userManager;
        _context = context;
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

    public async Task<IList<ApplicationUserDto>> GetTeachers(string searchText, int pageSize = 10, int pageIndex = 1)
    {
        try
        {
            var parameters = new Dictionary<string, object>
            {
                { "searchText", searchText }
            };

            string sql = $@"

SELECT u.Id, u.FirstName, u.LastName, u.Email 
FROM AspNetUsers AS u 
INNER JOIN
	(
	SELECT ur.UserId FROM AspNetUserRoles AS ur
	WHERE ur.RoleId IN (SELECT Id FROM AspNetRoles WHERE Name = 'Teacher')
	) as a 
ON a.UserId = u.Id
WHERE ( u.FirstName LIKE N'%'+@searchText + '%' OR u.LastName LIKE N'%'+@searchText + '%' OR u.Email LIKE N'%'+@searchText + '%' )
ORDER BY u.Id
OFFSET {(pageIndex-1) * pageSize} ROWS FETCH NEXT {pageSize} ROWS ONLY
";
            var data = await _context.ExecuteQueryAsync<ApplicationUserDto>(sql, parameters);

            return data.result;
        }
        catch(Exception)
        {
            throw;
        }
    }
}
