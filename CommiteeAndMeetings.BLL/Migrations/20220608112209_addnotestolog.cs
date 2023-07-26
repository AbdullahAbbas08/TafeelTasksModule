using Microsoft.EntityFrameworkCore.Migrations;

namespace CommiteeAndMeetings.BLL.Migrations
{
    public partial class addnotestolog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.RenameTable(
            //    name: "CommiteeSavedAttachmentSavedAttachment",
            //    newName: "CommiteeSavedAttachmentSavedAttachment",
            //    newSchema: "Committe");

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                schema: "Committe",
                table: "UpdateTaskLog",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CommiteeTaskEscalation_ComiteeTaskCategory_ComiteeTaskCategoryId",
                schema: "Committe",
                table: "CommiteeTaskEscalation",
                column: "ComiteeTaskCategoryId",
                principalSchema: "Committe",
                principalTable: "ComiteeTaskCategory",
                principalColumn: "ComiteeTaskCategoryId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommiteeTaskEscalation_ComiteeTaskCategory_ComiteeTaskCategoryId",
                schema: "Committe",
                table: "CommiteeTaskEscalation");

            migrationBuilder.DropColumn(
                name: "Notes",
                schema: "Committe",
                table: "UpdateTaskLog");

            //migrationBuilder.RenameTable(
            //    name: "CommiteeSavedAttachmentSavedAttachment",
            //    schema: "Committe",
            //    newName: "CommiteeSavedAttachmentSavedAttachment");
        }
    }
}
