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
                name: "Users",
                columns: table => new
                {
                    CardGuid = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.CardGuid);
                });

            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Position = table.Column<int>(type: "INTEGER", nullable: false),
                    UserCardGuid = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Locations_Users_UserCardGuid",
                        column: x => x.UserCardGuid,
                        principalTable: "Users",
                        principalColumn: "CardGuid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Runs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FinishDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Milliseconds = table.Column<double>(type: "REAL", nullable: false),
                    StartLocationId = table.Column<int>(type: "INTEGER", nullable: false),
                    EndLocationId = table.Column<int>(type: "INTEGER", nullable: false),
                    UserCardGuid = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false)
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
                        name: "FK_Runs_Users_UserCardGuid",
                        column: x => x.UserCardGuid,
                        principalTable: "Users",
                        principalColumn: "CardGuid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Locations_UserCardGuid",
                table: "Locations",
                column: "UserCardGuid");

            migrationBuilder.CreateIndex(
                name: "IX_Runs_EndLocationId",
                table: "Runs",
                column: "EndLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Runs_StartLocationId",
                table: "Runs",
                column: "StartLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Runs_UserCardGuid",
                table: "Runs",
                column: "UserCardGuid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Runs");

            migrationBuilder.DropTable(
                name: "Locations");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
