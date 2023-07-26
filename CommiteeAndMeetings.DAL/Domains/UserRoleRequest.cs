using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommiteeAndMeetings.DAL.Domains
{
    public class UserRoleRequest 
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity), Key]
        public int UserRoleRequestId { get; set; }
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public int OrganizationId { get; set; }
        public bool RoleOverridesUserPermissions { get; set; }
        public DateTimeOffset? EnabledSince { get; set; }
        public DateTimeOffset? EnabledUntil { get; set; }
        public string Notes { get; set; }
        public string UserRoleStatus { get; set; }
        public string RejectReason { get; set; }
        public DateTimeOffset? ActionDate { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        [ForeignKey("RoleId")]
        public virtual Role Role { get; set; }

        [ForeignKey("OrganizationId")]
        public virtual Organization Organization { get; set; }

        public int? CreatedBy { get; set; }
        [ForeignKey("CreatedBy")]
        public virtual User CreatedByUser { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }

        public int? UpdatedBy { get; set; }
        [ForeignKey("UpdatedBy")]
        public virtual User UpdatedByUser { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
    }

}
