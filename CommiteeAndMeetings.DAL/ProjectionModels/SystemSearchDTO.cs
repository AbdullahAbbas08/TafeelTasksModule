using System;

namespace Models.ProjectionModels
{
    public class SystemSearchDTO
    {
        public string @Fast { get; set; }
        public string @TransactionNumber { get; set; }
        public long? @TransactionId { get; set; }
        public string @TransactionSubject { get; set; }
        public int? @TransactionType { get; set; }
        public int? @ImportanceLevelId { get; set; }
        public int? @ClassificationId { get; set; }
        public int? @ConfidentialityLevelId { get; set; }
        public int? @RequiredActionId { get; set; }
        public int? @RegisteredBy { get; set; }
        public int? @CreatedByEmployee { get; set; }
        public int? @DelegateTo { get; set; }
        public int? @ExportedTo { get; set; }
        public DateTimeOffset? FromDate { get; set; }
        public DateTimeOffset? ToDate { get; set; }
        public string @LblReferenceNumber { get; set; }
        public string @AttachmentNotification { get; set; }
        public string @IncomingLetterNumber { get; set; }
        public int? @OrganizationId { get; set; }
        public int? @IncomingOrganizationId { get; set; }
        public DateTimeOffset? @IncomingLetterDate { get; set; }
        public int? @OwnerId { get; set; }
        public string @IdentificationNumber { get; set; }
        public string @UserMobile { get; set; }
        public string @ReferenceNumber { get; set; }
        public int? @SIZE { get; set; }
        public string @Year { get; set; }
        public string @Notes { get; set; }
        public string @TagIdList { get; set; }
        public string @TextInLetter { get; set; }
        public int? @CorrespondentUserId { get; set; }
        public int? @CompanyId { get; set; }

    }
}
