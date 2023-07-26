using Microsoft.EntityFrameworkCore.Migrations;

namespace CommiteeAndMeetings.BLL.Migrations
{
    public partial class addcol : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                 name: "CommiteeTaskId",
                 schema: "Committe",
                 table: "UpdateTaskLog",
                 type: "int",
                 nullable: false,
                 defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
             name: "CommiteeTaskId",
             schema: "Committe",
             table: "UpdateTaskLog");
        }
    }
}
