namespace Models.ProjectionModels
{
    public class deletedAttachmentDetailsDTO
    {
        /// this Transaction Have Been Delegated
        public bool delegated { get; set; }
        /// Deleted Success
        public bool success { get; set; }
        ///in Recipients Attachments
        public bool recipients_Exists { get; set; }
        public bool no_transactionAttachmentId { get; set; }
    }
}
