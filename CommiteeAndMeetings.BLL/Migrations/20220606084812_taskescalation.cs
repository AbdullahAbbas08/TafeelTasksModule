using Microsoft.EntityFrameworkCore.Migrations;

namespace CommiteeAndMeetings.BLL.Migrations
{
    public partial class taskescalation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.RenameTable(
            //    name: "CommiteeSavedAttachmentSavedAttachment",
            //    newName: "CommiteeSavedAttachmentSavedAttachment",
            //    newSchema: "Committe");

            migrationBuilder.CreateTable(
                name: "CommiteeTaskEscalation",
                schema: "Committe",
                columns: table => new
                {
                    MainAssinedUserId = table.Column<int>(type: "int", nullable: false),
                    DelayPeriod = table.Column<int>(type: "int", nullable: false),
                    ComiteeTaskCategoryId = table.Column<int>(type: "int", nullable: false),
                    NewMainAssinedUserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommiteeTaskEscalation", x => new { x.ComiteeTaskCategoryId, x.MainAssinedUserId, x.NewMainAssinedUserId, x.DelayPeriod });
                    table.ForeignKey(
                        name: "FK_CommiteeTaskEscalation_Users_MainAssinedUserId",
                        column: x => x.MainAssinedUserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CommiteeTaskEscalation_Users_NewMainAssinedUserId",
                        column: x => x.NewMainAssinedUserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CommiteeTaskEscalation_MainAssinedUserId",
                schema: "Committe",
                table: "CommiteeTaskEscalation",
                column: "MainAssinedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CommiteeTaskEscalation_NewMainAssinedUserId",
                schema: "Committe",
                table: "CommiteeTaskEscalation",
                column: "NewMainAssinedUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CommiteeTaskEscalation",
                schema: "Committe");

            //migrationBuilder.RenameTable(
            //    name: "CommiteeSavedAttachmentSavedAttachment",
            //    schema: "Committe",
            //    newName: "CommiteeSavedAttachmentSavedAttachment");
        }
    }
}
