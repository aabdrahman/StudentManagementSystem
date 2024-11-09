using System.ComponentModel.DataAnnotations;

namespace StudentManagementApplication.Models;

public class Department
{
    public int Id { get; set; }
    [Length(minimumLength: 2, maximumLength: 100)]
    public string Name { get; set; }
    public DateTime CreatedDate { get; set; }
    public bool isDeleted { get; set; } = false;

    //Defining Relationship
    public int? FacultyId { get; set; }
    public Faculty faculty { get; set; }

    //Relationship with Course
    public List<Course> courses { get; set; }
    public List<Student> Students { get; set; }
}
