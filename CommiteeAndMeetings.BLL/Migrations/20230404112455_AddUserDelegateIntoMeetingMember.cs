using Microsoft.EntityFrameworkCore.Migrations;

namespace CommiteeAndMeetings.BLL.Migrations
{
    public partial class AddUserDelegateIntoMeetingMember : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
          

            migrationBuilder.AddColumn<int>(
                name: "UserDelegateUserId",
                schema: "Meeting",
                table: "MeetingCoordinators",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserDelegateUserId",
                schema: "Meeting",
                table: "MeetingAttendee",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MeetingCoordinators_UserDelegateUserId",
                schema: "Meeting",
                table: "MeetingCoordinators",
                column: "UserDelegateUserId");

            migrationBuilder.CreateIndex(
                name: "IX_MeetingAttendee_UserDelegateUserId",
                schema: "Meeting",
                table: "MeetingAttendee",
                column: "UserDelegateUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_MeetingAttendee_Users_UserDelegateUserId",
                schema: "Meeting",
                table: "MeetingAttendee",
                column: "UserDelegateUserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MeetingCoordinators_Users_UserDelegateUserId",
                schema: "Meeting",
                table: "MeetingCoordinators",
                column: "UserDelegateUserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MeetingAttendee_Users_UserDelegateUserId",
                schema: "Meeting",
                table: "MeetingAttendee");

            migrationBuilder.DropForeignKey(
                name: "FK_MeetingCoordinators_Users_UserDelegateUserId",
                schema: "Meeting",
                table: "MeetingCoordinators");

            migrationBuilder.DropIndex(
                name: "IX_MeetingCoordinators_UserDelegateUserId",
                schema: "Meeting",
                table: "MeetingCoordinators");

            migrationBuilder.DropIndex(
                name: "IX_MeetingAttendee_UserDelegateUserId",
                schema: "Meeting",
                table: "MeetingAttendee");

            migrationBuilder.DropColumn(
                name: "UserDelegateUserId",
                schema: "Meeting",
                table: "MeetingCoordinators");

            migrationBuilder.DropColumn(
                name: "UserDelegateUserId",
                schema: "Meeting",
                table: "MeetingAttendee");

         
        }
    }
}
