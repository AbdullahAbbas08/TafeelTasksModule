using Microsoft.EntityFrameworkCore.Migrations;

namespace CommiteeAndMeetings.BLL.Migrations
{
    public partial class configureautogenerateonadd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.RenameTable(
            //    name: "CommiteeSavedAttachmentSavedAttachment",
            //    newName: "CommiteeSavedAttachmentSavedAttachment",
            //    newSchema: "Committe");
            //migrationBuilder.DropIndex(
            //    name: "IX_CommiteeTaskEscalation_CommiteeTaskEscalationIndex",
            //    schema: "Committe",
            //    table: "CommiteeTaskEscalation");
            //migrationBuilder.DropColumn(
            //     name: "CommiteeTaskEscalationIndex",
            //    schema: "Committe",
            //    table: "CommiteeTaskEscalation"
            //    );
            migrationBuilder.AddColumn<int>(
                name: "CommiteeTaskEscalationIndex",
                schema: "Committe",
                table: "CommiteeTaskEscalation",
                type: "int",
                nullable: false
               )
                .Annotation("SqlServer:Identity", "1, 1");

            //migrationBuilder.CreateIndex(
            //   name: "IX_CommiteeTaskEscalation_CommiteeTaskEscalationIndex",
            //   schema: "Committe",
            //   table: "CommiteeTaskEscalation",
            //   column: "CommiteeTaskEscalationIndex",
            //   unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.RenameTable(
            //    name: "CommiteeSavedAttachmentSavedAttachment",
            //    schema: "Committe",
            //    newName: "CommiteeSavedAttachmentSavedAttachment");
            //migrationBuilder.DropIndex(
            //    name: "IX_CommiteeTaskEscalation_CommiteeTaskEscalationIndex",
            //    schema: "Committe",
            //    table: "CommiteeTaskEscalation");

            //migrationBuilder.DropColumn(
            //    name: "CommiteeTaskEscalationIndex",
            //    schema: "Committe",
            //    table: "CommiteeTaskEscalation");

            //migrationBuilder.AlterColumn<int>(
            //    name: "CommiteeTaskEscalationIndex",
            //    schema: "Committe",
            //    table: "CommiteeTaskEscalation",
            //    type: "int",
            //    nullable: false,
            //    oldClrType: typeof(int),
            //    oldType: "int")
            //    .OldAnnotation("SqlServer:Identity", "1, 1");
        }
    }
}
