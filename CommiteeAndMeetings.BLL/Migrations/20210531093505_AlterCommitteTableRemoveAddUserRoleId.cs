using Microsoft.EntityFrameworkCore.Migrations;

namespace CommiteeAndMeetings.BLL.Migrations
{
    public partial class AlterCommitteTableRemoveAddUserRoleId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_Commitees_CommiteeUsersRoles_CreatedByRoleId",
            //    schema: "Committe",
            //    table: "Commitees");

            //migrationBuilder.DropIndex(
            //    name: "IX_Commitees_CreatedByRoleId",
            //    schema: "Committe",
            //    table: "Commitees");

            //migrationBuilder.DropColumn(
            //    name: "CreatedByRoleId",
            //    schema: "Committe",
            //    table: "Commitees");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommiteeUsersRoles_Commitees_CommiteeId",
                schema: "Committe",
                table: "CommiteeUsersRoles");

            migrationBuilder.DropIndex(
                name: "IX_CommiteeUsersRoles_CommiteeId",
                schema: "Committe",
                table: "CommiteeUsersRoles");

            //migrationBuilder.RenameTable(
            //    name: "CommiteeSavedAttachmentSavedAttachment",
            //    schema: "Committe",
            //    newName: "CommiteeSavedAttachmentSavedAttachment");

            migrationBuilder.AddColumn<int>(
                name: "CreatedByRoleId",
                schema: "Committe",
                table: "Commitees",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Commitees_CreatedByRoleId",
                schema: "Committe",
                table: "Commitees",
                column: "CreatedByRoleId",
                unique: true,
                filter: "[CreatedByRoleId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Commitees_CommiteeUsersRoles_CreatedByRoleId",
                schema: "Committe",
                table: "Commitees",
                column: "CreatedByRoleId",
                principalSchema: "Committe",
                principalTable: "CommiteeUsersRoles",
                principalColumn: "CommiteeUsersRoleId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
