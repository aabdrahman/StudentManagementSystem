using System.ComponentModel.DataAnnotations;

namespace StudentManagementApplication.Contracts.Enrollment;

public record class UpdateEnrollmentScoreDto
(
    [Required(ErrorMessage = "Enrollment Id is required.")]
    int EnrollmentId,
    [Range(0, 100, ErrorMessage = "The Scre must be between 0 and 100")]
    [Required(ErrorMessage = "Enrollment Score is required.")]
    int Score
);

