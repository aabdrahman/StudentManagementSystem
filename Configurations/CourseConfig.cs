using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StudentManagementApplication.Models;

namespace StudentManagementApplication.Configurations;

public class CourseConfig : IEntityTypeConfiguration<Course>
{
    public void Configure(EntityTypeBuilder<Course> builder)
    {
        //Configuring the database key
        builder.HasKey(x => x.Id);
        
        //Configuring index on course
        builder.HasIndex(x => x.CourseCode)
            .IsUnique();

        //Configuring relationship with department
        builder.HasOne(x => x.department)
            .WithMany(x => x.courses)
            .HasForeignKey(x => x.DepartmentId)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);
        builder.HasQueryFilter(x => !x.isDeleted);
    }
}
