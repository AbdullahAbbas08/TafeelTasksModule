using Microsoft.EntityFrameworkCore.Migrations;

namespace CommiteeAndMeetings.BLL.Migrations
{
    public partial class AlterCommitteeNotificationTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.RenameTable(
            //    name: "CommiteeSavedAttachmentSavedAttachment",
            //    newName: "CommiteeSavedAttachmentSavedAttachment",
            //    newSchema: "Committe");

            //migrationBuilder.AddColumn<int>(
            //    name: "CommiteeId",
            //    schema: "Committe",
            //    table: "CommitteeNotifications",
            //    type: "int",
            //    nullable: true);

            //migrationBuilder.CreateIndex(
            //    name: "IX_CommitteeNotifications_CommiteeId",
            //    schema: "Committe",
            //    table: "CommitteeNotifications",
            //    column: "CommiteeId");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_CommitteeNotifications_Commitees_CommiteeId",
            //    schema: "Committe",
            //    table: "CommitteeNotifications",
            //    column: "CommiteeId",
            //    principalSchema: "Committe",
            //    principalTable: "Commitees",
            //    principalColumn: "CommiteeId",
            //    onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommitteeNotifications_Commitees_CommiteeId",
                schema: "Committe",
                table: "CommitteeNotifications");

            migrationBuilder.DropIndex(
                name: "IX_CommitteeNotifications_CommiteeId",
                schema: "Committe",
                table: "CommitteeNotifications");

            migrationBuilder.DropColumn(
                name: "CommiteeId",
                schema: "Committe",
                table: "CommitteeNotifications");

            migrationBuilder.RenameTable(
                name: "CommiteeSavedAttachmentSavedAttachment",
                schema: "Committe",
                newName: "CommiteeSavedAttachmentSavedAttachment");
        }
    }
}
