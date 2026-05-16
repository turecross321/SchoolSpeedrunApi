using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolSpeedrunApi.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Registrations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    CardGuid = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    CreationDate = table.Column<string>(type: "TEXT", nullable: false),
                    ExpiryDate = table.Column<string>(type: "TEXT", nullable: false),
                    Scanned = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Registrations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CardGuid = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    Username = table.Column<string>(type: "TEXT", maxLength: 36, nullable: false),
                    RegistrationDate = table.Column<string>(type: "TEXT", nullable: false),
                    SchoolProgram = table.Column<int>(type: "INTEGER", nullable: false),
                    ProfilePictureHash = table.Column<string>(type: "TEXT", maxLength: 64, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Date = table.Column<string>(type: "TEXT", nullable: false),
                    Position = table.Column<int>(type: "INTEGER", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Locations_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Runs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FinishDate = table.Column<string>(type: "TEXT", nullable: false),
                    Milliseconds = table.Column<double>(type: "REAL", nullable: false),
                    StartPosition = table.Column<int>(type: "INTEGER", nullable: false),
                    EndPosition = table.Column<int>(type: "INTEGER", nullable: false),
                    StartLocationId = table.Column<int>(type: "INTEGER", nullable: false),
                    EndLocationId = table.Column<int>(type: "INTEGER", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Runs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Runs_Locations_EndLocationId",
                        column: x => x.EndLocationId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Runs_Locations_StartLocationId",
                        column: x => x.StartLocationId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Runs_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Locations_UserId",
                table: "Locations",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Runs_EndLocationId",
                table: "Runs",
                column: "EndLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Runs_StartLocationId",
                table: "Runs",
                column: "StartLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Runs_UserId",
                table: "Runs",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Registrations");

            migrationBuilder.DropTable(
                name: "Runs");

            migrationBuilder.DropTable(
                name: "Locations");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
