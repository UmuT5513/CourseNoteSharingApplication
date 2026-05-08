using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourseNoteSharingSystem.Migrations
{
    /// <inheritdoc />
    public partial class RenameColumnOfCourseCodeAndName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Course",
                newName: "CourseName");

            migrationBuilder.RenameColumn(
                name: "Code",
                table: "Course",
                newName: "CourseCode");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CourseName",
                table: "Course",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "CourseCode",
                table: "Course",
                newName: "Code");
        }
    }
}
