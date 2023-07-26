using System.Collections.Generic;

namespace Models.ProjectionModels
{
    public class FaxAttachmentDTO
    {
        public List<int> AttachmentIds { get; set; }
        public string organizationName { get; set; }
        public string faxNumber { get; set; }
    }
}
