using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace CommiteeAndMeetings.BLL.Migrations
{
    public partial class logFullTask : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.RenameTable(
            //    name: "CommiteeSavedAttachmentSavedAttachment",
            //    newName: "CommiteeSavedAttachmentSavedAttachment",
            //    newSchema: "Committe");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "EndDate",
                schema: "Committe",
                table: "UpdateTaskLog",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsShared",
                schema: "Committe",
                table: "UpdateTaskLog",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Subject",
                schema: "Committe",
                table: "UpdateTaskLog",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndDate",
                schema: "Committe",
                table: "UpdateTaskLog");

            migrationBuilder.DropColumn(
                name: "IsShared",
                schema: "Committe",
                table: "UpdateTaskLog");

            migrationBuilder.DropColumn(
                name: "Subject",
                schema: "Committe",
                table: "UpdateTaskLog");

            //migrationBuilder.RenameTable(
            //    name: "CommiteeSavedAttachmentSavedAttachment",
            //    schema: "Committe",
            //    newName: "CommiteeSavedAttachmentSavedAttachment");
        }
    }
}
