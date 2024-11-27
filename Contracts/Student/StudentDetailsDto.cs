using StudentManagementApplication.Contracts.Enrollment;

namespace StudentManagementApplication.Contracts.Student;

public record class StudentDetailsDto
(
    string Name,
    string MatricNumber,
    string Email,
    string Department,
    string Faculty,
    List<EnrollmentDetailsDto> Enrollments
);