using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace CommiteeAndMeetings.BLL.Migrations
{
    public partial class updatetasklog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.RenameTable(
            //    name: "CommiteeSavedAttachmentSavedAttachment",
            //    newName: "CommiteeSavedAttachmentSavedAttachment",
            //    newSchema: "Committe");

            migrationBuilder.CreateTable(
                name: "UpdateTaskLog",
                columns: table => new
                {
                    UpdateTaskLogId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CancelDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DeletedBy = table.Column<int>(type: "int", nullable: true),
                    DeletedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    CreatedByRoleId = table.Column<int>(type: "int", nullable: true),
                    Discriminator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AssistantUserId = table.Column<int>(type: "int", nullable: true),
                    MainAssignedUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UpdateTaskLog", x => x.UpdateTaskLogId);
                    table.ForeignKey(
                        name: "FK_UpdateTaskLog_CommiteeUsersRoles_CreatedByRoleId",
                        column: x => x.CreatedByRoleId,
                        principalSchema: "Committe",
                        principalTable: "CommiteeUsersRoles",
                        principalColumn: "CommiteeUsersRoleId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UpdateTaskLog_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UpdateTaskLog_CreatedBy",
                table: "UpdateTaskLog",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_UpdateTaskLog_CreatedByRoleId",
                table: "UpdateTaskLog",
                column: "CreatedByRoleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UpdateTaskLog");

            migrationBuilder.RenameTable(
                name: "CommiteeSavedAttachmentSavedAttachment",
                schema: "Committe",
                newName: "CommiteeSavedAttachmentSavedAttachment");
        }
    }
}
