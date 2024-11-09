using System.ComponentModel.DataAnnotations;

namespace StudentManagementApplication.Contracts.Enrollment;

public record class EnrollmentDetailsDto
(
    [Required(ErrorMessage = "Enrollment Id is required.")]
    int EnrollmentId,
    [Required(ErrorMessage = "Matric Number is required.")]
    string MatricNumber,
    [Required(ErrorMessage = "Student Name is required.")]
    string StudentName,
    [Required(ErrorMessage = "Course Title is required.")]
    string CourseTitle,
    [Required(ErrorMessage = "Course Code is required.")]
    string CourseCode,
    [Required(ErrorMessage = "Enrollment Year is required.")]
    int EnrollmentYear
);