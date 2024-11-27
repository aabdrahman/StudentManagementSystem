using System.ComponentModel.DataAnnotations;

namespace StudentManagementApplication.Contracts.Enrollment;

public record class UpdateEnrollmentDto
(
    [Required(ErrorMessage = "Enrollment Id is required.")]
    int Id,
    [Required(ErrorMessage = "Student Id is required.")]
    int StudentId,
    [Required(ErrorMessage = "Course Id is required.")]
    int CourseId,
    int? Score
);