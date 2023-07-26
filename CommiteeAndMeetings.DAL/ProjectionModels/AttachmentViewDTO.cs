using System;

namespace Models.ProjectionModels
{
    public class AttachmentViewDTO
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
        public int? PageCount { get; set; }
        public string Notes { get; set; }
        public string PhysicalAttachmentTypeNameAr { get; set; }
        public string PhysicalAttachmentTypeNameEn { get; set; }
        public string PhysicalAttachmentTypeNameFn { get; set; }

        public string AttachmentTypeCode { get; set; }
        public string AttachmentTypeNameAr { get; set; }
        public string AttachmentTypeNameEn { get; set; }
        public string AttachmentTypeNameFn { get; set; }

        public string Text { get; set; }
        public int? CreatedBy { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
        public string CreatedByFullNameEn { get; set; }
        public string CreatedByFullNameAr { get; set; }
        public string CreatedByFullNameFn { get; set; }

        public bool IsShared { get; set; }
        public int? ReferenceAttachmentId { get; set; }
        public bool FromRelatedTransaction { get; set; }
        public string OrgnazationNameAr { get; set; }
        public string OrgnazationNameEn { get; set; }
        public string OrgnazationNameFn { get; set; }

    }
}
