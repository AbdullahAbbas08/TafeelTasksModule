using Microsoft.EntityFrameworkCore.Migrations;

namespace CommiteeAndMeetings.BLL.Migrations
{
    public partial class AddIsCanceledtomeeting : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            

            migrationBuilder.AddColumn<bool>(
                name: "IsCanceled",
                schema: "Meeting",
                table: "Meetings",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCanceled",
                schema: "Meeting",
                table: "Meetings");

           
        }
    }
}
