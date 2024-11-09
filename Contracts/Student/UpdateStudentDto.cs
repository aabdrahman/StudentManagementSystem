using System.ComponentModel.DataAnnotations;

namespace StudentManagementApplication.Contracts.Student;

public record class UpdateStudentDto
(
    [Required(ErrorMessage = "Student Id is required.")]
    int Id,
    [Required(ErrorMessage = "Student Name is required.")]
    [StringLength(maximumLength:100, MinimumLength = 12, ErrorMessage = "Name must be between 12 and 100 caracters.")]
    string StudentName,
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid Email Provided.")]
    [RegularExpression(@"^[^@\s]+@[^@\s]+\.(com|net|org|gov)$", ErrorMessage = "Pattern not matched.")]
    string Email,
    [Required(ErrorMessage = "Password is required.")]
    [RegularExpression(@"^(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&#]{8,12}$", ErrorMessage = "Your password is expected to contain special character, alphabet and number.Password must not be more than 12 character.")]
    string Password,
    [Required(ErrorMessage = "Department Id is required.")]
    int DepartmentId
);