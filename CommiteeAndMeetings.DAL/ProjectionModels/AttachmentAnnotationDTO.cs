using System.Collections.Generic;

namespace Models
{
    public class AttachmentAnnotationDTO
    {
        public int AttachmentId { get; set; }
        public IEnumerable<AnnotationDetailsDTO> AnnotationDetails { get; set; }
    }
}
