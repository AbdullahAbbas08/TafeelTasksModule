using Microsoft.EntityFrameworkCore.Migrations;

namespace CommiteeAndMeetings.BLL.Migrations
{
    public partial class AlterCommitteeNotificationTableAddTextARANdEn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            //migrationBuilder.RenameColumn(
            //    name: "Text",
            //    schema: "Committe",
            //    table: "CommitteeNotifications",
            //    newName: "TextEn");

            //migrationBuilder.AddColumn<string>(
            //    name: "TextAR",
            //    schema: "Committe",
            //    table: "CommitteeNotifications",
            //    type: "nvarchar(max)",
            //    nullable: true);

            //migrationBuilder.CreateIndex(
            //    name: "IX_CommitteeNotifications_UserId",
            //    schema: "Committe",
            //    table: "CommitteeNotifications",
            //    column: "UserId");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_CommitteeNotifications_Users_UserId",
            //    schema: "Committe",
            //    table: "CommitteeNotifications",
            //    column: "UserId",
            //    principalTable: "Users",
            //    principalColumn: "UserId",
            //    onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommitteeNotifications_Users_UserId",
                schema: "Committe",
                table: "CommitteeNotifications");

            migrationBuilder.DropIndex(
                name: "IX_CommitteeNotifications_UserId",
                schema: "Committe",
                table: "CommitteeNotifications");

            migrationBuilder.DropColumn(
                name: "CreatedUserName",
                table: "Vw_TransactionBoxes_Inbox");

            migrationBuilder.DropColumn(
                name: "CreatedUserName",
                table: "ViewTransactionsView");

            migrationBuilder.DropColumn(
                name: "TextAR",
                schema: "Committe",
                table: "CommitteeNotifications");

            //migrationBuilder.RenameTable(
            //    name: "CommiteeSavedAttachmentSavedAttachment",
            //    schema: "Committe",
            //    newName: "CommiteeSavedAttachmentSavedAttachment");

            migrationBuilder.RenameColumn(
                name: "TextEn",
                schema: "Committe",
                table: "CommitteeNotifications",
                newName: "Text");

            migrationBuilder.AddForeignKey(
                name: "FK_Signatures_Users_CreatedBy",
                table: "Signatures",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
