using Microsoft.EntityFrameworkCore.Migrations;

namespace CommiteeAndMeetings.BLL.Migrations
{
    public partial class addtaskcategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.RenameTable(
            //    name: "CommiteeSavedAttachmentSavedAttachment",
            //    newName: "CommiteeSavedAttachmentSavedAttachment",
            //    newSchema: "Committe");

            migrationBuilder.AddColumn<int>(
                name: "ComiteeTaskCategoryId",
                schema: "Committe",
                table: "CommiteeTasks",
                type: "int",
                nullable: true);

            //migrationBuilder.CreateTable(
            //    name: "ComiteeTaskCategory",
            //    schema: "Committe",
            //    columns: table => new
            //    {
            //        ComiteeTaskCategoryId = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        categoryName = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
            //        table.PrimaryKey("PK_ComiteeTaskCategory", x => x.ComiteeTaskCategoryId);
            //        table.ForeignKey(
            //            name: "FK_ComiteeTaskCategory_CommiteeUsersRoles_CreatedByRoleId",
            //            column: x => x.CreatedByRoleId,
            //            principalSchema: "Committe",
            //            principalTable: "CommiteeUsersRoles",
            //            principalColumn: "CommiteeUsersRoleId",
            //            onDelete: ReferentialAction.Restrict);
            //        table.ForeignKey(
            //            name: "FK_ComiteeTaskCategory_Users_CreatedBy",
            //            column: x => x.CreatedBy,
            //            principalTable: "Users",
            //            principalColumn: "UserId",
            //            onDelete: ReferentialAction.Restrict);
            //    });

            //migrationBuilder.CreateIndex(
            //    name: "IX_ComiteeTaskCategory_CreatedBy",
            //    schema: "Committe",
            //    table: "ComiteeTaskCategory",
            //    column: "CreatedBy");

            //migrationBuilder.CreateIndex(
            //    name: "IX_ComiteeTaskCategory_CreatedByRoleId",
            //    schema: "Committe",
            //    table: "ComiteeTaskCategory",
            //    column: "CreatedByRoleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ComiteeTaskCategory",
                schema: "Committe");

            migrationBuilder.DropColumn(
                name: "ComiteeTaskCategoryId",
                schema: "Committe",
                table: "CommiteeTasks");


        }
    }
}
