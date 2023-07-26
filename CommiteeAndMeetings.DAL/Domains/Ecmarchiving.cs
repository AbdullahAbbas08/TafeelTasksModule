using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    [Table("ECMArchiving")]
    [Index(nameof(OrganizationId), Name = "IX_ECMArchiving_OrganizationId")]
    [Index(nameof(UserId), Name = "IX_ECMArchiving_UserId")]
    public partial class ECMArchiving
    {
        public ECMArchiving()
        {
            EcmarchivingPermissions = new HashSet<EcmarchivingPermission>();
        }

        [Key]
        [Column("ECMArchiveId")]
        public int EcmarchiveId { get; set; }
        public int? OrganizationId { get; set; }
        public int? UserId { get; set; }
        [Column("FolderEntryID")]
        public int FolderEntryId { get; set; }
        public int? CreatedBy { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
        public int? DeletedBy { get; set; }
        public DateTimeOffset? DeletedOn { get; set; }

        [ForeignKey(nameof(OrganizationId))]
        [InverseProperty("Ecmarchivings")]
        public virtual Organization Organization { get; set; }
        [ForeignKey(nameof(UserId))]
        [InverseProperty("Ecmarchivings")]
        public virtual User User { get; set; }
        [InverseProperty(nameof(EcmarchivingPermission.Ecmarchiving))]
        public virtual ICollection<EcmarchivingPermission> EcmarchivingPermissions { get; set; }
    }
}
