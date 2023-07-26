//using Microsoft.EntityFrameworkCore.Migrations;

//namespace CommiteeAndMeetings.BLL.Migrations
//{
//    public partial class AlterTablesAddUserRoleId : Migration
//    {
//        protected override void Up(MigrationBuilder migrationBuilder)
//        {
//            migrationBuilder.AddColumn<int>(
//                name: "CreatedByRoleId",
//                schema: "Committe",
//                table: "ValidityPeriods",
//                type: "int",
//                nullable: true);

//            migrationBuilder.AddColumn<int>(
//                name: "CreatedByRoleId",
//                schema: "Committe",
//                table: "UserTasks",
//                type: "int",
//                nullable: true);

//            migrationBuilder.AddColumn<int>(
//                name: "CreatedByRoleId",
//                schema: "Committe",
//                table: "TaskComments",
//                type: "int",
//                nullable: true);

//            migrationBuilder.AddColumn<int>(
//                name: "CreatedByRoleId",
//                schema: "Committe",
//                table: "SurveyUsers",
//                type: "int",
//                nullable: true);

//            migrationBuilder.AddColumn<int>(
//                name: "CreatedByRoleId",
//                schema: "Committe",
//                table: "Surveys",
//                type: "int",
//                nullable: true);

//            migrationBuilder.AddColumn<int>(
//                name: "CreatedByRoleId",
//                schema: "Committe",
//                table: "SurveyComments",
//                type: "int",
//                nullable: true);

//            migrationBuilder.AddColumn<int>(
//                name: "CreatedByRoleId",
//                schema: "Committe",
//                table: "SurveyAnswers",
//                type: "int",
//                nullable: true);

//            migrationBuilder.AddColumn<int>(
//                name: "CreatedByRoleId",
//                schema: "Committe",
//                table: "SavedAttachments",
//                type: "int",
//                nullable: true);

//            migrationBuilder.AddColumn<int>(
//                name: "CreatedByRoleId",
//                schema: "Committe",
//                table: "CurrentStatusReasons",
//                type: "int",
//                nullable: true);

//            migrationBuilder.AddColumn<int>(
//                name: "CreatedByRoleId",
//                schema: "Committe",
//                table: "CurrentStatus",
//                type: "int",
//                nullable: true);

//            migrationBuilder.AddColumn<int>(
//                name: "CreatedByRoleId",
//                schema: "Committe",
//                table: "CommitteeNotifications",
//                type: "int",
//                nullable: true);

//            migrationBuilder.AddColumn<int>(
//                name: "CreatedByRoleId",
//                schema: "Committe",
//                table: "CommiteeUsersRoles",
//                type: "int",
//                nullable: true);

//            migrationBuilder.AddColumn<int>(
//                name: "CreatedByRoleId",
//                schema: "Committe",
//                table: "CommiteeUsersPermission",
//                type: "int",
//                nullable: true);

//            migrationBuilder.AddColumn<int>(
//                name: "CreatedByRoleId",
//                schema: "Committe",
//                table: "CommiteeTypes",
//                type: "int",
//                nullable: true);

//            migrationBuilder.AddColumn<int>(
//                name: "CreatedByRoleId",
//                schema: "Committe",
//                table: "CommiteeTasks",
//                type: "int",
//                nullable: true);

//            migrationBuilder.AddColumn<int>(
//                name: "CreatedByRoleId",
//                schema: "Committe",
//                table: "CommiteeSavedAttachments",
//                type: "int",
//                nullable: true);

//            migrationBuilder.AddColumn<int>(
//                name: "CreatedByRoleId",
//                schema: "Committe",
//                table: "Commitees",
//                type: "int",
//                nullable: true);

//            migrationBuilder.AddColumn<int>(
//                name: "CreatedByRoleId",
//                schema: "Committe",
//                table: "CommiteeRoles",
//                type: "int",
//                nullable: true);

//            migrationBuilder.AddColumn<int>(
//                name: "CreatedByRoleId",
//                schema: "Committe",
//                table: "CommiteeRolePermissions",
//                type: "int",
//                nullable: true);

//            migrationBuilder.AddColumn<int>(
//                name: "CreatedByRoleId",
//                schema: "Committe",
//                table: "CommiteeMembers",
//                type: "int",
//                nullable: true);

//            migrationBuilder.AddColumn<int>(
//                name: "CreatedByRoleId",
//                schema: "Committe",
//                table: "CommiteeLocalizations",
//                type: "int",
//                nullable: true);

//            migrationBuilder.AddColumn<int>(
//                name: "CreatedByRoleId",
//                schema: "Committe",
//                table: "Comments",
//                type: "int",
//                nullable: true);

//            migrationBuilder.AddColumn<int>(
//                name: "CreatedByRoleId",
//                schema: "Committe",
//                table: "Categories",
//                type: "int",
//                nullable: true);

//            migrationBuilder.AddColumn<int>(
//                name: "CreatedByRoleId",
//                schema: "Committe",
//                table: "AttachmentUsers",
//                type: "int",
//                nullable: true);

//            migrationBuilder.AddColumn<int>(
//                name: "CreatedByRoleId",
//                schema: "Committe",
//                table: "AttachmentComments",
//                type: "int",
//                nullable: true);

//            migrationBuilder.CreateIndex(
//                name: "IX_ValidityPeriods_CreatedByRoleId",
//                schema: "Committe",
//                table: "ValidityPeriods",
//                column: "CreatedByRoleId");

//            migrationBuilder.CreateIndex(
//                name: "IX_UserTasks_CreatedByRoleId",
//                schema: "Committe",
//                table: "UserTasks",
//                column: "CreatedByRoleId");

//            migrationBuilder.CreateIndex(
//                name: "IX_TaskComments_CreatedByRoleId",
//                schema: "Committe",
//                table: "TaskComments",
//                column: "CreatedByRoleId");

//            migrationBuilder.CreateIndex(
//                name: "IX_SurveyUsers_CreatedByRoleId",
//                schema: "Committe",
//                table: "SurveyUsers",
//                column: "CreatedByRoleId");

//            migrationBuilder.CreateIndex(
//                name: "IX_Surveys_CreatedByRoleId",
//                schema: "Committe",
//                table: "Surveys",
//                column: "CreatedByRoleId");

//            migrationBuilder.CreateIndex(
//                name: "IX_SurveyComments_CreatedByRoleId",
//                schema: "Committe",
//                table: "SurveyComments",
//                column: "CreatedByRoleId");

//            migrationBuilder.CreateIndex(
//                name: "IX_SurveyAnswers_CreatedByRoleId",
//                schema: "Committe",
//                table: "SurveyAnswers",
//                column: "CreatedByRoleId");

//            migrationBuilder.CreateIndex(
//                name: "IX_SavedAttachments_CreatedByRoleId",
//                schema: "Committe",
//                table: "SavedAttachments",
//                column: "CreatedByRoleId");

//            migrationBuilder.CreateIndex(
//                name: "IX_CurrentStatusReasons_CreatedByRoleId",
//                schema: "Committe",
//                table: "CurrentStatusReasons",
//                column: "CreatedByRoleId");

//            migrationBuilder.CreateIndex(
//                name: "IX_CurrentStatus_CreatedByRoleId",
//                schema: "Committe",
//                table: "CurrentStatus",
//                column: "CreatedByRoleId");

//            migrationBuilder.CreateIndex(
//                name: "IX_CommitteeNotifications_CreatedBy",
//                schema: "Committe",
//                table: "CommitteeNotifications",
//                column: "CreatedBy");

//            migrationBuilder.CreateIndex(
//                name: "IX_CommitteeNotifications_CreatedByRoleId",
//                schema: "Committe",
//                table: "CommitteeNotifications",
//                column: "CreatedByRoleId");

//            migrationBuilder.CreateIndex(
//                name: "IX_CommiteeUsersRoles_CreatedByRoleId",
//                schema: "Committe",
//                table: "CommiteeUsersRoles",
//                column: "CreatedByRoleId");

//            migrationBuilder.CreateIndex(
//                name: "IX_CommiteeUsersPermission_CreatedByRoleId",
//                schema: "Committe",
//                table: "CommiteeUsersPermission",
//                column: "CreatedByRoleId");

//            migrationBuilder.CreateIndex(
//                name: "IX_CommiteeTypes_CreatedByRoleId",
//                schema: "Committe",
//                table: "CommiteeTypes",
//                column: "CreatedByRoleId");

//            migrationBuilder.CreateIndex(
//                name: "IX_CommiteeTasks_CreatedByRoleId",
//                schema: "Committe",
//                table: "CommiteeTasks",
//                column: "CreatedByRoleId");

//            migrationBuilder.CreateIndex(
//                name: "IX_CommiteeSavedAttachments_CreatedByRoleId",
//                schema: "Committe",
//                table: "CommiteeSavedAttachments",
//                column: "CreatedByRoleId");

//            migrationBuilder.CreateIndex(
//                name: "IX_Commitees_CreatedByRoleId",
//                schema: "Committe",
//                table: "Commitees",
//                column: "CreatedByRoleId",
//                unique: true,
//                filter: "[CreatedByRoleId] IS NOT NULL");

//            migrationBuilder.CreateIndex(
//                name: "IX_CommiteeRoles_CreatedByRoleId",
//                schema: "Committe",
//                table: "CommiteeRoles",
//                column: "CreatedByRoleId",
//                unique: true,
//                filter: "[CreatedByRoleId] IS NOT NULL");

//            migrationBuilder.CreateIndex(
//                name: "IX_CommiteeRolePermissions_CreatedByRoleId",
//                schema: "Committe",
//                table: "CommiteeRolePermissions",
//                column: "CreatedByRoleId");

//            migrationBuilder.CreateIndex(
//                name: "IX_CommiteeMembers_CreatedByRoleId",
//                schema: "Committe",
//                table: "CommiteeMembers",
//                column: "CreatedByRoleId");

//            migrationBuilder.CreateIndex(
//                name: "IX_CommiteeLocalizations_CreatedByRoleId",
//                schema: "Committe",
//                table: "CommiteeLocalizations",
//                column: "CreatedByRoleId");

//            migrationBuilder.CreateIndex(
//                name: "IX_Comments_CreatedByRoleId",
//                schema: "Committe",
//                table: "Comments",
//                column: "CreatedByRoleId");

//            migrationBuilder.CreateIndex(
//                name: "IX_Categories_CreatedByRoleId",
//                schema: "Committe",
//                table: "Categories",
//                column: "CreatedByRoleId");

//            migrationBuilder.CreateIndex(
//                name: "IX_AttachmentUsers_CreatedByRoleId",
//                schema: "Committe",
//                table: "AttachmentUsers",
//                column: "CreatedByRoleId");

//            migrationBuilder.CreateIndex(
//                name: "IX_AttachmentComments_CreatedByRoleId",
//                schema: "Committe",
//                table: "AttachmentComments",
//                column: "CreatedByRoleId");

//            migrationBuilder.AddForeignKey(
//                name: "FK_AttachmentComments_CommiteeUsersRoles_CreatedByRoleId",
//                schema: "Committe",
//                table: "AttachmentComments",
//                column: "CreatedByRoleId",
//                principalSchema: "Committe",
//                principalTable: "CommiteeUsersRoles",
//                principalColumn: "CommiteeUsersRoleId",
//                onDelete: ReferentialAction.Restrict);

//            migrationBuilder.AddForeignKey(
//                name: "FK_AttachmentUsers_CommiteeUsersRoles_CreatedByRoleId",
//                schema: "Committe",
//                table: "AttachmentUsers",
//                column: "CreatedByRoleId",
//                principalSchema: "Committe",
//                principalTable: "CommiteeUsersRoles",
//                principalColumn: "CommiteeUsersRoleId",
//                onDelete: ReferentialAction.Restrict);

//            migrationBuilder.AddForeignKey(
//                name: "FK_Categories_CommiteeUsersRoles_CreatedByRoleId",
//                schema: "Committe",
//                table: "Categories",
//                column: "CreatedByRoleId",
//                principalSchema: "Committe",
//                principalTable: "CommiteeUsersRoles",
//                principalColumn: "CommiteeUsersRoleId",
//                onDelete: ReferentialAction.Restrict);

//            migrationBuilder.AddForeignKey(
//                name: "FK_Comments_CommiteeUsersRoles_CreatedByRoleId",
//                schema: "Committe",
//                table: "Comments",
//                column: "CreatedByRoleId",
//                principalSchema: "Committe",
//                principalTable: "CommiteeUsersRoles",
//                principalColumn: "CommiteeUsersRoleId",
//                onDelete: ReferentialAction.Restrict);

//            migrationBuilder.AddForeignKey(
//                name: "FK_CommiteeLocalizations_CommiteeUsersRoles_CreatedByRoleId",
//                schema: "Committe",
//                table: "CommiteeLocalizations",
//                column: "CreatedByRoleId",
//                principalSchema: "Committe",
//                principalTable: "CommiteeUsersRoles",
//                principalColumn: "CommiteeUsersRoleId",
//                onDelete: ReferentialAction.Restrict);

//            migrationBuilder.AddForeignKey(
//                name: "FK_CommiteeMembers_CommiteeUsersRoles_CreatedByRoleId",
//                schema: "Committe",
//                table: "CommiteeMembers",
//                column: "CreatedByRoleId",
//                principalSchema: "Committe",
//                principalTable: "CommiteeUsersRoles",
//                principalColumn: "CommiteeUsersRoleId",
//                onDelete: ReferentialAction.Restrict);

//            migrationBuilder.AddForeignKey(
//                name: "FK_CommiteeRolePermissions_CommiteeUsersRoles_CreatedByRoleId",
//                schema: "Committe",
//                table: "CommiteeRolePermissions",
//                column: "CreatedByRoleId",
//                principalSchema: "Committe",
//                principalTable: "CommiteeUsersRoles",
//                principalColumn: "CommiteeUsersRoleId",
//                onDelete: ReferentialAction.Restrict);

//            migrationBuilder.AddForeignKey(
//                name: "FK_CommiteeRoles_CommiteeUsersRoles_CreatedByRoleId",
//                schema: "Committe",
//                table: "CommiteeRoles",
//                column: "CreatedByRoleId",
//                principalSchema: "Committe",
//                principalTable: "CommiteeUsersRoles",
//                principalColumn: "CommiteeUsersRoleId",
//                onDelete: ReferentialAction.Restrict);

//            migrationBuilder.AddForeignKey(
//                name: "FK_Commitees_CommiteeUsersRoles_CreatedByRoleId",
//                schema: "Committe",
//                table: "Commitees",
//                column: "CreatedByRoleId",
//                principalSchema: "Committe",
//                principalTable: "CommiteeUsersRoles",
//                principalColumn: "CommiteeUsersRoleId",
//                onDelete: ReferentialAction.Restrict);

//            migrationBuilder.AddForeignKey(
//                name: "FK_CommiteeSavedAttachments_CommiteeUsersRoles_CreatedByRoleId",
//                schema: "Committe",
//                table: "CommiteeSavedAttachments",
//                column: "CreatedByRoleId",
//                principalSchema: "Committe",
//                principalTable: "CommiteeUsersRoles",
//                principalColumn: "CommiteeUsersRoleId",
//                onDelete: ReferentialAction.Restrict);

//            migrationBuilder.AddForeignKey(
//                name: "FK_CommiteeTasks_CommiteeUsersRoles_CreatedByRoleId",
//                schema: "Committe",
//                table: "CommiteeTasks",
//                column: "CreatedByRoleId",
//                principalSchema: "Committe",
//                principalTable: "CommiteeUsersRoles",
//                principalColumn: "CommiteeUsersRoleId",
//                onDelete: ReferentialAction.Restrict);

//            migrationBuilder.AddForeignKey(
//                name: "FK_CommiteeTypes_CommiteeUsersRoles_CreatedByRoleId",
//                schema: "Committe",
//                table: "CommiteeTypes",
//                column: "CreatedByRoleId",
//                principalSchema: "Committe",
//                principalTable: "CommiteeUsersRoles",
//                principalColumn: "CommiteeUsersRoleId",
//                onDelete: ReferentialAction.Restrict);

//            migrationBuilder.AddForeignKey(
//                name: "FK_CommiteeUsersPermission_CommiteeUsersRoles_CreatedByRoleId",
//                schema: "Committe",
//                table: "CommiteeUsersPermission",
//                column: "CreatedByRoleId",
//                principalSchema: "Committe",
//                principalTable: "CommiteeUsersRoles",
//                principalColumn: "CommiteeUsersRoleId",
//                onDelete: ReferentialAction.Restrict);

//            migrationBuilder.AddForeignKey(
//                name: "FK_CommiteeUsersRoles_CommiteeUsersRoles_CreatedByRoleId",
//                schema: "Committe",
//                table: "CommiteeUsersRoles",
//                column: "CreatedByRoleId",
//                principalSchema: "Committe",
//                principalTable: "CommiteeUsersRoles",
//                principalColumn: "CommiteeUsersRoleId",
//                onDelete: ReferentialAction.Restrict);

//            migrationBuilder.AddForeignKey(
//                name: "FK_CommitteeNotifications_CommiteeUsersRoles_CreatedByRoleId",
//                schema: "Committe",
//                table: "CommitteeNotifications",
//                column: "CreatedByRoleId",
//                principalSchema: "Committe",
//                principalTable: "CommiteeUsersRoles",
//                principalColumn: "CommiteeUsersRoleId",
//                onDelete: ReferentialAction.Restrict);

//            migrationBuilder.AddForeignKey(
//                name: "FK_CommitteeNotifications_Users_CreatedBy",
//                schema: "Committe",
//                table: "CommitteeNotifications",
//                column: "CreatedBy",
//                principalTable: "Users",
//                principalColumn: "UserId",
//                onDelete: ReferentialAction.Restrict);

//            migrationBuilder.AddForeignKey(
//                name: "FK_CurrentStatus_CommiteeUsersRoles_CreatedByRoleId",
//                schema: "Committe",
//                table: "CurrentStatus",
//                column: "CreatedByRoleId",
//                principalSchema: "Committe",
//                principalTable: "CommiteeUsersRoles",
//                principalColumn: "CommiteeUsersRoleId",
//                onDelete: ReferentialAction.Restrict);

//            migrationBuilder.AddForeignKey(
//                name: "FK_CurrentStatusReasons_CommiteeUsersRoles_CreatedByRoleId",
//                schema: "Committe",
//                table: "CurrentStatusReasons",
//                column: "CreatedByRoleId",
//                principalSchema: "Committe",
//                principalTable: "CommiteeUsersRoles",
//                principalColumn: "CommiteeUsersRoleId",
//                onDelete: ReferentialAction.Restrict);

//            migrationBuilder.AddForeignKey(
//                name: "FK_SavedAttachments_CommiteeUsersRoles_CreatedByRoleId",
//                schema: "Committe",
//                table: "SavedAttachments",
//                column: "CreatedByRoleId",
//                principalSchema: "Committe",
//                principalTable: "CommiteeUsersRoles",
//                principalColumn: "CommiteeUsersRoleId",
//                onDelete: ReferentialAction.Restrict);

//            migrationBuilder.AddForeignKey(
//                name: "FK_SurveyAnswers_CommiteeUsersRoles_CreatedByRoleId",
//                schema: "Committe",
//                table: "SurveyAnswers",
//                column: "CreatedByRoleId",
//                principalSchema: "Committe",
//                principalTable: "CommiteeUsersRoles",
//                principalColumn: "CommiteeUsersRoleId",
//                onDelete: ReferentialAction.Restrict);

//            migrationBuilder.AddForeignKey(
//                name: "FK_SurveyComments_CommiteeUsersRoles_CreatedByRoleId",
//                schema: "Committe",
//                table: "SurveyComments",
//                column: "CreatedByRoleId",
//                principalSchema: "Committe",
//                principalTable: "CommiteeUsersRoles",
//                principalColumn: "CommiteeUsersRoleId",
//                onDelete: ReferentialAction.Restrict);

//            migrationBuilder.AddForeignKey(
//                name: "FK_Surveys_CommiteeUsersRoles_CreatedByRoleId",
//                schema: "Committe",
//                table: "Surveys",
//                column: "CreatedByRoleId",
//                principalSchema: "Committe",
//                principalTable: "CommiteeUsersRoles",
//                principalColumn: "CommiteeUsersRoleId",
//                onDelete: ReferentialAction.Restrict);

//            migrationBuilder.AddForeignKey(
//                name: "FK_SurveyUsers_CommiteeUsersRoles_CreatedByRoleId",
//                schema: "Committe",
//                table: "SurveyUsers",
//                column: "CreatedByRoleId",
//                principalSchema: "Committe",
//                principalTable: "CommiteeUsersRoles",
//                principalColumn: "CommiteeUsersRoleId",
//                onDelete: ReferentialAction.Restrict);

//            migrationBuilder.AddForeignKey(
//                name: "FK_TaskComments_CommiteeUsersRoles_CreatedByRoleId",
//                schema: "Committe",
//                table: "TaskComments",
//                column: "CreatedByRoleId",
//                principalSchema: "Committe",
//                principalTable: "CommiteeUsersRoles",
//                principalColumn: "CommiteeUsersRoleId",
//                onDelete: ReferentialAction.Restrict);

//            migrationBuilder.AddForeignKey(
//                name: "FK_UserTasks_CommiteeUsersRoles_CreatedByRoleId",
//                schema: "Committe",
//                table: "UserTasks",
//                column: "CreatedByRoleId",
//                principalSchema: "Committe",
//                principalTable: "CommiteeUsersRoles",
//                principalColumn: "CommiteeUsersRoleId",
//                onDelete: ReferentialAction.Restrict);

//            migrationBuilder.AddForeignKey(
//                name: "FK_ValidityPeriods_CommiteeUsersRoles_CreatedByRoleId",
//                schema: "Committe",
//                table: "ValidityPeriods",
//                column: "CreatedByRoleId",
//                principalSchema: "Committe",
//                principalTable: "CommiteeUsersRoles",
//                principalColumn: "CommiteeUsersRoleId",
//                onDelete: ReferentialAction.Restrict);
//        }

//        protected override void Down(MigrationBuilder migrationBuilder)
//        {
//            migrationBuilder.DropForeignKey(
//                name: "FK_AttachmentComments_CommiteeUsersRoles_CreatedByRoleId",
//                schema: "Committe",
//                table: "AttachmentComments");

//            migrationBuilder.DropForeignKey(
//                name: "FK_AttachmentUsers_CommiteeUsersRoles_CreatedByRoleId",
//                schema: "Committe",
//                table: "AttachmentUsers");

//            migrationBuilder.DropForeignKey(
//                name: "FK_Categories_CommiteeUsersRoles_CreatedByRoleId",
//                schema: "Committe",
//                table: "Categories");

//            migrationBuilder.DropForeignKey(
//                name: "FK_Comments_CommiteeUsersRoles_CreatedByRoleId",
//                schema: "Committe",
//                table: "Comments");

//            migrationBuilder.DropForeignKey(
//                name: "FK_CommiteeLocalizations_CommiteeUsersRoles_CreatedByRoleId",
//                schema: "Committe",
//                table: "CommiteeLocalizations");

//            migrationBuilder.DropForeignKey(
//                name: "FK_CommiteeMembers_CommiteeUsersRoles_CreatedByRoleId",
//                schema: "Committe",
//                table: "CommiteeMembers");

//            migrationBuilder.DropForeignKey(
//                name: "FK_CommiteeRolePermissions_CommiteeUsersRoles_CreatedByRoleId",
//                schema: "Committe",
//                table: "CommiteeRolePermissions");

//            migrationBuilder.DropForeignKey(
//                name: "FK_CommiteeRoles_CommiteeUsersRoles_CreatedByRoleId",
//                schema: "Committe",
//                table: "CommiteeRoles");

//            migrationBuilder.DropForeignKey(
//                name: "FK_Commitees_CommiteeUsersRoles_CreatedByRoleId",
//                schema: "Committe",
//                table: "Commitees");

//            migrationBuilder.DropForeignKey(
//                name: "FK_CommiteeSavedAttachments_CommiteeUsersRoles_CreatedByRoleId",
//                schema: "Committe",
//                table: "CommiteeSavedAttachments");

//            migrationBuilder.DropForeignKey(
//                name: "FK_CommiteeTasks_CommiteeUsersRoles_CreatedByRoleId",
//                schema: "Committe",
//                table: "CommiteeTasks");

//            migrationBuilder.DropForeignKey(
//                name: "FK_CommiteeTypes_CommiteeUsersRoles_CreatedByRoleId",
//                schema: "Committe",
//                table: "CommiteeTypes");

//            migrationBuilder.DropForeignKey(
//                name: "FK_CommiteeUsersPermission_CommiteeUsersRoles_CreatedByRoleId",
//                schema: "Committe",
//                table: "CommiteeUsersPermission");

//            migrationBuilder.DropForeignKey(
//                name: "FK_CommiteeUsersRoles_CommiteeUsersRoles_CreatedByRoleId",
//                schema: "Committe",
//                table: "CommiteeUsersRoles");

//            migrationBuilder.DropForeignKey(
//                name: "FK_CommitteeNotifications_CommiteeUsersRoles_CreatedByRoleId",
//                schema: "Committe",
//                table: "CommitteeNotifications");

//            migrationBuilder.DropForeignKey(
//                name: "FK_CommitteeNotifications_Users_CreatedBy",
//                schema: "Committe",
//                table: "CommitteeNotifications");

//            migrationBuilder.DropForeignKey(
//                name: "FK_CurrentStatus_CommiteeUsersRoles_CreatedByRoleId",
//                schema: "Committe",
//                table: "CurrentStatus");

//            migrationBuilder.DropForeignKey(
//                name: "FK_CurrentStatusReasons_CommiteeUsersRoles_CreatedByRoleId",
//                schema: "Committe",
//                table: "CurrentStatusReasons");

//            migrationBuilder.DropForeignKey(
//                name: "FK_SavedAttachments_CommiteeUsersRoles_CreatedByRoleId",
//                schema: "Committe",
//                table: "SavedAttachments");

//            migrationBuilder.DropForeignKey(
//                name: "FK_SurveyAnswers_CommiteeUsersRoles_CreatedByRoleId",
//                schema: "Committe",
//                table: "SurveyAnswers");

//            migrationBuilder.DropForeignKey(
//                name: "FK_SurveyComments_CommiteeUsersRoles_CreatedByRoleId",
//                schema: "Committe",
//                table: "SurveyComments");

//            migrationBuilder.DropForeignKey(
//                name: "FK_Surveys_CommiteeUsersRoles_CreatedByRoleId",
//                schema: "Committe",
//                table: "Surveys");

//            migrationBuilder.DropForeignKey(
//                name: "FK_SurveyUsers_CommiteeUsersRoles_CreatedByRoleId",
//                schema: "Committe",
//                table: "SurveyUsers");

//            migrationBuilder.DropForeignKey(
//                name: "FK_TaskComments_CommiteeUsersRoles_CreatedByRoleId",
//                schema: "Committe",
//                table: "TaskComments");

//            migrationBuilder.DropForeignKey(
//                name: "FK_UserTasks_CommiteeUsersRoles_CreatedByRoleId",
//                schema: "Committe",
//                table: "UserTasks");

//            migrationBuilder.DropForeignKey(
//                name: "FK_ValidityPeriods_CommiteeUsersRoles_CreatedByRoleId",
//                schema: "Committe",
//                table: "ValidityPeriods");

//            migrationBuilder.DropIndex(
//                name: "IX_ValidityPeriods_CreatedByRoleId",
//                schema: "Committe",
//                table: "ValidityPeriods");

//            migrationBuilder.DropIndex(
//                name: "IX_UserTasks_CreatedByRoleId",
//                schema: "Committe",
//                table: "UserTasks");

//            migrationBuilder.DropIndex(
//                name: "IX_TaskComments_CreatedByRoleId",
//                schema: "Committe",
//                table: "TaskComments");

//            migrationBuilder.DropIndex(
//                name: "IX_SurveyUsers_CreatedByRoleId",
//                schema: "Committe",
//                table: "SurveyUsers");

//            migrationBuilder.DropIndex(
//                name: "IX_Surveys_CreatedByRoleId",
//                schema: "Committe",
//                table: "Surveys");

//            migrationBuilder.DropIndex(
//                name: "IX_SurveyComments_CreatedByRoleId",
//                schema: "Committe",
//                table: "SurveyComments");

//            migrationBuilder.DropIndex(
//                name: "IX_SurveyAnswers_CreatedByRoleId",
//                schema: "Committe",
//                table: "SurveyAnswers");

//            migrationBuilder.DropIndex(
//                name: "IX_SavedAttachments_CreatedByRoleId",
//                schema: "Committe",
//                table: "SavedAttachments");

//            migrationBuilder.DropIndex(
//                name: "IX_CurrentStatusReasons_CreatedByRoleId",
//                schema: "Committe",
//                table: "CurrentStatusReasons");

//            migrationBuilder.DropIndex(
//                name: "IX_CurrentStatus_CreatedByRoleId",
//                schema: "Committe",
//                table: "CurrentStatus");

//            migrationBuilder.DropIndex(
//                name: "IX_CommitteeNotifications_CreatedBy",
//                schema: "Committe",
//                table: "CommitteeNotifications");

//            migrationBuilder.DropIndex(
//                name: "IX_CommitteeNotifications_CreatedByRoleId",
//                schema: "Committe",
//                table: "CommitteeNotifications");

//            migrationBuilder.DropIndex(
//                name: "IX_CommiteeUsersRoles_CreatedByRoleId",
//                schema: "Committe",
//                table: "CommiteeUsersRoles");

//            migrationBuilder.DropIndex(
//                name: "IX_CommiteeUsersPermission_CreatedByRoleId",
//                schema: "Committe",
//                table: "CommiteeUsersPermission");

//            migrationBuilder.DropIndex(
//                name: "IX_CommiteeTypes_CreatedByRoleId",
//                schema: "Committe",
//                table: "CommiteeTypes");

//            migrationBuilder.DropIndex(
//                name: "IX_CommiteeTasks_CreatedByRoleId",
//                schema: "Committe",
//                table: "CommiteeTasks");

//            migrationBuilder.DropIndex(
//                name: "IX_CommiteeSavedAttachments_CreatedByRoleId",
//                schema: "Committe",
//                table: "CommiteeSavedAttachments");

//            migrationBuilder.DropIndex(
//                name: "IX_Commitees_CreatedByRoleId",
//                schema: "Committe",
//                table: "Commitees");

//            migrationBuilder.DropIndex(
//                name: "IX_CommiteeRoles_CreatedByRoleId",
//                schema: "Committe",
//                table: "CommiteeRoles");

//            migrationBuilder.DropIndex(
//                name: "IX_CommiteeRolePermissions_CreatedByRoleId",
//                schema: "Committe",
//                table: "CommiteeRolePermissions");

//            migrationBuilder.DropIndex(
//                name: "IX_CommiteeMembers_CreatedByRoleId",
//                schema: "Committe",
//                table: "CommiteeMembers");

//            migrationBuilder.DropIndex(
//                name: "IX_CommiteeLocalizations_CreatedByRoleId",
//                schema: "Committe",
//                table: "CommiteeLocalizations");

//            migrationBuilder.DropIndex(
//                name: "IX_Comments_CreatedByRoleId",
//                schema: "Committe",
//                table: "Comments");

//            migrationBuilder.DropIndex(
//                name: "IX_Categories_CreatedByRoleId",
//                schema: "Committe",
//                table: "Categories");

//            migrationBuilder.DropIndex(
//                name: "IX_AttachmentUsers_CreatedByRoleId",
//                schema: "Committe",
//                table: "AttachmentUsers");

//            migrationBuilder.DropIndex(
//                name: "IX_AttachmentComments_CreatedByRoleId",
//                schema: "Committe",
//                table: "AttachmentComments");

//            migrationBuilder.DropColumn(
//                name: "CreatedByRoleId",
//                schema: "Committe",
//                table: "ValidityPeriods");

//            migrationBuilder.DropColumn(
//                name: "CreatedByRoleId",
//                schema: "Committe",
//                table: "UserTasks");

//            migrationBuilder.DropColumn(
//                name: "CreatedByRoleId",
//                schema: "Committe",
//                table: "TaskComments");

//            migrationBuilder.DropColumn(
//                name: "CreatedByRoleId",
//                schema: "Committe",
//                table: "SurveyUsers");

//            migrationBuilder.DropColumn(
//                name: "CreatedByRoleId",
//                schema: "Committe",
//                table: "Surveys");

//            migrationBuilder.DropColumn(
//                name: "CreatedByRoleId",
//                schema: "Committe",
//                table: "SurveyComments");

//            migrationBuilder.DropColumn(
//                name: "CreatedByRoleId",
//                schema: "Committe",
//                table: "SurveyAnswers");

//            migrationBuilder.DropColumn(
//                name: "CreatedByRoleId",
//                schema: "Committe",
//                table: "SavedAttachments");

//            migrationBuilder.DropColumn(
//                name: "CreatedByRoleId",
//                schema: "Committe",
//                table: "CurrentStatusReasons");

//            migrationBuilder.DropColumn(
//                name: "CreatedByRoleId",
//                schema: "Committe",
//                table: "CurrentStatus");

//            migrationBuilder.DropColumn(
//                name: "CreatedByRoleId",
//                schema: "Committe",
//                table: "CommitteeNotifications");

//            migrationBuilder.DropColumn(
//                name: "CreatedByRoleId",
//                schema: "Committe",
//                table: "CommiteeUsersRoles");

//            migrationBuilder.DropColumn(
//                name: "CreatedByRoleId",
//                schema: "Committe",
//                table: "CommiteeUsersPermission");

//            migrationBuilder.DropColumn(
//                name: "CreatedByRoleId",
//                schema: "Committe",
//                table: "CommiteeTypes");

//            migrationBuilder.DropColumn(
//                name: "CreatedByRoleId",
//                schema: "Committe",
//                table: "CommiteeTasks");

//            migrationBuilder.DropColumn(
//                name: "CreatedByRoleId",
//                schema: "Committe",
//                table: "CommiteeSavedAttachments");

//            migrationBuilder.DropColumn(
//                name: "CreatedByRoleId",
//                schema: "Committe",
//                table: "Commitees");

//            migrationBuilder.DropColumn(
//                name: "CreatedByRoleId",
//                schema: "Committe",
//                table: "CommiteeRoles");

//            migrationBuilder.DropColumn(
//                name: "CreatedByRoleId",
//                schema: "Committe",
//                table: "CommiteeRolePermissions");

//            migrationBuilder.DropColumn(
//                name: "CreatedByRoleId",
//                schema: "Committe",
//                table: "CommiteeMembers");

//            migrationBuilder.DropColumn(
//                name: "CreatedByRoleId",
//                schema: "Committe",
//                table: "CommiteeLocalizations");

//            migrationBuilder.DropColumn(
//                name: "CreatedByRoleId",
//                schema: "Committe",
//                table: "Comments");

//            migrationBuilder.DropColumn(
//                name: "CreatedByRoleId",
//                schema: "Committe",
//                table: "Categories");

//            migrationBuilder.DropColumn(
//                name: "CreatedByRoleId",
//                schema: "Committe",
//                table: "AttachmentUsers");

//            migrationBuilder.DropColumn(
//                name: "CreatedByRoleId",
//                schema: "Committe",
//                table: "AttachmentComments");

//            //migrationBuilder.RenameTable(
//            //    name: "CommiteeSavedAttachmentSavedAttachment",
//            //    schema: "Committe",
//            //    newName: "CommiteeSavedAttachmentSavedAttachment");

//            migrationBuilder.CreateIndex(
//                name: "IX_CommitteeNotifications_UserId",
//                schema: "Committe",
//                table: "CommitteeNotifications",
//                column: "UserId");

//            migrationBuilder.CreateIndex(
//                name: "IX_CommiteeUsersRoles_CommiteeId",
//                schema: "Committe",
//                table: "CommiteeUsersRoles",
//                column: "CommiteeId");

//            migrationBuilder.CreateIndex(
//                name: "IX_CommiteeUsersRoles_RoleId",
//                schema: "Committe",
//                table: "CommiteeUsersRoles",
//                column: "RoleId");

//            migrationBuilder.AddForeignKey(
//                name: "FK_CommiteeUsersRoles_CommiteeRoles_RoleId",
//                schema: "Committe",
//                table: "CommiteeUsersRoles",
//                column: "RoleId",
//                principalSchema: "Committe",
//                principalTable: "CommiteeRoles",
//                principalColumn: "CommiteeRoleId",
//                onDelete: ReferentialAction.Restrict);

//            migrationBuilder.AddForeignKey(
//                name: "FK_CommiteeUsersRoles_Commitees_CommiteeId",
//                schema: "Committe",
//                table: "CommiteeUsersRoles",
//                column: "CommiteeId",
//                principalSchema: "Committe",
//                principalTable: "Commitees",
//                principalColumn: "CommiteeId",
//                onDelete: ReferentialAction.Restrict);

//            migrationBuilder.AddForeignKey(
//                name: "FK_CommitteeNotifications_Users_UserId",
//                schema: "Committe",
//                table: "CommitteeNotifications",
//                column: "UserId",
//                principalTable: "Users",
//                principalColumn: "UserId",
//                onDelete: ReferentialAction.Restrict);
//        }
//    }
//}
