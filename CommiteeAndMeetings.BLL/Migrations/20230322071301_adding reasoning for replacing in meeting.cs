using Microsoft.EntityFrameworkCore.Migrations;

namespace CommiteeAndMeetings.BLL.Migrations
{
    public partial class addingreasoningforreplacinginmeeting : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            

            migrationBuilder.AddColumn<int>(
                name: "AlternativeCoordinatorId",
                schema: "Meeting",
                table: "MeetingCoordinators",
                type: "int",
                nullable: true,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ReasonForReplacing",
                schema: "Meeting",
                table: "MeetingCoordinators",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AlternativeAttendeeId",
                schema: "Meeting",
                table: "MeetingAttendee",
                type: "int",
                nullable: true,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ReasonForReplacing",
                schema: "Meeting",
                table: "MeetingAttendee",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MeetingCoordinators_AlternativeCoordinatorId",
                schema: "Meeting",
                table: "MeetingCoordinators",
                column: "AlternativeCoordinatorId");

            migrationBuilder.CreateIndex(
                name: "IX_MeetingAttendee_AlternativeAttendeeId",
                schema: "Meeting",
                table: "MeetingAttendee",
                column: "AlternativeAttendeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_MeetingAttendee_Users_AlternativeAttendeeId",
                schema: "Meeting",
                table: "MeetingAttendee",
                column: "AlternativeAttendeeId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MeetingCoordinators_Users_AlternativeCoordinatorId",
                schema: "Meeting",
                table: "MeetingCoordinators",
                column: "AlternativeCoordinatorId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MeetingAttendee_Users_AlternativeAttendeeId",
                schema: "Meeting",
                table: "MeetingAttendee");

            migrationBuilder.DropForeignKey(
                name: "FK_MeetingCoordinators_Users_AlternativeCoordinatorId",
                schema: "Meeting",
                table: "MeetingCoordinators");

            migrationBuilder.DropIndex(
                name: "IX_MeetingCoordinators_AlternativeCoordinatorId",
                schema: "Meeting",
                table: "MeetingCoordinators");

            migrationBuilder.DropIndex(
                name: "IX_MeetingAttendee_AlternativeAttendeeId",
                schema: "Meeting",
                table: "MeetingAttendee");

            migrationBuilder.DropColumn(
                name: "AlternativeCoordinatorId",
                schema: "Meeting",
                table: "MeetingCoordinators");

            migrationBuilder.DropColumn(
                name: "ReasonForReplacing",
                schema: "Meeting",
                table: "MeetingCoordinators");

            migrationBuilder.DropColumn(
                name: "AlternativeAttendeeId",
                schema: "Meeting",
                table: "MeetingAttendee");

            migrationBuilder.DropColumn(
                name: "ReasonForReplacing",
                schema: "Meeting",
                table: "MeetingAttendee");

           
        }
    }
}
