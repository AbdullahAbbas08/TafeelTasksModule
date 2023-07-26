using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    [Table("OfficeTempleteOrganization")]
    [Index(nameof(OfficeTempleteId), Name = "IX_OfficeTempleteOrganization_OfficeTempleteId")]
    [Index(nameof(OrganizationId), Name = "IX_OfficeTempleteOrganization_OrganizationId")]
    public partial class OfficeTempleteOrganization
    {
        [Key]
        public int Id { get; set; }
        public int OrganizationId { get; set; }
        public int OfficeTempleteId { get; set; }
        public int? CreatedBy { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
        public int? DeletedBy { get; set; }
        public DateTimeOffset? DeletedOn { get; set; }

        [ForeignKey(nameof(OfficeTempleteId))]
        [InverseProperty("OfficeTempleteOrganizations")]
        public virtual OfficeTemplete OfficeTemplete { get; set; }
        [ForeignKey(nameof(OrganizationId))]
        [InverseProperty("OfficeTempleteOrganizations")]
        public virtual Organization Organization { get; set; }
    }
}
