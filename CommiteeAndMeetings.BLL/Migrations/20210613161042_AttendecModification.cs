using Microsoft.EntityFrameworkCore.Migrations;

namespace CommiteeAndMeetings.BLL.Migrations
{
    public partial class AttendecModification : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //            migrationBuilder.DropForeignKey(
            //                name: "FK_MeetingAttendee_Users_CoordinatorId",
            //                schema: "Meeting",
            //                table: "MeetingAttendee");

            //            migrationBuilder.RenameColumn(
            //                name: "CoordinatorId",
            //                schema: "Meeting",
            //                table: "MeetingAttendee",
            //                newName: "AttendeeId");

            //            migrationBuilder.RenameIndex(
            //                name: "IX_MeetingAttendee_CoordinatorId",
            //                schema: "Meeting",
            //                table: "MeetingAttendee",
            //                newName: "IX_MeetingAttendee_AttendeeId");

            //            migrationBuilder.AddForeignKey(
            //                name: "FK_MeetingAttendee_Users_AttendeeId",
            //                schema: "Meeting",
            //                table: "MeetingAttendee",
            //                column: "AttendeeId",
            //                principalTable: "Users",
            //                principalColumn: "UserId",
            //                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //            migrationBuilder.DropForeignKey(
            //                name: "FK_MeetingAttendee_Users_AttendeeId",
            //                schema: "Meeting",
            //                table: "MeetingAttendee");

            //            //migrationBuilder.RenameTable(
            //            //    name: "CommiteeSavedAttachmentSavedAttachment",
            //            //    schema: "Committe",
            //            //    newName: "CommiteeSavedAttachmentSavedAttachment");

            //            migrationBuilder.RenameColumn(
            //                name: "AttendeeId",
            //                schema: "Meeting",
            //                table: "MeetingAttendee",
            //                newName: "CoordinatorId");

            //            migrationBuilder.RenameIndex(
            //                name: "IX_MeetingAttendee_AttendeeId",
            //                schema: "Meeting",
            //                table: "MeetingAttendee",
            //                newName: "IX_MeetingAttendee_CoordinatorId");

            //            migrationBuilder.AddForeignKey(
            //                name: "FK_MeetingAttendee_Users_CoordinatorId",
            //                schema: "Meeting",
            //                table: "MeetingAttendee",
            //                column: "CoordinatorId",
            //                principalTable: "Users",
            //                principalColumn: "UserId",
            //                onDelete: ReferentialAction.Restrict);
        }
    }
}
