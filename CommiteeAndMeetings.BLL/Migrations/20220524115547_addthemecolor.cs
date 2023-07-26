using Microsoft.EntityFrameworkCore.Migrations;

namespace CommiteeAndMeetings.BLL.Migrations
{
    public partial class addthemecolor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.RenameTable(
            //    name: "CommiteeSavedAttachmentSavedAttachment",
            //    newName: "CommiteeSavedAttachmentSavedAttachment",
            //    newSchema: "Committe");

            migrationBuilder.CreateTable(
                name: "Theme",
                schema: "Committe",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FirstColorHex = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecondColorHex = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ThirdColorHex = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Theme", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Theme",
                schema: "Committe");

            //migrationBuilder.RenameTable(
            //    name: "CommiteeSavedAttachmentSavedAttachment",
            //    schema: "Committe",
            //    newName: "CommiteeSavedAttachmentSavedAttachment");
        }
    }
}
