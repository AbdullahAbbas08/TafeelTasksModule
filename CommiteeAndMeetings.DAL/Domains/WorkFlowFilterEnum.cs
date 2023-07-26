using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    [Table("WorkFlowFilterEnum")]
    public partial class WorkFlowFilterEnum
    {
        [Key]
        public int WorkFlowFilterEnumId { get; set; }
        public string ObjectName { get; set; }
        public string Description { get; set; }
    }
}
