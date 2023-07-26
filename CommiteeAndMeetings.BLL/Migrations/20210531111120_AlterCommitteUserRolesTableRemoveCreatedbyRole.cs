using Microsoft.EntityFrameworkCore.Migrations;

namespace CommiteeAndMeetings.BLL.Migrations
{
    public partial class AlterCommitteUserRolesTableRemoveCreatedbyRole : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_CommiteeUsersRoles_CommiteeUsersRoles_CreatedByRoleId",
            //    schema: "Committe",
            //    table: "CommiteeUsersRoles");

            //migrationBuilder.DropIndex(
            //    name: "IX_CommiteeUsersRoles_CreatedByRoleId",
            //    schema: "Committe",
            //    table: "CommiteeUsersRoles");

            //migrationBuilder.DropColumn(
            //    name: "CreatedByRoleId",
            //    schema: "Committe",
            //    table: "CommiteeUsersRoles");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.RenameTable(
            //    name: "CommiteeSavedAttachmentSavedAttachment",
            //    schema: "Committe",
            //    newName: "CommiteeSavedAttachmentSavedAttachment");

            migrationBuilder.AddColumn<int>(
                name: "CreatedByRoleId",
                schema: "Committe",
                table: "CommiteeUsersRoles",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CommiteeUsersRoles_CreatedByRoleId",
                schema: "Committe",
                table: "CommiteeUsersRoles",
                column: "CreatedByRoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_CommiteeUsersRoles_CommiteeUsersRoles_CreatedByRoleId",
                schema: "Committe",
                table: "CommiteeUsersRoles",
                column: "CreatedByRoleId",
                principalSchema: "Committe",
                principalTable: "CommiteeUsersRoles",
                principalColumn: "CommiteeUsersRoleId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
