using StudentManagementApplication.Contracts.Faculty;
using StudentManagementApplication.Models;

namespace StudentManagementApplication.Mapper;

public static class FacultyMapper
{
    public static Faculty ToFacultyEntity(this CreateFacultyDto faculty)
    {
        return new Faculty()
        {
            Name = faculty.FacultyName
        };

    }

    public static FacultyDto ToFacultyDto(this Faculty faculty)
    {
        return new FacultyDto
        (
            Name: faculty.Name
        );
    }

    public static Faculty ToFacultyEntity(this UpdateFacultyDto updateFaculty)
    {
        return new Faculty()
        {
            Id = updateFaculty.Id,
            Name = updateFaculty.Name
        };
    }

    public static FacultyDetailsDto ToDetailsDto(this Faculty faculty)
    {
        return new FacultyDetailsDto(Id: faculty.Id, Name: $"Faculty Of {faculty.Name}", CreatedDate: faculty.CreatedDate);
    }
}
