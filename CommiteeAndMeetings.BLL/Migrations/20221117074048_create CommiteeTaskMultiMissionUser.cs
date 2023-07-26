using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CommiteeAndMeetings.BLL.Migrations
{
    public partial class createCommiteeTaskMultiMissionUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserTasks_CommiteeTaskMultiMission_CommiteeTaskMultiMissionId",
                schema: "Committe",
                table: "UserTasks");

            migrationBuilder.DropIndex(
                name: "IX_UserTasks_CommiteeTaskMultiMissionId",
                schema: "Committe",
                table: "UserTasks");

            migrationBuilder.DropColumn(
                name: "CommiteeTaskMultiMissionId",
                schema: "Committe",
                table: "UserTasks");

           

            migrationBuilder.CreateTable(
                name: "CommiteeTaskMultiMissionUsers",
                schema: "Committe",
                columns: table => new
                {
                    CommiteeTaskMultiMissionUserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CommiteeTaskMultiMissionId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_CommiteeTaskMultiMissionUsers", x => x.CommiteeTaskMultiMissionUserId);
                    table.ForeignKey(
                        name: "FK_CommiteeTaskMultiMissionUsers_CommiteeTaskMultiMission_CommiteeTaskMultiMissionId",
                        column: x => x.CommiteeTaskMultiMissionId,
                        principalSchema: "Committe",
                        principalTable: "CommiteeTaskMultiMission",
                        principalColumn: "CommiteeTaskMultiMissionId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CommiteeTaskMultiMissionUsers_CommiteeUsersRoles_CreatedByRoleId",
                        column: x => x.CreatedByRoleId,
                        principalSchema: "Committe",
                        principalTable: "CommiteeUsersRoles",
                        principalColumn: "CommiteeUsersRoleId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CommiteeTaskMultiMissionUsers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CommiteeTaskMultiMissionUsers_CommiteeTaskMultiMissionId",
                schema: "Committe",
                table: "CommiteeTaskMultiMissionUsers",
                column: "CommiteeTaskMultiMissionId");

            migrationBuilder.CreateIndex(
                name: "IX_CommiteeTaskMultiMissionUsers_CreatedByRoleId",
                schema: "Committe",
                table: "CommiteeTaskMultiMissionUsers",
                column: "CreatedByRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_CommiteeTaskMultiMissionUsers_UserId",
                schema: "Committe",
                table: "CommiteeTaskMultiMissionUsers",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CommiteeTaskMultiMissionUsers",
                schema: "Committe");

            

            migrationBuilder.AddColumn<int>(
                name: "CommiteeTaskMultiMissionId",
                schema: "Committe",
                table: "UserTasks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_UserTasks_CommiteeTaskMultiMissionId",
                schema: "Committe",
                table: "UserTasks",
                column: "CommiteeTaskMultiMissionId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserTasks_CommiteeTaskMultiMission_CommiteeTaskMultiMissionId",
                schema: "Committe",
                table: "UserTasks",
                column: "CommiteeTaskMultiMissionId",
                principalSchema: "Committe",
                principalTable: "CommiteeTaskMultiMission",
                principalColumn: "CommiteeTaskMultiMissionId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
