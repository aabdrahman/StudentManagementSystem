using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StudentManagementApplication.Models;

namespace StudentManagementApplication.Configurations;

public class StudentConfig : IEntityTypeConfiguration<Student>
{
    public void Configure(EntityTypeBuilder<Student> builder)
    {
        //Configuring database table key
        builder.HasKey(x => x.Id);

        //Configure index
        builder.HasIndex(x  => x.MatricNumber)
            .IsUnique();

        builder.HasIndex(x => x.Email)
            .IsUnique();

        //Configuring student - depatment relationships
        builder.HasOne(x => x.department)
            .WithMany(x => x.Students)
            .HasForeignKey(x => x.DepartmentId)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);

        //Setting global query filter
        builder.HasQueryFilter(x => !x.isDeleted);

        //Setting Current Date as register date
        builder.Property(x => x.RegisterDate)
            .HasDefaultValueSql("getdate()");
    }
}
