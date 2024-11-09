using StudentManagementApplication.Contracts.Department;
using StudentManagementApplication.Models;
using System.Runtime.CompilerServices;

namespace StudentManagementApplication.Mapper;

public static class DepartmentMapper
{
    public static Department ToDepartmentEntity(this CreateDepartmentDto NewDepartment)
    {
        return new Department()
        {
            Name = NewDepartment.Name,
            FacultyId = NewDepartment.FacultyId
        };

    }

    public static Department ToDepartmentEntity(this UpdateDepartmentDto department)
    {
        return new Department()
        {
            Name = department.Name,
            FacultyId = department.FacultyId,
            Id = department.Id
        };

    }

    public static DepartmentDetailsDto ToDepartmentDetails(this Department department)
    {
        return new DepartmentDetailsDto
        (
            Id: department.Id,
            Name: $"Department of {department.Name}",
            FacultyName: $"Faculty of {department.faculty?.Name}"
        );
    }
}
