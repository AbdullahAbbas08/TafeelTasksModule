using Microsoft.EntityFrameworkCore.Migrations;

namespace CommiteeAndMeetings.BLL.Migrations
{
    public partial class addnotificationsemailscommittetask : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.RenameTable(
            //    name: "CommiteeSavedAttachmentSavedAttachment",
            //    newName: "CommiteeSavedAttachmentSavedAttachment",
            //    newSchema: "Committe");

            migrationBuilder.AddColumn<bool>(
                name: "IsEmail",
                schema: "Committe",
                table: "CommiteeTasks",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsNotification",
                schema: "Committe",
                table: "CommiteeTasks",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSMS",
                schema: "Committe",
                table: "CommiteeTasks",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsEmail",
                schema: "Committe",
                table: "CommiteeTasks");

            migrationBuilder.DropColumn(
                name: "IsNotification",
                schema: "Committe",
                table: "CommiteeTasks");

            migrationBuilder.DropColumn(
                name: "IsSMS",
                schema: "Committe",
                table: "CommiteeTasks");

            //migrationBuilder.RenameTable(
            //    name: "CommiteeSavedAttachmentSavedAttachment",
            //    schema: "Committe",
            //    newName: "CommiteeSavedAttachmentSavedAttachment");
        }
    }
}
