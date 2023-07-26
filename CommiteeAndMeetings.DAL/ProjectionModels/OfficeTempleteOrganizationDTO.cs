using CommiteeAndMeetings.DAL.Domains;
using System;

namespace Models.ProjectionModels
{
    public class OfficeTempleteOrganizationDTO
    {
        public int Id { get; set; }
        public int OrganizationId { get; set; }
        public virtual Organization Organization { get; set; }
        public int OfficeTempleteId { get; set; }
        public virtual OfficeTemplete OfficeTemplete { get; set; }
        public int? DeletedBy { get; set; }
        public DateTimeOffset? DeletedOn { get; set; }
        public int? CreatedBy { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
    }
}
