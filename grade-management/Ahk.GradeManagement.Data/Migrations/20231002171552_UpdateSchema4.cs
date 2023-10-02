using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ahk.GradeManagement.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSchema4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Points_ExerciseId",
                table: "Points");

            migrationBuilder.CreateIndex(
                name: "IX_Points_ExerciseId",
                table: "Points",
                column: "ExerciseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Points_ExerciseId",
                table: "Points");

            migrationBuilder.CreateIndex(
                name: "IX_Points_ExerciseId",
                table: "Points",
                column: "ExerciseId",
                unique: true);
        }
    }
}
