using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToDoList.Migrations
{
    public partial class Init2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "TasksLists",
                columns: new[] { "Id", "CategoryId", "Title" },
                values: new object[] { 2147483647, null, "My day" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "TasksLists",
                keyColumn: "Id",
                keyValue: 2147483647);
        }
    }
}
