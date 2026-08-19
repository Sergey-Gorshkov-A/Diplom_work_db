using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Diplom.Migrations
{
    /// <inheritdoc />
    public partial class AddAdminAndSchedules : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lessons_Days_DayId",
                table: "Lessons");

            migrationBuilder.AlterColumn<int>(
                name: "DayId",
                table: "Lessons",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddForeignKey(
                name: "FK_Lessons_Days_DayId",
                table: "Lessons",
                column: "DayId",
                principalTable: "Days",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lessons_Days_DayId",
                table: "Lessons");

            migrationBuilder.AlterColumn<int>(
                name: "DayId",
                table: "Lessons",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Lessons_Days_DayId",
                table: "Lessons",
                column: "DayId",
                principalTable: "Days",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
