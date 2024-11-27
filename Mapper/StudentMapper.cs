using StudentManagementApplication.Contracts.Student;
using StudentManagementApplication.Models;

namespace StudentManagementApplication.Mapper;

public static class StudentMapper
{
    public static async Task<Student> ToStudentEntity(this CreateStudentDto NewStudent)
    {
        return  new Student()
                {
                    Name = NewStudent.StudentName,
                    MatricNumber = NewStudent.MatricNos.ToUpper(),
                    Email = NewStudent.Email,
                    Password = NewStudent.Password,
                    DepartmentId = NewStudent.DepartmentId
                };
    }

    public static async Task<StudentDto> ToStudentDto(this Student student)
    {
        return new StudentDto
        (
            studentId: student.Id,
            Name: student.Name,
            MatricNumber: student.MatricNumber,
            Email: student.Email
        );
    }

    public static async Task<Student> ToStudentEntity(this UpdateStudentDto ModiiedStudent)
    {
        return new Student()
        {
            Id = ModiiedStudent.Id,
            Name = ModiiedStudent.StudentName,
            DepartmentId = ModiiedStudent.DepartmentId,
            Email = ModiiedStudent.Email,
            Password = ModiiedStudent.Password,
        };
    }
    
    public static async Task<StudentDetailsDto> ToStudentDetails(this Student student)
    {
        return new StudentDetailsDto
        (
            Name: student.Name,
            MatricNumber: student.MatricNumber,
            Email: student.Email,
            Department: student.department?.Name,
            Faculty: student.department?.faculty?.Name,
            Enrollments: student.StudentEnrollments.Select(x => x.ToEnrollmentDetails()).ToList()
        );
    }
}
