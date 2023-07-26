using Microsoft.EntityFrameworkCore.Migrations;

namespace CommiteeAndMeetings.BLL.Migrations
{
    public partial class addcommentidinsaveattachment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommentAttachmentInTask_CommiteeTasks_commiteeTaskId",
                schema: "Committe",
                table: "CommentAttachmentInTask");

            migrationBuilder.DropIndex(
                name: "IX_CommentAttachmentInTask_commiteeTaskId",
                schema: "Committe",
                table: "CommentAttachmentInTask");

            migrationBuilder.DropColumn(
                name: "commiteeTaskId",
                schema: "Committe",
                table: "CommentAttachmentInTask");

            

            migrationBuilder.AddColumn<int>(
                name: "CommentId",
                schema: "Committe",
                table: "SavedAttachments",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SavedAttachments_CommentId",
                schema: "Committe",
                table: "SavedAttachments",
                column: "CommentId");

            migrationBuilder.AddForeignKey(
                name: "FK_SavedAttachments_Comments_CommentId",
                schema: "Committe",
                table: "SavedAttachments",
                column: "CommentId",
                principalSchema: "Committe",
                principalTable: "Comments",
                principalColumn: "CommentId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SavedAttachments_Comments_CommentId",
                schema: "Committe",
                table: "SavedAttachments");

            migrationBuilder.DropIndex(
                name: "IX_SavedAttachments_CommentId",
                schema: "Committe",
                table: "SavedAttachments");

            migrationBuilder.DropColumn(
                name: "CommentId",
                schema: "Committe",
                table: "SavedAttachments");

           

            migrationBuilder.AddColumn<int>(
                name: "commiteeTaskId",
                schema: "Committe",
                table: "CommentAttachmentInTask",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CommentAttachmentInTask_commiteeTaskId",
                schema: "Committe",
                table: "CommentAttachmentInTask",
                column: "commiteeTaskId");

            migrationBuilder.AddForeignKey(
                name: "FK_CommentAttachmentInTask_CommiteeTasks_commiteeTaskId",
                schema: "Committe",
                table: "CommentAttachmentInTask",
                column: "commiteeTaskId",
                principalSchema: "Committe",
                principalTable: "CommiteeTasks",
                principalColumn: "CommiteeTaskId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
