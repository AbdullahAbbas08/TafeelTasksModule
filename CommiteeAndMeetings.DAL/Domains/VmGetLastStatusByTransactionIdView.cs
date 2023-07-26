using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    [Table("Vm_GetLast_statusBy_TransactionId_View")]
    public partial class VmGetLastStatusByTransactionIdView
    {
        [Key]
        public int Id { get; set; }
        public int? TransactionActionId { get; set; }
        public int? ActionId { get; set; }
    }
}
