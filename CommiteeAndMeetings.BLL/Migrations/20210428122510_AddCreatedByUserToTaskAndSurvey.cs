using Microsoft.EntityFrameworkCore.Migrations;

namespace CommiteeAndMeetings.BLL.Migrations
{
    public partial class AddCreatedByUserToTaskAndSurvey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_Annotations_Attachment_AttachmentId",
            //    table: "Annotations");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_Attachment_Attachment_ReferenceAttachmentId",
            //    table: "Attachment");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_Attachment_AttachmentTypes_AttachmentTypeId",
            //    table: "Attachment");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_Attachment_PhysicalAttachmentTypes_PhysicalAttachmentTypeId",
            //    table: "Attachment");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_Attachment_Users_CreatedBy",
            //    table: "Attachment");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_Attachment_Users_UpdatedBy",
            //    table: "Attachment");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_AttachmentTags_Attachment_AttachmentId",
            //    table: "AttachmentTags");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_AttachmentVersions_Attachment_AttachmentId",
            //    table: "AttachmentVersions");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_ChatMessages_Attachment_AttachmentId",
            //    table: "ChatMessages");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_DeliveryCorrespondentTransactions_Attachment_AttachmentId",
            //    table: "DeliveryCorrespondentTransactions");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_DeliverySheetAttachments_Attachment_AttachmentId",
            //    table: "DeliverySheetAttachments");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_TransactionAttachments_Attachment_AttachmentId",
            //    table: "TransactionAttachments");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_Transactions_Commitees_CommiteeId",
            //    table: "Transactions");

            //migrationBuilder.DropIndex(
            //    name: "IX_Transactions_CommiteeId",
            //    table: "Transactions");

            //migrationBuilder.DropPrimaryKey(
            //    name: "PK_Attachment",
            //    table: "Attachment");

            //migrationBuilder.DropColumn(
            //    name: "CommiteeId",
            //    table: "Transactions");

            //migrationBuilder.RenameTable(
            //    name: "CommiteeSavedAttachmentSavedAttachment",
            //    newName: "CommiteeSavedAttachmentSavedAttachment",
            //    newSchema: "Committe");

            //migrationBuilder.RenameTable(
            //    name: "Attachment",
            //    newName: "Attachments");

            //migrationBuilder.AlterColumn<string>(
            //    name: "Code",
            //    table: "Vw_OrganizationsToReferral",
            //    type: "nvarchar(max)",
            //    nullable: true,
            //    oldClrType: typeof(long),
            //    oldType: "bigint");

            //migrationBuilder.AddColumn<int>(
            //    name: "CreatedBy",
            //    schema: "Committe",
            //    table: "TaskComments",
            //    type: "int",
            //    nullable: true);

            //migrationBuilder.AddColumn<DateTimeOffset>(
            //    name: "CreatedOn",
            //    schema: "Committe",
            //    table: "TaskComments",
            //    type: "datetimeoffset",
            //    nullable: true);

            //migrationBuilder.AddColumn<int>(
            //    name: "UpdatedBy",
            //    schema: "Committe",
            //    table: "TaskComments",
            //    type: "int",
            //    nullable: true);

            //migrationBuilder.AddColumn<DateTimeOffset>(
            //    name: "UpdatedOn",
            //    schema: "Committe",
            //    table: "TaskComments",
            //    type: "datetimeoffset",
            //    nullable: true);

            //migrationBuilder.AddColumn<int>(
            //    name: "CreatedBy",
            //    schema: "Committe",
            //    table: "SurveyComments",
            //    type: "int",
            //    nullable: true);

            //migrationBuilder.AddColumn<DateTimeOffset>(
            //    name: "CreatedOn",
            //    schema: "Committe",
            //    table: "SurveyComments",
            //    type: "datetimeoffset",
            //    nullable: true);

            //migrationBuilder.AddColumn<int>(
            //    name: "UpdatedBy",
            //    schema: "Committe",
            //    table: "SurveyComments",
            //    type: "int",
            //    nullable: true);

            //migrationBuilder.AddColumn<DateTimeOffset>(
            //    name: "UpdatedOn",
            //    schema: "Committe",
            //    table: "SurveyComments",
            //    type: "datetimeoffset",
            //    nullable: true);

            //migrationBuilder.AlterColumn<int>(
            //    name: "CurrenHeadUnitId",
            //    schema: "Committe",
            //    table: "Commitees",
            //    type: "int",
            //    nullable: true,
            //    oldClrType: typeof(int),
            //    oldType: "int");

            //migrationBuilder.AddPrimaryKey(
            //    name: "PK_Attachments",
            //    table: "Attachments",
            //    column: "AttachmentId");

            //migrationBuilder.CreateTable(
            //    name: "attachmentsViews",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        AttachmentId = table.Column<int>(type: "int", nullable: false),
            //        TransactionAttachmentId = table.Column<int>(type: "int", nullable: false),
            //        TransactionId = table.Column<long>(type: "bigint", nullable: false),
            //        AttachmentName = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        AttachmentTypeId = table.Column<int>(type: "int", nullable: false),
            //        OriginalName = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        MimeType = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        Size = table.Column<int>(type: "int", nullable: true),
            //        Width = table.Column<int>(type: "int", nullable: true),
            //        Height = table.Column<int>(type: "int", nullable: true),
            //        LFEntryId = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        PagesCount = table.Column<int>(type: "int", nullable: true),
            //        Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        PhysicalAttachmentTypeNameAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        PhysicalAttachmentTypeNameEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        PhysicalAttachmentTypeNameFn = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        AttachmentTypeCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        AttachmentTypeNameAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        AttachmentTypeNameEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        AttachmentTypeNameFn = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        IsShared = table.Column<bool>(type: "bit", nullable: false),
            //        CreatedBy = table.Column<int>(type: "int", nullable: true),
            //        CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
            //        CreatedByFullNameEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        CreatedByFullNameAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        CreatedByFullNameFn = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        ReferenceAttachmentId = table.Column<int>(type: "int", nullable: true),
            //        FromRelatedTransaction = table.Column<bool>(type: "bit", nullable: false),
            //        OrgnazationNameAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        OrgnazationNameEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        OrgnazationNameFn = table.Column<string>(type: "nvarchar(max)", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_attachmentsViews", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "COUNTS",
            //    columns: table => new
            //    {
            //        Id = table.Column<long>(type: "bigint", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        CNT = table.Column<long>(type: "bigint", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_COUNTS", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "TransactionByTypeReportDTO",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        TransactionTypeAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        TransactionTypeEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        TransactionTypeFn = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        Count = table.Column<int>(type: "int", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_TransactionByTypeReportDTO", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "ViewTransactionsView",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        TransactionId = table.Column<long>(type: "bigint", nullable: false),
            //        ActionId = table.Column<int>(type: "int", nullable: false),
            //        TransactionActionId = table.Column<int>(type: "int", nullable: false),
            //        CorrespondentUserId = table.Column<int>(type: "int", nullable: true),
            //        FromUserNameAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        FromUserNameEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        FromUserNameFn = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        DirectedToOrganizationId = table.Column<int>(type: "int", nullable: true),
            //        DirectedToUserId = table.Column<int>(type: "int", nullable: true),
            //        RequiredActionId = table.Column<int>(type: "int", nullable: true),
            //        RequiredActionNameAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        RequiredActionNameEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        RequiredActionNameFn = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        RecipientStatusId = table.Column<int>(type: "int", nullable: true),
            //        RecipientStatusNameAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        RecipientStatusNameEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        RecipientStatusNameFn = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        transactionStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        FromUserId = table.Column<int>(type: "int", nullable: true),
            //        FromOrganizationId = table.Column<int>(type: "int", nullable: true),
            //        IsCC = table.Column<bool>(type: "bit", nullable: false),
            //        IsCancelled = table.Column<bool>(type: "bit", nullable: true),
            //        ConfidentialityLevelId = table.Column<int>(type: "int", nullable: true),
            //        ConfidentialityLevelNameAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        ConfidentialityLevelNameEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        ConfidentialityLevelNameFn = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        transactionTypeId = table.Column<int>(type: "int", nullable: true),
            //        transactionTypeNameAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        transactionTypeNameEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        transactionTypeNameFn = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        IsUrgent = table.Column<bool>(type: "bit", nullable: false),
            //        IsConfidential = table.Column<bool>(type: "bit", nullable: false),
            //        TransactionActionRecipientId = table.Column<int>(type: "int", nullable: false),
            //        TransactionNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        Subject = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        SubjectEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        SubjectFn = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        createdOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
            //        TransactionActionCreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
            //        ProfileImage = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
            //        LetterCount = table.Column<int>(type: "int", nullable: false),
            //        DocumentCount = table.Column<int>(type: "int", nullable: false),
            //        PhysicalCount = table.Column<int>(type: "int", nullable: false),
            //        ActionLetterCount = table.Column<int>(type: "int", nullable: false),
            //        ActionDocumentCount = table.Column<int>(type: "int", nullable: false),
            //        ActionPhysicalCount = table.Column<int>(type: "int", nullable: false),
            //        RelatedTransactionsCount = table.Column<int>(type: "int", nullable: false),
            //        ImportanceLevelId = table.Column<int>(type: "int", nullable: true),
            //        FromOrganizationNameAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        FromOrganizationNameEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        FromOrganizationNameFn = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        ImportanceLevelNameAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        ImportanceLevelNameEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        ImportanceLevelNameFn = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        NormalizedSubject = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        IsLate = table.Column<int>(type: "int", nullable: false),
            //        IndivnidualsCounts = table.Column<int>(type: "int", nullable: false),
            //        ImportanceLevelColor = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        archieveDate = table.Column<int>(type: "int", nullable: true),
            //        TransactionNumberFormatted = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        ExecutionDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
            //        CorrespondingUserNameAR = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        CorrespondingUserNameEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        CorrespondingUserNameFn = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        ToOrganizationNameAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        ToOrganizationNameEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        ToOrganizationNameFn = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        ToUserFullNameAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        ToUserFullNameEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        ToUserFullNameFn = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        ToOrganizatioIsOuterOrganization = table.Column<bool>(type: "bit", nullable: true),
            //        IncomingOrganizationId = table.Column<int>(type: "int", nullable: true),
            //        IncomingLetterDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
            //        Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        ExecutionPeriod = table.Column<int>(type: "int", nullable: true),
            //        secretSubject = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        ReferrerTransactionActionId = table.Column<int>(type: "int", nullable: true),
            //        hasCorrespondence = table.Column<int>(type: "int", nullable: true),
            //        IsFinished = table.Column<bool>(type: "bit", nullable: false),
            //        IsShowAllTime = table.Column<bool>(type: "bit", nullable: false),
            //        FollowUPFinishedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_ViewTransactionsView", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Vw_TransactionBoxes_Inbox",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        TransactionId = table.Column<long>(type: "bigint", nullable: false),
            //        ActionId = table.Column<int>(type: "int", nullable: false),
            //        TransactionActionId = table.Column<int>(type: "int", nullable: false),
            //        CorrespondentUserId = table.Column<int>(type: "int", nullable: true),
            //        FromUserNameAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        FromUserNameEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        FromUserNameFn = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        DirectedToOrganizationId = table.Column<int>(type: "int", nullable: true),
            //        DirectedToUserId = table.Column<int>(type: "int", nullable: true),
            //        RequiredActionId = table.Column<int>(type: "int", nullable: true),
            //        RequiredActionNameAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        RequiredActionNameEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        RequiredActionNameFn = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        RecipientStatusId = table.Column<int>(type: "int", nullable: true),
            //        RecipientStatusNameAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        RecipientStatusNameEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        RecipientStatusNameFn = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        transactionStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        FromUserId = table.Column<int>(type: "int", nullable: true),
            //        FromOrganizationId = table.Column<int>(type: "int", nullable: true),
            //        IsCC = table.Column<bool>(type: "bit", nullable: false),
            //        ConfidentialityLevelId = table.Column<int>(type: "int", nullable: true),
            //        ConfidentialityLevelNameAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        ConfidentialityLevelNameEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        ConfidentialityLevelNameFn = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        transactionTypeId = table.Column<int>(type: "int", nullable: true),
            //        transactionTypeNameAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        transactionTypeNameEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        transactionTypeNameFn = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        IsUrgent = table.Column<bool>(type: "bit", nullable: false),
            //        IsConfidential = table.Column<bool>(type: "bit", nullable: false),
            //        TransactionActionRecipientId = table.Column<int>(type: "int", nullable: false),
            //        TransactionNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        Subject = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        SubjectEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        SubjectFn = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        createdOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
            //        TransactionActionCreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
            //        ProfileImage = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
            //        LetterCount = table.Column<int>(type: "int", nullable: false),
            //        DocumentCount = table.Column<int>(type: "int", nullable: false),
            //        PhysicalCount = table.Column<int>(type: "int", nullable: false),
            //        ActionLetterCount = table.Column<int>(type: "int", nullable: false),
            //        ActionDocumentCount = table.Column<int>(type: "int", nullable: false),
            //        ActionPhysicalCount = table.Column<int>(type: "int", nullable: false),
            //        RelatedTransactionsCount = table.Column<int>(type: "int", nullable: false),
            //        ImportanceLevelId = table.Column<int>(type: "int", nullable: true),
            //        FromOrganizationNameAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        FromOrganizationNameEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        FromOrganizationNameFn = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        ImportanceLevelNameAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        ImportanceLevelNameEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        ImportanceLevelNameFn = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        NormalizedSubject = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        IsLate = table.Column<int>(type: "int", nullable: false),
            //        IndivnidualsCounts = table.Column<int>(type: "int", nullable: false),
            //        ImportanceLevelColor = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        archieveDate = table.Column<int>(type: "int", nullable: true),
            //        TransactionNumberFormatted = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        ExecutionDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
            //        CorrespondingUserNameAR = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        CorrespondingUserNameEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        CorrespondingUserNameFn = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        ToOrganizationNameAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        ToOrganizationNameEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        ToOrganizationNameFn = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        ToUserFullNameAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        ToUserFullNameEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        ToUserFullNameFn = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        ToOrganizatioIsOuterOrganization = table.Column<bool>(type: "bit", nullable: true),
            //        hasCorrespondence = table.Column<int>(type: "int", nullable: true),
            //        IncomingOrganizationId = table.Column<int>(type: "int", nullable: true),
            //        IncomingLetterDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
            //        secretSubject = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        incomingOrganizationNameAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        incomingOrganizationNameEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        incomingOrganizationNameFn = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        ExecutionPeriod = table.Column<int>(type: "int", nullable: true),
            //        IsFinished = table.Column<bool>(type: "bit", nullable: false),
            //        FollowUPFinishedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
            //        IsPrinted = table.Column<bool>(type: "bit", nullable: false),
            //        CreatedByUserRoleId = table.Column<int>(type: "int", nullable: false),
            //        CreatedUserRoleOrganizationNameAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        CreatedUserRoleOrganizationNameEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        CreatedUserRoleOrganizationNameFn = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        IsShowAllTime = table.Column<bool>(type: "bit", nullable: false),
            //        IsRejected = table.Column<int>(type: "int", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Vw_TransactionBoxes_Inbox", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Vw_UsersToReferral",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        Username = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        FullNameAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        FullNameEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        FullNameFn = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        ProfileImageFileId = table.Column<int>(type: "int", nullable: true),
            //        NotificationByMail = table.Column<bool>(type: "bit", nullable: false),
            //        NotificationBySMS = table.Column<bool>(type: "bit", nullable: false),
            //        OrganizationId = table.Column<int>(type: "int", nullable: false),
            //        OrganizationNameAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        OrganizationNameEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        OrganizationNameFn = table.Column<string>(type: "nvarchar(max)", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Vw_UsersToReferral", x => x.Id);
            //    });

            //migrationBuilder.CreateIndex(
            //    name: "IX_TaskComments_CreatedBy",
            //    schema: "Committe",
            //    table: "TaskComments",
            //    column: "CreatedBy");

            //migrationBuilder.CreateIndex(
            //    name: "IX_SurveyComments_CreatedBy",
            //    schema: "Committe",
            //    table: "SurveyComments",
            //    column: "CreatedBy");

            //migrationBuilder.CreateIndex(
            //    name: "IX_SavedAttachments_CreatedBy",
            //    schema: "Committe",
            //    table: "SavedAttachments",
            //    column: "CreatedBy");

            //migrationBuilder.CreateIndex(
            //    name: "IX_CommiteeTasks_CreatedBy",
            //    schema: "Committe",
            //    table: "CommiteeTasks",
            //    column: "CreatedBy");

            //migrationBuilder.CreateIndex(
            //    name: "IX_CommiteeSavedAttachments_CreatedBy",
            //    schema: "Committe",
            //    table: "CommiteeSavedAttachments",
            //    column: "CreatedBy");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Comments_CreatedBy",
            //    schema: "Committe",
            //    table: "Comments",
            //    column: "CreatedBy");

            //migrationBuilder.CreateIndex(
            //    name: "IX_AttachmentUsers_CreatedBy",
            //    schema: "Committe",
            //    table: "AttachmentUsers",
            //    column: "CreatedBy");

            //migrationBuilder.CreateIndex(
            //    name: "IX_AttachmentComments_CreatedBy",
            //    schema: "Committe",
            //    table: "AttachmentComments",
            //    column: "CreatedBy");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Annotations_Attachments_AttachmentId",
            //    table: "Annotations",
            //    column: "AttachmentId",
            //    principalTable: "Attachments",
            //    principalColumn: "AttachmentId",
            //    onDelete: ReferentialAction.Restrict);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_AttachmentComments_Users_CreatedBy",
            //    schema: "Committe",
            //    table: "AttachmentComments",
            //    column: "CreatedBy",
            //    principalTable: "Users",
            //    principalColumn: "UserId",
            //    onDelete: ReferentialAction.Restrict);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Attachments_Attachments_ReferenceAttachmentId",
            //    table: "Attachments",
            //    column: "ReferenceAttachmentId",
            //    principalTable: "Attachments",
            //    principalColumn: "AttachmentId",
            //    onDelete: ReferentialAction.Restrict);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Attachments_AttachmentTypes_AttachmentTypeId",
            //    table: "Attachments",
            //    column: "AttachmentTypeId",
            //    principalTable: "AttachmentTypes",
            //    principalColumn: "AttachmentTypeId",
            //    onDelete: ReferentialAction.Restrict);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Attachments_PhysicalAttachmentTypes_PhysicalAttachmentTypeId",
            //    table: "Attachments",
            //    column: "PhysicalAttachmentTypeId",
            //    principalTable: "PhysicalAttachmentTypes",
            //    principalColumn: "PhysicalAttachmentTypeId",
            //    onDelete: ReferentialAction.Restrict);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Attachments_Users_CreatedBy",
            //    table: "Attachments",
            //    column: "CreatedBy",
            //    principalTable: "Users",
            //    principalColumn: "UserId",
            //    onDelete: ReferentialAction.Restrict);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Attachments_Users_UpdatedBy",
            //    table: "Attachments",
            //    column: "UpdatedBy",
            //    principalTable: "Users",
            //    principalColumn: "UserId",
            //    onDelete: ReferentialAction.Restrict);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_AttachmentTags_Attachments_AttachmentId",
            //    table: "AttachmentTags",
            //    column: "AttachmentId",
            //    principalTable: "Attachments",
            //    principalColumn: "AttachmentId",
            //    onDelete: ReferentialAction.Restrict);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_AttachmentUsers_Users_CreatedBy",
            //    schema: "Committe",
            //    table: "AttachmentUsers",
            //    column: "CreatedBy",
            //    principalTable: "Users",
            //    principalColumn: "UserId",
            //    onDelete: ReferentialAction.Restrict);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_AttachmentVersions_Attachments_AttachmentId",
            //    table: "AttachmentVersions",
            //    column: "AttachmentId",
            //    principalTable: "Attachments",
            //    principalColumn: "AttachmentId",
            //    onDelete: ReferentialAction.Restrict);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_ChatMessages_Attachments_AttachmentId",
            //    table: "ChatMessages",
            //    column: "AttachmentId",
            //    principalTable: "Attachments",
            //    principalColumn: "AttachmentId",
            //    onDelete: ReferentialAction.Restrict);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Comments_Users_CreatedBy",
            //    schema: "Committe",
            //    table: "Comments",
            //    column: "CreatedBy",
            //    principalTable: "Users",
            //    principalColumn: "UserId",
            //    onDelete: ReferentialAction.Restrict);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_CommiteeSavedAttachments_Users_CreatedBy",
            //    schema: "Committe",
            //    table: "CommiteeSavedAttachments",
            //    column: "CreatedBy",
            //    principalTable: "Users",
            //    principalColumn: "UserId",
            //    onDelete: ReferentialAction.Restrict);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_CommiteeTasks_Users_CreatedBy",
            //    schema: "Committe",
            //    table: "CommiteeTasks",
            //    column: "CreatedBy",
            //    principalTable: "Users",
            //    principalColumn: "UserId",
            //    onDelete: ReferentialAction.Restrict);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_DeliveryCorrespondentTransactions_Attachments_AttachmentId",
            //    table: "DeliveryCorrespondentTransactions",
            //    column: "AttachmentId",
            //    principalTable: "Attachments",
            //    principalColumn: "AttachmentId",
            //    onDelete: ReferentialAction.Restrict);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_DeliverySheetAttachments_Attachments_AttachmentId",
            //    table: "DeliverySheetAttachments",
            //    column: "AttachmentId",
            //    principalTable: "Attachments",
            //    principalColumn: "AttachmentId",
            //    onDelete: ReferentialAction.Restrict);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_SavedAttachments_Users_CreatedBy",
            //    schema: "Committe",
            //    table: "SavedAttachments",
            //    column: "CreatedBy",
            //    principalTable: "Users",
            //    principalColumn: "UserId",
            //    onDelete: ReferentialAction.Restrict);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_SurveyComments_Users_CreatedBy",
            //    schema: "Committe",
            //    table: "SurveyComments",
            //    column: "CreatedBy",
            //    principalTable: "Users",
            //    principalColumn: "UserId",
            //    onDelete: ReferentialAction.Restrict);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_TaskComments_Users_CreatedBy",
            //    schema: "Committe",
            //    table: "TaskComments",
            //    column: "CreatedBy",
            //    principalTable: "Users",
            //    principalColumn: "UserId",
            //    onDelete: ReferentialAction.Restrict);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_TransactionAttachments_Attachments_AttachmentId",
            //    table: "TransactionAttachments",
            //    column: "AttachmentId",
            //    principalTable: "Attachments",
            //    principalColumn: "AttachmentId",
            //    onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Annotations_Attachments_AttachmentId",
                table: "Annotations");

            migrationBuilder.DropForeignKey(
                name: "FK_AttachmentComments_Users_CreatedBy",
                schema: "Committe",
                table: "AttachmentComments");

            migrationBuilder.DropForeignKey(
                name: "FK_Attachments_Attachments_ReferenceAttachmentId",
                table: "Attachments");

            migrationBuilder.DropForeignKey(
                name: "FK_Attachments_AttachmentTypes_AttachmentTypeId",
                table: "Attachments");

            migrationBuilder.DropForeignKey(
                name: "FK_Attachments_PhysicalAttachmentTypes_PhysicalAttachmentTypeId",
                table: "Attachments");

            migrationBuilder.DropForeignKey(
                name: "FK_Attachments_Users_CreatedBy",
                table: "Attachments");

            migrationBuilder.DropForeignKey(
                name: "FK_Attachments_Users_UpdatedBy",
                table: "Attachments");

            migrationBuilder.DropForeignKey(
                name: "FK_AttachmentTags_Attachments_AttachmentId",
                table: "AttachmentTags");

            migrationBuilder.DropForeignKey(
                name: "FK_AttachmentUsers_Users_CreatedBy",
                schema: "Committe",
                table: "AttachmentUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AttachmentVersions_Attachments_AttachmentId",
                table: "AttachmentVersions");

            migrationBuilder.DropForeignKey(
                name: "FK_ChatMessages_Attachments_AttachmentId",
                table: "ChatMessages");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Users_CreatedBy",
                schema: "Committe",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_CommiteeSavedAttachments_Users_CreatedBy",
                schema: "Committe",
                table: "CommiteeSavedAttachments");

            migrationBuilder.DropForeignKey(
                name: "FK_CommiteeTasks_Users_CreatedBy",
                schema: "Committe",
                table: "CommiteeTasks");

            migrationBuilder.DropForeignKey(
                name: "FK_DeliveryCorrespondentTransactions_Attachments_AttachmentId",
                table: "DeliveryCorrespondentTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_DeliverySheetAttachments_Attachments_AttachmentId",
                table: "DeliverySheetAttachments");

            migrationBuilder.DropForeignKey(
                name: "FK_SavedAttachments_Users_CreatedBy",
                schema: "Committe",
                table: "SavedAttachments");

            migrationBuilder.DropForeignKey(
                name: "FK_SurveyComments_Users_CreatedBy",
                schema: "Committe",
                table: "SurveyComments");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskComments_Users_CreatedBy",
                schema: "Committe",
                table: "TaskComments");

            migrationBuilder.DropForeignKey(
                name: "FK_TransactionAttachments_Attachments_AttachmentId",
                table: "TransactionAttachments");

            migrationBuilder.DropTable(
                name: "attachmentsViews");

            migrationBuilder.DropTable(
                name: "COUNTS");

            migrationBuilder.DropTable(
                name: "TransactionByTypeReportDTO");

            migrationBuilder.DropTable(
                name: "ViewTransactionsView");

            migrationBuilder.DropTable(
                name: "Vw_TransactionBoxes_Inbox");

            migrationBuilder.DropTable(
                name: "Vw_UsersToReferral");

            migrationBuilder.DropIndex(
                name: "IX_TaskComments_CreatedBy",
                schema: "Committe",
                table: "TaskComments");

            migrationBuilder.DropIndex(
                name: "IX_SurveyComments_CreatedBy",
                schema: "Committe",
                table: "SurveyComments");

            migrationBuilder.DropIndex(
                name: "IX_SavedAttachments_CreatedBy",
                schema: "Committe",
                table: "SavedAttachments");

            migrationBuilder.DropIndex(
                name: "IX_CommiteeTasks_CreatedBy",
                schema: "Committe",
                table: "CommiteeTasks");

            migrationBuilder.DropIndex(
                name: "IX_CommiteeSavedAttachments_CreatedBy",
                schema: "Committe",
                table: "CommiteeSavedAttachments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_CreatedBy",
                schema: "Committe",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_AttachmentUsers_CreatedBy",
                schema: "Committe",
                table: "AttachmentUsers");

            migrationBuilder.DropIndex(
                name: "IX_AttachmentComments_CreatedBy",
                schema: "Committe",
                table: "AttachmentComments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Attachments",
                table: "Attachments");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                schema: "Committe",
                table: "TaskComments");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                schema: "Committe",
                table: "TaskComments");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                schema: "Committe",
                table: "TaskComments");

            migrationBuilder.DropColumn(
                name: "UpdatedOn",
                schema: "Committe",
                table: "TaskComments");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                schema: "Committe",
                table: "SurveyComments");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                schema: "Committe",
                table: "SurveyComments");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                schema: "Committe",
                table: "SurveyComments");

            migrationBuilder.DropColumn(
                name: "UpdatedOn",
                schema: "Committe",
                table: "SurveyComments");

            migrationBuilder.RenameTable(
                name: "CommiteeSavedAttachmentSavedAttachment",
                schema: "Committe",
                newName: "CommiteeSavedAttachmentSavedAttachment");

            migrationBuilder.RenameTable(
                name: "Attachments",
                newName: "Attachment");

            migrationBuilder.AlterColumn<long>(
                name: "Code",
                table: "Vw_OrganizationsToReferral",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CommiteeId",
                table: "Transactions",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CurrenHeadUnitId",
                schema: "Committe",
                table: "Commitees",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Attachment",
                table: "Attachment",
                column: "AttachmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_CommiteeId",
                table: "Transactions",
                column: "CommiteeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Annotations_Attachment_AttachmentId",
                table: "Annotations",
                column: "AttachmentId",
                principalTable: "Attachment",
                principalColumn: "AttachmentId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Attachment_Attachment_ReferenceAttachmentId",
                table: "Attachment",
                column: "ReferenceAttachmentId",
                principalTable: "Attachment",
                principalColumn: "AttachmentId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Attachment_AttachmentTypes_AttachmentTypeId",
                table: "Attachment",
                column: "AttachmentTypeId",
                principalTable: "AttachmentTypes",
                principalColumn: "AttachmentTypeId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Attachment_PhysicalAttachmentTypes_PhysicalAttachmentTypeId",
                table: "Attachment",
                column: "PhysicalAttachmentTypeId",
                principalTable: "PhysicalAttachmentTypes",
                principalColumn: "PhysicalAttachmentTypeId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Attachment_Users_CreatedBy",
                table: "Attachment",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Attachment_Users_UpdatedBy",
                table: "Attachment",
                column: "UpdatedBy",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AttachmentTags_Attachment_AttachmentId",
                table: "AttachmentTags",
                column: "AttachmentId",
                principalTable: "Attachment",
                principalColumn: "AttachmentId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AttachmentVersions_Attachment_AttachmentId",
                table: "AttachmentVersions",
                column: "AttachmentId",
                principalTable: "Attachment",
                principalColumn: "AttachmentId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ChatMessages_Attachment_AttachmentId",
                table: "ChatMessages",
                column: "AttachmentId",
                principalTable: "Attachment",
                principalColumn: "AttachmentId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DeliveryCorrespondentTransactions_Attachment_AttachmentId",
                table: "DeliveryCorrespondentTransactions",
                column: "AttachmentId",
                principalTable: "Attachment",
                principalColumn: "AttachmentId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DeliverySheetAttachments_Attachment_AttachmentId",
                table: "DeliverySheetAttachments",
                column: "AttachmentId",
                principalTable: "Attachment",
                principalColumn: "AttachmentId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionAttachments_Attachment_AttachmentId",
                table: "TransactionAttachments",
                column: "AttachmentId",
                principalTable: "Attachment",
                principalColumn: "AttachmentId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Commitees_CommiteeId",
                table: "Transactions",
                column: "CommiteeId",
                principalSchema: "Committe",
                principalTable: "Commitees",
                principalColumn: "CommiteeId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
