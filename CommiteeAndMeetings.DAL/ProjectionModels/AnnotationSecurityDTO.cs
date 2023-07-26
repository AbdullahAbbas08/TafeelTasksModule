using System;

namespace Models.ProjectionModels
{
    public class AnnotationSecurityDTO
    {
        public int Id { get; set; }
        public int AnnotationId { get; set; }
        public int UserId { get; set; }
        public int? CreatedBy { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
        public int? DeletedBy { get; set; }
        public DateTimeOffset? DeletedOn { get; set; }
    }
}
