using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Therasim.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddingLanguagetoSimulation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Language",
                table: "Simulations",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Language",
                table: "Simulations");
        }
    }
}
