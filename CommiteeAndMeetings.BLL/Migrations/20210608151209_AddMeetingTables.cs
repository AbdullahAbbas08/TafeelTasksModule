using Microsoft.EntityFrameworkCore.Migrations;

namespace CommiteeAndMeetings.BLL.Migrations
{
    public partial class AddMeetingTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //            migrationBuilder.EnsureSchema(
            //                name: "Meeting");

            //            //migrationBuilder.RenameTable(
            //            //    name: "CommiteeSavedAttachmentSavedAttachment",
            //            //    newName: "CommiteeSavedAttachmentSavedAttachment",
            //            //    newSchema: "Committe");

            //            migrationBuilder.AlterColumn<int>(
            //                name: "CommiteeId",
            //                schema: "Committe",
            //                table: "Surveys",
            //                type: "int",
            //                nullable: true,
            //                oldClrType: typeof(int),
            //                oldType: "int");

            //            migrationBuilder.AddColumn<int>(
            //                name: "MeetingTopicId",
            //                schema: "Committe",
            //                table: "Surveys",
            //                type: "int",
            //                nullable: true);

            //            migrationBuilder.CreateTable(
            //                name: "Meetings",
            //                schema: "Meeting",
            //                columns: table => new
            //                {
            //                    Id = table.Column<int>(type: "int", nullable: false)
            //                        .Annotation("SqlServer:Identity", "1, 1"),
            //                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //                    Subject = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //                    Repated = table.Column<bool>(type: "bit", nullable: false),
            //                    ReminderBeforeMinutes = table.Column<int>(type: "int", nullable: false),
            //                    IsSecret = table.Column<bool>(type: "bit", nullable: false),
            //                    PermitedToEnterMeeting = table.Column<bool>(type: "bit", nullable: false),
            //                    MemberConfirmation = table.Column<bool>(type: "bit", nullable: false),
            //                    CreatedBy = table.Column<int>(type: "int", nullable: true),
            //                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
            //                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
            //                    UpdatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
            //                    DeletedBy = table.Column<int>(type: "int", nullable: true),
            //                    DeletedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
            //                },
            //                constraints: table =>
            //                {
            //                    table.PrimaryKey("PK_Meetings", x => x.Id);
            //                });

            //            migrationBuilder.CreateTable(
            //                name: "Projects",
            //                schema: "Meeting",
            //                columns: table => new
            //                {
            //                    Id = table.Column<int>(type: "int", nullable: false)
            //                        .Annotation("SqlServer:Identity", "1, 1"),
            //                    ProjectNameAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //                    ProjectNameEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //                    CreatedBy = table.Column<int>(type: "int", nullable: true),
            //                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
            //                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
            //                    UpdatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
            //                    DeletedBy = table.Column<int>(type: "int", nullable: true),
            //                    DeletedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
            //                },
            //                constraints: table =>
            //                {
            //                    table.PrimaryKey("PK_Projects", x => x.Id);
            //                });

            //            migrationBuilder.CreateTable(
            //                name: "TopicTypes",
            //                schema: "Meeting",
            //                columns: table => new
            //                {
            //                    Id = table.Column<int>(type: "int", nullable: false)
            //                        .Annotation("SqlServer:Identity", "1, 1"),
            //                    TopicTypeNameAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //                    TopicTypeNameEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //                    DeletedBy = table.Column<int>(type: "int", nullable: true),
            //                    DeletedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
            //                    CreatedBy = table.Column<int>(type: "int", nullable: true),
            //                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
            //                },
            //                constraints: table =>
            //                {
            //                    table.PrimaryKey("PK_TopicTypes", x => x.Id);
            //                });

            //            migrationBuilder.CreateTable(
            //                name: "MeetingAttendee",
            //                schema: "Meeting",
            //                columns: table => new
            //                {
            //                    Id = table.Column<int>(type: "int", nullable: false)
            //                        .Annotation("SqlServer:Identity", "1, 1"),
            //                    CoordinatorId = table.Column<int>(type: "int", nullable: false),
            //                    MeetingId = table.Column<int>(type: "int", nullable: false),
            //                    Available = table.Column<bool>(type: "bit", nullable: false),
            //                    Confirmed = table.Column<bool>(type: "bit", nullable: false),
            //                    CreatedBy = table.Column<int>(type: "int", nullable: true),
            //                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
            //                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
            //                    UpdatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
            //                    DeletedBy = table.Column<int>(type: "int", nullable: true),
            //                    DeletedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
            //                },
            //                constraints: table =>
            //                {
            //                    table.PrimaryKey("PK_MeetingAttendee", x => x.Id);
            //                    table.ForeignKey(
            //                        name: "FK_MeetingAttendee_Meetings_MeetingId",
            //                        column: x => x.MeetingId,
            //                        principalSchema: "Meeting",
            //                        principalTable: "Meetings",
            //                        principalColumn: "Id",
            //                        onDelete: ReferentialAction.Restrict);
            //                    table.ForeignKey(
            //                        name: "FK_MeetingAttendee_Users_CoordinatorId",
            //                        column: x => x.CoordinatorId,
            //                        principalTable: "Users",
            //                        principalColumn: "UserId",
            //                        onDelete: ReferentialAction.Restrict);
            //                });

            //            migrationBuilder.CreateTable(
            //                name: "MeetingCoordinators",
            //                schema: "Meeting",
            //                columns: table => new
            //                {
            //                    Id = table.Column<int>(type: "int", nullable: false)
            //                        .Annotation("SqlServer:Identity", "1, 1"),
            //                    CoordinatorId = table.Column<int>(type: "int", nullable: false),
            //                    MeetingId = table.Column<int>(type: "int", nullable: false),
            //                    Available = table.Column<bool>(type: "bit", nullable: false),
            //                    Confirmed = table.Column<bool>(type: "bit", nullable: false),
            //                    CreatedBy = table.Column<int>(type: "int", nullable: true),
            //                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
            //                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
            //                    UpdatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
            //                    DeletedBy = table.Column<int>(type: "int", nullable: true),
            //                    DeletedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
            //                },
            //                constraints: table =>
            //                {
            //                    table.PrimaryKey("PK_MeetingCoordinators", x => x.Id);
            //                    table.ForeignKey(
            //                        name: "FK_MeetingCoordinators_Meetings_MeetingId",
            //                        column: x => x.MeetingId,
            //                        principalSchema: "Meeting",
            //                        principalTable: "Meetings",
            //                        principalColumn: "Id",
            //                        onDelete: ReferentialAction.Restrict);
            //                    table.ForeignKey(
            //                        name: "FK_MeetingCoordinators_Users_CoordinatorId",
            //                        column: x => x.CoordinatorId,
            //                        principalTable: "Users",
            //                        principalColumn: "UserId",
            //                        onDelete: ReferentialAction.Restrict);
            //                });

            //            migrationBuilder.CreateTable(
            //                name: "MeetingDates",
            //                schema: "Meeting",
            //                columns: table => new
            //                {
            //                    Id = table.Column<int>(type: "int", nullable: false)
            //                        .Annotation("SqlServer:Identity", "1, 1"),
            //                    MeetingId = table.Column<int>(type: "int", nullable: false),
            //                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
            //                    MeetingFromTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
            //                    MeetingToTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
            //                    MeetingState = table.Column<int>(type: "int", nullable: false),
            //                    CreatedBy = table.Column<int>(type: "int", nullable: true),
            //                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
            //                },
            //                constraints: table =>
            //                {
            //                    table.PrimaryKey("PK_MeetingDates", x => x.Id);
            //                    table.ForeignKey(
            //                        name: "FK_MeetingDates_Meetings_MeetingId",
            //                        column: x => x.MeetingId,
            //                        principalSchema: "Meeting",
            //                        principalTable: "Meetings",
            //                        principalColumn: "Id",
            //                        onDelete: ReferentialAction.Restrict);
            //                });

            //            migrationBuilder.CreateTable(
            //                name: "MeetingLocations",
            //                schema: "Meeting",
            //                columns: table => new
            //                {
            //                    Id = table.Column<int>(type: "int", nullable: false)
            //                        .Annotation("SqlServer:Identity", "1, 1"),
            //                    OnlineUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //                    PhysicalLocation = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //                    MeetingId = table.Column<int>(type: "int", nullable: false),
            //                    DeletedBy = table.Column<int>(type: "int", nullable: true),
            //                    DeletedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
            //                    CreatedBy = table.Column<int>(type: "int", nullable: true),
            //                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
            //                },
            //                constraints: table =>
            //                {
            //                    table.PrimaryKey("PK_MeetingLocations", x => x.Id);
            //                    table.ForeignKey(
            //                        name: "FK_MeetingLocations_Meetings_MeetingId",
            //                        column: x => x.MeetingId,
            //                        principalSchema: "Meeting",
            //                        principalTable: "Meetings",
            //                        principalColumn: "Id",
            //                        onDelete: ReferentialAction.Restrict);
            //                });

            //            migrationBuilder.CreateTable(
            //                name: "MinutesOfMeetings",
            //                schema: "Meeting",
            //                columns: table => new
            //                {
            //                    Id = table.Column<int>(type: "int", nullable: false)
            //                        .Annotation("SqlServer:Identity", "1, 1"),
            //                    MeetingId = table.Column<int>(type: "int", nullable: false),
            //                    FromTopic = table.Column<bool>(type: "bit", nullable: false),
            //                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //                    CreatedBy = table.Column<int>(type: "int", nullable: true),
            //                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
            //                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
            //                    UpdatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
            //                    DeletedBy = table.Column<int>(type: "int", nullable: true),
            //                    DeletedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
            //                },
            //                constraints: table =>
            //                {
            //                    table.PrimaryKey("PK_MinutesOfMeetings", x => x.Id);
            //                    table.ForeignKey(
            //                        name: "FK_MinutesOfMeetings_Meetings_MeetingId",
            //                        column: x => x.MeetingId,
            //                        principalSchema: "Meeting",
            //                        principalTable: "Meetings",
            //                        principalColumn: "Id",
            //                        onDelete: ReferentialAction.Restrict);
            //                });

            //            migrationBuilder.CreateTable(
            //                name: "MeetingProjects",
            //                schema: "Meeting",
            //                columns: table => new
            //                {
            //                    Id = table.Column<int>(type: "int", nullable: false)
            //                        .Annotation("SqlServer:Identity", "1, 1"),
            //                    ProjectId = table.Column<int>(type: "int", nullable: false),
            //                    MeetingId = table.Column<int>(type: "int", nullable: false),
            //                    DeletedBy = table.Column<int>(type: "int", nullable: true),
            //                    DeletedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
            //                    CreatedBy = table.Column<int>(type: "int", nullable: true),
            //                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
            //                },
            //                constraints: table =>
            //                {
            //                    table.PrimaryKey("PK_MeetingProjects", x => x.Id);
            //                    table.ForeignKey(
            //                        name: "FK_MeetingProjects_Meetings_MeetingId",
            //                        column: x => x.MeetingId,
            //                        principalSchema: "Meeting",
            //                        principalTable: "Meetings",
            //                        principalColumn: "Id",
            //                        onDelete: ReferentialAction.Restrict);
            //                    table.ForeignKey(
            //                        name: "FK_MeetingProjects_Projects_ProjectId",
            //                        column: x => x.ProjectId,
            //                        principalSchema: "Meeting",
            //                        principalTable: "Projects",
            //                        principalColumn: "Id",
            //                        onDelete: ReferentialAction.Restrict);
            //                });

            //            migrationBuilder.CreateTable(
            //                name: "MeetingTopics",
            //                schema: "Meeting",
            //                columns: table => new
            //                {
            //                    Id = table.Column<int>(type: "int", nullable: false)
            //                        .Annotation("SqlServer:Identity", "1, 1"),
            //                    TopicTypeId = table.Column<int>(type: "int", nullable: false),
            //                    TopicTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //                    TopicPoints = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //                    TopicDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
            //                    TopicFromDateTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
            //                    TopicToDateTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
            //                    TopicAcualStartDateTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
            //                    TopicAcualEndDateTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
            //                    TopicState = table.Column<int>(type: "int", nullable: false),
            //                    CreatedBy = table.Column<int>(type: "int", nullable: true),
            //                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
            //                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
            //                    UpdatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
            //                    DeletedBy = table.Column<int>(type: "int", nullable: true),
            //                    DeletedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
            //                    MeetingId = table.Column<int>(type: "int", nullable: true),
            //                    MinuteOfMeetingId = table.Column<int>(type: "int", nullable: true)
            //                },
            //                constraints: table =>
            //                {
            //                    table.PrimaryKey("PK_MeetingTopics", x => x.Id);
            //                    table.ForeignKey(
            //                        name: "FK_MeetingTopics_Meetings_MeetingId",
            //                        column: x => x.MeetingId,
            //                        principalSchema: "Meeting",
            //                        principalTable: "Meetings",
            //                        principalColumn: "Id",
            //                        onDelete: ReferentialAction.Restrict);
            //                    table.ForeignKey(
            //                        name: "FK_MeetingTopics_MinutesOfMeetings_MinuteOfMeetingId",
            //                        column: x => x.MinuteOfMeetingId,
            //                        principalSchema: "Meeting",
            //                        principalTable: "MinutesOfMeetings",
            //                        principalColumn: "Id",
            //                        onDelete: ReferentialAction.Restrict);
            //                    table.ForeignKey(
            //                        name: "FK_MeetingTopics_TopicTypes_TopicTypeId",
            //                        column: x => x.TopicTypeId,
            //                        principalSchema: "Meeting",
            //                        principalTable: "TopicTypes",
            //                        principalColumn: "Id",
            //                        onDelete: ReferentialAction.Restrict);
            //                });

            //            migrationBuilder.CreateTable(
            //                name: "TopicComments",
            //                schema: "Meeting",
            //                columns: table => new
            //                {
            //                    Id = table.Column<int>(type: "int", nullable: false)
            //                        .Annotation("SqlServer:Identity", "1, 1"),
            //                    TopicId = table.Column<int>(type: "int", nullable: false),
            //                    CommentId = table.Column<int>(type: "int", nullable: false),
            //                    CommentType = table.Column<int>(type: "int", nullable: false),
            //                    CreatedBy = table.Column<int>(type: "int", nullable: true),
            //                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
            //                },
            //                constraints: table =>
            //                {
            //                    table.PrimaryKey("PK_TopicComments", x => x.Id);
            //                    table.ForeignKey(
            //                        name: "FK_TopicComments_Comments_CommentId",
            //                        column: x => x.CommentId,
            //                        principalSchema: "Committe",
            //                        principalTable: "Comments",
            //                        principalColumn: "CommentId",
            //                        onDelete: ReferentialAction.Restrict);
            //                    table.ForeignKey(
            //                        name: "FK_TopicComments_MeetingTopics_TopicId",
            //                        column: x => x.TopicId,
            //                        principalSchema: "Meeting",
            //                        principalTable: "MeetingTopics",
            //                        principalColumn: "Id",
            //                        onDelete: ReferentialAction.Restrict);
            //                });

            //            migrationBuilder.CreateTable(
            //                name: "TopicPauseDates",
            //                schema: "Meeting",
            //                columns: table => new
            //                {
            //                    Id = table.Column<int>(type: "int", nullable: false)
            //                        .Annotation("SqlServer:Identity", "1, 1"),
            //                    TopicId = table.Column<int>(type: "int", nullable: false),
            //                    PauseDateTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
            //                    ContinueDateTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
            //                    CreatedBy = table.Column<int>(type: "int", nullable: true),
            //                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
            //                },
            //                constraints: table =>
            //                {
            //                    table.PrimaryKey("PK_TopicPauseDates", x => x.Id);
            //                    table.ForeignKey(
            //                        name: "FK_TopicPauseDates_MeetingTopics_TopicId",
            //                        column: x => x.TopicId,
            //                        principalSchema: "Meeting",
            //                        principalTable: "MeetingTopics",
            //                        principalColumn: "Id",
            //                        onDelete: ReferentialAction.Restrict);
            //                });

            //            migrationBuilder.CreateIndex(
            //                name: "IX_Surveys_MeetingTopicId",
            //                schema: "Committe",
            //                table: "Surveys",
            //                column: "MeetingTopicId");

            //            migrationBuilder.CreateIndex(
            //                name: "IX_MeetingAttendee_CoordinatorId",
            //                schema: "Meeting",
            //                table: "MeetingAttendee",
            //                column: "CoordinatorId");

            //            migrationBuilder.CreateIndex(
            //                name: "IX_MeetingAttendee_MeetingId",
            //                schema: "Meeting",
            //                table: "MeetingAttendee",
            //                column: "MeetingId");

            //            migrationBuilder.CreateIndex(
            //                name: "IX_MeetingCoordinators_CoordinatorId",
            //                schema: "Meeting",
            //                table: "MeetingCoordinators",
            //                column: "CoordinatorId");

            //            migrationBuilder.CreateIndex(
            //                name: "IX_MeetingCoordinators_MeetingId",
            //                schema: "Meeting",
            //                table: "MeetingCoordinators",
            //                column: "MeetingId");

            //            migrationBuilder.CreateIndex(
            //                name: "IX_MeetingDates_MeetingId",
            //                schema: "Meeting",
            //                table: "MeetingDates",
            //                column: "MeetingId");

            //            migrationBuilder.CreateIndex(
            //                name: "IX_MeetingLocations_MeetingId",
            //                schema: "Meeting",
            //                table: "MeetingLocations",
            //                column: "MeetingId");

            //            migrationBuilder.CreateIndex(
            //                name: "IX_MeetingProjects_MeetingId",
            //                schema: "Meeting",
            //                table: "MeetingProjects",
            //                column: "MeetingId");

            //            migrationBuilder.CreateIndex(
            //                name: "IX_MeetingProjects_ProjectId",
            //                schema: "Meeting",
            //                table: "MeetingProjects",
            //                column: "ProjectId");

            //            migrationBuilder.CreateIndex(
            //                name: "IX_MeetingTopics_MeetingId",
            //                schema: "Meeting",
            //                table: "MeetingTopics",
            //                column: "MeetingId");

            //            migrationBuilder.CreateIndex(
            //                name: "IX_MeetingTopics_MinuteOfMeetingId",
            //                schema: "Meeting",
            //                table: "MeetingTopics",
            //                column: "MinuteOfMeetingId");

            //            migrationBuilder.CreateIndex(
            //                name: "IX_MeetingTopics_TopicTypeId",
            //                schema: "Meeting",
            //                table: "MeetingTopics",
            //                column: "TopicTypeId");

            //            migrationBuilder.CreateIndex(
            //                name: "IX_MinutesOfMeetings_MeetingId",
            //                schema: "Meeting",
            //                table: "MinutesOfMeetings",
            //                column: "MeetingId");

            //            migrationBuilder.CreateIndex(
            //                name: "IX_TopicComments_CommentId",
            //                schema: "Meeting",
            //                table: "TopicComments",
            //                column: "CommentId");

            //            migrationBuilder.CreateIndex(
            //                name: "IX_TopicComments_TopicId",
            //                schema: "Meeting",
            //                table: "TopicComments",
            //                column: "TopicId");

            //            migrationBuilder.CreateIndex(
            //                name: "IX_TopicPauseDates_TopicId",
            //                schema: "Meeting",
            //                table: "TopicPauseDates",
            //                column: "TopicId");

            //            migrationBuilder.AddForeignKey(
            //                name: "FK_Surveys_MeetingTopics_MeetingTopicId",
            //                schema: "Committe",
            //                table: "Surveys",
            //                column: "MeetingTopicId",
            //                principalSchema: "Meeting",
            //                principalTable: "MeetingTopics",
            //                principalColumn: "Id",
            //                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //            migrationBuilder.DropForeignKey(
            //                name: "FK_Surveys_MeetingTopics_MeetingTopicId",
            //                schema: "Committe",
            //                table: "Surveys");

            //            migrationBuilder.DropTable(
            //                name: "MeetingAttendee",
            //                schema: "Meeting");

            //            migrationBuilder.DropTable(
            //                name: "MeetingCoordinators",
            //                schema: "Meeting");

            //            migrationBuilder.DropTable(
            //                name: "MeetingDates",
            //                schema: "Meeting");

            //            migrationBuilder.DropTable(
            //                name: "MeetingLocations",
            //                schema: "Meeting");

            //            migrationBuilder.DropTable(
            //                name: "MeetingProjects",
            //                schema: "Meeting");

            //            migrationBuilder.DropTable(
            //                name: "TopicComments",
            //                schema: "Meeting");

            //            migrationBuilder.DropTable(
            //                name: "TopicPauseDates",
            //                schema: "Meeting");

            //            migrationBuilder.DropTable(
            //                name: "Projects",
            //                schema: "Meeting");

            //            migrationBuilder.DropTable(
            //                name: "MeetingTopics",
            //                schema: "Meeting");

            //            migrationBuilder.DropTable(
            //                name: "MinutesOfMeetings",
            //                schema: "Meeting");

            //            migrationBuilder.DropTable(
            //                name: "TopicTypes",
            //                schema: "Meeting");

            //            migrationBuilder.DropTable(
            //                name: "Meetings",
            //                schema: "Meeting");

            //            migrationBuilder.DropIndex(
            //                name: "IX_Surveys_MeetingTopicId",
            //                schema: "Committe",
            //                table: "Surveys");

            //            migrationBuilder.DropColumn(
            //                name: "MeetingTopicId",
            //                schema: "Committe",
            //                table: "Surveys");

            //            migrationBuilder.RenameTable(
            //                name: "CommiteeSavedAttachmentSavedAttachment",
            //                schema: "Committe",
            //                newName: "CommiteeSavedAttachmentSavedAttachment");

            //            migrationBuilder.AlterColumn<int>(
            //                name: "CommiteeId",
            //                schema: "Committe",
            //                table: "Surveys",
            //                type: "int",
            //                nullable: false,
            //                defaultValue: 0,
            //                oldClrType: typeof(int),
            //                oldType: "int",
            //                oldNullable: true);
        }
    }
}
