using System.ComponentModel.DataAnnotations;

namespace StudentManagementApplication.Contracts.Course;

public record class UpdateCourseDto
(

    [Required(ErrorMessage = "Faculty Id is required.", AllowEmptyStrings = false)]
    int Id,
    [Required(ErrorMessage = "Course Code is required.")]
    [MaxLength(100, ErrorMessage = "Course Code must be 6 characters.")]
    string CourseCode,
    [Required(ErrorMessage = "Course Title is required.")]
    [MaxLength(100, ErrorMessage = "Course Title cannot be more than 100 characters.")]
    string CourseTitle,
    [Required(ErrorMessage = "Faculty Id is required.", AllowEmptyStrings = false)]
    int DepartmentId
);
