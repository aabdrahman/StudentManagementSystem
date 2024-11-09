using System.ComponentModel.DataAnnotations;

namespace StudentManagementApplication.Contracts.Student;

public record class LoginStudentDto
(
    [Required(ErrorMessage = "Matric Number is required")]
    string MatricNos,
    [Required(ErrorMessage = "Password is required.")]
    [RegularExpression(@"^(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&#]{8,12}$", ErrorMessage = "Your password is expected to contain special character, alphabet and number.Password must not be more than 12 character.")]
    string Password
);