using Microsoft.EntityFrameworkCore.Migrations;

namespace CommiteeAndMeetings.BLL.Migrations
{
    public partial class AddIsCreatorToMeetingCoordinator : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.AddColumn<bool>(
            //    name: "IsCreator",
            //    schema: "Meeting",
            //    table: "MeetingCoordinators",
            //    type: "bit",
            //    nullable: false,
            //    defaultValue: false);

            //migrationBuilder.CreateIndex(
            //    name: "IX_Meetings_CreatedBy",
            //    schema: "Meeting",
            //    table: "Meetings",
            //    column: "CreatedBy");

            //migrationBuilder.CreateIndex(
            //    name: "IX_MeetingComment_CreatedBy",
            //    schema: "Meeting",
            //    table: "MeetingComment",
            //    column: "CreatedBy");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_MeetingComment_Users_CreatedBy",
            //    schema: "Meeting",
            //    table: "MeetingComment",
            //    column: "CreatedBy",
            //    principalTable: "Users",
            //    principalColumn: "UserId",
            //    onDelete: ReferentialAction.Restrict);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Meetings_Users_CreatedBy",
            //    schema: "Meeting",
            //    table: "Meetings",
            //    column: "CreatedBy",
            //    principalTable: "Users",
            //    principalColumn: "UserId",
            //    onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MeetingComment_Users_CreatedBy",
                schema: "Meeting",
                table: "MeetingComment");

            migrationBuilder.DropForeignKey(
                name: "FK_Meetings_Users_CreatedBy",
                schema: "Meeting",
                table: "Meetings");

            migrationBuilder.DropIndex(
                name: "IX_Meetings_CreatedBy",
                schema: "Meeting",
                table: "Meetings");

            migrationBuilder.DropIndex(
                name: "IX_MeetingComment_CreatedBy",
                schema: "Meeting",
                table: "MeetingComment");

            migrationBuilder.DropColumn(
                name: "IsCreator",
                schema: "Meeting",
                table: "MeetingCoordinators");


        }
    }
}
