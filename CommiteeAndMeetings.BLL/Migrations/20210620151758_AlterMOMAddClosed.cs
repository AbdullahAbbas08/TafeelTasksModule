using Microsoft.EntityFrameworkCore.Migrations;

namespace CommiteeAndMeetings.BLL.Migrations
{
    public partial class AlterMOMAddClosed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.RenameTable(
            //    name: "CommiteeSavedAttachmentSavedAttachment",
            //    newName: "CommiteeSavedAttachmentSavedAttachment",
            //    newSchema: "Committe");

            //migrationBuilder.AddColumn<bool>(
            //    name: "Colsed",
            //    schema: "Meeting",
            //    table: "MinutesOfMeetings",
            //    type: "bit",
            //    nullable: false,
            //    defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Colsed",
                schema: "Meeting",
                table: "MinutesOfMeetings");

            //migrationBuilder.RenameTable(
            //    name: "CommiteeSavedAttachmentSavedAttachment",
            //    schema: "Committe",
            //    newName: "CommiteeSavedAttachmentSavedAttachment");
        }
    }
}
