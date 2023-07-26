using Microsoft.EntityFrameworkCore.Migrations;

namespace CommiteeAndMeetings.BLL.Migrations
{
    public partial class systemsettingcommittee : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.RenameTable(
            //    name: "CommiteeSavedAttachmentSavedAttachment",
            //    newName: "CommiteeSavedAttachmentSavedAttachment",
            //    newSchema: "Committe");

            migrationBuilder.CreateTable(
                name: "CommitteeMeetingSystemSetting",
                schema: "Committe",
                columns: table => new
                {
                    SystemSettingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SystemSettingCode = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: true),
                    SystemSettingNameAr = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: true),
                    SystemSettingNameEn = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: true),
                    SystemSettingValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SystemSettingCategoryId = table.Column<int>(type: "int", nullable: true),
                    IsClientSide = table.Column<bool>(type: "bit", nullable: false),
                    Instructions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsHidden = table.Column<bool>(type: "bit", nullable: false),
                    SystemSettingNameFn = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommitteeMeetingSystemSetting", x => x.SystemSettingId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CommitteeMeetingSystemSetting",
                schema: "Committe");

            //migrationBuilder.RenameTable(
            //    name: "CommiteeSavedAttachmentSavedAttachment",
            //    schema: "Committe",
            //    newName: "CommiteeSavedAttachmentSavedAttachment");
        }
    }
}
