using System.ComponentModel.DataAnnotations;

namespace OnlineTutorFinder.Web.Models.PostModels
{
    public class AddSubjectScheduleModel
    {
        [Required]
        [StringLength(50)]
        public string? SubjectName { get; set; }

        [Required]
        public TimeSpan StartTime { get; set; }

        [Required]
        public TimeSpan EndTime { get; set; }

        [Required]
        public IList<DayOfWeek> DayOfWeeks { get; set; } = null!;
    }
}
