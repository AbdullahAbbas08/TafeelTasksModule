namespace Models
{
    public class DocumentDTO
    {
        public int SavedAttachmentId { get; set; }
        public string AttachmentName { get; set; }
        public int AttachmentTypeId { get; set; }
        public string OriginalName { get; set; }
        public string MimeType { get; set; }
        public int Size { get; set; }
        public int? Width { get; set; }
        public int? Height { get; set; }
        public string LFEntryId { get; set; }
        public byte[] BinaryContent { get; set; }
        public int? PagesCount { get; set; }
        public string Notes { get; set; }
        public int? ReferenceAttachmentId { get; set; }
        public int? CommentId { get; set; }
        
    }
}