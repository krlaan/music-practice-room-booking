using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class RemoveApprovedByInstructor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_People_ApprovedByInstructorId",
                table: "Bookings");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_ApprovedByInstructorId",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "ApprovedByInstructorId",
                table: "Bookings");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ApprovedByInstructorId",
                table: "Bookings",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_ApprovedByInstructorId",
                table: "Bookings",
                column: "ApprovedByInstructorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_People_ApprovedByInstructorId",
                table: "Bookings",
                column: "ApprovedByInstructorId",
                principalTable: "People",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
