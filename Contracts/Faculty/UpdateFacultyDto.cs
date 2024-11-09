using System.ComponentModel.DataAnnotations;

namespace StudentManagementApplication.Contracts.Faculty;

public record class UpdateFacultyDto
(
    [Required(ErrorMessage = "Faculty Id is required.")]
    int Id,
    [Required(ErrorMessage = "Faculty Name is required.")]
    [MaxLength(100, ErrorMessage = "Faculty Name cannot be more than 100 characters.")]
    string Name
 );
