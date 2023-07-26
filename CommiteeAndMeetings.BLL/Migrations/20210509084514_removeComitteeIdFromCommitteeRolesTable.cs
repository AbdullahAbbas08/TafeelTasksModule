using Microsoft.EntityFrameworkCore.Migrations;

namespace CommiteeAndMeetings.BLL.Migrations
{
    public partial class removeComitteeIdFromCommitteeRolesTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_CommiteeRoles_Commitees_CommiteeId",
            //    schema: "Committe",
            //    table: "CommiteeRoles");

            //migrationBuilder.DropIndex(
            //    name: "IX_CommiteeRoles_CommiteeId",
            //    schema: "Committe",
            //    table: "CommiteeRoles");

            //migrationBuilder.DropColumn(
            //    name: "CommiteeId",
            //    schema: "Committe",
            //    table: "CommiteeRoles");

            //migrationBuilder.RenameTable(
            //    name: "CommiteeSavedAttachmentSavedAttachment",
            //    newName: "CommiteeSavedAttachmentSavedAttachment",
            //    newSchema: "Committe");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "CommiteeSavedAttachmentSavedAttachment",
                schema: "Committe",
                newName: "CommiteeSavedAttachmentSavedAttachment");

            migrationBuilder.AddColumn<int>(
                name: "CommiteeId",
                schema: "Committe",
                table: "CommiteeRoles",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CommiteeRoles_CommiteeId",
                schema: "Committe",
                table: "CommiteeRoles",
                column: "CommiteeId");

            migrationBuilder.AddForeignKey(
                name: "FK_CommiteeRoles_Commitees_CommiteeId",
                schema: "Committe",
                table: "CommiteeRoles",
                column: "CommiteeId",
                principalSchema: "Committe",
                principalTable: "Commitees",
                principalColumn: "CommiteeId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
