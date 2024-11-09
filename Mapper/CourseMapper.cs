using StudentManagementApplication.Contracts.Course;
using StudentManagementApplication.Models;

namespace StudentManagementApplication.Mapper;

public static class CourseMapper
{
    public static Course ToCourseEntity(this UpdateCourseDto updateCourseDto)
    {
        return new Course()
        {
            CourseCode = updateCourseDto.CourseCode,
            CourseTitle = updateCourseDto.CourseTitle,
            DepartmentId = updateCourseDto.DepartmentId,
            Id = updateCourseDto.Id,
        };

    }
    public static Course ToCourseEmtity(this CreateCourseDto NewCourse)
    {
        return new Course()
        {
            CourseCode = NewCourse.CourseCode,
            CourseTitle = NewCourse.CourseTitle,
            DepartmentId = NewCourse.DepartmentId
        };

    }

    public static CourseDto ToCourseDto(this Course course)
    {
        return new CourseDto
        (
            CourseTitle: course.CourseTitle,
            CourseCode: course.CourseCode,
            Department: course.department?.Name
        );
    }
}
