using Microsoft.EntityFrameworkCore.Migrations;

namespace CommiteeAndMeetings.BLL.Migrations
{
    public partial class removeVotingFlagIntoSurvyAnswerAndReturnAnswer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VotingFlag",
                schema: "Committe",
                table: "SurveyAnswers");

           
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
           

            migrationBuilder.AddColumn<int>(
                name: "VotingFlag",
                schema: "Committe",
                table: "SurveyAnswers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
