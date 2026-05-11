using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourseNoteSharingSystem.Migrations
{
    /// <inheritdoc />
    public partial class NoteCourseRelationshipAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Note_Course_CourseId",
                table: "Note");

            migrationBuilder.AlterColumn<int>(
                name: "CourseId",
                table: "Note",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NoteId",
                table: "Course",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Note_Course_CourseId",
                table: "Note",
                column: "CourseId",
                principalTable: "Course",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Note_Course_CourseId",
                table: "Note");

            migrationBuilder.DropColumn(
                name: "NoteId",
                table: "Course");

            migrationBuilder.AlterColumn<int>(
                name: "CourseId",
                table: "Note",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Note_Course_CourseId",
                table: "Note",
                column: "CourseId",
                principalTable: "Course",
                principalColumn: "Id");
        }
    }
}
