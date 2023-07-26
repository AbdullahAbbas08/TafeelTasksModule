using Microsoft.EntityFrameworkCore.Migrations;

namespace CommiteeAndMeetings.BLL.Migrations
{
    public partial class AddIsDelegatedIntoCommiteeUsersPermission : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
          

            migrationBuilder.AddColumn<bool>(
                name: "IsDelegated",
                schema: "Committe",
                table: "CommiteeUsersPermission",
                type: "bit",
                nullable: false,
                defaultValue: false);

           
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
           

            migrationBuilder.DropColumn(
                name: "IsDelegated",
                schema: "Committe",
                table: "CommiteeUsersPermission");

        }
    }
}
