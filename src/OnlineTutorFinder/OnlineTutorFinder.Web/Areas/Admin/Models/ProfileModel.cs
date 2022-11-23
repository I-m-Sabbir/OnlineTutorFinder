using Microsoft.AspNetCore.Authorization;
using OnlineTutorFinder.Web.Entities.Membership;
using OnlineTutorFinder.Web.Extensions;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace OnlineTutorFinder.Web.Areas.Admin.Models
{
    [Authorize(Roles = "Admin")]
    public class ProfileModel : AdminBaseModel
    {
        [Required]
        [Display(Name = "First Name")]
        [StringLength(20, ErrorMessage = "Not More than 20 Characters.")]
        public string? FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        [StringLength(20, ErrorMessage = "Not More than 20 Characters.")]
        public string? LastName { get; set; }

        public string? Email { get; set; }

        [StringLength(100, ErrorMessage = "Not More than 100 Characters.")]
        public string? Address { get; set; }

        [OnlyAccept("Male", "Female", "Other")]
        public string? Gender { get; set; }

        public string? PictureUrl { get; set; }

        [Image(ErrorMessage = "Only '.jpg', '.jpeg', '.png' Files Allowed Under 1MB")]
        public IFormFile? Picture { get; set; }


        internal void LoadData(ApplicationUser user)
        {
            this.FirstName = user.FirstName;
            this.LastName = user.LastName;
            this.Email = user.Email;
            this.Address = user.Address;
            this.PictureUrl = user.ProfilePicture;
            this.Gender = user.Gender;
        }


        
        internal ApplicationUser Update(ApplicationUser user)
        {
            user.FirstName = this.FirstName;
            user.LastName = this.LastName;
            user.Address = this.Address;
            user.ProfilePicture = this.PictureUrl;

            return user;
        }
    }
}
