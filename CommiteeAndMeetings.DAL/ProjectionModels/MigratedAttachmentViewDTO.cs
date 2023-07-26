namespace Models.ProjectionModels
{
    public class MigratedAttachmentViewDTO
    {
        public long Id { get; set; }
        public int AttachmentId { get; set; }
        public long MigratedTransactionAttachmentId { get; set; }
        public long MigratedTransactionId { get; set; }
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
        public string AttachmentTypeCode { get; set; }
        public string AttachmentTypeNameAr { get; set; }
        public string AttachmentTypeNameEn { get; set; }
        public byte[] AttachmentBinary { get; set; }
    }
}
