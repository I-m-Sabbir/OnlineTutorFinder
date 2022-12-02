using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;

namespace OnlineTutorFinder.Web.Areas.Teacher.Models.PostModels
{
    [Authorize(Roles = "Teacher")]
    public class AddSubjectScheduleModel : TeacherBaseModel
    {
        [Required]
        [StringLength(50)]
        [Display(Name = "Subject Name")]
        public string? SubjectName { get; set; }

        [Required]
        [Display(Name = "Start Time")]
        public TimeSpan StartTime { get; set; }

        [Required]
        [Display(Name = "End Time")]
        public TimeSpan EndTime { get; set; }

        [Required(ErrorMessage = "At Least Chose One Day.")]
        [Display(Name = "Teaching Days")]
        public IList<DayOfWeek> DayOfWeeks { get; set; } = null!;

        public Guid TeacherId { get; set; }
                        
    }
}
