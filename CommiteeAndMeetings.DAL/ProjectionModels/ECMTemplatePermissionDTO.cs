using System;

namespace Models.ProjectionModels
{
    public class ECMTemplatePermissionDTO
    {
        public int ECMTemplatePermitionId { get; set; }
        public int PermissionId { get; set; }
        public int ECMTemplateId { get; set; }
        public int? CreatedBy { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
    }
}
