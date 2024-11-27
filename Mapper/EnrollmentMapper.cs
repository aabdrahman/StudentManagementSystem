using StudentManagementApplication.Contracts.Enrollment;
using StudentManagementApplication.Models;
using System.Runtime.CompilerServices;

namespace StudentManagementApplication.Mapper;

public static class EnrollmentMapper
{
    public static async Task<Enrollment> ToEnrollmentEntity(this CreateEnrollmentDto NewEnrollment)
    {
        return new Enrollment()
        {
            StudentId = NewEnrollment.StudentId,
            CourseId = NewEnrollment.CourseId
        };
    }

    public static async Task<Enrollment> ToEnrollmentEntity(this UpdateEnrollmentDto UpdatedEnrollment)
    {
        return new Enrollment()
        { 
            StudentId = UpdatedEnrollment.StudentId,
            CourseId = UpdatedEnrollment.CourseId,
            Score = UpdatedEnrollment?.Score
        };
    }

    public static async Task<EnrollmentDto> ToEnrollmentDto(this Enrollment enrollment)
    {
        return new EnrollmentDto
        (
            EnrollmentId: enrollment.Id,
            StudentId: (int)enrollment.StudentId!,
            CourseId: (int)enrollment.CourseId!,
            EnrollmentYear: enrollment.EnrollmentYear
        );
    }

    public static EnrollmentDetailsDto ToEnrollmentDetails(this Enrollment enrollment)
    {
        return new EnrollmentDetailsDto
        (
            EnrollmentId: enrollment.Id,
            MatricNumber: enrollment.EnrolledStudent?.MatricNumber!,
            StudentName: enrollment.EnrolledStudent?.Name!,
            CourseTitle: enrollment.EnrolledCourse?.CourseTitle!,
            CourseCode: enrollment.EnrolledCourse?.CourseCode!,
            EnrollmentYear: enrollment.EnrollmentYear
        );
    }
}
