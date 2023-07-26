using Microsoft.EntityFrameworkCore.Migrations;

namespace CommiteeAndMeetings.BLL.Migrations
{
    public partial class addcommentattachmentToTask : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           

            migrationBuilder.AddColumn<int>(
                name: "commiteeTaskId",
                schema: "Committe",
                table: "CommentAttachmentInTask",
                type: "int",
                nullable: false,
                defaultValue: 0);

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

        protected override void Down(MigrationBuilder migrationBuilder)
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

           
        }
    }
}
