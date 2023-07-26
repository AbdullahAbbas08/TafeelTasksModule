using Microsoft.EntityFrameworkCore.Migrations;

namespace CommiteeAndMeetings.BLL.Migrations
{
    public partial class addnewnametolog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.RenameTable(
            //    name: "CommiteeSavedAttachmentSavedAttachment",
            //    newName: "CommiteeSavedAttachmentSavedAttachment",
            //    newSchema: "Committe");

            migrationBuilder.AddColumn<string>(
                name: "NewFullNameAr",
                schema: "Committe",
                table: "UpdateTaskLog",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NewFullNameEn",
                schema: "Committe",
                table: "UpdateTaskLog",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NewMainAssignedUserId",
                schema: "Committe",
                table: "UpdateTaskLog",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NewFullNameAr",
                schema: "Committe",
                table: "UpdateTaskLog");

            migrationBuilder.DropColumn(
                name: "NewFullNameEn",
                schema: "Committe",
                table: "UpdateTaskLog");

            migrationBuilder.DropColumn(
                name: "NewMainAssignedUserId",
                schema: "Committe",
                table: "UpdateTaskLog");

            migrationBuilder.RenameTable(
                name: "CommiteeSavedAttachmentSavedAttachment",
                schema: "Committe",
                newName: "CommiteeSavedAttachmentSavedAttachment");
        }
    }
}
