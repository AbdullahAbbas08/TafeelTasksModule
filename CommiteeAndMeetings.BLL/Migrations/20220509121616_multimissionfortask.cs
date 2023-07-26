﻿using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace CommiteeAndMeetings.BLL.Migrations
{
    public partial class multimissionfortask : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {


            migrationBuilder.CreateTable(
                name: "CommiteeTaskMultiMission",
                schema: "Committe",
                columns: table => new
                {
                    CommiteeTaskMultiMissionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    state = table.Column<bool>(type: "bit", nullable: false),
                    CommiteeTaskId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommiteeTaskMultiMission", x => x.CommiteeTaskMultiMissionId);
                    table.ForeignKey(
                        name: "FK_CommiteeTaskMultiMission_CommiteeTasks_CommiteeTaskId",
                        column: x => x.CommiteeTaskId,
                        principalSchema: "Committe",
                        principalTable: "CommiteeTasks",
                        principalColumn: "CommiteeTaskId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CommiteeTaskMultiMission_CommiteeTaskId",
                schema: "Committe",
                table: "CommiteeTaskMultiMission",
                column: "CommiteeTaskId");
            migrationBuilder.AddColumn<int>(
              name: "CreatedBy",
              schema: "Committe",
              table: "CommiteeTaskMultiMission",
              type: "int",
              nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreatedByRoleId",
                schema: "Committe",
                table: "CommiteeTaskMultiMission",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedOn",
                schema: "Committe",
                table: "CommiteeTaskMultiMission",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeletedBy",
                schema: "Committe",
                table: "CommiteeTaskMultiMission",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DeletedOn",
                schema: "Committe",
                table: "CommiteeTaskMultiMission",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UpdatedBy",
                schema: "Committe",
                table: "CommiteeTaskMultiMission",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "UpdatedOn",
                schema: "Committe",
                table: "CommiteeTaskMultiMission",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CommiteeTaskMultiMission_CreatedBy",
                schema: "Committe",
                table: "CommiteeTaskMultiMission",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_CommiteeTaskMultiMission_CreatedByRoleId",
                schema: "Committe",
                table: "CommiteeTaskMultiMission",
                column: "CreatedByRoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_CommiteeTaskMultiMission_CommiteeUsersRoles_CreatedByRoleId",
                schema: "Committe",
                table: "CommiteeTaskMultiMission",
                column: "CreatedByRoleId",
                principalSchema: "Committe",
                principalTable: "CommiteeUsersRoles",
                principalColumn: "CommiteeUsersRoleId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CommiteeTaskMultiMission_Users_CreatedBy",
                schema: "Committe",
                table: "CommiteeTaskMultiMission",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CommiteeTaskMultiMission",
                schema: "Committe");
            migrationBuilder.DropForeignKey(
               name: "FK_CommiteeTaskMultiMission_CommiteeUsersRoles_CreatedByRoleId",
               schema: "Committe",
               table: "CommiteeTaskMultiMission");

            migrationBuilder.DropForeignKey(
                name: "FK_CommiteeTaskMultiMission_Users_CreatedBy",
                schema: "Committe",
                table: "CommiteeTaskMultiMission");

            migrationBuilder.DropIndex(
                name: "IX_CommiteeTaskMultiMission_CreatedBy",
                schema: "Committe",
                table: "CommiteeTaskMultiMission");

            migrationBuilder.DropIndex(
                name: "IX_CommiteeTaskMultiMission_CreatedByRoleId",
                schema: "Committe",
                table: "CommiteeTaskMultiMission");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                schema: "Committe",
                table: "CommiteeTaskMultiMission");

            migrationBuilder.DropColumn(
                name: "CreatedByRoleId",
                schema: "Committe",
                table: "CommiteeTaskMultiMission");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                schema: "Committe",
                table: "CommiteeTaskMultiMission");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                schema: "Committe",
                table: "CommiteeTaskMultiMission");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                schema: "Committe",
                table: "CommiteeTaskMultiMission");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                schema: "Committe",
                table: "CommiteeTaskMultiMission");

            migrationBuilder.DropColumn(
                name: "UpdatedOn",
                schema: "Committe",
                table: "CommiteeTaskMultiMission");

            //migrationBuilder.RenameTable(
            //    name: "CommiteeSavedAttachmentSavedAttachment",
            //    schema: "Committe",
            //    newName: "CommiteeSavedAttachmentSavedAttachment");
        }
    }
}
