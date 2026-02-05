using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class UserToPerson : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Users_ApprovedByInstructorId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Users_StudentId",
                table: "Bookings");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.CreateTable(
                name: "People",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    FullName = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    UserType = table.Column<string>(type: "TEXT", nullable: false),
                    ProgramType = table.Column<string>(type: "TEXT", nullable: true),
                    WeeklyQuotaHours = table.Column<int>(type: "INTEGER", nullable: true),
                    CurrentWeekUsedHours = table.Column<double>(type: "REAL", nullable: false),
                    WeekStartDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    TotalPracticeHours = table.Column<double>(type: "REAL", nullable: false),
                    NoShowCount = table.Column<int>(type: "INTEGER", nullable: false),
                    PenaltyMultiplier = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_People", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_People_ApprovedByInstructorId",
                table: "Bookings",
                column: "ApprovedByInstructorId",
                principalTable: "People",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_People_StudentId",
                table: "Bookings",
                column: "StudentId",
                principalTable: "People",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_People_ApprovedByInstructorId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_People_StudentId",
                table: "Bookings");

            migrationBuilder.DropTable(
                name: "People");

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    CurrentWeekUsedHours = table.Column<double>(type: "REAL", nullable: false),
                    FullName = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    NoShowCount = table.Column<int>(type: "INTEGER", nullable: false),
                    PenaltyMultiplier = table.Column<double>(type: "REAL", nullable: false),
                    ProgramType = table.Column<string>(type: "TEXT", nullable: true),
                    TotalPracticeHours = table.Column<double>(type: "REAL", nullable: false),
                    UserType = table.Column<string>(type: "TEXT", nullable: false),
                    WeekStartDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    WeeklyQuotaHours = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Users_ApprovedByInstructorId",
                table: "Bookings",
                column: "ApprovedByInstructorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Users_StudentId",
                table: "Bookings",
                column: "StudentId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
