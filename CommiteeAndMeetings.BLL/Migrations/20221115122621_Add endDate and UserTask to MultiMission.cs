using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CommiteeAndMeetings.BLL.Migrations
{
    public partial class AddendDateandUserTasktoMultiMission : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           

            migrationBuilder.AddColumn<int>(
                name: "CommiteeTaskMultiMissionId",
                schema: "Committe",
                table: "UserTasks",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "EndDateMultiMission",
                schema: "Committe",
                table: "CommiteeTaskMultiMission",
                type: "datetimeoffset",
                nullable: true);

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

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "EndDateMultiMission",
                schema: "Committe",
                table: "CommiteeTaskMultiMission");

           
        }
    }
}
