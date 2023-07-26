using Microsoft.EntityFrameworkCore.Migrations;

namespace CommiteeAndMeetings.BLL.Migrations
{
    public partial class AddNotificationColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            //migrationBuilder.AddColumn<int>(
            //    name: "MeetingId",
            //    schema: "Committe",
            //    table: "CommitteeNotifications",
            //    type: "int",
            //    nullable: true);

            //migrationBuilder.AddColumn<int>(
            //    name: "MeetingTopicId",
            //    schema: "Committe",
            //    table: "CommitteeNotifications",
            //    type: "int",
            //    nullable: true);

            //migrationBuilder.AddColumn<int>(
            //    name: "MinuteOfMeetingId",
            //    schema: "Committe",
            //    table: "CommitteeNotifications",
            //    type: "int",
            //    nullable: true);

            //migrationBuilder.CreateIndex(
            //    name: "IX_CommitteeNotifications_MeetingId",
            //    schema: "Committe",
            //    table: "CommitteeNotifications",
            //    column: "MeetingId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_CommitteeNotifications_MeetingTopicId",
            //    schema: "Committe",
            //    table: "CommitteeNotifications",
            //    column: "MeetingTopicId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_CommitteeNotifications_MinuteOfMeetingId",
            //    schema: "Committe",
            //    table: "CommitteeNotifications",
            //    column: "MinuteOfMeetingId");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_CommitteeNotifications_Meetings_MeetingId",
            //    schema: "Committe",
            //    table: "CommitteeNotifications",
            //    column: "MeetingId",
            //    principalSchema: "Meeting",
            //    principalTable: "Meetings",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Restrict);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_CommitteeNotifications_MeetingTopics_MeetingTopicId",
            //    schema: "Committe",
            //    table: "CommitteeNotifications",
            //    column: "MeetingTopicId",
            //    principalSchema: "Meeting",
            //    principalTable: "MeetingTopics",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Restrict);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_CommitteeNotifications_MinutesOfMeetings_MinuteOfMeetingId",
            //    schema: "Committe",
            //    table: "CommitteeNotifications",
            //    column: "MinuteOfMeetingId",
            //    principalSchema: "Meeting",
            //    principalTable: "MinutesOfMeetings",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommitteeNotifications_Meetings_MeetingId",
                schema: "Committe",
                table: "CommitteeNotifications");

            migrationBuilder.DropForeignKey(
                name: "FK_CommitteeNotifications_MeetingTopics_MeetingTopicId",
                schema: "Committe",
                table: "CommitteeNotifications");

            migrationBuilder.DropForeignKey(
                name: "FK_CommitteeNotifications_MinutesOfMeetings_MinuteOfMeetingId",
                schema: "Committe",
                table: "CommitteeNotifications");

            migrationBuilder.DropIndex(
                name: "IX_CommitteeNotifications_MeetingId",
                schema: "Committe",
                table: "CommitteeNotifications");

            migrationBuilder.DropIndex(
                name: "IX_CommitteeNotifications_MeetingTopicId",
                schema: "Committe",
                table: "CommitteeNotifications");

            migrationBuilder.DropIndex(
                name: "IX_CommitteeNotifications_MinuteOfMeetingId",
                schema: "Committe",
                table: "CommitteeNotifications");

            migrationBuilder.DropColumn(
                name: "MeetingId",
                schema: "Committe",
                table: "CommitteeNotifications");

            migrationBuilder.DropColumn(
                name: "MeetingTopicId",
                schema: "Committe",
                table: "CommitteeNotifications");

            migrationBuilder.DropColumn(
                name: "MinuteOfMeetingId",
                schema: "Committe",
                table: "CommitteeNotifications");


        }
    }
}
