using Microsoft.EntityFrameworkCore.Migrations;

namespace CommiteeAndMeetings.BLL.Migrations
{
    public partial class AlterHeaderAndFooterTableAddTitleAREn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            //migrationBuilder.AddColumn<string>(
            //    name: "TitleAR",
            //    schema: "Meeting",
            //    table: "MeetingHeaderAndFooters",
            //    type: "nvarchar(max)",
            //    nullable: true);

            //migrationBuilder.AddColumn<string>(
            //    name: "TitleEn",
            //    schema: "Meeting",
            //    table: "MeetingHeaderAndFooters",
            //    type: "nvarchar(max)",
            //    nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TitleAR",
                schema: "Meeting",
                table: "MeetingHeaderAndFooters");

            migrationBuilder.DropColumn(
                name: "TitleEn",
                schema: "Meeting",
                table: "MeetingHeaderAndFooters");

            migrationBuilder.RenameTable(
                name: "CommiteeSavedAttachmentSavedAttachment",
                schema: "Committe",
                newName: "CommiteeSavedAttachmentSavedAttachment");
        }
    }
}
