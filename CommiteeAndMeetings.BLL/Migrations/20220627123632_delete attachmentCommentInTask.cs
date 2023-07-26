using Microsoft.EntityFrameworkCore.Migrations;

namespace CommiteeAndMeetings.BLL.Migrations
{
    public partial class deleteattachmentCommentInTask : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                 name: "CommentAttachmentInTask",
                 schema: "Committe");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
           
        }
    }
}
