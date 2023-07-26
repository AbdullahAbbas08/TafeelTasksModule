using Microsoft.EntityFrameworkCore.Migrations;

namespace CommiteeAndMeetings.BLL.Migrations
{
    public partial class DbModification : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //            migrationBuilder.DropForeignKey(
            //                name: "FK_MeetingTopics_TopicTypes_TopicTypeId",
            //                schema: "Meeting",
            //                table: "MeetingTopics");

            //            migrationBuilder.DropTable(
            //                name: "MeetingDates",
            //                schema: "Meeting");

            //            migrationBuilder.DropTable(
            //                name: "MeetingLocations",
            //                schema: "Meeting");

            //            migrationBuilder.DropTable(
            //                name: "TopicTypes",
            //                schema: "Meeting");

            //            migrationBuilder.DropIndex(
            //                name: "IX_MeetingTopics_TopicTypeId",
            //                schema: "Meeting",
            //                table: "MeetingTopics");

            //            //migrationBuilder.RenameTable(
            //            //    name: "CommiteeSavedAttachmentSavedAttachment",
            //            //    newName: "CommiteeSavedAttachmentSavedAttachment",
            //            //    newSchema: "Committe");

            //            migrationBuilder.AddColumn<int>(
            //                name: "MinuteOfMeetingId",
            //                schema: "Committe",
            //                table: "Surveys",
            //                type: "int",
            //                nullable: true);

            //            migrationBuilder.AddColumn<string>(
            //                name: "Title",
            //                schema: "Meeting",
            //                table: "MinutesOfMeetings",
            //                type: "nvarchar(max)",
            //                nullable: true);

            //            migrationBuilder.AddColumn<int>(
            //                name: "TopicType",
            //                schema: "Meeting",
            //                table: "MeetingTopics",
            //                type: "int",
            //                nullable: false,
            //                defaultValue: 0);

            //            migrationBuilder.AddColumn<DateTime>(
            //                name: "Date",
            //                schema: "Meeting",
            //                table: "Meetings",
            //                type: "datetime2",
            //                nullable: false,
            //                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            //            migrationBuilder.AddColumn<DateTimeOffset>(
            //                name: "MeetingFromTime",
            //                schema: "Meeting",
            //                table: "Meetings",
            //                type: "datetimeoffset",
            //                nullable: false,
            //                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            //            migrationBuilder.AddColumn<int>(
            //                name: "MeetingHeaderAndFooterId",
            //                schema: "Meeting",
            //                table: "Meetings",
            //                type: "int",
            //                nullable: true);

            //            migrationBuilder.AddColumn<DateTimeOffset>(
            //                name: "MeetingToTime",
            //                schema: "Meeting",
            //                table: "Meetings",
            //                type: "datetimeoffset",
            //                nullable: false,
            //                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            //            migrationBuilder.AddColumn<string>(
            //                name: "PhysicalLocation",
            //                schema: "Meeting",
            //                table: "Meetings",
            //                type: "nvarchar(max)",
            //                nullable: true);

            //            migrationBuilder.AddColumn<int>(
            //                name: "ReferenceNumber",
            //                schema: "Meeting",
            //                table: "Meetings",
            //                type: "int",
            //                nullable: false,
            //                defaultValue: 0);

            //            migrationBuilder.AddColumn<bool>(
            //                name: "ConfirmeAttendance",
            //                schema: "Meeting",
            //                table: "MeetingCoordinators",
            //                type: "bit",
            //                nullable: false,
            //                defaultValue: false);

            //            migrationBuilder.AddColumn<bool>(
            //                name: "ConfirmeAttendance",
            //                schema: "Meeting",
            //                table: "MeetingAttendee",
            //                type: "bit",
            //                nullable: false,
            //                defaultValue: false);

            //            migrationBuilder.CreateTable(
            //                name: "MeetingHeaderAndFooters",
            //                schema: "Meeting",
            //                columns: table => new
            //                {
            //                    Id = table.Column<int>(type: "int", nullable: false)
            //                        .Annotation("SqlServer:Identity", "1, 1"),
            //                    Html = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //                    HeaderAndFooterType = table.Column<int>(type: "int", nullable: false),
            //                    CreatedBy = table.Column<int>(type: "int", nullable: true),
            //                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
            //                },
            //                constraints: table =>
            //                {
            //                    table.PrimaryKey("PK_MeetingHeaderAndFooters", x => x.Id);
            //                });

            //            migrationBuilder.CreateTable(
            //                name: "MeetingURls",
            //                schema: "Meeting",
            //                columns: table => new
            //                {
            //                    Id = table.Column<int>(type: "int", nullable: false)
            //                        .Annotation("SqlServer:Identity", "1, 1"),
            //                    OnlineUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //                    MeetingId = table.Column<int>(type: "int", nullable: false),
            //                    DeletedBy = table.Column<int>(type: "int", nullable: true),
            //                    DeletedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
            //                    CreatedBy = table.Column<int>(type: "int", nullable: true),
            //                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
            //                },
            //                constraints: table =>
            //                {
            //                    table.PrimaryKey("PK_MeetingURls", x => x.Id);
            //                    table.ForeignKey(
            //                        name: "FK_MeetingURls_Meetings_MeetingId",
            //                        column: x => x.MeetingId,
            //                        principalSchema: "Meeting",
            //                        principalTable: "Meetings",
            //                        principalColumn: "Id",
            //                        onDelete: ReferentialAction.Restrict);
            //                });

            //            migrationBuilder.CreateTable(
            //                name: "MOMComments",
            //                schema: "Meeting",
            //                columns: table => new
            //                {
            //                    Id = table.Column<int>(type: "int", nullable: false)
            //                        .Annotation("SqlServer:Identity", "1, 1"),
            //                    MinuteOfMeetingId = table.Column<int>(type: "int", nullable: false),
            //                    CommentId = table.Column<int>(type: "int", nullable: false),
            //                    CommentType = table.Column<int>(type: "int", nullable: false),
            //                    CreatedBy = table.Column<int>(type: "int", nullable: true),
            //                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
            //                },
            //                constraints: table =>
            //                {
            //                    table.PrimaryKey("PK_MOMComments", x => x.Id);
            //                    table.ForeignKey(
            //                        name: "FK_MOMComments_Comments_CommentId",
            //                        column: x => x.CommentId,
            //                        principalSchema: "Committe",
            //                        principalTable: "Comments",
            //                        principalColumn: "CommentId",
            //                        onDelete: ReferentialAction.Restrict);
            //                    table.ForeignKey(
            //                        name: "FK_MOMComments_MinutesOfMeetings_MinuteOfMeetingId",
            //                        column: x => x.MinuteOfMeetingId,
            //                        principalSchema: "Meeting",
            //                        principalTable: "MinutesOfMeetings",
            //                        principalColumn: "Id",
            //                        onDelete: ReferentialAction.Restrict);
            //                });

            //            migrationBuilder.CreateIndex(
            //                name: "IX_Surveys_MinuteOfMeetingId",
            //                schema: "Committe",
            //                table: "Surveys",
            //                column: "MinuteOfMeetingId");

            //            migrationBuilder.CreateIndex(
            //                name: "IX_Meetings_MeetingHeaderAndFooterId",
            //                schema: "Meeting",
            //                table: "Meetings",
            //                column: "MeetingHeaderAndFooterId");

            //            migrationBuilder.CreateIndex(
            //                name: "IX_MeetingURls_MeetingId",
            //                schema: "Meeting",
            //                table: "MeetingURls",
            //                column: "MeetingId");

            //            migrationBuilder.CreateIndex(
            //                name: "IX_MOMComments_CommentId",
            //                schema: "Meeting",
            //                table: "MOMComments",
            //                column: "CommentId");

            //            migrationBuilder.CreateIndex(
            //                name: "IX_MOMComments_MinuteOfMeetingId",
            //                schema: "Meeting",
            //                table: "MOMComments",
            //                column: "MinuteOfMeetingId");

            //            migrationBuilder.AddForeignKey(
            //                name: "FK_Meetings_MeetingHeaderAndFooters_MeetingHeaderAndFooterId",
            //                schema: "Meeting",
            //                table: "Meetings",
            //                column: "MeetingHeaderAndFooterId",
            //                principalSchema: "Meeting",
            //                principalTable: "MeetingHeaderAndFooters",
            //                principalColumn: "Id",
            //                onDelete: ReferentialAction.Restrict);

            //            migrationBuilder.AddForeignKey(
            //                name: "FK_Surveys_MinutesOfMeetings_MinuteOfMeetingId",
            //                schema: "Committe",
            //                table: "Surveys",
            //                column: "MinuteOfMeetingId",
            //                principalSchema: "Meeting",
            //                principalTable: "MinutesOfMeetings",
            //                principalColumn: "Id",
            //                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //            migrationBuilder.DropForeignKey(
            //                name: "FK_Meetings_MeetingHeaderAndFooters_MeetingHeaderAndFooterId",
            //                schema: "Meeting",
            //                table: "Meetings");

            //            migrationBuilder.DropForeignKey(
            //                name: "FK_Surveys_MinutesOfMeetings_MinuteOfMeetingId",
            //                schema: "Committe",
            //                table: "Surveys");

            //            migrationBuilder.DropTable(
            //                name: "MeetingHeaderAndFooters",
            //                schema: "Meeting");

            //            migrationBuilder.DropTable(
            //                name: "MeetingURls",
            //                schema: "Meeting");

            //            migrationBuilder.DropTable(
            //                name: "MOMComments",
            //                schema: "Meeting");

            //            migrationBuilder.DropIndex(
            //                name: "IX_Surveys_MinuteOfMeetingId",
            //                schema: "Committe",
            //                table: "Surveys");

            //            migrationBuilder.DropIndex(
            //                name: "IX_Meetings_MeetingHeaderAndFooterId",
            //                schema: "Meeting",
            //                table: "Meetings");

            //            migrationBuilder.DropColumn(
            //                name: "MinuteOfMeetingId",
            //                schema: "Committe",
            //                table: "Surveys");

            //            migrationBuilder.DropColumn(
            //                name: "Title",
            //                schema: "Meeting",
            //                table: "MinutesOfMeetings");

            //            migrationBuilder.DropColumn(
            //                name: "TopicType",
            //                schema: "Meeting",
            //                table: "MeetingTopics");

            //            migrationBuilder.DropColumn(
            //                name: "Date",
            //                schema: "Meeting",
            //                table: "Meetings");

            //            migrationBuilder.DropColumn(
            //                name: "MeetingFromTime",
            //                schema: "Meeting",
            //                table: "Meetings");

            //            migrationBuilder.DropColumn(
            //                name: "MeetingHeaderAndFooterId",
            //                schema: "Meeting",
            //                table: "Meetings");

            //            migrationBuilder.DropColumn(
            //                name: "MeetingToTime",
            //                schema: "Meeting",
            //                table: "Meetings");

            //            migrationBuilder.DropColumn(
            //                name: "PhysicalLocation",
            //                schema: "Meeting",
            //                table: "Meetings");

            //            migrationBuilder.DropColumn(
            //                name: "ReferenceNumber",
            //                schema: "Meeting",
            //                table: "Meetings");

            //            migrationBuilder.DropColumn(
            //                name: "ConfirmeAttendance",
            //                schema: "Meeting",
            //                table: "MeetingCoordinators");

            //            migrationBuilder.DropColumn(
            //                name: "ConfirmeAttendance",
            //                schema: "Meeting",
            //                table: "MeetingAttendee");

            //            migrationBuilder.RenameTable(
            //                name: "CommiteeSavedAttachmentSavedAttachment",
            //                schema: "Committe",
            //                newName: "CommiteeSavedAttachmentSavedAttachment");

            //            migrationBuilder.CreateTable(
            //                name: "MeetingDates",
            //                schema: "Meeting",
            //                columns: table => new
            //                {
            //                    Id = table.Column<int>(type: "int", nullable: false)
            //                        .Annotation("SqlServer:Identity", "1, 1"),
            //                    CreatedBy = table.Column<int>(type: "int", nullable: true),
            //                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
            //                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
            //                    MeetingFromTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
            //                    MeetingId = table.Column<int>(type: "int", nullable: false),
            //                    MeetingState = table.Column<int>(type: "int", nullable: false),
            //                    MeetingToTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
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
            //                    CreatedBy = table.Column<int>(type: "int", nullable: true),
            //                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
            //                    DeletedBy = table.Column<int>(type: "int", nullable: true),
            //                    DeletedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
            //                    MeetingId = table.Column<int>(type: "int", nullable: false),
            //                    OnlineUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //                    PhysicalLocation = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
            //                name: "TopicTypes",
            //                schema: "Meeting",
            //                columns: table => new
            //                {
            //                    Id = table.Column<int>(type: "int", nullable: false)
            //                        .Annotation("SqlServer:Identity", "1, 1"),
            //                    CreatedBy = table.Column<int>(type: "int", nullable: true),
            //                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
            //                    DeletedBy = table.Column<int>(type: "int", nullable: true),
            //                    DeletedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
            //                    TopicTypeNameAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //                    TopicTypeNameEn = table.Column<string>(type: "nvarchar(max)", nullable: true)
            //                },
            //                constraints: table =>
            //                {
            //                    table.PrimaryKey("PK_TopicTypes", x => x.Id);
            //                });

            //            migrationBuilder.CreateIndex(
            //                name: "IX_MeetingTopics_TopicTypeId",
            //                schema: "Meeting",
            //                table: "MeetingTopics",
            //                column: "TopicTypeId");

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

            //            migrationBuilder.AddForeignKey(
            //                name: "FK_MeetingTopics_TopicTypes_TopicTypeId",
            //                schema: "Meeting",
            //                table: "MeetingTopics",
            //                column: "TopicTypeId",
            //                principalSchema: "Meeting",
            //                principalTable: "TopicTypes",
            //                principalColumn: "Id",
            //                onDelete: ReferentialAction.Restrict);
        }
    }
}
