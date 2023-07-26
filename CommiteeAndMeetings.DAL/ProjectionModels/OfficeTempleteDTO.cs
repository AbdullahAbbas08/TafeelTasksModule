using System;

namespace Models.ProjectionModels
{
    public class OfficeTempleteDTO
    {
        public int? OfficeTempleteId { get; set; }
        public string TempleteName { get; set; }
        public Byte[] TempleteFile { get; set; }
        public bool IsShared { get; set; }
        public string Description { get; set; }
        public string OfficeTemplateOrganizations { get; set; }
        public string ContentType { get; set; }

    }
}
