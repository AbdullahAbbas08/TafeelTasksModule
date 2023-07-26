using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace CommiteeAndMeetings.BLL.Migrations
{
    public partial class addAttachmentToTask : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {


            migrationBuilder.CreateTable(
                name: "CommitteeTaskAttachment",
                schema: "Committe",
                columns: table => new
                {
                    CommitteeTaskAttachmentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CommiteeTaskId = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_CommitteeTaskAttachment", x => x.CommitteeTaskAttachmentId);
                    table.ForeignKey(
                        name: "FK_CommitteeTaskAttachment_CommiteeTasks_CommiteeTaskId",
                        column: x => x.CommiteeTaskId,
                        principalSchema: "Committe",
                        principalTable: "CommiteeTasks",
                        principalColumn: "CommiteeTaskId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CommitteeTaskAttachment_CommiteeUsersRoles_CreatedByRoleId",
                        column: x => x.CreatedByRoleId,
                        principalSchema: "Committe",
                        principalTable: "CommiteeUsersRoles",
                        principalColumn: "CommiteeUsersRoleId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CommitteeTaskAttachment_SavedAttachments_AttachmentId",
                        column: x => x.AttachmentId,
                        principalSchema: "Committe",
                        principalTable: "SavedAttachments",
                        principalColumn: "SavedAttachmentId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CommitteeTaskAttachment_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });


            migrationBuilder.CreateIndex(
                name: "IX_CommitteeTaskAttachment_AttachmentId",
                schema: "Committe",
                table: "CommitteeTaskAttachment",
                column: "AttachmentId");

            migrationBuilder.CreateIndex(
                name: "IX_CommitteeTaskAttachment_CommiteeTaskId",
                schema: "Committe",
                table: "CommitteeTaskAttachment",
                column: "CommiteeTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_CommitteeTaskAttachment_CreatedBy",
                schema: "Committe",
                table: "CommitteeTaskAttachment",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_CommitteeTaskAttachment_CreatedByRoleId",
                schema: "Committe",
                table: "CommitteeTaskAttachment",
                column: "CreatedByRoleId");


        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {


            migrationBuilder.DropTable(
                name: "CommitteeTaskAttachment",
                schema: "Committe");


        }
    }
}
