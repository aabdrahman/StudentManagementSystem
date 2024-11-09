namespace StudentManagementApplication.Models;

public class Faculty
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime CreatedDate { get; set; }
    public bool isDeleted { get; set; } = false;

    //Defining Relationships
    public List<Department> departments { get; set; } 
}
