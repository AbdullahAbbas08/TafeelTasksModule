using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace CommiteeAndMeetings.BLL.Migrations
{
    public partial class AddDefultValueForTopicState : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {


            //migrationBuilder.AlterColumn<DateTimeOffset>(
            //    name: "PauseDateTime",
            //    schema: "Meeting",
            //    table: "TopicPauseDates",
            //    type: "datetimeoffset",
            //    nullable: true,
            //    oldClrType: typeof(DateTimeOffset),
            //    oldType: "datetimeoffset");

            //migrationBuilder.AlterColumn<DateTimeOffset>(
            //    name: "ContinueDateTime",
            //    schema: "Meeting",
            //    table: "TopicPauseDates",
            //    type: "datetimeoffset",
            //    nullable: true,
            //    oldClrType: typeof(DateTimeOffset),
            //    oldType: "datetimeoffset");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.RenameTable(
            //    name: "CommiteeSavedAttachmentSavedAttachment",
            //    schema: "Committe",
            //    newName: "CommiteeSavedAttachmentSavedAttachment");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "PauseDateTime",
                schema: "Meeting",
                table: "TopicPauseDates",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "ContinueDateTime",
                schema: "Meeting",
                table: "TopicPauseDates",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldNullable: true);
        }
    }
}
