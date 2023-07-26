using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CommiteeAndMeetings.BLL.Migrations
{
    public partial class CreateCommentAttachmentInTask : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           

            migrationBuilder.CreateTable(
                name: "CommentAttachmentInTask",
                schema: "Committe",
                columns: table => new
                {
                    CommentAttachmentInTaskId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CommentId = table.Column<int>(type: "int", nullable: false),
                    AttachmentId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DeletedBy = table.Column<int>(type: "int", nullable: true),
                    DeletedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    CreatedByRoleId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommentAttachmentInTask", x => x.CommentAttachmentInTaskId);
                    table.ForeignKey(
                        name: "FK_CommentAttachmentInTask_Comments_CommentId",
                        column: x => x.CommentId,
                        principalSchema: "Committe",
                        principalTable: "Comments",
                        principalColumn: "CommentId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CommentAttachmentInTask_CommiteeUsersRoles_CreatedByRoleId",
                        column: x => x.CreatedByRoleId,
                        principalSchema: "Committe",
                        principalTable: "CommiteeUsersRoles",
                        principalColumn: "CommiteeUsersRoleId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CommentAttachmentInTask_SavedAttachments_AttachmentId",
                        column: x => x.AttachmentId,
                        principalSchema: "Committe",
                        principalTable: "SavedAttachments",
                        principalColumn: "SavedAttachmentId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CommentAttachmentInTask_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CommentAttachmentInTask_AttachmentId",
                schema: "Committe",
                table: "CommentAttachmentInTask",
                column: "AttachmentId");

            migrationBuilder.CreateIndex(
                name: "IX_CommentAttachmentInTask_CommentId",
                schema: "Committe",
                table: "CommentAttachmentInTask",
                column: "CommentId");

            migrationBuilder.CreateIndex(
                name: "IX_CommentAttachmentInTask_CreatedBy",
                schema: "Committe",
                table: "CommentAttachmentInTask",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_CommentAttachmentInTask_CreatedByRoleId",
                schema: "Committe",
                table: "CommentAttachmentInTask",
                column: "CreatedByRoleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CommentAttachmentInTask",
                schema: "Committe");

           
        }
    }
}
