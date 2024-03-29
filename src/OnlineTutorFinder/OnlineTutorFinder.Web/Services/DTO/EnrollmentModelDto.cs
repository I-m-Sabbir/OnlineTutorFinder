﻿namespace OnlineTutorFinder.Web.Services.DTO;

public class EnrollmentModelDto
{
    public Guid Id { get; set; }
    public string? SubjectName { get; set; }
    public TimeSpan? StartTime { get; set; }
    public TimeSpan? EndTime { get; set; }
    public string? TeacherName { get; set; }
    public string? TeachingDays { get; set; }
}
