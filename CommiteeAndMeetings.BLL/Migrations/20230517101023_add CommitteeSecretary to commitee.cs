using Microsoft.EntityFrameworkCore.Migrations;

namespace CommiteeAndMeetings.BLL.Migrations
{
    public partial class addCommitteeSecretarytocommitee : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            

            

            migrationBuilder.AddColumn<int>(
                name: "CommitteeSecretaryId",
                schema: "Committe",
                table: "Commitees",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Commitees_CommitteeSecretaryId",
                schema: "Committe",
                table: "Commitees",
                column: "CommitteeSecretaryId",
                unique: true,
                filter: "[CommitteeSecretaryId] IS NOT NULL");

            

            migrationBuilder.AddForeignKey(
                name: "FK_Commitees_Users_CommitteeSecretaryId",
                schema: "Committe",
                table: "Commitees",
                column: "CommitteeSecretaryId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Commitees_Users_CommitteeSecretaryId",
                schema: "Committe",
                table: "Commitees");

            migrationBuilder.DropIndex(
                name: "IX_Commitees_CommitteeSecretaryId",
                schema: "Committe",
                table: "Commitees");

           

            migrationBuilder.DropColumn(
                name: "CommitteeSecretaryId",
                schema: "Committe",
                table: "Commitees");

           
        }
    }
}
