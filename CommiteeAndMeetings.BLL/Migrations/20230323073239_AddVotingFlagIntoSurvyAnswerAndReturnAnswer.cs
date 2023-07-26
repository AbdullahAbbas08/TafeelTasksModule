using Microsoft.EntityFrameworkCore.Migrations;

namespace CommiteeAndMeetings.BLL.Migrations
{
    public partial class AddVotingFlagIntoSurvyAnswerAndReturnAnswer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           

            migrationBuilder.AlterColumn<string>(
                name: "Answer",
                schema: "Committe",
                table: "SurveyAnswers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "VotingFlag",
                schema: "Committe",
                table: "SurveyAnswers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VotingFlag",
                schema: "Committe",
                table: "SurveyAnswers");

           

            migrationBuilder.AlterColumn<int>(
                name: "Answer",
                schema: "Committe",
                table: "SurveyAnswers",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
