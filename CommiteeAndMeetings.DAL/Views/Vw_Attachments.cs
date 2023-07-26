using System;

namespace CommiteeAndMeetings.DAL.Views
{
    public class Vw_Attachments
    {
        public int Id { get; set; }
        public int AttachmentId { get; set; }
        public int TransactionAttachmentId { get; set; }
        public long TransactionId { get; set; }
        public string AttachmentName { get; set; }
        public int AttachmentTypeId { get; set; }
        public string OriginalName { get; set; }
        public string MimeType { get; set; }
        public int? Size { get; set; }
        public int? Width { get; set; }
        public int? Height { get; set; }
        public string LFEntryId { get; set; }
        public int? PagesCount { get; set; }
        public string Notes { get; set; }
        public string PhysicalAttachmentTypeNameAr { get; set; }
        public string PhysicalAttachmentTypeNameEn { get; set; }
        public string PhysicalAttachmentTypeNameFn { get; set; }

        public string AttachmentTypeCode { get; set; }
        public string AttachmentTypeNameAr { get; set; }
        public string AttachmentTypeNameEn { get; set; }
        public string AttachmentTypeNameFn { get; set; }

        public bool IsShared { get; set; }


        public int? CreatedBy { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
        public string CreatedByFullNameEn { get; set; }
        public string CreatedByFullNameAr { get; set; }

        public string CreatedByFullNameFn { get; set; }

        public int? ReferenceAttachmentId { get; set; }
        public bool FromRelatedTransaction { get; set; }
        public string OrgnazationNameAr { get; set; }
        public string OrgnazationNameEn { get; set; }
        public string OrgnazationNameFn { get; set; }
        //  public int? TransactionActionAttachmentId { get; set; }
        //   public int AttachmentId { get; set; }
        // public int? TransactionActionId { get; set; }
        //  public int? PhysicalAttachmentTypeId { get; set; }
        // public int? AttachmentVersionId { get; set; }
        //public string text { get; set; }
        //public int? TransactionActionRecipientAttachmentId { get; set; }
        // public int? TransactionActionRecipientId { get; set; }
        //public int? AttachmentStatusId { get; set; }
        //public string AttachmentStatusCode { get; set; }
        //public string AttachmentStatusNameAr { get; set; }
        //public string AttachmentStatusNameEn { get; set; }
    }
}
