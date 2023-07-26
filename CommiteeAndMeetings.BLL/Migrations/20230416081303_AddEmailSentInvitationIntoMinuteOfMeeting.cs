using Microsoft.EntityFrameworkCore.Migrations;

namespace CommiteeAndMeetings.BLL.Migrations
{
    public partial class AddEmailSentInvitationIntoMinuteOfMeeting : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           

            migrationBuilder.AddColumn<bool>(
                name: "EmailSentInvitation",
                schema: "Meeting",
                table: "MinutesOfMeetings",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmailSentInvitation",
                schema: "Meeting",
                table: "MinutesOfMeetings");

         
        }
    }
}
