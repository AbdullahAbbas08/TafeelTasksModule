using Microsoft.EntityFrameworkCore.Migrations;

namespace CommiteeAndMeetings.BLL.Migrations
{
    public partial class AddMOMComment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            //migrationBuilder.AddColumn<bool>(
            //    name: "Colsed",
            //    schema: "Meeting",
            //    table: "Meetings",
            //    type: "bit",
            //    nullable: false,
            //    defaultValue: false);

            //migrationBuilder.CreateTable(
            //    name: "MOMComments",
            //    schema: "Meeting",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        MinuteOfMeetingId = table.Column<int>(type: "int", nullable: false),
            //        CommentId = table.Column<int>(type: "int", nullable: false),
            //        CommentType = table.Column<int>(type: "int", nullable: false),
            //        CreatedBy = table.Column<int>(type: "int", nullable: true),
            //        CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_MOMComments", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_MOMComments_Comments_CommentId",
            //            column: x => x.CommentId,
            //            principalSchema: "Committe",
            //            principalTable: "Comments",
            //            principalColumn: "CommentId",
            //            onDelete: ReferentialAction.Restrict);
            //        table.ForeignKey(
            //            name: "FK_MOMComments_MinutesOfMeetings_MinuteOfMeetingId",
            //            column: x => x.MinuteOfMeetingId,
            //            principalSchema: "Meeting",
            //            principalTable: "MinutesOfMeetings",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Restrict);
            //    });

            //migrationBuilder.CreateIndex(
            //    name: "IX_MOMComments_CommentId",
            //    schema: "Meeting",
            //    table: "MOMComments",
            //    column: "CommentId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_MOMComments_MinuteOfMeetingId",
            //    schema: "Meeting",
            //    table: "MOMComments",
            //    column: "MinuteOfMeetingId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MOMComments",
                schema: "Meeting");

            migrationBuilder.DropColumn(
                name: "Colsed",
                schema: "Meeting",
                table: "Meetings");

            //migrationBuilder.RenameTable(
            //    name: "CommiteeSavedAttachmentSavedAttachment",
            //    schema: "Committe",
            //    newName: "CommiteeSavedAttachmentSavedAttachment");
        }
    }
}
