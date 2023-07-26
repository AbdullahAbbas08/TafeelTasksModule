using Microsoft.EntityFrameworkCore.Migrations;

namespace CommiteeAndMeetings.BLL.Migrations
{
    public partial class solvebugs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_CommiteeTaskEscalation",
                schema: "Committe",
                table: "CommiteeTaskEscalation");

            //migrationBuilder.DropIndex(
            //    name: "IX_CommiteeTaskEscalation_CommiteeTaskEscalationIndex",
            //    schema: "Committe",
            //    table: "CommiteeTaskEscalation");

            //migrationBuilder.RenameTable(
            //    name: "CommiteeSavedAttachmentSavedAttachment",
            //    newName: "CommiteeSavedAttachmentSavedAttachment",
            //    newSchema: "Committe");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CommiteeTaskEscalation",
                schema: "Committe",
                table: "CommiteeTaskEscalation",
                column: "CommiteeTaskEscalationIndex");

            migrationBuilder.CreateIndex(
                name: "IX_CommiteeTaskEscalation_ComiteeTaskCategoryId_MainAssinedUserId_NewMainAssinedUserId_DelayPeriod",
                schema: "Committe",
                table: "CommiteeTaskEscalation",
                columns: new[] { "ComiteeTaskCategoryId", "MainAssinedUserId", "NewMainAssinedUserId", "DelayPeriod" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_CommiteeTaskEscalation",
                schema: "Committe",
                table: "CommiteeTaskEscalation");

            migrationBuilder.DropIndex(
                name: "IX_CommiteeTaskEscalation_ComiteeTaskCategoryId_MainAssinedUserId_NewMainAssinedUserId_DelayPeriod",
                schema: "Committe",
                table: "CommiteeTaskEscalation");

            //migrationBuilder.RenameTable(
            //    name: "CommiteeSavedAttachmentSavedAttachment",
            //    schema: "Committe",
            //    newName: "CommiteeSavedAttachmentSavedAttachment");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CommiteeTaskEscalation",
                schema: "Committe",
                table: "CommiteeTaskEscalation",
                columns: new[] { "ComiteeTaskCategoryId", "MainAssinedUserId", "NewMainAssinedUserId", "DelayPeriod" });

            migrationBuilder.CreateIndex(
                name: "IX_CommiteeTaskEscalation_CommiteeTaskEscalationIndex",
                schema: "Committe",
                table: "CommiteeTaskEscalation",
                column: "CommiteeTaskEscalationIndex",
                unique: true);
        }
    }
}
