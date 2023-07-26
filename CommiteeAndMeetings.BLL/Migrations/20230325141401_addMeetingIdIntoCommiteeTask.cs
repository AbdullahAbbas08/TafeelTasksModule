using Microsoft.EntityFrameworkCore.Migrations;

namespace CommiteeAndMeetings.BLL.Migrations
{
    public partial class addMeetingIdIntoCommiteeTask : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           

            migrationBuilder.AddColumn<int>(
                name: "MeetingId",
                schema: "Committe",
                table: "CommiteeTasks",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CommiteeTasks_MeetingId",
                schema: "Committe",
                table: "CommiteeTasks",
                column: "MeetingId");

            migrationBuilder.AddForeignKey(
                name: "FK_CommiteeTasks_Meetings_MeetingId",
                schema: "Committe",
                table: "CommiteeTasks",
                column: "MeetingId",
                principalSchema: "Meeting",
                principalTable: "Meetings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommiteeTasks_Meetings_MeetingId",
                schema: "Committe",
                table: "CommiteeTasks");

            migrationBuilder.DropIndex(
                name: "IX_CommiteeTasks_MeetingId",
                schema: "Committe",
                table: "CommiteeTasks");

            migrationBuilder.DropColumn(
                name: "MeetingId",
                schema: "Committe",
                table: "CommiteeTasks");

           
        }
    }
}
