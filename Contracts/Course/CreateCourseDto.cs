using System.ComponentModel.DataAnnotations;

namespace StudentManagementApplication.Contracts.Course;

public record class CreateCourseDto
(
    [Required(ErrorMessage = "Course Title is required.")]
    [MaxLength(100, ErrorMessage = "Course Title cannot be more than 100 characters.")]
    string CourseTitle,
    [Required(ErrorMessage = "Course Code is required.")]
    [Length(6, 6, ErrorMessage = "Course Code must have 6 characters.")]
    string CourseCode,
    [Required(ErrorMessage = "Department Id is required.")]
    int DepartmentId
);
