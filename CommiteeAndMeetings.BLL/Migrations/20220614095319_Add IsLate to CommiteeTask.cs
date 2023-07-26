using Microsoft.EntityFrameworkCore.Migrations;

namespace CommiteeAndMeetings.BLL.Migrations
{
    public partial class AddIsLatetoCommiteeTask : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            

            //migrationBuilder.AddColumn<bool>(
            //    name: "Islated",
            //    schema: "Committe",
            //    table: "CommiteeTasks",
            //    type: "bit",
            //    nullable: false,
            //    defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Islated",
                schema: "Committe",
                table: "CommiteeTasks");

            
        }
    }
}
