using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StudentManagementApplication.Models;

namespace StudentManagementApplication.Configurations;

public class FacultyConfig : IEntityTypeConfiguration<Faculty>
{
    public void Configure(EntityTypeBuilder<Faculty> builder)
    {
        //Configuring table key
        builder.HasKey(x => x.Id);

        //Configuring table index
        builder.HasIndex(x => x.Name)
            .IsUnique();

        //Setting default value for createdDate column
        builder.Property(x => x.CreatedDate)
            .HasDefaultValueSql("getdate()");
        builder.HasQueryFilter(x => !x.isDeleted);
    }
}
