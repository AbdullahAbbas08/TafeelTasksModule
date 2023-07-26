using Microsoft.EntityFrameworkCore.Migrations;

namespace CommiteeAndMeetings.BLL.Migrations
{
    public partial class AddHeaderAndFooterMeetingTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_Meetings_MeetingHeaderAndFooters_MeetingHeaderAndFooterId",
            //    schema: "Meeting",
            //    table: "Meetings");

            //migrationBuilder.DropIndex(
            //    name: "IX_Meetings_MeetingHeaderAndFooterId",
            //    schema: "Meeting",
            //    table: "Meetings");

            //migrationBuilder.DropColumn(
            //    name: "MeetingHeaderAndFooterId",
            //    schema: "Meeting",
            //    table: "Meetings");



            //migrationBuilder.CreateTable(
            //    name: "Meeting_Meeting_HeaderAndFooters",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        MeetingId = table.Column<int>(type: "int", nullable: false),
            //        HeaderAndFooterId = table.Column<int>(type: "int", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Meeting_Meeting_HeaderAndFooters", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_Meeting_Meeting_HeaderAndFooters_MeetingHeaderAndFooters_HeaderAndFooterId",
            //            column: x => x.HeaderAndFooterId,
            //            principalSchema: "Meeting",
            //            principalTable: "MeetingHeaderAndFooters",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Restrict);
            //        table.ForeignKey(
            //            name: "FK_Meeting_Meeting_HeaderAndFooters_Meetings_MeetingId",
            //            column: x => x.MeetingId,
            //            principalSchema: "Meeting",
            //            principalTable: "Meetings",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Restrict);
            //    });

            //migrationBuilder.CreateIndex(
            //    name: "IX_Meeting_Meeting_HeaderAndFooters_HeaderAndFooterId",
            //    table: "Meeting_Meeting_HeaderAndFooters",
            //    column: "HeaderAndFooterId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Meeting_Meeting_HeaderAndFooters_MeetingId",
            //    table: "Meeting_Meeting_HeaderAndFooters",
            //    column: "MeetingId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Meeting_Meeting_HeaderAndFooters");



            migrationBuilder.AddColumn<int>(
                name: "MeetingHeaderAndFooterId",
                schema: "Meeting",
                table: "Meetings",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Meetings_MeetingHeaderAndFooterId",
                schema: "Meeting",
                table: "Meetings",
                column: "MeetingHeaderAndFooterId");

            migrationBuilder.AddForeignKey(
                name: "FK_Meetings_MeetingHeaderAndFooters_MeetingHeaderAndFooterId",
                schema: "Meeting",
                table: "Meetings",
                column: "MeetingHeaderAndFooterId",
                principalSchema: "Meeting",
                principalTable: "MeetingHeaderAndFooters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
