using Microsoft.EntityFrameworkCore.Migrations;

namespace CommiteeAndMeetings.BLL.Migrations
{
    public partial class AddRelationforCommitteAndMeetings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {


            //migrationBuilder.AddColumn<int>(
            //    name: "CommiteeId",
            //    schema: "Meeting",
            //    table: "Meetings",
            //    type: "int",
            //    nullable: true);

            //migrationBuilder.AddColumn<int>(
            //    name: "CommitteId",
            //    schema: "Meeting",
            //    table: "Meetings",
            //    type: "int",
            //    nullable: true);

            //migrationBuilder.CreateIndex(
            //    name: "IX_Meetings_CommiteeId",
            //    schema: "Meeting",
            //    table: "Meetings",
            //    column: "CommiteeId");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Meetings_Commitees_CommiteeId",
            //    schema: "Meeting",
            //    table: "Meetings",
            //    column: "CommiteeId",
            //    principalSchema: "Committe",
            //    principalTable: "Commitees",
            //    principalColumn: "CommiteeId",
            //    onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Meetings_Commitees_CommiteeId",
                schema: "Meeting",
                table: "Meetings");

            migrationBuilder.DropIndex(
                name: "IX_Meetings_CommiteeId",
                schema: "Meeting",
                table: "Meetings");

            migrationBuilder.DropColumn(
                name: "CommiteeId",
                schema: "Meeting",
                table: "Meetings");

            migrationBuilder.DropColumn(
                name: "CommitteId",
                schema: "Meeting",
                table: "Meetings");


        }
    }
}
