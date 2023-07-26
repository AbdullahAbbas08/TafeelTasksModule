using Microsoft.EntityFrameworkCore.Migrations;

namespace CommiteeAndMeetings.BLL.Migrations
{
    public partial class RemoveCreatedbyRoleFromMebers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_CommiteeMembers_CommiteeUsersRoles_CreatedByRoleId",
            //    schema: "Committe",
            //    table: "CommiteeMembers");

            //migrationBuilder.DropIndex(
            //    name: "IX_CommiteeRoles_CreatedByRoleId",
            //    schema: "Committe",
            //    table: "CommiteeRoles");

            //migrationBuilder.DropIndex(
            //    name: "IX_CommiteeMembers_CreatedByRoleId",
            //    schema: "Committe",
            //    table: "CommiteeMembers");

            //migrationBuilder.DropColumn(
            //    name: "CreatedByRoleId",
            //    schema: "Committe",
            //    table: "CommiteeMembers");

            ////migrationBuilder.RenameTable(
            ////    name: "CommiteeSavedAttachmentSavedAttachment",
            ////    newName: "CommiteeSavedAttachmentSavedAttachment",
            ////    newSchema: "Committe");

            //migrationBuilder.CreateIndex(
            //    name: "IX_CommiteeUsersRoles_RoleId",
            //    schema: "Committe",
            //    table: "CommiteeUsersRoles",
            //    column: "RoleId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_CommiteeRoles_CreatedByRoleId",
            //    schema: "Committe",
            //    table: "CommiteeRoles",
            //    column: "CreatedByRoleId");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_CommiteeUsersRoles_CommiteeRoles_RoleId",
            //    schema: "Committe",
            //    table: "CommiteeUsersRoles",
            //    column: "RoleId",
            //    principalSchema: "Committe",
            //    principalTable: "CommiteeRoles",
            //    principalColumn: "CommiteeRoleId",
            //    onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommiteeUsersRoles_CommiteeRoles_RoleId",
                schema: "Committe",
                table: "CommiteeUsersRoles");

            migrationBuilder.DropIndex(
                name: "IX_CommiteeUsersRoles_RoleId",
                schema: "Committe",
                table: "CommiteeUsersRoles");

            migrationBuilder.DropIndex(
                name: "IX_CommiteeRoles_CreatedByRoleId",
                schema: "Committe",
                table: "CommiteeRoles");

            migrationBuilder.RenameTable(
                name: "CommiteeSavedAttachmentSavedAttachment",
                schema: "Committe",
                newName: "CommiteeSavedAttachmentSavedAttachment");

            migrationBuilder.AddColumn<int>(
                name: "CreatedByRoleId",
                schema: "Committe",
                table: "CommiteeMembers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CommiteeRoles_CreatedByRoleId",
                schema: "Committe",
                table: "CommiteeRoles",
                column: "CreatedByRoleId",
                unique: true,
                filter: "[CreatedByRoleId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_CommiteeMembers_CreatedByRoleId",
                schema: "Committe",
                table: "CommiteeMembers",
                column: "CreatedByRoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_CommiteeMembers_CommiteeUsersRoles_CreatedByRoleId",
                schema: "Committe",
                table: "CommiteeMembers",
                column: "CreatedByRoleId",
                principalSchema: "Committe",
                principalTable: "CommiteeUsersRoles",
                principalColumn: "CommiteeUsersRoleId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
