namespace StudentManagementApplication.Models;

public class Enrollment
{
    public int Id { get; set; }
    public int EnrollmentYear { get; set; }
    public DateTime EnrollmentDate { get; set; }
    public bool isDeleted { get; set; } = false;

    //Configuring relationships
    public int? CourseId { get; set; }
    public int? StudentId { get; set; }
    public Course EnrolledCourse { get; set; }
    public Student EnrolledStudent { get; set; }

}
