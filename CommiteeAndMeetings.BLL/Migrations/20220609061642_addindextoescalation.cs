using Microsoft.EntityFrameworkCore.Migrations;

namespace CommiteeAndMeetings.BLL.Migrations
{
    public partial class addindextoescalation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.RenameTable(
            //    name: "CommiteeSavedAttachmentSavedAttachment",
            //    newName: "CommiteeSavedAttachmentSavedAttachment",
            //    newSchema: "Committe");

            //migrationBuilder.AddColumn<int>(
            //    name: "CommiteeTaskEscalationIndex",
            //    schema: "Committe",
            //    table: "CommiteeTaskEscalation",
            //    type: "int",
            //    nullable: false,
            //    defaultValue: 0);

           
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {


            //migrationBuilder.RenameTable(
            //    name: "CommiteeSavedAttachmentSavedAttachment",
            //    schema: "Committe",
            //    newName: "CommiteeSavedAttachmentSavedAttachment");
        }
    }
}
