using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentManagementApplication.Context.DbMigrations
{
    /// <inheritdoc />
    public partial class AddEnrollmentScoreAndGradeColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Grade",
                table: "Enrollments",
                type: "nvarchar(1)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Score",
                table: "Enrollments",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Grade",
                table: "Enrollments");

            migrationBuilder.DropColumn(
                name: "Score",
                table: "Enrollments");
        }
    }
}
