using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolSpeedrunApi.Migrations
{
    /// <inheritdoc />
    public partial class AddStartAndEndPositionToRun : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EndPosition",
                table: "Runs",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StartPosition",
                table: "Runs",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndPosition",
                table: "Runs");

            migrationBuilder.DropColumn(
                name: "StartPosition",
                table: "Runs");
        }
    }
}
