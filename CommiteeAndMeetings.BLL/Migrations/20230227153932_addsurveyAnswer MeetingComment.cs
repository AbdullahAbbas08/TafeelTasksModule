using Microsoft.EntityFrameworkCore.Migrations;

namespace CommiteeAndMeetings.BLL.Migrations
{
    public partial class addsurveyAnswerMeetingComment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            

            migrationBuilder.AddColumn<int>(
                name: "MeetingCommentId",
                schema: "Committe",
                table: "SurveyAnswers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SurveyAnswers_MeetingCommentId",
                schema: "Committe",
                table: "SurveyAnswers",
                column: "MeetingCommentId");

            migrationBuilder.AddForeignKey(
                name: "FK_SurveyAnswers_MeetingComment_MeetingCommentId",
                schema: "Committe",
                table: "SurveyAnswers",
                column: "MeetingCommentId",
                principalSchema: "Meeting",
                principalTable: "MeetingComment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SurveyAnswers_MeetingComment_MeetingCommentId",
                schema: "Committe",
                table: "SurveyAnswers");

            migrationBuilder.DropIndex(
                name: "IX_SurveyAnswers_MeetingCommentId",
                schema: "Committe",
                table: "SurveyAnswers");

            migrationBuilder.DropColumn(
                name: "MeetingCommentId",
                schema: "Committe",
                table: "SurveyAnswers");

            
        }
    }
}
