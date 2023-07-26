using System;

namespace Models
{
    public class AnnotationDetailsDTO
    {
        public int AnnotationId { get; set; }
        public int AnnotationTypeId { get; set; }
        public int AttachmentId { get; set; }
        public int? PageNumber { get; set; }
        public float? X { get; set; }
        public float? Y { get; set; }
        public double? Width { get; set; }
        public double? Hight { get; set; }
        public float? Scale { get; set; }
        public string Content { get; set; }
        public int? SignatureId { get; set; }
        public long? ReferrerTransactionId { get; set; }
        public int? ReferrerTransactionActionId { get; set; }
        public int? ReferrerTransactionActionRecipientId { get; set; }
        public string MimeType { get; set; }
        public byte[] SignatureFile { get; set; }
        public int NaturalPageWidth { get; set; }
        public string TemplateHTML { get; set; }
        public string CreatedByUserName { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
        public bool IsDelegated { get; set; }
        public bool IsAllowed { get; set; }
    }
}
