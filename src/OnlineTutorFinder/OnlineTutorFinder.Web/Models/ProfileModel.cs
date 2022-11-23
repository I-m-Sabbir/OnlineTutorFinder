using OnlineTutorFinder.Web.Entities.Membership;
using OnlineTutorFinder.Web.Extensions;
using System.ComponentModel.DataAnnotations;

namespace OnlineTutorFinder.Web.Models
{
    public class ProfileModel
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

    }
}
