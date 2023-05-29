namespace OnlineTutorFinder.Web.Services.DTO;

public class ApplicationUserDto
{
    public Guid Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public int TotalRecord { get; set; }
}
