﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace CommiteeAndMeetings.BLL.Migrations
{
    public partial class AddpropTaskToView : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           

            migrationBuilder.AddColumn<bool>(
                name: "TaskToView",
                schema: "Committe",
                table: "CommiteeTasks",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TaskToView",
                schema: "Committe",
                table: "CommiteeTasks");

           
        }
    }
}
