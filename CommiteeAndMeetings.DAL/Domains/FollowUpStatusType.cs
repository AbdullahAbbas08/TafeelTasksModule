using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    [Table("FollowUpStatusType")]
    public partial class FollowUpStatusType
    {
        public FollowUpStatusType()
        {
            FollowUpStatuses = new HashSet<FollowUpStatus>();
            FollowUps = new HashSet<FollowUp>();
        }

        [Key]
        public int FollowUpStatusTypeId { get; set; }
        public string FollowUpStatusCode { get; set; }
        public string FollowUpStatusNameAr { get; set; }
        public string FollowUpStatusNameEn { get; set; }
        public string FollowUpStatusNameFn { get; set; }

        [InverseProperty(nameof(FollowUpStatus.FollowStatusType))]
        public virtual ICollection<FollowUpStatus> FollowUpStatuses { get; set; }
        [InverseProperty(nameof(FollowUp.FollowUpStatusType))]
        public virtual ICollection<FollowUp> FollowUps { get; set; }
    }
}
