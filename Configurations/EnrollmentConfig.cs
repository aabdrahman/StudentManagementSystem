using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StudentManagementApplication.Models;

namespace StudentManagementApplication.Configurations;

public class EnrollmentConfig : IEntityTypeConfiguration<Enrollment>
{
    public void Configure(EntityTypeBuilder<Enrollment> builder)
    {
        //Configure tanle key
        builder.HasKey(e => e.Id);

        //Configure index
        builder.HasIndex(e => e.Id);

        //Configure relationship with course
        builder.HasOne(x => x.EnrolledCourse)
            .WithMany(x => x.CourseEnrollments)
            .HasForeignKey(x => x.CourseId)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);

        //Configure relationship with students
        builder.HasOne(x => x.EnrolledStudent)
            .WithMany(x => x.StudentEnrollments)
            .HasForeignKey(x => x.StudentId)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);

        //Setting default value for Enrollment date and year
        builder.Property(x => x.EnrollmentDate)
            .HasDefaultValueSql("getdate()");
        builder.Property(x => x.EnrollmentYear)
            .HasDefaultValueSql("datepart(year, getdate())");
        builder.Property(s => s.Grade)
            .HasMaxLength(5);
    }
}
