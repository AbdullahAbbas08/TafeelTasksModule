using Microsoft.EntityFrameworkCore.Migrations;

namespace CommiteeAndMeetings.BLL.Migrations
{
    public partial class changetableschema : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "UpdateTaskLog",
                newName: "UpdateTaskLog",
                newSchema: "Committe");

            //migrationBuilder.RenameTable(
            //    name: "CommiteeSavedAttachmentSavedAttachment",
            //    newName: "CommiteeSavedAttachmentSavedAttachment",
            //    newSchema: "Committe");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "UpdateTaskLog",
                schema: "Committe",
                newName: "UpdateTaskLog");

            migrationBuilder.RenameTable(
                name: "CommiteeSavedAttachmentSavedAttachment",
                schema: "Committe",
                newName: "CommiteeSavedAttachmentSavedAttachment");
        }
    }
}
