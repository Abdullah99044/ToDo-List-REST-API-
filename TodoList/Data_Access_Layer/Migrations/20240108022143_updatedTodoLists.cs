using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class updatedTodoLists : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "color",
                table: "TodoList");

            migrationBuilder.DropColumn(
                name: "finishedTodoLists",
                table: "TodoList");

            migrationBuilder.DropColumn(
                name: "tottalTodoLists",
                table: "TodoList");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "color",
                table: "TodoList",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "finishedTodoLists",
                table: "TodoList",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "tottalTodoLists",
                table: "TodoList",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
