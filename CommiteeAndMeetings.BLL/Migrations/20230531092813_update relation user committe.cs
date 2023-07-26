using Microsoft.EntityFrameworkCore.Migrations;

namespace CommiteeAndMeetings.BLL.Migrations
{
    public partial class updaterelationusercommitte : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Commitees_CommitteeSecretaryId",
                schema: "Committe",
                table: "Commitees");

            //migrationBuilder.DropIndex(
            //    name: "IX_Commitees_CurrenHeadUnitId",
            //    schema: "Committe",
            //    table: "Commitees");

           

            migrationBuilder.CreateIndex(
                name: "IX_Commitees_CommitteeSecretaryId",
                schema: "Committe",
                table: "Commitees",
                column: "CommitteeSecretaryId");

            migrationBuilder.CreateIndex(
                name: "IX_Commitees_CurrenHeadUnitId",
                schema: "Committe",
                table: "Commitees",
                column: "CurrenHeadUnitId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Commitees_CommitteeSecretaryId",
                schema: "Committe",
                table: "Commitees");

            //migrationBuilder.DropIndex(
            //    name: "IX_Commitees_CurrenHeadUnitId",
            //    schema: "Committe",
            //    table: "Commitees");

           
            migrationBuilder.CreateIndex(
                name: "IX_Commitees_CommitteeSecretaryId",
                schema: "Committe",
                table: "Commitees",
                column: "CommitteeSecretaryId",
                unique: true,
                filter: "[CommitteeSecretaryId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Commitees_CurrenHeadUnitId",
                schema: "Committe",
                table: "Commitees",
                column: "CurrenHeadUnitId",
                unique: true,
                filter: "[CurrenHeadUnitId] IS NOT NULL");
        }
    }
}
