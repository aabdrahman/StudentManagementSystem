using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StudentManagementApplication.Models;

namespace StudentManagementApplication.Configurations;

public class DepartmentConfig : IEntityTypeConfiguration<Department>
{
    public void Configure(EntityTypeBuilder<Department> builder)
    {
        //Setting database key
        builder.HasKey(x => x.Id);
        
        //Comfiguring Name as Index and unique column
        builder.HasIndex(x => x.Name)
            .IsUnique();

        //Configuring relationship between faculty and department(many departments-to-one faculty)
        builder.HasOne(x => x.faculty)
            .WithMany(x => x.departments)
            .HasForeignKey(x => x.FacultyId)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);
        builder.HasQueryFilter(x => !x.isDeleted);
    }
}
