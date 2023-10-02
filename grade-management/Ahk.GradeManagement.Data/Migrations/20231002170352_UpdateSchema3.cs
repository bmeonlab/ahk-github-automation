using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ahk.GradeManagement.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSchema3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Grades_AssignmentId",
                table: "Grades");

            migrationBuilder.CreateIndex(
                name: "IX_Grades_AssignmentId",
                table: "Grades",
                column: "AssignmentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Grades_AssignmentId",
                table: "Grades");

            migrationBuilder.CreateIndex(
                name: "IX_Grades_AssignmentId",
                table: "Grades",
                column: "AssignmentId",
                unique: true);
        }
    }
}
