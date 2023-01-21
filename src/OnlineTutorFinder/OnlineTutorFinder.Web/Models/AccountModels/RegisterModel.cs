using OnlineTutorFinder.Web.Extensions;
using System.ComponentModel.DataAnnotations;

namespace OnlineTutorFinder.Web.Models.AccountModels;

public class RegisterModel
{
    [Required]
    [Display(Name = "First Name")]
    [RegularExpression(@"^[a-zA-Z'' ''.']+$", ErrorMessage = "Alphabets, '.' and White Space Only.")]
    public string FirstName { get; set; } = null!;

    [Required]
    [Display(Name = "Last Name")]
    [RegularExpression(@"^[a-zA-Z'' ''.']+$", ErrorMessage = "Alphabets, '.' and White Space Only.")]
    public string LastName { get; set; } = null!;

    [Required]
    [EmailAddress]
    [Display(Name = "Email")]
    public string Email { get; set; } = null!;

    [Required]
    [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public string Password { get; set; } = null!;

    [DataType(DataType.Password)]
    [Display(Name = "Confirm password")]
    [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
    public string ConfirmPassword { get; set; } = null!;

    [OnlyAccept("Male","Female","Other")]
    [Display(Name = "Gender")]
    public string? Gender { get; set; }

    [Display(Name = "Address")]
    public string? Address { get; set; }

    [Required(ErrorMessage = "Please Choose an Option.")]
    [Display(Name = "Regiter As")]
    public string RegisterAs { get; set; } = null!;

    public string? ReturnUrl { get; set; }
}
