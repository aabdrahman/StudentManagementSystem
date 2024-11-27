using StudentManagementApplication.Contracts.Course;
using StudentManagementApplication.Contracts.Department;
using StudentManagementApplication.Contracts.Enrollment;
using StudentManagementApplication.Contracts.Faculty;
using StudentManagementApplication.Contracts.Student;
using StudentManagementApplication.Models;

namespace StudentManagementApplication.Interfaces;

public interface IStudentManagementService
{
    Task<Response> RegisterStudent(CreateStudentDto NewStudent);
    Task<Response> UpdateStudent(UpdateStudentDto ModifiedStudent);
    Task<Response> LoginStudent(LoginStudentDto student);
    Task<Response> DeleteStudent(LoginStudentDto loginStudent);
    Task<Response> ChangePassword(StudentPasswordChangeDto passwordChangeDto);
    
    Task<Response> CreateCourse(CreateCourseDto NewCourse);
    Task<Response> UpdateCourse(UpdateCourseDto NewCourse);
    Task<Response> GetCourseById(int Id);
    Task<Response> GetCourses();

    Task<CourseDto> GetCourseByTitle(string Title);

    Task<Response> CreateFaculty(CreateFacultyDto NewFaculty);
    Task<Response> UpdateFaculty(UpdateFacultyDto newFaculty);
    Task<Faculty> GetFacultyById(int Id);
    Task<Response> GetAllFaculties();
    Task<Response> DeleteFaculty(int Id);

    Task<Response> CreateDepartment(CreateDepartmentDto NewDepartment);
    Task<Response> UpdateDepartment(UpdateDepartmentDto NewDepartment);
    Task<Department> GetDepartmentById(int Id);
    Task<Response> GetAllDepartments();
    Task<Response> DeleteDepartment(int DepartmentId);
    Task<Response> EnrollCourse(CreateEnrollmentDto NewEnrollment);
    Task<Response> UpdateEnrollment(UpdateEnrollmentDto NewEnrollment);
    Task<Response> GetStudentEnrollments(int StudentId);
    Task<Response> UpdateEnrollmentScore(UpdateEnrollmentScoreDto studentEnrollmentScore);
    
}
