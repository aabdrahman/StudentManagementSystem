namespace StudentManagementApplication.Contracts.Student;

public record class StudentDetailsDto
(
    string Name,
    string MatricNumber,
    string Email,
    string Department,
    string Faculty
);