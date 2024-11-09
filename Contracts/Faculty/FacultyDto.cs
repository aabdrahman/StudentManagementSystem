namespace StudentManagementApplication.Contracts.Faculty;

public record class FacultyDto
(string Name);


public record class FacultyDetailsDto
(
    int Id,
    string Name,
    DateTime CreatedDate
);
