namespace Models.ProjectionModels
{
    public class DeleteAttachmentAnnotationDTO
    {
        public int? AnnotationId { get; set; }
        public bool delegated { get; set; }
        /// Deleted Success
        public bool success { get; set; }
        ///in Recipients Attachments
        public bool ErrorId { get; set; }
        public string Error { get; set; }
    }
}
