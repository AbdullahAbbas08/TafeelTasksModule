namespace Models
{
    public class SignatureDetailsDTO
    {
        public int SignatureId { get; set; }
        public int? UserId { get; set; }
        public byte[] SignatureFile { get; set; }
        public string MimeType { get; set; }
        public int? AnnotationTypeId { get; set; }
        public bool isActive { get; set; }
        public int? OrganizationId { get; set; }
        public string TemplateHTML { get; set; }
        public bool? wrongPassword { get; set; }
        public bool? NoSignature { get; set; }
        //user defined properties
        //public bool is_current { get; set; }
    }
}
