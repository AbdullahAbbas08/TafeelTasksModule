using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    [Table("OfficeTemplete")]
    public partial class OfficeTemplete
    {
        public OfficeTemplete()
        {
            OfficeTempleteOrganizations = new HashSet<OfficeTempleteOrganization>();
        }

        [Key]
        public int OfficeTempleteId { get; set; }
        public string TempleteName { get; set; }
        public byte[] TempleteFile { get; set; }
        public bool IsShared { get; set; }
        public int? CreatedBy { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
        public int? DeletedBy { get; set; }
        public DateTimeOffset? DeletedOn { get; set; }
        public string Description { get; set; }
        [StringLength(200)]
        public string ContentType { get; set; }

        [InverseProperty(nameof(OfficeTempleteOrganization.OfficeTemplete))]
        public virtual ICollection<OfficeTempleteOrganization> OfficeTempleteOrganizations { get; set; }
    }
}
