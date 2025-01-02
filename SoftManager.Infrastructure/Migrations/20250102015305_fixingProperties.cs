using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoftManager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class fixingProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "task",
                table: "UserTasks",
                newName: "Task");

            migrationBuilder.AlterColumn<string>(
                name: "Message",
                table: "UserTasks",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(2000)",
                oldMaxLength: 2000);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Task",
                table: "UserTasks",
                newName: "task");

            migrationBuilder.AlterColumn<string>(
                name: "Message",
                table: "UserTasks",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(2000)",
                oldMaxLength: 2000,
                oldNullable: true);
        }
    }
}
