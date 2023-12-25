using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class TodoListsUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ListId",
                table: "TodoLists",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_TodoLists_ListId",
                table: "TodoLists",
                column: "ListId");

            migrationBuilder.AddForeignKey(
                name: "FK_TodoLists_Lists_ListId",
                table: "TodoLists",
                column: "ListId",
                principalTable: "Lists",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TodoLists_Lists_ListId",
                table: "TodoLists");

            migrationBuilder.DropIndex(
                name: "IX_TodoLists_ListId",
                table: "TodoLists");

            migrationBuilder.DropColumn(
                name: "ListId",
                table: "TodoLists");
        }
    }
}
