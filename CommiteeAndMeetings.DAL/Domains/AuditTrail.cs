using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    public partial class AuditTrail
    {
        [Key]
        public int AuditTrailId { get; set; }
        public int ActionId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public int OrganizationId { get; set; }
        public string OrganizationName { get; set; }
        public DateTime CreatedDate { get; set; }
        [Column("IP")]
        public string Ip { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
    }
}
