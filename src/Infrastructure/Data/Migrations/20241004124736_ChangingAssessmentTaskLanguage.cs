using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Therasim.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangingAssessmentTaskLanguage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Instructions",
                table: "AssessmentTaskLanguage",
                newName: "EffectiveResponse");

            migrationBuilder.RenameColumn(
                name: "ClientPersona",
                table: "AssessmentTaskLanguage",
                newName: "ClientInitialDialogue");

            migrationBuilder.AddColumn<string>(
                name: "AssessmentCriteria",
                table: "AssessmentTaskLanguage",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AssessmentCriteria",
                table: "AssessmentTaskLanguage");

            migrationBuilder.RenameColumn(
                name: "EffectiveResponse",
                table: "AssessmentTaskLanguage",
                newName: "Instructions");

            migrationBuilder.RenameColumn(
                name: "ClientInitialDialogue",
                table: "AssessmentTaskLanguage",
                newName: "ClientPersona");
        }
    }
}
