using System;

namespace Models
{
    public class SignatureSummaryDTO
    {
        public int SignatureId { get; set; }
        public int UserId { get; set; }
        public byte[] SignatureFile { get; set; }
        public string MimeType { get; set; }
        public int? AnnotationTypeId { get; set; }
        public DateTimeOffset ActiveSince { get; set; }
        public DateTimeOffset? ActiveUntil { get; set; }
    }
}
