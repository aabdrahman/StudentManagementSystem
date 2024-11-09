using System.ComponentModel.DataAnnotations;

namespace StudentManagementApplication.Models;

public class Student
{
    public int Id { get; set; }
    public DateTime RegisterDate { get; set; }
    [StringLength(maximumLength: 100, MinimumLength = 12)]
    public string Name { get; set; }
    public string MatricNumber { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public bool isDeleted { get; set; } = false;

    //Configuring relationships
    public int? DepartmentId { get; set; }
    public Department department { get; set; }
    public List<Enrollment> StudentEnrollments { get; set; }

}
