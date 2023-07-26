using Microsoft.EntityFrameworkCore.Migrations;

namespace CommiteeAndMeetings.BLL.Migrations
{
    public partial class AlterMeetingAddSurveyRelationship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_Surveys_MinutesOfMeetings_MinuteOfMeetingId",
            //    schema: "Committe",
            //    table: "Surveys");


            //migrationBuilder.RenameColumn(
            //    name: "MinuteOfMeetingId",
            //    schema: "Committe",
            //    table: "Surveys",
            //    newName: "MeetingId");

            //migrationBuilder.RenameIndex(
            //    name: "IX_Surveys_MinuteOfMeetingId",
            //    schema: "Committe",
            //    table: "Surveys",
            //    newName: "IX_Surveys_MeetingId");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Surveys_Meetings_MeetingId",
            //    schema: "Committe",
            //    table: "Surveys",
            //    column: "MeetingId",
            //    principalSchema: "Meeting",
            //    principalTable: "Meetings",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Surveys_Meetings_MeetingId",
                schema: "Committe",
                table: "Surveys");

            //migrationBuilder.RenameTable(
            //    name: "CommiteeSavedAttachmentSavedAttachment",
            //    schema: "Committe",
            //    newName: "CommiteeSavedAttachmentSavedAttachment");

            migrationBuilder.RenameColumn(
                name: "MeetingId",
                schema: "Committe",
                table: "Surveys",
                newName: "MinuteOfMeetingId");

            migrationBuilder.RenameIndex(
                name: "IX_Surveys_MeetingId",
                schema: "Committe",
                table: "Surveys",
                newName: "IX_Surveys_MinuteOfMeetingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Surveys_MinutesOfMeetings_MinuteOfMeetingId",
                schema: "Committe",
                table: "Surveys",
                column: "MinuteOfMeetingId",
                principalSchema: "Meeting",
                principalTable: "MinutesOfMeetings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
