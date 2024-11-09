namespace StudentManagementApplication.Models;

public class Course
{
    public int Id { get; set; }
    public string CourseCode { get; set; }
    public string CourseTitle { get; set; }

    public bool isDeleted { get; set; } = false;

    //Configuring relationships
    public int? DepartmentId { get; set; }
    public Department department { get; set; }

    public List<Enrollment> CourseEnrollments { get; set; }
}
