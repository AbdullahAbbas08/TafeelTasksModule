using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    [Table("Vm_UserRoleCountOfUnDelivered")]
    public partial class VmUserRoleCountOfUnDelivered
    {
        [Key]
        public long Id { get; set; }
        public int UserRoleId { get; set; }
        public long CountOfUnDelivered { get; set; }
    }
}
