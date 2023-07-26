using Microsoft.EntityFrameworkCore.Migrations;

namespace CommiteeAndMeetings.BLL.Migrations
{
    public partial class addtaskcatrelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.RenameTable(
            //    name: "CommiteeSavedAttachmentSavedAttachment",
            //    newName: "CommiteeSavedAttachmentSavedAttachment",
            //    newSchema: "Committe");

            migrationBuilder.AddColumn<int>(
                name: "CommiteePermissionCategoryId",
                schema: "Committe",
                table: "CommiteeTasks",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CommiteeTasks_ComiteeTaskCategoryId",
                schema: "Committe",
                table: "CommiteeTasks",
                column: "ComiteeTaskCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_CommiteeTasks_CommiteePermissionCategoryId",
                schema: "Committe",
                table: "CommiteeTasks",
                column: "CommiteePermissionCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_CommiteeTasks_ComiteeTaskCategory_ComiteeTaskCategoryId",
                schema: "Committe",
                table: "CommiteeTasks",
                column: "ComiteeTaskCategoryId",
                principalSchema: "Committe",
                principalTable: "ComiteeTaskCategory",
                principalColumn: "ComiteeTaskCategoryId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CommiteeTasks_CommiteePermissionCategories_CommiteePermissionCategoryId",
                schema: "Committe",
                table: "CommiteeTasks",
                column: "CommiteePermissionCategoryId",
                principalSchema: "Committe",
                principalTable: "CommiteePermissionCategories",
                principalColumn: "CommiteePermissionCategoryId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommiteeTasks_ComiteeTaskCategory_ComiteeTaskCategoryId",
                schema: "Committe",
                table: "CommiteeTasks");

            migrationBuilder.DropForeignKey(
                name: "FK_CommiteeTasks_CommiteePermissionCategories_CommiteePermissionCategoryId",
                schema: "Committe",
                table: "CommiteeTasks");

            migrationBuilder.DropIndex(
                name: "IX_CommiteeTasks_ComiteeTaskCategoryId",
                schema: "Committe",
                table: "CommiteeTasks");

            migrationBuilder.DropIndex(
                name: "IX_CommiteeTasks_CommiteePermissionCategoryId",
                schema: "Committe",
                table: "CommiteeTasks");

            migrationBuilder.DropColumn(
                name: "CommiteePermissionCategoryId",
                schema: "Committe",
                table: "CommiteeTasks");

            migrationBuilder.RenameTable(
                name: "CommiteeSavedAttachmentSavedAttachment",
                schema: "Committe",
                newName: "CommiteeSavedAttachmentSavedAttachment");
        }
    }
}
