namespace Models.ProjectionModels
{
    public class LettersDTO
    {
        public string AttachmentName { get; set; }
        public int AttachmentTypeId { get; set; }
        public string Text { get; set; }
        public int AttachmentId { get; set; }
        public long TransactionId { get; set; }
        public int TransactionAttachmentId { get; set; }
        public int? CommiteeId { get; set; }
        public int? CommiteeAttachmentId { get; set; }
        //public IEnumerable<AttachmentVersionDTO> attachmentVersionDTOs { get; set; }
    }
}
