using Microsoft.EntityFrameworkCore.Migrations;

namespace CommiteeAndMeetings.BLL.Migrations
{
    public partial class AddMinuteOfMeetingTopicTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_MeetingTopics_MinutesOfMeetings_MinuteOfMeetingId",
            //    schema: "Meeting",
            //    table: "MeetingTopics");

            //migrationBuilder.DropIndex(
            //    name: "IX_MeetingTopics_MinuteOfMeetingId",
            //    schema: "Meeting",
            //    table: "MeetingTopics");

            //migrationBuilder.DropColumn(
            //    name: "MinuteOfMeetingId",
            //    schema: "Meeting",
            //    table: "MeetingTopics");



            //migrationBuilder.CreateTable(
            //    name: "MinuteOfMeetingTopics",
            //    schema: "Meeting",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        MinuteOfMeetingId = table.Column<int>(type: "int", nullable: false),
            //        MeetingTopicId = table.Column<int>(type: "int", nullable: false),
            //        CreatedBy = table.Column<int>(type: "int", nullable: true),
            //        CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_MinuteOfMeetingTopics", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_MinuteOfMeetingTopics_MeetingTopics_MeetingTopicId",
            //            column: x => x.MeetingTopicId,
            //            principalSchema: "Meeting",
            //            principalTable: "MeetingTopics",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Restrict);
            //        table.ForeignKey(
            //            name: "FK_MinuteOfMeetingTopics_MinutesOfMeetings_MinuteOfMeetingId",
            //            column: x => x.MinuteOfMeetingId,
            //            principalSchema: "Meeting",
            //            principalTable: "MinutesOfMeetings",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Restrict);
            //    });

            //migrationBuilder.CreateIndex(
            //    name: "IX_MinuteOfMeetingTopics_MeetingTopicId",
            //    schema: "Meeting",
            //    table: "MinuteOfMeetingTopics",
            //    column: "MeetingTopicId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_MinuteOfMeetingTopics_MinuteOfMeetingId",
            //    schema: "Meeting",
            //    table: "MinuteOfMeetingTopics",
            //    column: "MinuteOfMeetingId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MinuteOfMeetingTopics",
                schema: "Meeting");

            //migrationBuilder.RenameTable(
            //    name: "CommiteeSavedAttachmentSavedAttachment",
            //    schema: "Committe",
            //    newName: "CommiteeSavedAttachmentSavedAttachment");

            migrationBuilder.AddColumn<int>(
                name: "MinuteOfMeetingId",
                schema: "Meeting",
                table: "MeetingTopics",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MeetingTopics_MinuteOfMeetingId",
                schema: "Meeting",
                table: "MeetingTopics",
                column: "MinuteOfMeetingId");

            migrationBuilder.AddForeignKey(
                name: "FK_MeetingTopics_MinutesOfMeetings_MinuteOfMeetingId",
                schema: "Meeting",
                table: "MeetingTopics",
                column: "MinuteOfMeetingId",
                principalSchema: "Meeting",
                principalTable: "MinutesOfMeetings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
