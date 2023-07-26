using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CommiteeAndMeetings.BLL.Migrations
{
    public partial class createtaskgrouptable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           

            migrationBuilder.CreateTable(
                name: "TaskGroups",
                columns: table => new
                {
                    TaskGroupId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GroupId = table.Column<int>(type: "int", nullable: false),
                    TaskId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    CreatedByRoleId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskGroups", x => x.TaskGroupId);
                    table.ForeignKey(
                        name: "FK_TaskGroups_CommiteeTasks_TaskId",
                        column: x => x.TaskId,
                        principalSchema: "Committe",
                        principalTable: "CommiteeTasks",
                        principalColumn: "CommiteeTaskId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TaskGroups_CommiteeUsersRoles_CreatedByRoleId",
                        column: x => x.CreatedByRoleId,
                        principalSchema: "Committe",
                        principalTable: "CommiteeUsersRoles",
                        principalColumn: "CommiteeUsersRoleId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TaskGroups_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "GroupId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TaskGroups_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TaskGroups_CreatedBy",
                table: "TaskGroups",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_TaskGroups_CreatedByRoleId",
                table: "TaskGroups",
                column: "CreatedByRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskGroups_GroupId",
                table: "TaskGroups",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskGroups_TaskId",
                table: "TaskGroups",
                column: "TaskId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TaskGroups");

            
        }
    }
}
