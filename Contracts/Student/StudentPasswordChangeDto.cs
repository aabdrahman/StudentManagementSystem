using System.ComponentModel.DataAnnotations;

namespace StudentManagementApplication.Interfaces;

public record class StudentPasswordChangeDto
(
    string MatricNumber,
    [Required(ErrorMessage = "Password is required.")]
    [RegularExpression(@"^(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&#]{8,12}$", ErrorMessage = "Your password is expected to contain special character, alphabet and number.Password must not be more than 12 character.")]
    string OldPassword,
    [Required(ErrorMessage = "Password is required.")]
    [RegularExpression(@"^(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&#]{8,12}$", ErrorMessage = "Your password is expected to contain special character, alphabet and number.Password must not be more than 12 character.")]
    string NewPassword
);