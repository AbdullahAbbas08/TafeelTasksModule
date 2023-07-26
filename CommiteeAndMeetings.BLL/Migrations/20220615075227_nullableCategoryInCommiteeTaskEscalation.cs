using Microsoft.EntityFrameworkCore.Migrations;

namespace CommiteeAndMeetings.BLL.Migrations
{
    public partial class nullableCategoryInCommiteeTaskEscalation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropIndex(
            //    name: "IX_CommiteeTaskEscalation_ComiteeTaskCategoryId_MainAssinedUserId_NewMainAssinedUserId_DelayPeriod",
            //    schema: "Committe",
            //    table: "CommiteeTaskEscalation");

           

            //migrationBuilder.AlterColumn<int>(
            //    name: "ComiteeTaskCategoryId",
            //    schema: "Committe",
            //    table: "CommiteeTaskEscalation",
            //    type: "int",
            //    nullable: true,
            //    oldClrType: typeof(int),
            //    oldType: "int");

            //migrationBuilder.CreateIndex(
            //    name: "IX_CommiteeTaskEscalation_ComiteeTaskCategoryId_MainAssinedUserId_NewMainAssinedUserId_DelayPeriod",
            //    schema: "Committe",
            //    table: "CommiteeTaskEscalation",
            //    columns: new[] { "ComiteeTaskCategoryId", "MainAssinedUserId", "NewMainAssinedUserId", "DelayPeriod" },
            //    unique: true,
            //    filter: "[ComiteeTaskCategoryId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CommiteeTaskEscalation_ComiteeTaskCategoryId_MainAssinedUserId_NewMainAssinedUserId_DelayPeriod",
                schema: "Committe",
                table: "CommiteeTaskEscalation");

            
            migrationBuilder.AlterColumn<int>(
                name: "ComiteeTaskCategoryId",
                schema: "Committe",
                table: "CommiteeTaskEscalation",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CommiteeTaskEscalation_ComiteeTaskCategoryId_MainAssinedUserId_NewMainAssinedUserId_DelayPeriod",
                schema: "Committe",
                table: "CommiteeTaskEscalation",
                columns: new[] { "ComiteeTaskCategoryId", "MainAssinedUserId", "NewMainAssinedUserId", "DelayPeriod" },
                unique: true);
        }
    }
}
