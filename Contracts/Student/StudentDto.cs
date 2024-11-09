namespace StudentManagementApplication.Contracts.Student;

public record class StudentDto
(
    int? studentId,
    string Name,
    string MatricNumber,
    string Email
);
