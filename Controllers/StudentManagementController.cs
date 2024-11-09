using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentManagementApplication.Contracts;
using StudentManagementApplication.Contracts.Course;
using StudentManagementApplication.Contracts.Department;
using StudentManagementApplication.Contracts.Enrollment;
using StudentManagementApplication.Contracts.Faculty;
using StudentManagementApplication.Contracts.Student;
using StudentManagementApplication.Interfaces;
using StudentManagementApplication.Mapper;
using StudentManagementApplication.Models;


namespace StudentManagementApplication.Controllers;

[Route("api/StudentManagementService")]
[ApiController]
public class StudentManagementController : ControllerBase
{
    private readonly IStudentManagementService _studentManagementService;

    public StudentManagementController(IStudentManagementService studentManagementService)
    {
        _studentManagementService = studentManagementService;
    }

    [HttpPost("/CreateFaculty")]
    public async Task<ActionResult> CreateFaculty(CreateFacultyDto NewFaculty)
    {
        if(!ModelState.IsValid)
        {
            return BadRequest(NewFaculty);
        }
        try
        {
            var response = await _studentManagementService.CreateFaculty(NewFaculty);

            return Ok(response);

        }
        catch (Exception ex)
        {
            return Ok(ex);
        }
    }

    [HttpPut("/UpdateFaculty")]
    public async Task<ActionResult> UpdateFaculty(UpdateFacultyDto NewFaculty)
    {
        if(!ModelState.IsValid)
        {
            return BadRequest(NewFaculty);

        }

        try
        {
            Response response = await _studentManagementService.UpdateFaculty(NewFaculty);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return Ok(ex);
        }
    }

    [HttpPost("/CreateDepartment")]
    public async Task<ActionResult> CreateDepartment(CreateDepartmentDto NewDepartment)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(NewDepartment);
        }

        try
        {
            Response response = await _studentManagementService.CreateDepartment(NewDepartment);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return Ok(ex);
        }
    }

    [HttpPut("/UpdateDepartment")]
    public async Task<ActionResult> UpdateDepartment(UpdateDepartmentDto NewDepartment)
    {
        try
        {
            Response response = await _studentManagementService.UpdateDepartment(NewDepartment);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return Ok(ex);
        }
    }

    [HttpGet("/GetDepartmentById/{Id}")]
    public async Task<ActionResult> GetDepartmentId(int Id)
    {
        try
        {
            var department = await _studentManagementService.GetDepartmentById(Id);


            return department != null ?
                       Ok(CreateResponse.CreateSuccessfulResponse(department.ToDepartmentDetails(), "Department Fetched Successfully")) :
                       Ok(CreateResponse.CreateUnsuccessfulResponse("09", "Error Fetching Department", "Department does not exist", $"No department with Id: {Id}"));
        }
        catch(ArgumentNullException ex)
        {
            return Ok(CreateResponse.CreateUnsuccessfulResponse("09", ex.Message, "Department does not exist", $"No department with Id: {Id}"));
        }
        catch (Exception ex)
        {
            return Ok(CreateResponse.CreateUnsuccessfulResponse("09", ex.Message, $"{ex.GetType().Name}", $"{ex.GetType().FullName}"));
        }

    }

    [Authorize]
    [HttpGet("/GetAllDepartments")]
    public async Task<ActionResult> GetAllDepartments()
    {
        var response = await _studentManagementService.GetAllDepartments();
        return Ok(response);
    }

    [HttpGet("/GetFacultyById/{Id}")]
    public async Task<ActionResult> GetFacultyById(int Id)
    {
        try
        {
            var faculty = await _studentManagementService.GetFacultyById(Id);
            return Ok(CreateResponse.CreateSuccessfulResponse(faculty.ToDetailsDto(), "Faculty Fetched Successfully"));
        }
        catch(ArgumentNullException ex)
        {
            return Ok(CreateResponse.CreateUnsuccessfulResponse("09", ex.Message, "Faculty does not exist", $"No faculty exists with Id: {Id}"));
        }
    }

    [HttpGet("/GetAllFaculties")]
    public async Task<ActionResult> GetAllFaculties()
    {
        var response = await _studentManagementService.GetAllFaculties();
        return Ok(response);
    }

    [HttpDelete("/DeleteDepartment/{Id}")]
    public async Task<ActionResult> DeleteDepartmentById(int Id)
    {
        try
        {
            var response = await _studentManagementService.DeleteDepartment(Id);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return Ok(CreateResponse.CreateUnsuccessfulResponse("99", ex.Message, ex.GetType().Name, ex.Message));
        }
    }

    [HttpDelete("/DeleteFaculty/{Id}")]
    public async Task<ActionResult> DeleteFacultyById(int Id)
    {
        try
        {
            var response = await _studentManagementService.DeleteFaculty(Id);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return Ok(CreateResponse.CreateUnsuccessfulResponse("99", ex.Message, ex.GetType().Name, ex.Message));
        }
    }

    [HttpPost("/CreateCourse")]
    public async Task<ActionResult> CreateCourse(CreateCourseDto NewCourse)
    {
        try
        {
            var response = await _studentManagementService.CreateCourse(NewCourse);

            return Ok(response);
        }
        catch(NullReferenceException ex)
        {
            return Ok(CreateResponse.CreateUnsuccessfulResponse("99", "An Error Occurred", ex.GetType().Name, ex.GetType().FullName));
        }
        catch (Exception ex)
        {
            return Ok(CreateResponse.CreateUnsuccessfulResponse("90", "An Error Occurred.", ex.GetType().Name, ex.GetType().FullName));
        }
    }

    [HttpPut("/UpdateCourse")]
    public async Task<ActionResult> UpdateCourse(UpdateCourseDto NewCourse)
    {
        try
        {
            var response = await _studentManagementService.UpdateCourse(NewCourse);
            return Ok(response);
        }
        catch (NullReferenceException ex)
        {
            return Ok(CreateResponse.CreateUnsuccessfulResponse("99", "An Error Occurred", ex.GetType().Name, ex.GetType().FullName));
        }
        catch (Exception ex)
        {
            return Ok(CreateResponse.CreateUnsuccessfulResponse("90", "An Error Occurred.", ex.GetType().Name, ex.GetType().FullName));
        }
    }

    [HttpGet("/GetCourses/{Id}")]
    public async Task<ActionResult> GetCourseById(int Id)
    {
        try
        {
            var response = await _studentManagementService.GetCourseById(Id);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return Ok(CreateResponse.CreateUnsuccessfulResponse("90", "An Error Occurred.", ex.GetType().Name, ex.GetType().FullName));
        }
    }
    [HttpGet("/GetCourses")]   
    public async Task<ActionResult> GetCourses()
    {
        try
        {
            var response = await _studentManagementService.GetCourses();
            return Ok(response);
        }
        catch (Exception ex)
        {
            return Ok(CreateResponse.CreateUnsuccessfulResponse("90", "An Error Occurred.", ex.GetType().Name, ex.GetType().FullName));
        }
    }

    [HttpPost("/RegisterStudent")]
    public async Task<ActionResult> RegisterStudent(CreateStudentDto NewStudent)
    {
        try
        {
            var response = await _studentManagementService.RegisterStudent(NewStudent);
            return Ok(response);
        }
        catch (DbUpdateException ex)
        {
            return Ok(CreateResponse.CreateUnsuccessfulResponse("99", $"An Error Occurred Registering Student : {ex.Message}", ex.GetType().ToString(), ex.GetType().FullName));
        }
        catch (Exception ex)
        {
            return Ok(CreateResponse.CreateUnsuccessfulResponse("99", "An Error Occurred Registering Student", ex.GetType().ToString(), ex.GetType().FullName));
        }
    }

    [HttpPut("/UpdateStudent")]
    public async Task<ActionResult> UpdateStudent(UpdateStudentDto ModifiedStudent)
    {
        try
        {
            var response = await _studentManagementService.UpdateStudent(ModifiedStudent);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return Ok(CreateResponse.CreateUnsuccessfulResponse("99", "An Error Occurred Updating Student Details", ex.GetType().ToString(), ex.GetType().FullName));
        }
    }

    [Authorize]
    [HttpPost("/LoginStudent")]
    public async Task<ActionResult> LoginStudent(LoginStudentDto loginStudentDto)
    {
        try
        {
            var response = await _studentManagementService.LoginStudent(loginStudentDto);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return Ok(CreateResponse.CreateUnsuccessfulResponse("99", "An Error Occurred Logging in Student", ex.GetType().ToString(), ex.GetType().FullName));
        }
    }

    [Authorize]
    [HttpPost("/DeleteStudent")]
    public async Task<ActionResult> DeleteStudent(LoginStudentDto loginStudent)
    {
        try
        {
            var response = await _studentManagementService.DeleteStudent(loginStudent);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return Ok(CreateResponse.CreateUnsuccessfulResponse("99", "An Error Occurred Registering Student", ex.GetType().ToString(), ex.GetType().FullName));
        }
    }

    [HttpPost("/ChangeStudentPassword")]
    public async Task<ActionResult> ChangeStudentPassword(StudentPasswordChangeDto passwordChangeDto)
    {
        try
        {
            var response = await _studentManagementService.ChangePassword(passwordChangeDto);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return Ok(CreateResponse.CreateUnsuccessfulResponse("99", "An Error Occurred Registering Student", ex.GetType().ToString(), ex.GetType().FullName));
        }
    }
    [HttpPost("/EnrollCourse")]
    public async Task<ActionResult> EnrollCourse(CreateEnrollmentDto NewEnrollment)
    {
        
        try
        {
            var respnse = await _studentManagementService.EnrollCourse(NewEnrollment);
            return Ok(respnse);
        }
        catch (Exception ex)
        {
            return Ok(CreateResponse.CreateUnsuccessfulResponse("99", "An Error Occurred Registering Student", ex.GetType().ToString(), ex.GetType().FullName));
        }
    }

    [HttpPut("/UpdateEnrollment")]
    public async Task<ActionResult> UpdateEnrollment(UpdateEnrollmentDto UpdateEnrollment)
    {
        try
        {
            var respnse = await _studentManagementService.UpdateEnrollment(UpdateEnrollment);
            return Ok(respnse);
        }
        catch (Exception ex)
        {
            return Ok(CreateResponse.CreateUnsuccessfulResponse("99", "An Error Occurred Registering Student", ex.GetType().ToString(), ex.GetType().FullName));
        }
    }

    [HttpGet("/GetStudentEnrollments/{Id}")]
    public async Task<ActionResult> GetStudentEnrollments(int Id)
    {
        try
        {
            var respnse = await _studentManagementService.GetStudentEnrollments(Id);
            return Ok(respnse);
        }
        catch (Exception ex)
        {
            return Ok(CreateResponse.CreateUnsuccessfulResponse("99", "An Error Occurred Registering Student", ex.GetType().ToString(), ex.GetType().FullName));
        }
    }
}
