using Microsoft.EntityFrameworkCore.Migrations;

namespace CommiteeAndMeetings.BLL.Migrations
{
    public partial class AddTaskDetailstoUpdateTaskLogMainAssignedUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           
            migrationBuilder.AddColumn<string>(
                name: "TaskDetails",
                schema: "Committe",
                table: "UpdateTaskLog",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TaskDetails",
                schema: "Committe",
                table: "UpdateTaskLog");

           
        }
    }
}
