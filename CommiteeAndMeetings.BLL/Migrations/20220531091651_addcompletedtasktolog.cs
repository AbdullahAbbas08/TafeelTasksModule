using Microsoft.EntityFrameworkCore.Migrations;

namespace CommiteeAndMeetings.BLL.Migrations
{
    public partial class addcompletedtasktolog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropPrimaryKey(
            //    name: "PK_Theme",
            //    schema: "Committe",
            //    table: "Theme");

            //migrationBuilder.EnsureSchema(
            //    name: "Reports");

            //migrationBuilder.RenameTable(
            //    name: "CommiteeSavedAttachmentSavedAttachment",
            //    newName: "CommiteeSavedAttachmentSavedAttachment",
            //    newSchema: "Committe");

            //migrationBuilder.RenameTable(
            //    name: "Theme",
            //    schema: "Committe",
            //    newName: "Themes",
            //    newSchema: "Reports");

            migrationBuilder.AddColumn<bool>(
                name: "Completed",
                schema: "Committe",
                table: "UpdateTaskLog",
                type: "bit",
                nullable: true);

            //migrationBuilder.AddPrimaryKey(
            //    name: "PK_Themes",
            //    schema: "Reports",
            //    table: "Themes",
            //    column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropPrimaryKey(
            //    name: "PK_Themes",
            //    schema: "Reports",
            //    table: "Themes");

            migrationBuilder.DropColumn(
                name: "Completed",
                schema: "Committe",
                table: "UpdateTaskLog");

            //migrationBuilder.RenameTable(
            //    name: "CommiteeSavedAttachmentSavedAttachment",
            //    schema: "Committe",
            //    newName: "CommiteeSavedAttachmentSavedAttachment");

            //migrationBuilder.RenameTable(
            //    name: "Themes",
            //    schema: "Reports",
            //    newName: "Theme",
            //    newSchema: "Committe");

            //migrationBuilder.AddPrimaryKey(
            //    name: "PK_Theme",
            //    schema: "Committe",
            //    table: "Theme",
            //    column: "Id");
        }
    }
}
