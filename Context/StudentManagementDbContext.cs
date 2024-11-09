using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using StudentManagementApplication.Configurations;
using StudentManagementApplication.Models;

namespace StudentManagementApplication.Context;

public class StudentManagementDbContext : DbContext
{   
    private readonly DbSettings _dbSettings;
    private readonly string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=StudentManagement;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";

    public StudentManagementDbContext(IOptions<DbSettings> dbSettings, DbContextOptions<StudentManagementDbContext> options) : base(options)
    {
        _dbSettings = dbSettings.Value;
    }

    public DbSet<Student> Students { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<Faculty> Faculties { get; set; }
    public DbSet<Enrollment> Enrollments { get; set; }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(_dbSettings.ConnectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new FacultyConfig());
        modelBuilder.ApplyConfiguration(new EnrollmentConfig());
        modelBuilder.ApplyConfiguration(new DepartmentConfig());
        modelBuilder.ApplyConfiguration(new StudentConfig());
        modelBuilder.ApplyConfiguration(new CourseConfig());
    }
}
