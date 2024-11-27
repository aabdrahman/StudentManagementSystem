using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using StudentManagementApplication.Context;
using StudentManagementApplication.Contracts;
using StudentManagementApplication.Contracts.Course;
using StudentManagementApplication.Contracts.Department;
using StudentManagementApplication.Contracts.Enrollment;
using StudentManagementApplication.Contracts.Faculty;
using StudentManagementApplication.Contracts.Student;
using StudentManagementApplication.Interfaces;
using StudentManagementApplication.Mapper;
using StudentManagementApplication.Middleware;
using StudentManagementApplication.Models;

namespace StudentManagementApplication.Services;

public class StudentManagementService : IStudentManagementService
{
    private readonly StudentManagementDbContext _context;
    private readonly ILogger<StudentManagementService> _logger;
    private readonly IPasswordHasher _passwordHasher;
    private readonly TokenProvider _tokenProvider;
    private readonly IServiceProvider _serviceProvider;

    public StudentManagementService(StudentManagementDbContext context, ILogger<StudentManagementService> logger, IPasswordHasher passwordHasher, TokenProvider tokenProvider, IServiceProvider serviceProvider)
    {
        _context = context;
        _logger = logger;
        _passwordHasher = passwordHasher;
        _tokenProvider = tokenProvider;
        _serviceProvider = serviceProvider;
    }

    public async Task<Response> CreateCourse(CreateCourseDto NewCourse)
    {
        try
        {
            var department = await _context.Departments.FindAsync(NewCourse.DepartmentId);

            if (department == null)
            {
                return CreateResponse.CreateUnsuccessfulResponse("09", "Invalid Department Provided", "Error Fetching Department", $"No department with the provided department Id: {NewCourse.DepartmentId}");
            }

            if(await _context.Courses.AnyAsync(c => c.CourseCode == NewCourse.CourseCode))
            {
                return CreateResponse.CreateUnsuccessfulResponse("19", "An Error Occurred.", "Course Code Exists", $"A course exists wuth the provided Course Code: {NewCourse.CourseCode}");
            }
            
            Course course = NewCourse.ToCourseEmtity();

            await _context.Courses.AddAsync(course);

            await _context.SaveChangesAsync();

            return CreateResponse.CreateSuccessfulResponse(course.ToCourseDto(), "Course Created Successfully");

        }

        catch (Exception ex)
        {
            _logger.LogError(ex, "An Error Occurred");
            throw new Exception("Error Occurred Creating Course.");
        }

    }


    public async Task<Response> RegisterStudent(CreateStudentDto NewStudent)
    {
        try
        {

            if (await ExistsEmail(NewStudent.Email))
            {
                return new Response()
                {
                    ResponseCode = "99",
                    ResponseDescription = "Error Occurred registering user",
                    ResponseData = null,
                    Error = new ErrorResponse() { Title = "Email Exists", ErrorMessage = $"The provided email: {NewStudent.Email} is already used by another user." }
                };
            }

            var StudentDepartment = await _context.Departments.FirstOrDefaultAsync(d => d.Id == NewStudent.DepartmentId);
            if (StudentDepartment == null)
            {
                return CreateResponse.CreateUnsuccessfulResponse("09", "Invalid Department Provided", "Error Fetching Department", $"Department with the provided Id does not Exist: {NewStudent.DepartmentId}");
            }

            var ExistingStudent = await _context.Students.IgnoreQueryFilters().FirstOrDefaultAsync(s => s.MatricNumber == NewStudent.MatricNos.ToUpper());

            if (ExistingStudent != null)
            {
                if (ExistingStudent.isDeleted)
                {
                    ExistingStudent.isDeleted = false;
                    _context.Students.Update(ExistingStudent);
                    _context.SaveChanges();
                    return CreateResponse.CreateSuccessfulResponse(await ExistingStudent.ToStudentDto(), "Student Activated Successfully.");

                }
                return CreateResponse.CreateUnsuccessfulResponse("19", "Invalid Matric Number Provided", "Error Registering Student", $"There is a student with Matric Number: {NewStudent.MatricNos}");
            }

            Student student = await NewStudent.ToStudentEntity();
            student.Password = await _passwordHasher.HashPassword(student.Password);

            await _context.Students.AddAsync(student);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch(DbUpdateException ex)
            {
                _logger.LogError(ex, "An Error Occurred creating student");
                throw new DbUpdateException("An Error Occurred creating student");
            }

            return new Response()
            {
                Error = null,
                ResponseCode = "00",
                ResponseDescription = "Student Created successfully.",
                ResponseData = await student.ToStudentDto()
            };
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "An Error Occurred creating student");
            throw new DbUpdateException($"An Error Occurred creating student : {ex.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error Occurred");
            throw new Exception("An Error Occurred Registering user.");
        }


    }

    public async Task<CourseDto> GetCourseByTitle(string courseCode)
    {
        var course = await _context.Courses.Include(c => c.department).SingleOrDefaultAsync(c => c.CourseCode == courseCode);

        return course.ToCourseDto();
    }

    public async Task<Response> UpdateCourse(UpdateCourseDto NewCourse)
    {
        
        Course ExistingCourse = await _context.Courses.FindAsync(NewCourse.Id);

        if (ExistingCourse == null)
        {
            return CreateResponse.CreateUnsuccessfulResponse("09", "PError Updating Coure", "Not Found", $"No Course with specified Id: {NewCourse.Id}");
        }

        Department ExistingDepartment = await _context.Departments.FindAsync(NewCourse.DepartmentId);

        if (ExistingDepartment == null)
        {
            return CreateResponse.CreateUnsuccessfulResponse("19", "Error Updating Coure", "Not Found", $"No Department with specified Id: {NewCourse.DepartmentId}");
        }

        ExistingCourse.CourseCode = NewCourse.CourseCode;
        ExistingCourse.CourseTitle = NewCourse.CourseTitle;
        ExistingCourse.DepartmentId = NewCourse.DepartmentId;

        _context.Courses.Update(ExistingCourse);

        await _context.SaveChangesAsync();

        return CreateResponse.CreateSuccessfulResponse(ExistingCourse.ToCourseDto(), "Course update successful.");


    }

    public async Task<Response> CreateFaculty(CreateFacultyDto NewFaculty)
    {
        bool ExistsName = await _context.Faculties.AnyAsync(f => f.Name == NewFaculty.FacultyName);
        if (ExistsName)
        {
            return CreateResponse.CreateUnsuccessfulResponse("09", "Cannot create faculty", "Faculty Exists", $"Faculty with Name: {NewFaculty.FacultyName} already exists.");
        }

        Faculty faculty = NewFaculty.ToFacultyEntity();

        await _context.Faculties.AddAsync(faculty);

        await _context.SaveChangesAsync();

        return CreateResponse.CreateSuccessfulResponse(faculty.ToDetailsDto(), "Faculty created Successfully.");
    }

    public async Task<Response> UpdateFaculty(UpdateFacultyDto newFaculty)
    {
        Faculty? ExistingFaculty = await _context.Faculties.FindAsync(newFaculty.Id);

        if (ExistingFaculty == null)
        {
            return CreateResponse.CreateUnsuccessfulResponse("09", "Error editing faculty", "Faculty does not Exist", $"Faculty with Id: {newFaculty.Id} does not exist.");
        }

        ExistingFaculty.Name = newFaculty.Name;

        _context.Faculties.Update(ExistingFaculty);

        await _context.SaveChangesAsync();

        return CreateResponse.CreateSuccessfulResponse(ExistingFaculty.ToDetailsDto(), $"Faculty with Id: {ExistingFaculty.Id} updated successfully");
       
    }

    private async Task<bool> ExistsEmail(string Email)
    {
        return await _context.Students.AnyAsync(s => s.Email == Email);
    }

    public async Task<Response> CreateDepartment(CreateDepartmentDto NewDepartment)
    {
        bool DepartmentExist = await _context.Departments.AnyAsync(d => d.Name == NewDepartment.Name);

        if (DepartmentExist)
        {
            return CreateResponse.CreateUnsuccessfulResponse("09", "Invalid Department Provided", "Error Fetching Department", $"Department with the provided department Name Exists: {NewDepartment.Name}");
        }

        Faculty DepartmentFaculty = await _context.Faculties.FindAsync(NewDepartment.FacultyId);
        if (DepartmentFaculty == null)
        {
            return CreateResponse.CreateUnsuccessfulResponse("09", "Invalid Department Provided", "Error Fetching Faculty", $"Faculty with the provided Faculty Id does not Exist: {NewDepartment.FacultyId}");
        }

        Department department = NewDepartment.ToDepartmentEntity();

        await _context.Departments.AddAsync(department);

        await _context.SaveChangesAsync();

        return CreateResponse.CreateSuccessfulResponse(department.ToDepartmentDetails(), "Department Created Successfully");

    }

    public async Task<Response> UpdateDepartment(UpdateDepartmentDto NewDepartment)
    {
        Department? ExistingDepartment = await _context.Departments.FindAsync(NewDepartment.Id);

        if (ExistingDepartment == null)
        {
            return CreateResponse.CreateUnsuccessfulResponse("09", "Invalid Department Provided", "Error Fetching Department", $"No Department with the provided department Id: {NewDepartment.Id}");
        }

        Faculty ExistingFaculty = await _context.Faculties.FindAsync(NewDepartment.FacultyId);
        if (ExistingFaculty == null)
        {
            return CreateResponse.CreateUnsuccessfulResponse("19", "Invalid Faculty Provided", "Error Fetching Faculty", $"No Faculty with the provided department Id: {NewDepartment.FacultyId}");
        }

        ExistingDepartment.Name = NewDepartment.Name;
        ExistingDepartment.FacultyId = NewDepartment.FacultyId;

        _context.Faculties.Update(ExistingFaculty);

        await _context.SaveChangesAsync();

        return CreateResponse.CreateSuccessfulResponse(ExistingDepartment.ToDepartmentDetails(), "Department Updated Successfully");

    }

    public async Task<Response> DeleteFaculty(int Id)
    {
        Faculty FacultyToDelete = await _context.Faculties.FirstOrDefaultAsync(x => x.Id == Id);

        if (FacultyToDelete == null)
        {
            return CreateResponse.CreateUnsuccessfulResponse("09", "Error Deleting Response", "Faculty does not exist", $"No Faculty with Id: {Id}");
        }

        FacultyToDelete.isDeleted = true;
        _context.Faculties.Update(FacultyToDelete);
        await _context.SaveChangesAsync();
        return CreateResponse.CreateSuccessfulResponse("00", "Faculty deeted successfully");
    }

    public async Task<Response> DeleteDepartment(int DepartmentId)
    {
        Department department = await _context.Departments.FirstOrDefaultAsync(d => d.Id == DepartmentId);
        if (department == null)
        {
            return CreateResponse.CreateUnsuccessfulResponse("09", "Error Fetching Department", "Department does not exist", $"No Department exists with Id: {DepartmentId}");
        }
        department.isDeleted = true;
        _context.Departments.Update(department);
        await _context.SaveChangesAsync();
        return CreateResponse.CreateSuccessfulResponse("00", $"Department deleted Successfully.");
    }

    public async Task<Response> GetAllFaculties()
    {
        List<FacultyDetailsDto> AllFaculties = await _context.Faculties
                                                        .Select(f => f.ToDetailsDto())
                                                        .ToListAsync();
        return CreateResponse.CreateSuccessfulResponse(AllFaculties, "Faculties Fetched Successfully.");
    }

    public async Task<Department> GetDepartmentById(int Id)
    {
        try
        {
            Department? desiredDepartment = await _context.Departments
                                            .FirstOrDefaultAsync(d => d.Id == Id);

            if(desiredDepartment == null)
            {
                throw new ArgumentNullException($"No department with Id: {Id}");
            }

            await _context.Entry(desiredDepartment)
                .Reference(d => d.faculty)
                .LoadAsync();

            return desiredDepartment;
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An Error Occurred Fetching department");
            throw new Exception("An Error Occurred Fetching Department");
        }
                                                    
    }

    public async Task<Response> GetAllDepartments()
    {
        try
        {
            List<DepartmentDetailsDto> AllDepartments = await _context.Departments
                                                       .Include(d => d.faculty)
                                                       .Select(f => f.ToDepartmentDetails())
                                                       .ToListAsync();

            var response = CreateResponse.CreateSuccessfulResponse(AllDepartments, "Departments Fetched Sucessfully.");
            return response;
        }
        catch(InvalidOperationException ex)
        {
            return CreateResponse.CreateUnsuccessfulResponse("99", "An Error Occurred", ex.GetType().FullName, ex.Message);
        }

    }

    public async Task<Faculty> GetFacultyById(int Id)
    {
        try
        {
            Faculty faculty = await _context.Faculties.FirstOrDefaultAsync(f => f.Id == Id);

            if (faculty == null)
            {
                throw new ArgumentNullException($"Faculty with Id: {Id} cannot be found");
            }

            return faculty;
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An Error Occurred Fetching Faculty.");
            throw new Exception("An Error Occurred.");
        }

        
    }

    public async Task<Response> GetCourseById(int Id)
    {
        Course course = await _context.Courses
                                            .Include(c => c.department)
                                            .FirstOrDefaultAsync(c => c.Id == Id);

        if(course == null)
        {
            return CreateResponse.CreateUnsuccessfulResponse("09", "No course found", "Invalid Id provided", $"No Course with provided Id: {Id}");
        }

        return CreateResponse.CreateSuccessfulResponse(course.ToCourseDto(), "Course Fetched Successfully");
    }

    public async Task<Response> GetCourses()
    {
        var AllCourses = await _context.Courses
                                            .Include(c => c.department)
                                            .Select(c => c.ToCourseDto()).ToListAsync();

        return CreateResponse.CreateSuccessfulResponse(AllCourses, "Courses Fetched Successfully.");
    }

    public async Task<Response> UpdateStudent(UpdateStudentDto ModifiedStudent)
    {
        var ExistingStudent = await _context.Students.FirstOrDefaultAsync(s => s.Id == ModifiedStudent.Id);
        if(ExistingStudent == null)
        {
            return CreateResponse.CreateUnsuccessfulResponse("09", "Invalid Student Id Provided", "Error Fetching Student", $"Student with the provided Department Id does not Exist: {ModifiedStudent.Id}");
        }

        var StudentDepartment = _context.Departments.FirstOrDefaultAsync(d => d.Id == ModifiedStudent.Id);
        if(StudentDepartment == null)
        {
            return CreateResponse.CreateUnsuccessfulResponse("19", "Invalid Department Provided", "Error Fetching Department", $"Department with the provided department Id Exists: {ModifiedStudent.DepartmentId}");
        }

        ExistingStudent.Name = ModifiedStudent.StudentName;
        ExistingStudent.DepartmentId = ModifiedStudent.DepartmentId;
        ExistingStudent.Email = ModifiedStudent.Email;

        _context.Students.Update(ExistingStudent);

        await _context.SaveChangesAsync();

        return CreateResponse.CreateSuccessfulResponse(await ExistingStudent.ToStudentDetails(), $"Student details updated successfully.");
    }

    public async Task<Response> LoginStudent(LoginStudentDto student)
    
    {
        var StudentToLogin = await _context.Students
                                            .Include(s => s.department)
                                                .ThenInclude(s => s.faculty)
                                            .Include(s => s.StudentEnrollments)
                                            .FirstOrDefaultAsync(s => s.MatricNumber == student.MatricNos.ToUpper());

        if(StudentToLogin == null)
        {
            return CreateResponse.CreateUnsuccessfulResponse("09", "Invalid Matric Number", "Invalid Login", $"No student registered with matric number: {student.MatricNos}");
        }

        var IsCorrectPassword = await _passwordHasher.VerifyPassword(student.Password, StudentToLogin.Password);

        if(!IsCorrectPassword)
        {
            return CreateResponse.CreateUnsuccessfulResponse("19", "Error Attempting Login", "Invalid Login Details", $"Incorrect Password provided.");
        }

        var token = await _tokenProvider.GenrateToken(student, StudentToLogin.Email);

        var responseData = new
        {
            Token = token,
            StudentDetails = StudentToLogin.ToStudentDetails()
        };

        return CreateResponse.CreateSuccessfulResponse(responseData, "Login Successfull.");
    }

    public async Task<Response> DeleteStudent(LoginStudentDto loginStudent)
    {
        var ExistingStudent = await _context.Students.FirstOrDefaultAsync(s => s.MatricNumber == loginStudent.MatricNos.ToUpper());

        if (ExistingStudent == null)
        {
            return CreateResponse.CreateUnsuccessfulResponse("09", "Invalid Matric Number", "Invalid Details", $"No student registered with matric number: {loginStudent.MatricNos}");
        }

        bool IsPasswordCorrect = await _passwordHasher.VerifyPassword(loginStudent.Password, ExistingStudent.Password);
        if (!IsPasswordCorrect)
        {
            return CreateResponse.CreateUnsuccessfulResponse("19", "Error Attempting Login", "Invalid Login Details", $"Incorrect Password provided.");
        }

        ExistingStudent.isDeleted = true;

        _context.Students.Update(ExistingStudent);

        await _context.SaveChangesAsync();

        return CreateResponse.CreateSuccessfulResponse("Success", $"Student with Matric Number: {loginStudent.MatricNos} deleted Successfully.");
    }

    public async Task<Response> ChangePassword(StudentPasswordChangeDto passwordChangeDto)
    {
        var IsSamePassword = await ValidatePasswordChange(passwordChangeDto);
        if(IsSamePassword)
        {
            return CreateResponse.CreateUnsuccessfulResponse("59", "Error Changing Password", "Same Password Provided", $"Your old and new password are the same.");
        }
        Student ExistingStudent = await _context.Students.FirstOrDefaultAsync(s => s.MatricNumber == passwordChangeDto.MatricNumber.ToUpper());
        if (ExistingStudent == null)
        {
            return CreateResponse.CreateUnsuccessfulResponse("19", "Error Changing Password", "Invalid Login Details", $"Incorrect Matric Number provided.");
        }
        bool isCorrectPassword = await _passwordHasher.VerifyPassword(passwordChangeDto.OldPassword, ExistingStudent.Password);
        if(!isCorrectPassword)
        {
            return CreateResponse.CreateUnsuccessfulResponse("29", "Error Changing Password", "Invalid Login Details", $"Incorrect Password provided.");
        }

        ExistingStudent.Password = await _passwordHasher.HashPassword(passwordChangeDto.NewPassword);

        _context.Students.Update(ExistingStudent);

        await _context.SaveChangesAsync();

        return CreateResponse.CreateSuccessfulResponse(await ExistingStudent.ToStudentDetails(), $"Password Changed Successfully.");
    }

    private async Task<bool> ValidatePasswordChange(StudentPasswordChangeDto passwordChangeDto)
    {
        return await Task.FromResult(string.Equals(passwordChangeDto.OldPassword, passwordChangeDto.NewPassword));
    }

    public async Task<Response> EnrollCourse(CreateEnrollmentDto NewEnrollment)
    {
        Student EnrollingStudent = await _context.Students.FirstOrDefaultAsync(s => s.Id == NewEnrollment.StudentId);
        if (EnrollingStudent == null)
        {
            return CreateResponse.CreateUnsuccessfulResponse("29", "Error Enrolling Course", "Invalid Student Details", $"Student with Id: {NewEnrollment.StudentId} does not exist");
        }

        Course EnrollingCourse = await _context.Courses.FirstOrDefaultAsync(c => c.Id == NewEnrollment.CourseId);
        if(EnrollingCourse == null)
        {
            return CreateResponse.CreateUnsuccessfulResponse("29", "Error Enrolling Course", "Invalid Course Details", $"No course with provided Id: {NewEnrollment.CourseId}.");
        }

        Enrollment EnrollmentRecord = await NewEnrollment.ToEnrollmentEntity();

        await _context.Enrollments.AddAsync(EnrollmentRecord);

        await _context.SaveChangesAsync();

        return CreateResponse.CreateSuccessfulResponse(await EnrollmentRecord.ToEnrollmentDto(), "Course Enrollment Successful");
    }

    public async Task<Response> UpdateEnrollment(UpdateEnrollmentDto ModifiedEnrollment)
    {
        if (ModifiedEnrollment.Score != 0 && (ModifiedEnrollment.Score < 0 || ModifiedEnrollment.Score > 100))
        {
            return CreateResponse.CreateUnsuccessfulResponse("09", "Error Updating Enrollment", "Invalid Enrollment Update Details", $"Enrollment cannot have a score of: {ModifiedEnrollment.Score}");
        }
       
        Enrollment ExistingRecord = await _context.Enrollments.FirstOrDefaultAsync(e => e.Id == ModifiedEnrollment.Id);
        if (ExistingRecord == null)
        {
            return CreateResponse.CreateUnsuccessfulResponse("09", "Error Updating Enrollment", "Invalid Enrollment Details", $"Enrollment with Id: {ModifiedEnrollment.StudentId} does not exist");
        }

        Student EnrollingStudent = await _context.Students.FirstOrDefaultAsync(s => s.Id == ModifiedEnrollment.StudentId);
        if (EnrollingStudent == null)
        {
            return CreateResponse.CreateUnsuccessfulResponse("19", "Error Updating Enrollment", "Invalid Student Details", $"Student with Id: {ModifiedEnrollment.StudentId} does not exist");
        }

        Course EnrollingCourse = await _context.Courses.FirstOrDefaultAsync(c => c.Id == ModifiedEnrollment.CourseId);
        if (EnrollingCourse == null)
        {
            return CreateResponse.CreateUnsuccessfulResponse("29", "Error Updating Enrollment", "Invalid Course Details", $"No course with provided Id: {ModifiedEnrollment.CourseId}.");
        }

        ExistingRecord.StudentId = ModifiedEnrollment.StudentId;
        ExistingRecord.CourseId = ModifiedEnrollment.CourseId;

        if(ModifiedEnrollment.Score != 0)
        {
            ExistingRecord.Score = ModifiedEnrollment.Score;
            ExistingRecord.Grade = ModifiedEnrollment.Score >= 70 ? "A" :
                                            ModifiedEnrollment.Score >= 60 ? "B" :
                                            ModifiedEnrollment.Score >= 50 ? "C" :
                                            ModifiedEnrollment.Score >= 45 ? "D" : "F";
        }

        _context.Enrollments.Update(ExistingRecord);

        await _context.SaveChangesAsync();

        return CreateResponse.CreateSuccessfulResponse(await ExistingRecord.ToEnrollmentDto(), "Course Enrollment Update Successful");
    }

    public async Task<Response> GetStudentEnrollments(int StudentId)
    {
        var Enrollments = _context.Enrollments
                                        .Include(e => e.EnrolledStudent)
                                        .Include(e => e.EnrolledCourse)
                                        .Where(e => e.StudentId == StudentId)
                                        .OrderByDescending(e => e.EnrollmentYear)
                                        .Select(e => e.ToEnrollmentDetails())
                                        .ToList();
        return CreateResponse.CreateSuccessfulResponse(Enrollments, "Enrollment Details Fetched Successfully.");
    }

    public async Task<Response> UpdateEnrollmentScore(UpdateEnrollmentScoreDto studentEnrollmentScore)
    {
        
       try
       {
            var enrollmentRecord = await _context.Enrollments.FirstOrDefaultAsync(e => e.Id == studentEnrollmentScore.EnrollmentId);

            if (enrollmentRecord == null)
            {
                return CreateResponse.CreateUnsuccessfulResponse("09", "Error Updating Course", "Invalid Enrollment Details", $"Enrollment with Id: {studentEnrollmentScore.EnrollmentId} does not exist");
            }


            var Grade = studentEnrollmentScore.Score >= 70 ? "A" :
                                            studentEnrollmentScore.Score >= 60 ? "B" :
                                            studentEnrollmentScore.Score >= 50 ? "C" :
                                            studentEnrollmentScore.Score >= 45 ? "D" : "F";

            enrollmentRecord.Score = studentEnrollmentScore.Score;
            enrollmentRecord.Grade = Grade;

            try
            {
                using var scope = _serviceProvider.CreateScope();
                var updateDbContext = scope.ServiceProvider.GetRequiredService<StudentManagementDbContext>();
                //_context.Enrollments.Entry(enrollmentRecord).State = EntityState.Modified;
                updateDbContext.Enrollments.Update(enrollmentRecord);

                await updateDbContext.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                return CreateResponse.CreateUnsuccessfulResponse("99", "Error Updating Database", ex.GetType().FullName, ex.Message);
            }


            return CreateResponse.CreateSuccessfulResponse("Score Updated Successfully", "Score Update Successful.");
       }
        
       catch(Exception ex)
       {
            return CreateResponse.CreateUnsuccessfulResponse("99", "Error Updating Score", ex.GetType().FullName, ex.Message);
        }
    }

}
