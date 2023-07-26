using Microsoft.EntityFrameworkCore.Migrations;

namespace CommiteeAndMeetings.BLL.Migrations
{
    public partial class categoryWithnamesaranden : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.RenameTable(
            //    name: "CommiteeSavedAttachmentSavedAttachment",
            //    newName: "CommiteeSavedAttachmentSavedAttachment",
            //    newSchema: "Committe");

            migrationBuilder.RenameColumn(
                name: "categoryName",
                schema: "Committe",
                table: "ComiteeTaskCategory",
                newName: "categoryNameEn");

            migrationBuilder.AddColumn<string>(
                name: "categoryNameAr",
                schema: "Committe",
                table: "ComiteeTaskCategory",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "categoryNameAr",
                schema: "Committe",
                table: "ComiteeTaskCategory");

            migrationBuilder.RenameTable(
                name: "CommiteeSavedAttachmentSavedAttachment",
                schema: "Committe",
                newName: "CommiteeSavedAttachmentSavedAttachment");

            migrationBuilder.RenameColumn(
                name: "categoryNameEn",
                schema: "Committe",
                table: "ComiteeTaskCategory",
                newName: "categoryName");
        }
    }
}
