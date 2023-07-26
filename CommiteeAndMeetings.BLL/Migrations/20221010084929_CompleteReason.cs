using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CommiteeAndMeetings.BLL.Migrations
{
    public partial class CompleteReason : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           

           

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CompleteReasonDate",
                schema: "Committe",
                table: "CommiteeTasks",
                type: "datetimeoffset",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompleteReasonDate",
                schema: "Committe",
                table: "CommiteeTasks");

           
           
        }
    }
}
