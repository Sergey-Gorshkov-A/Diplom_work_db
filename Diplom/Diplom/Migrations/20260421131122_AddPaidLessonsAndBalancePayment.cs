using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Diplom.Migrations
{
    /// <inheritdoc />
    public partial class AddPaidLessonsAndBalancePayment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PaidLessons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    StudentId = table.Column<int>(type: "INTEGER", nullable: false),
                    LessonId = table.Column<int>(type: "INTEGER", nullable: false),
                    ParentId = table.Column<int>(type: "INTEGER", nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaidLessons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaidLessons_Lessons_LessonId",
                        column: x => x.LessonId,
                        principalTable: "Lessons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PaidLessons_Parents_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Parents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PaidLessons_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PaidLessons_LessonId",
                table: "PaidLessons",
                column: "LessonId");

            migrationBuilder.CreateIndex(
                name: "IX_PaidLessons_ParentId",
                table: "PaidLessons",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_PaidLessons_StudentId",
                table: "PaidLessons",
                column: "StudentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PaidLessons");
        }
    }
}
