using System.ComponentModel.DataAnnotations;

namespace StudentManagementApplication.Contracts.Enrollment;

public record class EnrollmentDto
(
    [Required(ErrorMessage = "Enrollment Id is required.")]
    int EnrollmentId,
    [Required(ErrorMessage = "Student Id is required.")]
    int StudentId,
    [Required(ErrorMessage = "Course Id is required.")]
    int CourseId,
    [Required(ErrorMessage = "Enrollment Year is required.")]
    int EnrollmentYear
);
