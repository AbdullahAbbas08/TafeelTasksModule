using Microsoft.EntityFrameworkCore.Migrations;

namespace CommiteeAndMeetings.BLL.Migrations
{
    public partial class addtable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.RenameTable(
            //    name: "CommiteeSavedAttachmentSavedAttachment",
            //    newName: "CommiteeSavedAttachmentSavedAttachment",
            //    newSchema: "Committe");

            //migrationBuilder.CreateTable(
            //    name: "CommiteeTaskMultiMission",
            //    schema: "Committe",
            //    columns: table => new
            //    {
            //        CommiteeTaskMultiMissionId = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        state = table.Column<bool>(type: "bit", nullable: false),
            //        CommiteeTaskId = table.Column<int>(type: "int", nullable: false),
            //        CreatedBy = table.Column<int>(type: "int", nullable: true),
            //        CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
            //        UpdatedBy = table.Column<int>(type: "int", nullable: true),
            //        UpdatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
            //        DeletedBy = table.Column<int>(type: "int", nullable: true),
            //        DeletedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
            //        CreatedByRoleId = table.Column<int>(type: "int", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_CommiteeTaskMultiMission", x => x.CommiteeTaskMultiMissionId);
            //        table.ForeignKey(
            //            name: "FK_CommiteeTaskMultiMission_CommiteeTasks_CommiteeTaskId",
            //            column: x => x.CommiteeTaskId,
            //            principalSchema: "Committe",
            //            principalTable: "CommiteeTasks",
            //            principalColumn: "CommiteeTaskId",
            //            onDelete: ReferentialAction.Restrict);
            //        table.ForeignKey(
            //            name: "FK_CommiteeTaskMultiMission_CommiteeUsersRoles_CreatedByRoleId",
            //            column: x => x.CreatedByRoleId,
            //            principalSchema: "Committe",
            //            principalTable: "CommiteeUsersRoles",
            //            principalColumn: "CommiteeUsersRoleId",
            //            onDelete: ReferentialAction.Restrict);
            //        table.ForeignKey(
            //            name: "FK_CommiteeTaskMultiMission_Users_CreatedBy",
            //            column: x => x.CreatedBy,
            //            principalTable: "Users",
            //            principalColumn: "UserId",
            //            onDelete: ReferentialAction.Restrict);
            //    });

            //migrationBuilder.CreateIndex(
            //    name: "IX_CommiteeTaskMultiMission_CommiteeTaskId",
            //    schema: "Committe",
            //    table: "CommiteeTaskMultiMission",
            //    column: "CommiteeTaskId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_CommiteeTaskMultiMission_CreatedBy",
            //    schema: "Committe",
            //    table: "CommiteeTaskMultiMission",
            //    column: "CreatedBy");

            //migrationBuilder.CreateIndex(
            //    name: "IX_CommiteeTaskMultiMission_CreatedByRoleId",
            //    schema: "Committe",
            //    table: "CommiteeTaskMultiMission",
            //    column: "CreatedByRoleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CommiteeTaskMultiMission",
                schema: "Committe");

            migrationBuilder.RenameTable(
                name: "CommiteeSavedAttachmentSavedAttachment",
                schema: "Committe",
                newName: "CommiteeSavedAttachmentSavedAttachment");
        }
    }
}
