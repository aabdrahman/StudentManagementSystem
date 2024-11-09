using System.ComponentModel.DataAnnotations;

namespace StudentManagementApplication.Contracts.Faculty;

public record class CreateFacultyDto
(
    [Required(ErrorMessage = "Faculty Name is required.", AllowEmptyStrings = false)]
    string FacultyName
);