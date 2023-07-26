using Microsoft.EntityFrameworkCore.Migrations;

namespace CommiteeAndMeetings.BLL.Migrations
{
    public partial class AlterMeetingCoordinatorAndAttendessTableAddTitleAREn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropColumn(
            //    name: "Confirmed",
            //    schema: "Meeting",
            //    table: "MeetingCoordinators");

            //migrationBuilder.DropColumn(
            //    name: "Confirmed",
            //    schema: "Meeting",
            //    table: "MeetingAttendee");



            //migrationBuilder.AddColumn<int>(
            //    name: "State",
            //    schema: "Meeting",
            //    table: "MeetingCoordinators",
            //    type: "int",
            //    nullable: false,
            //    defaultValue: 0);

            //migrationBuilder.AddColumn<int>(
            //    name: "State",
            //    schema: "Meeting",
            //    table: "MeetingAttendee",
            //    type: "int",
            //    nullable: false,
            //    defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "State",
                schema: "Meeting",
                table: "MeetingCoordinators");

            migrationBuilder.DropColumn(
                name: "State",
                schema: "Meeting",
                table: "MeetingAttendee");

            migrationBuilder.RenameTable(
                name: "CommiteeSavedAttachmentSavedAttachment",
                schema: "Committe",
                newName: "CommiteeSavedAttachmentSavedAttachment");

            migrationBuilder.AddColumn<bool>(
                name: "Confirmed",
                schema: "Meeting",
                table: "MeetingCoordinators",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Confirmed",
                schema: "Meeting",
                table: "MeetingAttendee",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
