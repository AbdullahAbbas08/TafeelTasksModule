using System;

namespace Models
{
    public class AttachmentDetailsDTO
    {
        public int AttachmentId { get; set; }
        public string AttachmentName { get; set; }
        public int AttachmentTypeId { get; set; }
        public bool IsDisabled { get; set; } = false;
        public string OriginalName { get; set; }
        public string MimeType { get; set; }
        public int? Size { get; set; }
        public int? Width { get; set; }
        public int? Height { get; set; }
        public int? PhysicalAttachmentTypeId { get; set; }
        public int? PagesCount { get; set; }
        public string Notes { get; set; }
        public int CreatedBy { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
        public int UpadtedBy { get; set; }
        public DateTimeOffset? UpadtedOn { get; set; }
        public string LFEntryId { get; set; }


        public string AttachmentTypeName { get; set; }
        public byte[] BinaryContent { get; set; }
        public int? LastestAttachmentVersionId { get; set; }
        public string Text { get; set; }
        public string PhysicalAttachmentTypeName { get; set; }
        public int? PageCount { get; set; }
        public int? ReferenceAttachmentId { get; set; }

    }
}