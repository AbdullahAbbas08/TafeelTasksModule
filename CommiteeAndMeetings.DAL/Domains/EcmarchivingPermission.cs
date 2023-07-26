using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    [Table("ECMArchivingPermissions")]
    [Index(nameof(EcmarchivingId), Name = "IX_ECMArchivingPermissions_ECMArchivingId")]
    [Index(nameof(PermissionId), Name = "IX_ECMArchivingPermissions_PermissionId")]
    public partial class EcmarchivingPermission
    {
        [Key]
        [Column("ECMArchivingPermitionId")]
        public int EcmarchivingPermitionId { get; set; }
        public int PermissionId { get; set; }
        [Column("ECMArchivingId")]
        public int EcmarchivingId { get; set; }
        public int? CreatedBy { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }

        [ForeignKey(nameof(EcmarchivingId))]
        [InverseProperty("EcmarchivingPermissions")]
        public virtual ECMArchiving Ecmarchiving { get; set; }
        [ForeignKey(nameof(PermissionId))]
        [InverseProperty("EcmarchivingPermissions")]
        public virtual Permission Permission { get; set; }
    }
}
