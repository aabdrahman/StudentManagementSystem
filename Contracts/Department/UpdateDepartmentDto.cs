using System.ComponentModel.DataAnnotations;

namespace StudentManagementApplication.Contracts.Department;

public record class UpdateDepartmentDto
(
    [Required(ErrorMessage = "Id is required.", AllowEmptyStrings = false)]
    int Id,
    [Required(ErrorMessage = "Department Name is required.", AllowEmptyStrings = false)]
    [MaxLength(100, ErrorMessage = "Department Name cannot be more than 100 characters")]
    string Name,
    [Required(ErrorMessage = "Faculty Id is required.", AllowEmptyStrings = false)]
    int FacultyId
);